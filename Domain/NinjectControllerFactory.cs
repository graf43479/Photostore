using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Infrasructure.Abstract;
using Ninject;
using Domain.Infrasructure.Concrete;


namespace Domain
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            ninjectKernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            ninjectKernel.Bind<IUserRepository>().To<EFUserRepository>();
            ninjectKernel.Bind<ICommentRepository>().To<EFCommentRepository>();
            ninjectKernel.Bind<ICalendarRepository>().To<EFCalendarRepository>();
            ninjectKernel.Bind<IDeliveryProcessor>().To<DeliveryProcessor>();
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

            ninjectKernel.Bind<PhotoDBContext>()
                         .ToSelf()
                         .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings[0].ConnectionString
                                                  ); 
            ninjectKernel.Inject(Membership.Provider);
            ninjectKernel.Inject(Roles.Provider);
        }
    }
}
