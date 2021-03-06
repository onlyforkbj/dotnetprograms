using Generate.Lib.Personal;

namespace Generate.Commands
{
	public class PnrCommand : IGeneratorCommand
	{
		public string Name { get { return "pnr"; } }

		public PnrCommand ()
		{
		}

		public string Generate()
		{
			return new SsnGenerator().GeneratePnr();
		}
	}
}

