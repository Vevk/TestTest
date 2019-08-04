using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UTT.Domain.Customers.Services;

namespace Tests2
{
    public class AutoMoqInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(
                new AutoMoqResolver(container.Kernel));
            container.Register(Component.For(typeof(Mock<>)));
            container.Register(Classes.FromAssemblyContaining<CustomerService>()
                .Pick()
                .WithServiceSelf()
                .LifestyleTransient()
                );                
        }
    }
}
