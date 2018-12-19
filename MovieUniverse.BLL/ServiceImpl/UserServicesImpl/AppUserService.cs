using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using MovieUniverse.Abstract.ApiModels;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Enums;
using MovieUniverse.Abstract.Exceptions;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;
using MovieUniverse.Abstract.Services;
using MovieUniverse.Abstract.Services.UserServices;
using Ninject;

namespace MovieUniverse.Services.ServiceImpl.UserServicesImpl
{
    public class AppUserService : ServiceBase<AppUser>, IAppUserService
    {
        private const string Key = "fdsafs-#dfd-s78-9798fds-$%a321k^nf9034rfjdkla09r-0324324";
        private const int ConfirmationTotalHours = 24;
        private IAppUserRepository Repository => GenereicRepository as IAppUserRepository;

        [Inject]
        private IEmailService EmailService { get; set; }
        
        public bool IsUserPublic(long userId)
        {
            return Repository.IsPublicUser(userId);
        }
        public bool IsUserActive(long userId)
        {
            try
            {
                return Repository.IsUserActive(userId);
            }
            catch (NullReferenceException ex)
            {

                throw new MovieUniverseException(ExceptionType.UserByIdNotExist);
            }

        }

        public bool IsFriends(long userId, long friendUserId)
        {
            return Repository.IsFriends(userId, friendUserId);
        }

        public bool CheckUserAccessStatus(long userId, long requestedUserId)
        {
            return (IsUserActive(requestedUserId) && IsUserPublic(requestedUserId) ||
                    IsFriends(userId, requestedUserId) || userId == requestedUserId);
            
        }

        public IEnumerable<AppUser> GetAll(long? userId, FilterBase<AppUser> filter)
        {
            return Repository.GetAll(userId, filter);
        }
        //TODO set EmailComfired = false
        public void RegisterUser(AppUser obj)
        {
            var user = Repository.GetUser(obj.Email);
            if (user != null && !user.EmailComfirmed)
            {
                if (DateTimeOffset.Now.Subtract(user.RegistrationDate).TotalHours < ConfirmationTotalHours)
                    throw new MovieUniverseException(ExceptionType.UserByThisEmailExist);

                Repository.Delete(user.Id);
                Repository.Save();
            }
            obj.RegistrationDate = DateTimeOffset.Now;
            obj.PhotoUrl = "http://www.lcfc.com/images/common/bg_player_profile_default_big.png";
            obj.PasswordHash = ComputeHash(obj.Password);
            obj.EmailComfirmed = true;

            obj = Insert(obj);
            EmailService.SendEmail(GenerateConfiramtionEmail(obj));
        }


        public override AppUser GetById(long id)
        {
            throw new NotImplementedException("GetById with one parametr not impleneted");
        }
        //TODO Refactor
        public AppUser GetById(long userId, long requestedUserId)
        {
            if(!IsUserActive(requestedUserId))
                throw new MovieUniverseException(ExceptionType.UserByIdNotExist);
            
            AppUser user = Repository.GetById(userId, requestedUserId);
            if (CheckUserAccessStatus(userId, requestedUserId))
            {
                user.Token = null;
                user.PasswordHash = null;

                if (userId != requestedUserId)
                    user.Email = null;
                else
                    user.Connection = null;
                
                user.IsPublicUser = true;
                return user;
            }
            user = new AppUser()
            {
                Id = user.Id,
                Name = user.Name,
                PhotoUrl = user.PhotoUrl,
                Connection = user.Connection,
                FriendsCount = user.FriendsCount,
                WatchMoviesCount = user.WatchMoviesCount,
                WatchedMoviesCount = user.WatchedMoviesCount,
            };
            return user;
        }

        private string ComputeHash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private Token GenerateToken(AppUser user)
        {
            var token = user.Name + user.PasswordHash + DateTimeOffset.Now;
            var expirationDate = DateTimeOffset.Now.Add(new TimeSpan(1, 0, 0, 0));
            return new Token()
            {
                Key = ComputeHash(token),
                ExpirationDate = expirationDate,
            };
        }
        
        public AppUser Login(UserLoginModel loginModel)
        {
            if (ComputeHash(loginModel.Password) == Repository.GetUserPassword(loginModel.Email))
            {
                var user = Repository.GetUser(loginModel.Email);
                if (!user.EmailComfirmed)
                    throw new MovieUniverseException(ExceptionType.InvalidEmailorPassword);

                if (user.Token != null && user.Token.ExpirationDate.CompareTo(DateTimeOffset.Now) > 0)
                    return user;

                var token = GenerateToken(user);
                token.UserId = user.Id;
                user.Token = InsertToken(token);
                return user;
            }
            throw new MovieUniverseException(ExceptionType.InvalidEmailorPassword);
        }

        public long GetUserIdByToken(string key)
        {
            var token = Repository.GetToken(key);
            if (token != null && token.ExpirationDate.CompareTo(DateTimeOffset.Now) > 0)
                return token.UserId;
            return -1;
        }

