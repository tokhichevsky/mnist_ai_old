using System;
namespace AIEasy
{
	public class SecondLayer
	{
		public double[] Result;
		public int ResultLength;
		public double ResultAccuracy;
		public SecondLayer(FirstLayer Layer, int nElements, Weight Weights, int Answer)
		{
			Result = new double[nElements];
			ResultLength = nElements;
			for (int x = 0; x < nElements; x++)
			{
				if (Result[x] != 0) Result[x] = 0;
				for (int y = 0; y < Layer.HiddenLength; y++)
				{
					Result[x] += Layer.Hidden[y] * Weights.Body[y, x];
				}
				Result[x] = MathAI.Sigmoid(Result[x]);
			}
			double sumResult = 0;
			for (int i = 0; i < nElements; i++)
				sumResult += Result[i];
			ResultAccuracy = Result[Answer] / sumResult;
		}
		public SecondLayer(FirstLayer Layer, int nElements, Weight Weights)
		{
			Result = new double[nElements];
			ResultLength = nElements;
			for (int x = 0; x < nElements; x++)
			{
				if (Result[x] != 0) Result[x] = 0;
				for (int y = 0; y < Layer.HiddenLength; y++)
				{
					Result[x] += Layer.Hidden[y] * Weights.Body[y, x];
				}
				Result[x] = MathAI.Sigmoid(Result[x]);
			}
		}
	}
}
