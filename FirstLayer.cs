using System;
namespace AIEasy
{
	public class FirstLayer
	{
		public double[] Hidden;
		public int HiddenLength;
		public double[] curLayer;
		public FirstLayer(Digit Enter, int dimHiddenLayer, Weight Weights)
		{
			int layerlength = Enter.pixelWidth * Enter.pixelHeight;
			curLayer = Enter.LineMatrix;

			Hidden = new double[dimHiddenLayer];
			HiddenLength = dimHiddenLayer;

			//layer[0] = 1;
			for (int x = 0; x < dimHiddenLayer; x++)
			{
				if (Hidden[x] != 0) Hidden[x] = 0;
				for (int y = 0; y < layerlength; y++)
				{
					Hidden[x] += curLayer[y] * Weights.Body[y, x];
				}
				Hidden[x] = MathAI.Sigmoid(Hidden[x]);
			}
			//return hidden;
		}
	}
}
