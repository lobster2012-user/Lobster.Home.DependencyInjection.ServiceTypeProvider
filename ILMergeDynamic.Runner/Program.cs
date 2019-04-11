using Lobster.Home.DependencyInjection;
using System;
using System.Linq;

namespace ILMergeDynamic
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new PluginServiceTypeProvider(new AssemblyLoaderBuilder()
                                      .UseLoadedAssemblies()
                                      .Directories("plugins")
                                      .Load());
            Console.WriteLine("bs :{0}", typeof(BaseClass).FullName);
            System.AppDomain.CurrentDomain.FirstChanceException += (a, b) =>
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
            System.AppDomain.CurrentDomain.AssemblyResolve += (a, b) =>
            {

                Console.WriteLine("assemblyresolve: {0}",b.Name);
                if (b.Name.StartsWith("ILMergeDynamic.BaseModule"))
                {
                    Console.WriteLine("Return this");
                    return typeof(Program).Assembly;
                }
                return null;
            };
            System.AppDomain.CurrentDomain.TypeResolve += (a, b) =>
            {
                Console.WriteLine("typeresolve: {0}", b.Name);
                return null;
            };
            var types = serviceProvider.GetServiceTypes<BaseClass>().ToArray();
            foreach (var o in types)
            {
               // Console.WriteLine(o.FullName);
            }
            Console.WriteLine(Activator.CreateInstance(types[0]));
        }
    }
}
