using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestoranWeb.Data;

namespace RestoranWeb.Handlers
{
    public class Authorized : ActionFilterAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        private List<string> RolesList = new();
       private bool Isauthenticated { get; set; } = false;
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            //var userId = context.HttpContext.Session.GetString(Global.LoginSession);
            RolesList = (Roles ?? " ").Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList(); 
          var Db =  (AppDbContext)context.HttpContext.RequestServices.GetService(typeof(AppDbContext));
            var LoggedInUser = Db.GetLoggedInUser();
            if (LoggedInUser != null)
            {
                if (RolesList.Any())
                {
                    //foreach(var role in RolesList)
                    //{
                    //    foreach(var userRole in Db.LoggedInUser.AppRole)
                    //    {
                    //        if(role == userRole)
                    //        {
                    //            Isauthenticated = true;
                    //        }
                    //        if (Isauthenticated) break;
                    //    } 
                    //        if (Isauthenticated) break;
                    //}
                 Isauthenticated =   RolesList.Any(rr => LoggedInUser.AppRole.Any(ur => rr == ur));
                }
                else
                {
                    Isauthenticated = true;
                }

            }
            //else
            //{
            //    //check Roles
            //    //Account Status Active
            //    //Some other checks
            //}
            if (!Isauthenticated)
            {

            context.Result = new RedirectResult("~/Login/index");
            }
            
        }
    }
}
