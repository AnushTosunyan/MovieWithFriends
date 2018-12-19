using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using MovieUniverse.Abstract.Data;

namespace MovieUniverse.Abstract.Entities
{
    public class ModelBase : IObjectState, IEntity
    {
        [Key]
        public long Id { get; set; }

        [NotMapped]
        public ObjectState State { get; set; }
    }
}
