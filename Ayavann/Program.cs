using System;

namespace Ayavann
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new AyavannGame())
				game.Run();
		}
	}
}
