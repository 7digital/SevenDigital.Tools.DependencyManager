using System.Reflection;

namespace SevenDigital.Tools.DependencyManager
{
	public class Dependency
	{
		public AssemblyName ReferencedAssembly { get; set; }
		public AssemblyName ReferencingAssembly { get; set; }
		public AssemblyName LibAssembly { get; set; }
		
		public bool IsConflicted
		{
			get { return LibAssembly.Version != ReferencedAssembly.Version; }
		}

		public bool IsMajorConflict {
			get 
			{
				return !(ReferencedAssembly.Version.Major == LibAssembly.Version.Major
						 && ReferencedAssembly.Version.Minor == LibAssembly.Version.Minor);
			}
		}

	}
}