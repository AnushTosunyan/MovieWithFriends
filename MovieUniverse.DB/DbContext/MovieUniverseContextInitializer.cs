using System;
using System.Data.Entity;
using MovieUniverse.Abstract.Entities;

namespace MovieUniverse.DB.DbContext
{
    public class MovieUniverseContextInitializer:DropCreateDatabaseIfModelChanges<MovieUniverseContext>
    {
        public override void InitializeDatabase(MovieUniverseContext context)
        {
            base.InitializeDatabase(context);

            try
            {
                context.Database.ExecuteSqlCommand(
              $@"create function [dbo].GetUserMovieFriendCounts()
                    Returns table
                return
                        select IsNull(R.UserId,r3.Id) as UserId ,ISNULL(R.WatchedMoviesCount,0) as WatchedMoviesCount,ISNULL(R.WatchMoviesCount,0) as WatchMoviesCount,ISNULL(r3.friendCount,0) as FriendsCount From 

		 (Select ISNULL(R1.UserId, R2.UserId) as UserId,ISNULL(R1.watchedCount, 0) as WatchedMoviesCount,ISNULL(R2.watchCount, 0) as WatchMoviesCount From
           (select userId, status, count(*) as watchedCount from UserWatchMovies u1 group by u1.UserId, status having status = 1) R1
                full outer join
                (select userId, status, count(*) as watchCount from UserWatchMovies u1 group by u1.UserId, status having status = 0) R2
                     on r1.UserId = r2.UserId)  R
					 full outer join
					 (select f.UserId as Id,count(*) as friendCount From Friends f where f.status=1 group by f.UserId) R3
		on r3.Id = r.UserId
");

                context.Database.ExecuteSqlCommand(@"create function [dbo].GetMovies(@userId bigint)
returns table
return
Select m.Id, m.Title, m.Year, m.ViewCount, IsNull(R1.Rating, 0) as Rating, i.Url as HeadImageUri, u.Status as WatchStatus
 From Movies m left join
Images i on m.id = i.MovieId
left join
(select r.MovieId, cast ( IsNull(avg(r.Star), 0) as float) as Rating From Ratings r group by r.MovieId) R1 on R1.MovieId = m.Id
left join
UserWatchMovies u on u.MovieId = m.Id and  u.UserId = @userId");



                context.Database.ExecuteSqlCommand(@"create function [dbo].GetUserFriendListForRoom(@userIdFriendList bigint,@roomIdFriendList bigint)
returns table
return
Select f.FriendUserId as UserId,R.RoomId,R.MemberType,R.InvitationId,R.InvitationStatus From 

(select * From Friends where [status]=1 and UserId = @userIdFriendList) f 
left join

(select RoomId,UserId,MemberType,null as InvitationStatus,null as InvitationId From RoomMembers where RoomId = @roomIdFriendList
union
select RoomId,InvitedUserId as UserId,null as MemberType,Status as InvitationStatus,id as InvitationId From RoomInvitations where Status = 0 and RoomId=@roomIdFriendList
) R on f.FriendUserId = r.UserId
");

            }
            catch (Exception ex)
            {
                
            }

           
        }

        
    }
}