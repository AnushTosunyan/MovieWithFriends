using System.Data.Entity.Infrastructure.Interception;

namespace MovieUniverse.Abstract.Entities
{
    public interface IEntity
    {
         long Id { get; set; } 
    }
}