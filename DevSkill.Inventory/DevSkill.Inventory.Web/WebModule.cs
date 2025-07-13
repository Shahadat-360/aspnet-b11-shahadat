using Autofac;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Infrastructure.Services;
using DevSkill.Inventory.Web.Data;

namespace DevSkill.Inventory.Web
{
    public class WebModule:Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString",_connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitRepository>().As<IUnitRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SaleRepository>().As<ISaleRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CashAccountRepository>().As<ICashAccountRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BankAccountRepository>().As<IBankAccountRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MobileAccountRepository>().As<IMobileAccountRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<IdGenerator>().As<IIdGenerator>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ImageService>().As<IImageService>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
