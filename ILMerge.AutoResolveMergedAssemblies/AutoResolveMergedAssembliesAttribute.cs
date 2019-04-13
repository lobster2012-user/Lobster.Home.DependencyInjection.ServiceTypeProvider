using ILMerge.AutoResolveMergedAssemblies;
using System;

namespace ILMerge.AutoResolveMergedAssemblies
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AutoResolveMergedAssembliesAttributeAttribute : Attribute
    {
        public AutoResolveMergedAssembliesAttributeAttribute(params string[] assemblyNames)
        {
            AssemblyNames = assemblyNames;
        }
        public string[] AssemblyNames { get; }
    }
}
