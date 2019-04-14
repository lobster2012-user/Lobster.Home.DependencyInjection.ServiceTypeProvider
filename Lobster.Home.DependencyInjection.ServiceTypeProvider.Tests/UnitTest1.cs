using ILMergeDynamic;
using Lobster.Home.DependencyInjection;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
#if !NETCOREAPP
#if FODY
            ILMergeDynamic.Program.CosturaUtility_Initialize();
#endif
#endif
            Test1Impl();
        }
        public void Test1Impl()
        {
#if NETCOREAPP
            Console.WriteLine(typeof(IServiceTypeProvider));
            FieldInfo writeCoreHook = typeof(Debug).GetField("s_WriteCore", BindingFlags.Static | BindingFlags.NonPublic);
            writeCoreHook.SetValue(null, new Action<string>((s) => Console.WriteLine(s)));
#else
            Debug.Listeners.Add(new ConsoleTraceListener());
#endif
            // First use our test logger to verify the output
            //var originalWriteCoreHook = writeCoreHook.GetValue(null);
            //writeCoreHook.SetValue(null, new Action<string>(WriteLogger.s_instance.WriteCore));
            //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            //netcore3.0 SetProvider(DebugProvider provider)

            var assemblies = new AssemblyLoaderBuilder()
                                   .UseLoadedAssemblies()
                                   .Directories(System.IO.Path.GetDirectoryName(
                                       typeof(Tests).Assembly.Location))
                                   .Load();
#if !FODY
            ILMerge.AutoResolveMergedAssemblies.AutoResolverInstaller.EnsureInstalled();
#endif
            assemblies = assemblies
                                    .Where(z => !z.GetName()
                                        .Name.StartsWith("nunit", StringComparison.OrdinalIgnoreCase))
                                    .ToArray();

            var provider = new PluginServiceTypeProvider(assemblies);
            var types = provider.GetServiceTypes<BaseClass>().ToArray();
            Console.WriteLine();
            Console.WriteLine(string.Join("\r\n", types.Select(z => z.FullName)));
            Assert.AreEqual(1, types.Length);
            Assert.AreEqual("DerivedClass", types[0].Name);
        }
    }
}