        public AppUser OAuthLogin(OAuthUserLoginModel loginModel)
        {
            HashSet<string> providers = new HashSet<string>()
            {
                "google",
                "facebook",
            };
            if (!providers.Contains(loginModel.Provider))
                throw new MovieUniverseException(ExceptionType.InvalidProvider);

            var oAuthUser = Repository.GetOAuthUser(loginModel);
            if (oAuthUser != null)
            {
                if (oAuthUser.Token.ExpirationDate.CompareTo(DateTimeOffset.Now) > 0)
                    return oAuthUser;

                var token = GenerateToken(oAuthUser);
                token.UserId = oAuthUser.Id;
                oAuthUser.Token = InsertToken(token);
                return oAuthUser;
            }
            var appUser = new AppUser()
            {
                Email = loginModel.Email,
                EmailComfirmed = true,
                Name = loginModel.Name,
                Provider = loginModel.Provider,
                ProviderUserId = loginModel.ProviderId,
                PhotoUrl = loginModel.ProviderProfilePhotoUrl,
                ProviderProfileUrl = loginModel.ProviderProfileUrl,
                IsPublicUser = true,
                
            };
            appUser.Token = GenerateToken(appUser);
            var user = Repository.Insert(appUser);
            Repository.Save();
            return user;
        }

        public override IEnumerable<AppUser> GetAll(FilterBase<AppUser> filter = null)
        {
            var userFilter = filter as AppUserFilter;
            if(userFilter == null)
                throw new ArgumentException("Invalid Filter Parametr in AppUserService GetAll Function");
            userFilter.IsActiveUser = true;

            return Repository.GetAll(userFilter);

        }

        private EMailModel GenerateConfiramtionEmail(AppUser user)
        {
            var link = GenereateConfirimationEmailLink(user);
            EMailModel email = new EMailModel()
            {
                DestinationAddress = user.Email,
                MessegeBody =
                    $"for confiramtion click in this <a href = 'http://movieuniverse.cloudapp.net:3000/verfication?email={user.Email}&key={link}' target = '_blanck'>link</a>",
                Subject = "Confirmation Message",
            };
            return email;
        }
        public void ConfirmUser(AccountActivationModel model)
        {
            var key = model.Email + Key;
            var hash = ComputeHash(key);
            if (hash == model.Key)
            {
                var user = Repository.GetUser(model.Email);
                if (user != null && DateTimeOffset.Now.Subtract(user.RegistrationDate).TotalHours < ConfirmationTotalHours)
                {
                    user.EmailComfirmed = true;
                    Repository.Save();
                    return;
                }
                throw new MovieUniverseException(ExceptionType.InvalidActivationKeyOrEmail);
            }
            throw new MovieUniverseException(ExceptionType.InvalidActivationKeyOrEmail);
        }

        private string GenereateConfirimationEmailLink(AppUser user)
        {
            var link = user.Email + Key;
            var hash = ComputeHash(link);
            return hash;
        }

        private Token InsertToken(Token token)
        {
            var oldToken = Repository.GetToken(token.UserId);
            if (oldToken == null)
                oldToken = Repository.InsertToken(token);
            else
            {
                oldToken.Key = token.Key;
                oldToken.ExpirationDate = token.ExpirationDate;
            }
            Repository.Save();
            return oldToken;
        }

        public override int GetCount(FilterBase<AppUser> filter = null)
        {
            var domainFilter = filter as AppUserFilter;
            if(domainFilter == null)
                throw new ArgumentException("invalid type in AppUserService GetCount() function");
            domainFilter.IsActiveUser = true;
            return base.GetCount(filter);
        }

        public IEnumerable<UserUserActivity> GetUserUserActivities(long userId)
        {
            return Repository.GetUserUserActivities(userId);
        }

        public IEnumerable<UserMovieActivity> GetUserMovieActivities(long userId)
        {
            return Repository.GetUserMovieActivities(userId);
        }

        public IEnumerable<UserMovieActivity> GetUserRateMovieActivities(long userId)
        {
            return Repository.GetUserRateMoiveActivites(userId);
        }

        public AppUser EditUser(long userId, UserEditModel userData)
        {
            int count = 0;

            var user = Repository.GetUser(userId);
            if (user.Name != userData.Name)
            {
                user.Name = userData.Name;
                count++;
            }

            if (user.IsPublicUser != userData.IsPublic)
            {
                user.IsPublicUser = userData.IsPublic;
                count++;
            }

            if (!string.IsNullOrEmpty(userData.Email) && user.Email != userData.Email)
                user.Email = userData.Email;

            if (!string.IsNullOrEmpty(userData.PhotoUrl))
            {
                user.PhotoUrl = userData.PhotoUrl;
                count++;
            }

            if (!string.IsNullOrEmpty(userData.OldPassword) && ComputeHash(userData.OldPassword) == user.PasswordHash && !string.IsNullOrEmpty(userData.NewPassword) &&
                userData.NewPassword == userData.NewPasswordConfirm)
            {
                user.Password = userData.NewPassword;
                user.PasswordHash = ComputeHash(userData.NewPassword);
                count++;
            }
            if (!string.IsNullOrEmpty(userData.OldPassword))
            {
                if (ComputeHash(userData.OldPassword) != user.PasswordHash) 
                    throw new MovieUniverseException(ExceptionType.InvalidPassword);
                if(string.IsNullOrEmpty(userData.NewPassword) || userData.NewPassword != userData.NewPasswordConfirm)
                    throw new MovieUniverseException(ExceptionType.InvalidNewPassword);
            }
            else
            {
                if(!string.IsNullOrEmpty(userData.NewPassword) || !string.IsNullOrEmpty(userData.NewPasswordConfirm))
                    throw new MovieUniverseException(ExceptionType.InvalidPassword);
            }
            if(count > 0)
                Repository.Save();

            return user;

        }
    }
}