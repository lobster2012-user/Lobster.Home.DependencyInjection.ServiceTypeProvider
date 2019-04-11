using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lobster.Home.DependencyInjection
{
    public class PluginServiceTypeProvider : IServiceTypeProvider
    {
        private readonly Assembly[] _assemblies;
        private readonly Func<Type, bool> _filter;

        public PluginServiceTypeProvider(IEnumerable<Assembly> assemblies, Func<Type, bool> filter = null)
        {
            _assemblies = assemblies.ToArray() ?? throw new ArgumentNullException(nameof(assemblies));
            _filter = filter ?? new Func<Type, bool>((_) => true);
        }
        public IEnumerable<Type> GetServiceTypes(Type expectedType)
        {
            return _assemblies.Where(z=>!z.IsDynamic)
                              .SelectMany(assembly => assembly.GetTypes())
                              .Where(type =>
                                        !type.IsAbstract 
                                    && (type.IsNestedPublic ||type.IsPublic) 
                                    && expectedType.IsAssignableFrom(type) 
                                    && _filter(type));
        }
    }
}
