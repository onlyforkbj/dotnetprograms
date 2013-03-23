using System;
using Generate.Lib;
using System.Linq;

namespace Generate
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var command = args.Length == 0 ? "unknown" : args [0];
			Console.WriteLine (Run(command));
		}

		private static string Usage()
		{
			return string.Format("Usage: pnr");
		}

		private static string Run(string command)
		{
			switch (command)
			{
				case "pnr":
					return new PnrCommand().Generate();
				default:
					return "Unknown command " + command;
			}
		}
	}
}