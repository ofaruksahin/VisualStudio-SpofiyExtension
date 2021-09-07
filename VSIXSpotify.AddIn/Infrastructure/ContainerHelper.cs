using Autofac;
using VSIXSpotify.AddIn.Core.IRepository;
using VSIXSpotify.AddIn.Infrastructure.Repository;

namespace VSIXSpotify.AddIn.Infrastructure
{
    public static class ContainerHelper
    {
        private static IContainer _container = null;

        public static IContainer Build()
        {
            if (_container != null)
                return _container;
            var builder = new ContainerBuilder();

            builder
                .RegisterType(typeof(AuthService))
                .As<IAuthService>()
                .SingleInstance();

            _container = builder.Build();
            return _container;
        }        

        public static void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }
    }
}
