using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace SampleSingupModule.App_Start
{
    public class SqlFunctions
    {
        private SqlConnection ServiceConnection(string name)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ToString());
        }
        public DataSet GetQueryResults(string conStringName, string query)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = ServiceConnection(conStringName);
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public Object GetExecuteScalar(string conStringName, string query)
        {
            Object i = "0";
            SqlConnection con = ServiceConnection(conStringName);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                i = cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                i = 0;
            }
            finally
            {
                con.Close();
            }
            return i;
        }

        public int GetExecuteNonQuery(string conStringName, string query)
        {
            int i = 0;
            SqlConnection con = ServiceConnection(conStringName);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                i = 0;
            }
            finally
            {
                con.Close();
            }
            return i;
        }
    }
    public class ServiceSecurity
    {
    }
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authentication = actionContext.Request.Headers.Authorization.Parameter;
                string decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authentication));
                if (decodedCredentials.Split(':').Length == 3)
                {

                    if (ValidateCredentials(decodedCredentials.Split(':')[0], decodedCredentials.Split(':')[1], decodedCredentials.Split(':')[2]) != "Success")
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);

                    }
                    
                }

            }
            }
        protected string ValidateCredentials(string ClientUN, string ClientPwd, string SecurityKey)
        {
            string AuthStatus = "";

            SqlFunctions sf = new SqlFunctions();
            string query = "Select *  from client_login cl (nolock) where cl.client_status = 1 and  cl.client_username ='" + ClientUN + "' and cl.client_password ='" + ClientPwd + "' and cl.Securitykey ='" + SecurityKey + "'";
            DataSet ds = sf.GetQueryResults("dbcon", query);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                AuthStatus = "Success";

            }
            else
            {
                AuthStatus = "Fail";

            }
            return AuthStatus;
        }
    }
        
}