using ILMerge.AutoResolveMergedAssemblies;
using System.Collections.Generic;
using System.Diagnostics;
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
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().OrderBy(z => z.FullName).ToArray();
            var assemblyNames = assemblies.Select(z => z.GetName().Name).OrderBy(z=>z).ToArray();

            foreach (var assembly in assemblies)
            {
                lock (Assemblies)
                {
                    if (!Assemblies.Add(assembly))
                    {
                        continue;
                    }
                }
                var mergedAssemblies = new HashSet<string>(assembly.GetCustomAttributes<AutoResolveMergedAssembliesAttributeAttribute>()
                    .SelectMany(z => z.AssemblyNames)).ToArray();
                mergedAssemblies = mergedAssemblies.Except(assemblyNames).ToArray();
                
                if (mergedAssemblies.Length > 0)
                {
                    Debug.WriteLine($"{assembly.GetName().Name} {string.Join(",",mergedAssemblies)}");
                    System.AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                    {
                        
                        var assemblyName = new AssemblyName(args.Name);
                        if (mergedAssemblies.Contains(assemblyName.Name))
                        {
                            Debug.WriteLine($"Resolved {assemblyName.Name}");
                            return assembly;
                        }
                        return null;
                    };
                }
            }
        }
    }
}
