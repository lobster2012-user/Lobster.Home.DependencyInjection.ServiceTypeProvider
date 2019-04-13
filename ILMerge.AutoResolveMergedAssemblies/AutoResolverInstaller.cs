using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ILMerge.AutoResolveMergedAssemblies
{
    public static class AutoResolverInstaller
    {
        private readonly static HashSet<Assembly> Assemblies = new HashSet<Assembly>();

        static AutoResolverInstaller()
        {

        }
       
        public static void EnsureInstalled()
        {
            foreach(var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                lock (Assemblies)
                {
                    if (!Assemblies.Add(assembly))
                    {
                        continue;
                    }
                }
                var mergedAssemblies = new HashSet<string>(assembly.GetCustomAttributes<AutoResolverMergedAssembliesAttribute>()
                    .SelectMany(z => z.AssemblyNames)).ToArray();
                if (mergedAssemblies.Length > 0)
                {
                    System.AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                    {
                        var assemblyName = new AssemblyName(args.Name);
                        if (mergedAssemblies.Contains(assemblyName.Name))
                        {
                            return assembly;
                        }
                        return null;
                    };
                }
            }
        }
    }
}
