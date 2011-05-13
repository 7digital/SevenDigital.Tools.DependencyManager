using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests
{
    [TestFixture]
    public class AssemblyLoaderTests
    {
        [Test]
        public void Loader_returns_null_if_assembly_is_not_found_at_path()
        {
            var assemblyLoader = new AssemblyLoader();
            var assembly = assemblyLoader.LoadAssembly(@"c:\a\non\existent\path\assembly.dll");

            Assert.That(assembly, Is.Null);
        }

        [Test]
        public void Loader_returns_assembly_if_assembly_is_found_at_path()
        {
            var assemblyLoader = new AssemblyLoader();

            string fullPath = Path.GetFullPath(@"assemblies\log4net.dll");
            var assembly = assemblyLoader.LoadAssembly(fullPath);

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.GetName().Name, Is.EqualTo("log4net"));
        }
    }
}