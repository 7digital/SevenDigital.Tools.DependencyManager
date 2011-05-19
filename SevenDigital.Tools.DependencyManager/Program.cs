using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DependencyManager;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager
{
	public class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("usage: SevenDigital.Tools.DependencyManager.exe pathContainingAssemblies startingassembly.dll");
				return;
			}

            var outputPath = args[0];
			var assembly = new WrappedAssembly(Assembly.LoadFrom(Path.Combine(outputPath, args[1])));

			var finder = new DependencyFinder(new AssemblyLoader());
			var dependencies = finder.AnalyseAssembly(assembly, outputPath);

			var logger = new Logger();
			PrintSummary(logger, dependencies);
			logger.ReadLine();
		}

		private static void PrintSummary(ILog logger, IEnumerable<Dependency> dependencies)
		{
			PrintHeading(logger, "Count of assemblies referenced");
			var list = dependencies
				.GroupBy(x => x.ReferencedAssembly.Name + "_" + x.ReferencedAssembly.Version)
				.ToList();
				
			foreach (IGrouping<string, Dependency> key in list)
			{
				logger.WriteLine(key.Key.PadRight(50) + key.Count());
			}

			PrintHeading(logger, "Incorrect Dependencies Summary");

			foreach(var dependency in dependencies) {
				PrintDependencySummary(logger, dependency);
			}
		}

		private static void PrintHeading(ILog logger, string text) {
			logger.WriteLine("------------------------------------------");
			logger.WriteLine(text);
			logger.WriteLine("------------------------------------------");
		}

		private static void PrintDependencySummary(ILog logger, Dependency dependency) {

			if (dependency.IsConflicted) {

				if (dependency.IsMajorConflict) {
					logger.WriteLine("***** Major Revision Conflict ***** ");
				}

				logger.WriteLine(@"{1} is referencing assembly {2}{0}  Version being referenced {3}{0}  Version in Lib {4}{0}",
					Environment.NewLine,
					 dependency.ReferencingAssembly.Name,
					 dependency.ReferencedAssembly.Name,
					 dependency.ReferencedAssembly.Version,
					 dependency.LibAssembly.Version);
			}
		}
	}
}
