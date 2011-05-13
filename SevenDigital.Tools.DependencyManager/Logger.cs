using System;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager
{
	public class Logger : ILog
	{
		public void Write(string line)
		{
			Console.Write(line);
		}

		public void WriteLine(string line, params object[] args)
		{
			Console.WriteLine(line, args);
		}

		public void ReadLine() {
			Console.ReadLine();
		}
	}
}