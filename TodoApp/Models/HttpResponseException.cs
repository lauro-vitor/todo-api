using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class HttpResponseException
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public List<HttpResponseException> Details { get; set; }
        public HttpResponseException InnerError { get; set; }

        public HttpResponseException(Exception ex, string target)
        {
            Message = ex.Message;
            Target = target;
            Code = "";
        
            Exception innerException = ex.InnerException;

            if(innerException != null)
            {
                InnerError = new HttpResponseException(innerException, target);
            }
        }
    }
}
