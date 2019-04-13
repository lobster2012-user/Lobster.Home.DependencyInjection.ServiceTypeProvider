using ILMergeDynamic;
using Lobster.Home.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            ILMerge.AutoResolveMergedAssemblies.AutoResolverInstaller.EnsureInstalled();
            var assemblies = new AssemblyLoaderBuilder()
                                    .UseLoadedAssemblies()
                                    .Directories(System.IO.Path.GetDirectoryName(
                                        typeof(Tests).Assembly.Location))
                                    .Load();
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