using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
    public class InvalidTypeEntityException : ApplicationException
    {
        public InvalidTypeEntityException(string entity, string type) : base($"Entity {entity} not support type {type}")
        {
            
        }
    }
}
