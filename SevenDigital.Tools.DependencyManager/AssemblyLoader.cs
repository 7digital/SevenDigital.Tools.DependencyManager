using System.IO;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager
{
	public class AssemblyLoader : IAssemblyLoader
	{
        public IAssembly LoadAssembly(string fullyQualifiedDll)
        {
            try
            {
                var assembly = new WrappedAssembly(Assembly.LoadFrom(fullyQualifiedDll));
                return assembly;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
	}
}