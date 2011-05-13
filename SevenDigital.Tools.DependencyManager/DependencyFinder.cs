using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SevenDigital.Tools.DependencyManager;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace DependencyManager
{
	public class DependencyFinder
	{
		private readonly IAssemblyLoader _assemblyLoader;
		private readonly List<Dependency> _dependencies;

		public DependencyFinder(IAssemblyLoader assemblyLoader) {
			_assemblyLoader = assemblyLoader;
			_dependencies = new List<Dependency>();
		}

		public List<Dependency> AnalyseAssembly(IAssembly currentAssembly, string outputPath) {
			var assemblyName = currentAssembly.GetName();
			
            if (!IsAlreadyAnalysed(assemblyName)) {

                foreach (AssemblyName referencedName in currentAssembly.GetReferencedAssemblies())
                {
                    var assemblyInLib = _assemblyLoader.LoadAssembly(outputPath + referencedName.Name + ".dll");
                    if (assemblyInLib != null)
                    {
                        AssemblyName assemblyInLibName = assemblyInLib.GetName();
                        AddToDependencyList(assemblyName, referencedName, assemblyInLibName);
                        AnalyseAssembly(assemblyInLib, outputPath);
                    }
                }
			}
			return _dependencies;
		}

		private void AddToDependencyList(AssemblyName currentAssemblyName, AssemblyName referencedName, AssemblyName assemblyInLibName) {
			
			var dependency = new Dependency
         	{
         		ReferencedAssembly = referencedName,
         		ReferencingAssembly = currentAssemblyName,
         		LibAssembly = assemblyInLibName
         	};

			_dependencies.Add(dependency);
		}

		private bool IsAlreadyAnalysed(AssemblyName currentAssemblyName) {
			return _dependencies
				.Where(x => x.ReferencingAssembly.FullName == currentAssemblyName.FullName)
				.Count() > 0;
		}
	}
}