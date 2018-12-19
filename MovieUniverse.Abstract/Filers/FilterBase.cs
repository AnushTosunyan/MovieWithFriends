using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MovieUniverse.Abstract.Entities;

namespace MovieUniverse.Abstract.Filers
{
    public abstract class FilterBase<T> where T : IEntity
    {
        
        public int? Id { get; set; }

        public int? SkipCount { get; set; }

        public int? SelectCount { get; set; }
        

        public virtual IQueryable<T> Filter(IQueryable<T> query)
        {
            return query;
        }
    }
}
