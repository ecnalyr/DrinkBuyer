using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace DrinkBuyer.WebUI.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;

    using Moq;
    using DrinkBuyer.Domain.Concrete;

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext,
        Type controllerType)
        {
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            // put additional bindings here
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    }
}