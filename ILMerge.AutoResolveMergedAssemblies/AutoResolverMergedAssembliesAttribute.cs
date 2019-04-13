using System;
using System.Collections.Generic;
using System.Text;

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
