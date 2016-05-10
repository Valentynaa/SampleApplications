using System;
using System.Net;
using MagentoConnect.Utilities;

namespace MagentoConnect.Controllers
{
    public class BaseController
    {
        protected UrlFormatter UrlFormatter;

        public BaseController()
        {
            UrlFormatter = new UrlFormatter();
        }

        /**
         * This function allows you to run CheckStatusCode without an expected code, it overrides with 200-OK
         * 
         * @param   HttpStatusCode  HTTP code returned
         */
        public void CheckStatusCode(HttpStatusCode actualCode)
        {
            CheckStatusCode(actualCode, HttpStatusCode.OK);
        }

        /**
         * This function allows you to check that the code returned was the one expected
         * If an unexpected code was returned, it throws the exception wrapped in a message 
         *
         * @param   HttpStatusCode  HTTP code returned
         * @param   HttpStatusCode  Expected HTTP status code
         */
        public void CheckStatusCode(HttpStatusCode actualCode, HttpStatusCode expectedCode)
        {
            if (actualCode != expectedCode)
            {
                throw new Exception(string.Format("Unexpected HTTP status code. Expected {0}, returned {1}", expectedCode, actualCode));
            }
        }
    }
}
