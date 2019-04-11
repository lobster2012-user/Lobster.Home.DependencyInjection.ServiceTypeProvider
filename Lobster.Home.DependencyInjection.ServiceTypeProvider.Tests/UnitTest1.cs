using Lobster.Home.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public abstract class IAC
    {

    }
    public abstract class AC : IAC
    {

    }
    public abstract class AC2 : AC
    {

    }
    public class DC : AC2
    {

    }
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var provider = new PluginServiceTypeProvider(new AssemblyLoaderBuilder()
                                    .UseLoadedAssemblies()
                                    .Load());
            {
                var types = provider.GetServiceTypes(typeof(AC)).ToArray();
                Console.WriteLine(string.Join("\r\n", types.Select(z => z.FullName)));
                Assert.AreEqual(1, types.Length);
                Assert.AreEqual(typeof(DC), types[0]);
            }
            {
                var types = provider.GetServiceTypes<IAC>().ToArray();
                Console.WriteLine();
                Console.WriteLine(string.Join("\r\n", types.Select(z => z.FullName)));
                Assert.AreEqual(typeof(DC), types[0]);
            }
        }
    }
}