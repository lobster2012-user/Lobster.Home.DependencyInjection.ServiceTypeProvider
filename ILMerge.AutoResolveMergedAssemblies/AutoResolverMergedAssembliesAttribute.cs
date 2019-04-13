using ILMerge.AutoResolveMergedAssemblies;
using System;

namespace ILMerge.AutoResolveMergedAssemblies
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AutoResolverMergedAssembliesAttribute : Attribute
    {
        public AutoResolverMergedAssembliesAttribute(params string[] assemblyNames)
        {
            AssemblyNames = assemblyNames;
        }
        public string[] AssemblyNames { get; }
    }
}
