using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lobster.Home.DependencyInjection
{
    public static class ServiceTypeProviderExtensions
    {
        public static IEnumerable<Type> GetServiceTypes<TService>(this IServiceTypeProvider provider)
            => provider.GetServiceTypes(typeof(TService));
    }
}
