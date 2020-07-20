using Autofac;
using Autofac.Integration.Mvc;
using SP_ASPNET_1.DbFiles.Operations;
using SP_ASPNET_1.DbFiles.UnitsOfWork;
using System.Web.Mvc;

namespace SP_ASPNET_1.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

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