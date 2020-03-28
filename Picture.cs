using System;
using System.Drawing;
using System.IO;

namespace AIEasy
{
	public class Picture
	{
		public Bitmap Image;
		//public int d;
		public int pixelWidth { get; }
		public int pixelHeight { get; }
		public Picture(string fileSrc)
		{
			Image = LoadBitmap(fileSrc);
			pixelWidth = Image.Width;
			pixelHeight = Image.Height;
		}
		private static Bitmap LoadBitmap(string fileScr)
		{
			using (FileStream fs = new FileStream(@fileScr, FileMode.Open, FileAccess.Read, FileShare.Read))
				return new Bitmap(fs);
		}
	}
	public class Digit : Picture
	{
		//public byte[,,] ByteMatrix;
		//public double[,] DoubleMatrix;
		public double[] LineMatrix;
		public Digit(string fileScr) : base(fileScr)
		{
			double[,] DoubleMatrix = ByteToNormal(BitmapToByteRgbNaive(Image), pixelWidth, pixelHeight);
			LineMatrix = new double[DoubleMatrix.GetLength(0) * DoubleMatrix.GetLength(1)];
			//HiddenLength = dimHiddenLayer;
			for (int y = 0; y < DoubleMatrix.GetLength(1); y++)
				for (int x = 0; x < DoubleMatrix.GetLength(0); x++)
				{
					if (LineMatrix[x + y * (DoubleMatrix.GetLength(0))] != 0) LineMatrix[x + y * DoubleMatrix.GetLength(0)] = 0;
					LineMatrix[x + y * DoubleMatrix.GetLength(0)] = DoubleMatrix[x, y];

				}
			DoubleMatrix = null;
		}
		private byte[,,] BitmapToByteRgbNaive(Bitmap bmp)
		{
			int width = bmp.Width,
				height = bmp.Height;
			byte[,,] res = new byte[width, height, 3];
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					Color color = bmp.GetPixel(x, y);
					res[x, y, 0] = color.R;
					res[x, y, 1] = color.G;
					res[x, y, 2] = color.B;
				}
			}
			return res;
		}

		private double[,] ByteToNormal(byte[,,] bmp, int width, int height)
		{
			double[,] rgbmat = new double[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
				{
					rgbmat[x, y] = (Convert.ToDouble(bmp[x, y, 0]) + Convert.ToDouble(bmp[x, y, 1]) + Convert.ToDouble(bmp[x, y, 2])) / (255d * 3d);
					//if (rgbmat[x, y] < 0.5) rgbmat[x, y] = 0; else rgbmat[x, y] = 1;
				}
			return rgbmat;
		}
	}
}
