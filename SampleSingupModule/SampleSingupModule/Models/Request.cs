using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleSingupModule.Models
{
    public class Request
    {
    }

}
namespace RequestModel
{
    public class RegistrationRQ
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}