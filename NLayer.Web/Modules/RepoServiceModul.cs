using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repository;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;
namespace NLayer.Web.Modules
{
    public class RepoServiceModul : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //bunar 1 tane :
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();


            var apiAssembly = Assembly.GetExecutingAssembly(); //apim
            //*** Katmanların adını da verebilirim ama olabildiğince tip güvenli gidiyorum.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //repository katmanından herhangi bir class
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //servis katmanından herhangi bi class





            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //InstancePerLifeScope => Scope = 1 request bağladı, bitene kadar aynı instance'ı kullansın.
            //InstancePerDependency => transient = her seferinde yeni bir instance oluşturuyor.

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                 Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();


        }
    }
}
