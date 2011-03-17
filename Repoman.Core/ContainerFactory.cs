using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Data.Objects;

namespace Repoman.Core
{
    public class ContainerFactory<TStore> where TStore : ObjectContext
    {
        private ContainerBuilder _containerBuilder;

        public ContainerFactory() : this(Environments.Production) { }

        public ContainerFactory(Environments environment) : this(environment.ToString()) { }

        public ContainerFactory(string environment)
        {
            _containerBuilder = new ContainerBuilder();

            if (environment == Environments.Test.ToString())
            {
                _containerBuilder.RegisterType(typeof(TStore)).As(typeof(TStore)).SingleInstance();
            }
            else
            {
                _containerBuilder.RegisterType(typeof(TStore)).As(typeof(TStore)).SingleInstance().ExternallyOwned();
                _containerBuilder.RegisterGeneric(typeof(EntityFrameworkRepositoryFactory<>)).SingleInstance().ExternallyOwned();
            }
        }

        public ContainerBuilder Builder()
        {
            return _containerBuilder;
        }

        public IContainer Build()
        {
            return _containerBuilder.Build();
        }
    }
}
