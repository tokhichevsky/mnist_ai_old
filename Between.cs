using System;
namespace AIEasy
{
	public class Between
	{
		public Digit[,] Digits;
		public int nFilesFull;
		public int[] nFiles;
		public Weight Weights1;
		public Weight Weights2;
		public Between(Digit[,] Digits, int nFilesFull, int[] nFiles, Weight Weights1, Weight Weights2)
		{
			this.Digits = Digits;
			this.nFilesFull = nFilesFull;
			this.nFiles = nFiles;
			this.Weights1 = Weights1;
			this.Weights2 = Weights2;
		}

	}
}
