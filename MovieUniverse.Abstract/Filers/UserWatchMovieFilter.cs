using System.Linq;
using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.Abstract.Filers
{
    public class UserWatchMovieFilter:ModelFilterBase<UserWatchMovie>
    {
        public WatchStatus? Status { get; set; }

        public long? UserId { get; set; }
        public override IQueryable<UserWatchMovie> Filter(IQueryable<UserWatchMovie> query)
        {
            if (Status.HasValue)
                query = query.Where(x => x.Status == Status.Value);
            if(UserId.HasValue)
                query = query.Where(x => x.UserId == UserId);
            return base.Filter(query);
        }
    }
}