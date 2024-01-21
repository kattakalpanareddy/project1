using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleSingupModule.App_Start;

namespace SampleSingupModule.Controllers
{
    public class SignupController : ApiController
    {
        [HttpPost]
        [BasicAuthentication]
        public HttpResponseMessage POST([FromBody]RequestModel.RegistrationRQ request)
        {
            try
            {
                SingupRS.SingupResponse obj = new SingupRS.SingupResponse();
                obj = CreateAcc(request);
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }
        internal SingupRS.SingupResponse CreateAcc(RequestModel.RegistrationRQ request)
        {
            SingupRS.SingupResponse obj = new SingupRS.SingupResponse();
            obj.Header = new SingupRS.HeaderSection();
            obj.RegStatus = new SingupRS.RegistrationStatus();

            string cmdtxt = "select * from Users where Username='"+request.UserName+"'";
            SqlFunctions sf = new SqlFunctions();
            DataSet ds = sf.GetQueryResults("dbcon", cmdtxt);
            try
            {
                if (request.Password != request.ConfirmPassword)
                {
                    obj.Header.Status = "OK";
                    obj.Header.Message = "Fail";
                    obj.RegStatus.RegStatusMsg = "Password and confirm password not matching";
                }

                else if (ds != null && ds.Tables[0].Rows.Count == 1)
                {
                    obj.Header.Status = "OK";
                    obj.Header.Message = "Fail";
                    obj.RegStatus.RegStatusMsg = "Account Already Exists with this username";

                }
                else
                {
                    cmdtxt = "insert into Users(firstname,lastname,username,password) values('" + request.FirstName + "','" + request.LastName + "','" + request.UserName + "','" + request.Password + "')";

                    int i = sf.GetExecuteNonQuery("dbcon", cmdtxt);


                    obj.Header.Status = "OK";
                    obj.Header.Message = "Success";
                    obj.RegStatus.RegStatusMsg = "Account Created Successfully";
                }
            }
            catch(Exception ex)
            {
                obj.Header.Status = "OK";
                obj.Header.Message = "Success";
                obj.RegStatus.RegStatusMsg = ex.ToString() ;

            }
            return obj;
        }
    }
}
