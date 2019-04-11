using System;
using System.Collections.Generic;

namespace Lobster.Home.DependencyInjection
{
    public interface IServiceTypeProvider
    {
        IEnumerable<Type> GetServiceTypes(Type type);
    }
}
