using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundApplicationExeption : BaseApplicationException
    {
        public NotFoundApplicationExeption(string message) : base(message)
        {
        }

        public NotFoundApplicationExeption(string message, Exception inner) : base(message, inner)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public override string Title => "Entity not found";
    }
}
