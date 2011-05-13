using System.Collections;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager
{
    public class WrappedAssembly : IAssembly
    {
        private readonly Assembly _innerAssembly;

        public WrappedAssembly(Assembly innerAssembly)
        {
            _innerAssembly = innerAssembly;
        }

        public AssemblyName GetName()
        {
            return _innerAssembly.GetName();
        }

        public IEnumerable GetReferencedAssemblies()
        {
            return _innerAssembly.GetReferencedAssemblies();
        }
    }
}