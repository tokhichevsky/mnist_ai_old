using System;
namespace AIEasy
{
	public class MathAI
	{
		public static int Max(int[] n)
		{
			int result = 0;
			for (int i = 0; i < n.GetLength(0); i++)
				if (n[i] > result)
					result = n[i];
			return result;
		}
		public static double Max(double[] n)
		{
			double result = 0;
			for (int i = 0; i < n.GetLength(0); i++)
				if (n[i] > result)
					result = n[i];
			return result;
		}
		public static int MaxI(double[] n)
		{
			int result = 0;
			for (int i = 0; i < n.GetLength(0); i++)
				if (n[i] > n[result])
					result = i;
			return result;
		}
		public static double GetRandom()
		{
			Random random = new Random();
			double value = random.NextDouble();
			return value;
		}
		public static int GetRandom(int a, int b)
		{
			Random random = new Random();
			int value = random.Next(a, b + 1);
			return value;
		}
		public static double Th(double x)
		{
			return Math.Tanh(x);
		}
		public static double derTh(double x)
		{
			return (1 + x) * (1 - x);
		}
		public static double Sigmoid(double x)
		{
			return 1 / (1 + Math.Exp(-x));
		}
		public static double derSigmoid(double x)
		{
			double sigmx = Sigmoid(x);
			return sigmx * (1 - sigmx);
		}
		public static double derSigmoid2(double sigmx)
		{
			//double sigmx = Sigmoid(x);
			return sigmx * (1 - sigmx);
		}
		public static double ReLU(double x)
		{
			if (x > 0) return x * x; else return 0;
		}
		public static double derReLU(double x)
		{
			if (x > 0) return 2 * x; else return 0;
		}
		public static double bSigmoid(double x)
		{
			return 2 / (1 + Math.Exp(-x)) - 1;
		}
		public static double derbSigmoid(double x)
		{
			return 0.5 * (1 + x) * (1 - x);
		}
	}
}
