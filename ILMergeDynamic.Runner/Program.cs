using ILMerge.AutoResolveMergedAssemblies;
using Lobster.Home.DependencyInjection;
using System;
using System.Linq;


[assembly: ILMerge.AutoResolveMergedAssemblies.AutoResolverMergedAssemblies("ILMergeDynamic.BaseModule")]
[assembly: ILMerge.AutoResolveMergedAssemblies.AutoResolverMergedAssemblies("Lobster.Home.DependencyInjection.ServiceTypeProvider")]

namespace ILMergeDynamic
{

    class Program
    {
        static void Main(string[] args)
        {
            AutoResolverInstaller.EnsureInstalled();

            var assemblies = new AssemblyLoaderBuilder()
                                      .UseLoadedAssemblies()
                                      .Directories(System.AppDomain.CurrentDomain.BaseDirectory)
                                      .Load();
            foreach(var a in assemblies)
            {
                Console.WriteLine(a.FullName);
            }
            var serviceProvider = new PluginServiceTypeProvider(assemblies);
            Console.WriteLine("bs :{0}", typeof(BaseClass).FullName);
            AppDomain.CurrentDomain.FirstChanceException += (a, b) =>
            {
                Console.WriteLine(b.Exception);
                if(b.Exception is System.Reflection.ReflectionTypeLoadException rte)
                {
                    foreach(var ex in rte.LoaderExceptions)
                    {
                        Console.WriteLine(ex);
                    }
                }
            };
            var types = serviceProvider.GetServiceTypes<BaseClass>().ToArray();
            Console.WriteLine(Activator.CreateInstance(types[0]));
        }
    }
}
