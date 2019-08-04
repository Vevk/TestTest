using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Moq;
using System;

namespace Tests2
{
    public class AutoMoqResolver : ISubDependencyResolver
    {
        public AutoMoqResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        private readonly IKernel _kernel;

        public bool CanResolve(
            CreationContext context, 
            ISubDependencyResolver contextHandlerResolver, 
            ComponentModel model, 
            DependencyModel dependency)
        {
            return dependency.TargetType.IsInterface;
        }

        public object Resolve(
            CreationContext context, 
            ISubDependencyResolver contextHandlerResolver, 
            ComponentModel model, 
            DependencyModel dependency)
        {
            var mockType = typeof(Mock<>).MakeGenericType(dependency.TargetType);
            return ((Mock)this._kernel.Resolve(mockType)).Object;
        }
    }
}
