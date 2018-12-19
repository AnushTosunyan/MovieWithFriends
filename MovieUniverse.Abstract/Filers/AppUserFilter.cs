using System.Linq;
using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.Abstract.Filers
{
    public class AppUserFilter:ModelFilterBase<AppUser>
    {
        public bool IsActiveUser { get; set; }

        public bool IsMutual { get; set; }
        public override IQueryable<AppUser> Filter(IQueryable<AppUser> query)
        {
            query = query.Where(x => x.EmailComfirmed == IsActiveUser);
            return base.Filter(query);
        }
    }
}