using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SP_ASPNET_1.DbFiles.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    /// <summary>
    /// The role manager used by the application to store roles and their connections to users
    /// </summary>
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            ///It is based on the same context as the ApplicationUserManager
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<IceCreamBlogContext>()));

            return appRoleManager;
        }
    }
}