using System;
namespace AIEasy
{
	public class Weight3
	{
		public int Height;
		public int Width;
		public int Deep;
		public double[,,] Body;
		public Weight3(int Width, int Height, int Deep)
		{
			this.Width = Width;
			this.Height = Height;
			this.Deep = Deep;
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					for (int z = 0; z < Deep; z++)
						Body[x, y, z] = MathAI.GetRandom() * 0.25 + 0.1;
		}
	}
}
