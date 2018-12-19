using System;
using System.Collections.Generic;

namespace MovieUniverse.Abstract.Exceptions
{
    public class MovieUniverseException:Exception
    {
        public ExceptionType Type { get; set; }

        public IEnumerable<string> Errors { get; set; } 

        public MovieUniverseException(ExceptionType type,IEnumerable<string> errors = null)
        {
            Type = type;
            Errors = errors;
        }

    }
}