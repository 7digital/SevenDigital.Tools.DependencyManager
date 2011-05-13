namespace SevenDigital.Tools.DependencyManager.Interfaces
{
	public interface IAssemblyLoader
	{
		IAssembly LoadAssembly(string fullyQualifiedDll);
	}
}