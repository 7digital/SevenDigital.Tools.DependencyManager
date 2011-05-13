using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyManager;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests
{
	[TestFixture]
	public class DependencyFinderTests
	{
		private IAssemblyLoader _assemblyLoader;
		private IAssembly _mockAssemblyA;
		private IAssembly _mockAssemblyB;
		private IAssembly _mockAssemblyC;
		private IAssembly _mockAssemblyD;
		private IAssembly _mockAssemblyE;
		private AssemblyName _assemblyNameA;
		private AssemblyName _assemblyNameB;
		private AssemblyName _assemblyNameC;
		private AssemblyName _assemblyNameD;
		private AssemblyName _assemblyNameE;
		private const string PATH = "PATH";

		[SetUp]
		public void setup() {
			
			_assemblyNameA = new AssemblyName("SevenDigital.AssemblyA");
			_assemblyNameB = new AssemblyName("SevenDigital.AssemblyB");
			_assemblyNameC = new AssemblyName("SevenDigital.AssemblyC");
			_assemblyNameD = new AssemblyName("SevenDigital.AssemblyD");
			_assemblyNameE = new AssemblyName("SevenDigital.AssemblyE");

			var listA = new List<AssemblyName> { _assemblyNameB, _assemblyNameC, _assemblyNameE };
			var listB = new List<AssemblyName> { _assemblyNameC, _assemblyNameD };
			var listC = new List<AssemblyName> { _assemblyNameE };
			var listD = new List<AssemblyName> { _assemblyNameE };

			_mockAssemblyA = GenerateMockAssembly(_assemblyNameA, listA);
			_mockAssemblyB = GenerateMockAssembly(_assemblyNameB, listB);
			_mockAssemblyC = GenerateMockAssembly(_assemblyNameC, listC);
			_mockAssemblyD = GenerateMockAssembly(_assemblyNameD, listD);
			_mockAssemblyE = GenerateMockAssembly(_assemblyNameE, new List<AssemblyName>());

			_assemblyLoader = MockRepository.GenerateStub<IAssemblyLoader>();
			_assemblyLoader.Stub(x => x.LoadAssembly(PATH + _assemblyNameA + ".dll")).Return(_mockAssemblyA);
			_assemblyLoader.Stub(x => x.LoadAssembly(PATH + _assemblyNameB + ".dll")).Return(_mockAssemblyB);
			_assemblyLoader.Stub(x => x.LoadAssembly(PATH + _assemblyNameC + ".dll")).Return(_mockAssemblyC);
			_assemblyLoader.Stub(x => x.LoadAssembly(PATH + _assemblyNameD + ".dll")).Return(_mockAssemblyD);
			_assemblyLoader.Stub(x => x.LoadAssembly(PATH + _assemblyNameE + ".dll")).Return(_mockAssemblyE);
			
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_at_the_top_of_the_chain() {
			var dependencyReporter = new DependencyFinder(_assemblyLoader);
			
			var dependencies = dependencyReporter.AnalyseAssembly(_mockAssemblyA, PATH);
			Assert.That(dependencies.Count, Is.EqualTo(7));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameB).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameC).Count(), Is.EqualTo(2));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameD).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameE).Count(), Is.EqualTo(3));
		}


		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_in_the_middle_of_the_chain()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader);

			var dependencies = dependencyReporter.AnalyseAssembly(_mockAssemblyB, PATH);
			Assert.That(dependencies.Count, Is.EqualTo(4));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameC).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameD).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.ReferencedAssembly == _assemblyNameE).Count(), Is.EqualTo(2));
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_when_assembly_has_only_one_dependency()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader);

			var dependencies = dependencyReporter.AnalyseAssembly(_mockAssemblyC, PATH);
			Assert.That(dependencies.Count, Is.EqualTo(1));
			Assert.That(dependencies[0].ReferencedAssembly, Is.EqualTo(_assemblyNameE));
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_when_assembly_has_no_dependencies()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader);

			var dependencies = dependencyReporter.AnalyseAssembly(_mockAssemblyE, PATH);
			Assert.That(dependencies.Count, Is.EqualTo(0));
		}

		private IAssembly GenerateMockAssembly(AssemblyName assemblyName, List<AssemblyName> dependencyList) {
			var mockAssembly = MockRepository.GenerateStub<IAssembly>();
			mockAssembly.Stub(x => x.GetName()).Return(assemblyName);
			mockAssembly.Stub(x => x.GetReferencedAssemblies()).Return(dependencyList);
			return mockAssembly;
		}
	}
}