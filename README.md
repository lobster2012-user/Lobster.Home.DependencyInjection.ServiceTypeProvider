# IServiceTypeProvider
# ILMerge.AutoResolveMergedAssemblies

* IServiceTypeProvider  - ServiceProvider for Types. 
* ILMerge.AutoResolveMergedAssemblies
- Example of using [**ILMerge**](https://github.com/dotnet/ILMerge) for dynamically loaded assemblies.
  You need use attributes in main assembly
```csharp
  
  [assembly: AutoResolveMergedAssembliesAttribute(nameof(ILMerge.AutoResolveMergedAssemblies))]
  [assembly: AutoResolveMergedAssembliesAttribute("ILMergeDynamic.BaseModule")]
  [assembly: AutoResolveMergedAssembliesAttribute("Lobster.Home.DependencyInjection.ServiceTypeProvider")]
```

Under the hood

```csharp

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
                var mergedAssemblies = new HashSet<string>(assembly.GetCustomAttributes<AutoResolveMergedAssembliesAttributeAttribute>()
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

```
