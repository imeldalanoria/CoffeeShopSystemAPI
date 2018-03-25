using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CoffeeShop.EntityFramework;
using CoffeeShop.EntityFramework.Persistence;
using CoffeeShop.EntityFramework.Repositories;
using CoffeeShop.Web.Areas.HelpPage.Controllers;
using CoffeeShop.Web.Controllers;
using System.Data.Entity;

namespace CoffeeShop.Web.Infrastructure
{
    public class CoffeeShopDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn(typeof(OfficesController)).LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn(typeof(OrdersController)).LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn(typeof(ProductsController)).LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn(typeof(HelpController)).LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn(typeof(HomeController)).LifestyleTransient(),
                Component.For<DbContext>().ImplementedBy<CoffeeShopContext>(),
                Component.For(typeof(Repository<>)).ImplementedBy<OfficeRepository>(),
                Component.For(typeof(Repository<>)).ImplementedBy<ProductRepository>(),
                Component.For(typeof(Repository<>)).ImplementedBy<OrderItemRepository>(),
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>))
                );

        }
    }
}