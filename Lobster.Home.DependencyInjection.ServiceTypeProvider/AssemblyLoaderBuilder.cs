using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lobster.Home.DependencyInjection
{
    public class AssemblyLoaderBuilder
    {
        private readonly List<string> _directories = new List<string>();
        private readonly List<string> _files = new List<string>();
        private readonly List<Assembly> _assemblies = new List<Assembly>();
        private string _searchPattern = "*.dll";

        public AssemblyLoaderBuilder()
        {

        }
        public AssemblyLoaderBuilder SearchPattern(string searchPattern)
        {
            _searchPattern = searchPattern ?? throw new ArgumentNullException(nameof(searchPattern));
            return this;
        }
        public AssemblyLoaderBuilder Directories(params String[] directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException(nameof(directories));
            }

            _directories.AddRange(directories);
            return this;
        }
        public AssemblyLoaderBuilder Files(params String[] files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            _files.AddRange(files);
            return this;
        }
        public AssemblyLoaderBuilder UseLoadedAssemblies()
        {
            this.Assemblies(System.AppDomain.CurrentDomain.GetAssemblies());
            return this;
        }
        public AssemblyLoaderBuilder Assemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            _assemblies.AddRange(assemblies);
            return this;
        }
        public Assembly[] Load()
        {
            var assemblies = new List<Assembly>(_assemblies);
            var files = new List<string>(_files);
            files.AddRange(_directories.SelectMany(dir => Directory.GetFiles(dir, _searchPattern)));
            assemblies.AddRange(files.Select(file => Assembly.LoadFile(file)));
            return assemblies.ToArray();
        }
    }
}
