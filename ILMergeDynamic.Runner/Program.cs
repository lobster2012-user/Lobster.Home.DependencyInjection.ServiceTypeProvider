using Lobster.Home.DependencyInjection;
using System;
using System.Linq;

#if !FODY
using ILMerge.AutoResolveMergedAssemblies;
[assembly: AutoResolveMergedAssembliesAttribute("ILMerge.AutoResolveMergedAssemblies")]
[assembly: AutoResolveMergedAssembliesAttribute("ILMergeDynamic.BaseModule")]
[assembly: AutoResolveMergedAssembliesAttribute("Lobster.Home.DependencyInjection.ServiceTypeProvider")]
#endif
namespace ILMergeDynamic
{

    public class Program
    {
#if !NETCOREAPP
#if FODY
        public static void CosturaUtility_Initialize()
        {
            CosturaUtility.Initialize();
        }
#endif
#endif
        static void Main(string[] args)
        {
#if FODY
#if !NETCOREAPP
            CosturaUtility.Initialize();
#endif
#else

            AutoResolverInstaller.EnsureInstalled();
#endif

            var assemblies = new AssemblyLoaderBuilder()
                                      .UseLoadedAssemblies()
                                      .Directories(AppDomain.CurrentDomain.BaseDirectory)
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
