using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SP_ASPNET_1.Controllers;
using SP_ASPNET_1.DbFiles.Operations;
using SP_ASPNET_1.DbFiles.UnitsOfWork;
using SP_ASPNET_1.Models;
using System.Web;
using System.Web.Mvc;


namespace SP_ASPNET_1.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

           
            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<RoleStore<IdentityRole>> ().As<IRoleStore<IdentityRole, string>>().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.RegisterType<BlogPostOperations>().As<IBlogPostOperations>().SingleInstance();
            builder.RegisterType<PostCommentOperations>().As<IPostCommentOperations>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
            //builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}