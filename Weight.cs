using System;
using System.IO;

namespace AIEasy
{
	public class Weight
	{
		public int Height;
		public int Width;
		public double[,] Body;
		public static double fError;
		public Weight(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
			Body = new double[Width, Height];
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					Body[x, y] = MathAI.GetRandom() * 0.15 + 0.1;
		}
		public void Save(string filename)
		{
			StreamWriter outputFile = new StreamWriter(@filename,false);
			for (int y = 0; y < Height; y++)
			{
				if (y != 0) outputFile.WriteLine();
				for (int x = 0; x < Width; x++)
				{
					outputFile.Write(Body[x, y]);
					if (x != Width - 1) outputFile.Write(" ");
				}
			}
			outputFile.Close();
		}
		public void SaveTest(string filename)
		{
			StreamWriter outputFile = new StreamWriter(filename, false);
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					outputFile.Write(Body[x, y]);
					if (x != Width*Height - 1) outputFile.Write(" ");
				}
			}
		}
		public Weight(string filename, int width)
		{
			string[] lines = File.ReadAllLines(@filename);
			double[,] num = new double[width, lines.Length];
			//Console.WriteLine(lines.Length);
			//double[] d = File.ReadAllLines("file.txt").Select(double.Parse).to;
			for (int i = 0; i < lines.Length; i++)
			{
				string[] temp = lines[i].Split(' ');
				for (int j = 0; j < temp.Length; j++)
				{
					num[j, i] = double.Parse(temp[j]);
					//Console.WriteLine(num[j, i]);
				}
			}
			//return num;
		}
		public Weight(string filename)
		{
			string[] lines = File.ReadAllLines(@filename);
			Body = new double[lines[0].Split(' ').GetLength(0), lines.Length];
			Width = Body.GetLength(0);
			Height = Body.GetLength(1);
			//Console.WriteLine(lines.Length);
			//double[] d = File.ReadAllLines("file.txt").Select(double.Parse).to;
			for (int i = 0; i < Height; i++)
			{
				string[] temp = lines[i].Split(' ');
				for (int j = 0; j < Width; j++)
				{
					Body[j, i] = double.Parse(temp[j]);
					//Console.WriteLine(num[j, i]);
				}
			}
		}
		public static void Correct(FirstLayer Layer1, SecondLayer Layer2, Weight Weights1, Weight Weights2, int curAnswer, int nElements, int dimHiddenLayer, double LearningRate)
		{
			double[] lError = new double[nElements];
			double[] deltaWeights = new double[nElements];
			double fullResultWork = 0;
			for (int i = 0; i < Layer2.ResultLength; i++) fullResultWork += Layer2.Result[i];
			for (int i = 0; i < nElements; i++)
				if (i != curAnswer)
					lError[i] = Layer2.Result[i];
				else lError[i] = (Layer2.Result[i] - 1);
			for (int i = 0; i < nElements; i++)
				deltaWeights[i] = lError[i] * MathAI.derSigmoid2(Layer2.Result[i]);

			double[] lError2 = new double[dimHiddenLayer];
			double[] deltaWeights2 = new double[dimHiddenLayer];
			for (int i = 0; i < dimHiddenLayer; i++)
			{
				lError2[i] = 0;
				for (int j = 0; j < nElements; j++)
					lError2[i] += Weights2.Body[i, j] * deltaWeights[j];
			}
			for (int i = 0; i < dimHiddenLayer; i++)
				deltaWeights2[i] = lError2[i] * MathAI.derSigmoid2(Layer1.Hidden[i]);
			for (int x = 0; x < Weights2.Width; x++)
				for (int y = 0; y < Weights2.Height; y++)
					//Weights2.Body[x, y] -= Layer1.Hidden[x] * deltaWeights[y] * LearningRate;
					Weights2.Body[x, y] -= Layer1.Hidden[x] * deltaWeights[y] * LearningRate;
			for (int x = 0; x < Weights1.Width; x++)
				for (int y = 0; y < Weights1.Height; y++)
					Weights1.Body[x, y] -= Layer1.curLayer[x] * deltaWeights2[y] * LearningRate;
			//aResult[(k - 1) * nButch + Epoch - 1] = Result[answer] / fullResultWork;

			fError = 0;
			for (int i = 0; i < lError.GetLength(0); i++)
				fError += lError[i] * lError[i];
			fError /= 2;

		}

	}
}
