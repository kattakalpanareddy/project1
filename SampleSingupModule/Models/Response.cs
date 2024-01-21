using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleSingupModule.Models
{
    public class Response
    {
    }
   
}
namespace SingupRS
{

    public class SingupResponse
    {
        public HeaderSection Header { get; set; }
        public RegistrationStatus RegStatus { get; set; }
    }
    public class HeaderSection
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class RegistrationStatus
    {
        public string RegStatusMsg { get; set; }

    }
}