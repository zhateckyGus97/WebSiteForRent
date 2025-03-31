using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class BaseApplicationException : ApplicationException
    {
        public BaseApplicationException(string message) 
            : base(message) { }

        public BaseApplicationException(string message, Exception inner)
            : base(message, inner) { }

        public virtual HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public virtual string Title => "Application Exception Occured";
    }
}
