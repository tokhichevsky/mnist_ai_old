using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Threading;

namespace AIEasy
{
	class Program
	{
		public static int dimHiddenLayer = 40;
		public static int nElements = 10; //Количество ответов (Цифр)
		public static double E = 0.95; //Точность обучения
		public static double E2 = 0.001;
		public static double LearningRate = 0.1; //Скорость обучения
												  //Размерность изображений цифр
		public static int sourcesize = 28;

		public static double averageEpochAccuracy = 0;
		public static int Epoch = 0;
		public static int iteration;
		public static double testdel = 1;
		public static bool ThreadSave = false;
		public static string MainFolder;
		static object locker = new object();
		//Подсчет числа изображений цифр в каждой папке(вариантов каждой цифры)
		public static int HowManyFiles(string s)
		{
			return System.IO.Directory.GetFiles(@s, "*.png").Length;
		}
		public static void Teach(object A)
		{
			Between B = (Between)A;
			do
			{
				int curAnswer = MathAI.GetRandom(0, nElements - 1);
				//Console.WriteLine(curAnswer + " " + B.nFiles[curAnswer]);
				int curDigitVar = MathAI.GetRandom(0, B.nFiles[curAnswer] - 1);
				Digit curDigit = B.Digits[curAnswer, curDigitVar];
				FirstLayer Layer1 = new FirstLayer(curDigit, dimHiddenLayer, B.Weights1);
				SecondLayer Layer2 = new SecondLayer(Layer1, nElements, B.Weights2, curAnswer);
				lock (locker)
				{
					iteration++;
					Epoch = iteration / B.nFilesFull + 1;
					if ((Convert.ToDouble(iteration) - Convert.ToDouble((Epoch - 1) * B.nFilesFull)) != 0) testdel = (Convert.ToDouble(iteration) - Convert.ToDouble((Epoch - 1) * B.nFilesFull));

					if (iteration % 2000 == 0)
					{
						Console.WriteLine("Epoch #" + Epoch + " | Theard #" + Thread.CurrentThread.Name);
						Console.WriteLine("Answer = " + (curAnswer));
						Console.WriteLine("Full Error = " + (Weight.fError));
						Console.WriteLine("Accuration = " + Layer2.ResultAccuracy * 100 + "%");
						Console.WriteLine("Epoch Accuration = " + averageEpochAccuracy / (testdel) * 100 + "%");
					}
					Weight.Correct(Layer1, Layer2, B.Weights1, B.Weights2, curAnswer, nElements, dimHiddenLayer, LearningRate);
					if (iteration % B.nFilesFull == 0) averageEpochAccuracy = 0;
					averageEpochAccuracy += Layer2.ResultAccuracy;
				}
			} while (averageEpochAccuracy / (testdel)<E || (Convert.ToDouble(iteration) - Convert.ToDouble((Epoch - 1) * B.nFilesFull))<B.nFilesFull*0.9);
			lock (locker)
			{
				if (!ThreadSave)
				{
					ThreadSave = true;
					B.Weights1.Save(MainFolder + @"\weights1.txt");
					B.Weights2.Save(MainFolder + @"\weights2.txt");
				}
			}
		}
		public static void Main(string[] args)
		{
			string firstquestion;
			string TrainingFolder; //Основная папка с изображениями для обучения
			if (Directory.Exists(@"\mnist\")) MainFolder = @"";
			else MainFolder = @"D:";
			Console.WriteLine("DataSet[1] Or Check[2]?");
			firstquestion = Console.ReadLine();
			if (firstquestion[0] == '1')
			{
				do
				{
					Console.WriteLine(@"Address of images (Folder): " + @MainFolder + @"\mnist\mnist_png\training");
					TrainingFolder = @MainFolder + @"\mnist\mnist_png\training";
					Console.WriteLine(@"Starting...");
					int[] nFiles = new int[nElements];
					List<string>[] filesDigit = new List<string>[nElements];
					int nFilesFull = 0;
					for (int i = 0; i < nElements; i++)
					{
						nFiles[i] = HowManyFiles(@TrainingFolder + @"\" + i + @"\");
						Console.WriteLine(@"In " + @TrainingFolder + @"\" + i + @"\ " + nFiles[i] + " files.");
					}
					for (int i = 0; i < 10; i++) nFilesFull += nFiles[i];
					Console.WriteLine(@"Total: " + nFilesFull + " files.");
					for (int i = 0; i < nElements; i++)
						filesDigit[i] = Directory.GetFiles(@TrainingFolder + @"\" + i + @"\", "*.png").ToList<string>();
					//Заносим все файлы в массив
					Digit[,] Digits = new Digit[nElements, MathAI.Max(nFiles)];
					for (int i = 0; i < Digits.GetLength(0); i++)
						for (int k = 0; k < nFiles[i]; k++)
							Digits[i, k] = new Digit(filesDigit[i].ElementAt(k));
					filesDigit = null;
					Console.WriteLine("Digits downloaded.");
					Weight Weights1 = new Weight(sourcesize * sourcesize, dimHiddenLayer);
					Weight Weights2 = new Weight(dimHiddenLayer, nElements);
					//StreamWriter graf = new StreamWriter(@"D:\graf.txt", false);
					Console.WriteLine("Weights created.");
					Between A = new Between(Digits, nFilesFull, nFiles, Weights1, Weights2);
					Thread t1 = new Thread(new ParameterizedThreadStart(Teach));
					Thread t2 = new Thread(new ParameterizedThreadStart(Teach));
					Thread t3 = new Thread(new ParameterizedThreadStart(Teach));
					Thread t4 = new Thread(new ParameterizedThreadStart(Teach));
					t1.Name = "1";
					t2.Name = "2";
					t3.Name = "3";
					t4.Name = "4";
					Console.WriteLine("Threads starting...");
					t1.Start(A);
					t2.Start(A);
					t3.Start(A);
					t4.Start(A);
					t1.Join();
					t2.Join();
					t3.Join();
					t4.Join();
					Console.WriteLine("Teaching end.");
					Console.WriteLine();
					Digits = null;

					Console.WriteLine(@"Address of test images (Folder):"+ MainFolder + @"\mnist\mnist_png\testing");
					string TestingFolder = MainFolder + @"\mnist\mnist_png\testing";
					Console.WriteLine(@"Starting testing...");
					int[] nFilesT = new int[nElements];
					for (int i = 0; i < nElements; i++)
					{
						nFilesT[i] = HowManyFiles(@TestingFolder + @"\" + i + @"\");
						Console.WriteLine(@"In " + @TestingFolder + @"\" + i + @"\ " + nFilesT[i] + " files.");
					}
					int nFilesTFull = 0;
					for (int i = 0; i < 10; i++)
						nFilesTFull += nFilesT[i];
					Console.WriteLine(@"Total: " + nFilesTFull + " files.");
					List<string>[] filesTest = new List<string>[nElements];
					for (int i = 0; i < nElements; i++)
						filesTest[i] = Directory.GetFiles(@TestingFolder + @"\" + i + @"\", "*.png").ToList<string>();
					Digit[,] DigitsTest = new Digit[nElements, MathAI.Max(nFilesT)];
					for (int i = 0; i < DigitsTest.GetLength(0); i++)
						for (int k = 0; k < nFilesT[i]; k++)
							DigitsTest[i, k] = new Digit(filesTest[i].ElementAt(k));
					//Bitmap imageT;
					double[] aResult = new double[nFilesTFull];
					double averageResult = 0;
					for (int iteration2 = 1; iteration2 <= nFilesTFull; iteration2++)
					{
						
						int curAnswer = MathAI.GetRandom(0, nElements - 1);
						int curDigitVar = MathAI.GetRandom(0, nFilesT[curAnswer] - 1);
						Digit curDigit = DigitsTest[curAnswer, curDigitVar];
						FirstLayer Layer1 = new FirstLayer(curDigit, dimHiddenLayer, Weights1);
						SecondLayer Layer2 = new SecondLayer(Layer1, nElements, Weights2, curAnswer);
						bool GoodAnswer;
						if (MathAI.MaxI(Layer2.Result) == curAnswer) GoodAnswer = true; else GoodAnswer = false;
						Console.WriteLine("Iteration #" + iteration2 + " | " + GoodAnswer);
						Console.WriteLine("Answer = " + curAnswer);
						Console.WriteLine("Output = " + MathAI.MaxI(Layer2.Result));
						Console.WriteLine("Average = " + Layer2.Result[curAnswer]);
						for (int i = 0; i < Layer2.ResultLength; i++)
							Console.WriteLine(i + ": " + (Layer2.Result[i]));
						//for (int i = 0; i < Layer2.ResultLength; i++) fullResultWork += Result[i];
						 aResult[iteration2 - 1] = Layer2.Result[curAnswer];
					}
					for (int i = 0; i < nFilesTFull; i++)
						averageResult += aResult[i];
					averageResult /= nFilesTFull;
					Console.WriteLine("-------------\nTotal Average = " + averageResult);
					Console.ReadKey();
					DigitsTest = null;
					break;
				} while (TrainingFolder != @"\q");

			}
			else
			{
				string FileAddress;
				do
				{
					Console.WriteLine("Enter address of image: ");

					FileAddress = Console.ReadLine();
					Weight Weights1 = new Weight(@"D:\weights1.txt");
					Weight Weights2 = new Weight(@"D:\weights2.txt");
					Digit curDigit = new Digit(@FileAddress);
					FirstLayer Layer1 = new FirstLayer(curDigit, dimHiddenLayer, Weights1);
					SecondLayer Layer2 = new SecondLayer(Layer1, nElements, Weights2);
					Console.WriteLine("Answer: " + MathAI.MaxI(Layer2.Result));
					Console.WriteLine("Detail: ");
					for (int i = 0; i < Layer2.ResultLength; i++)
						Console.WriteLine(i + ": " + (Layer2.Result[i]));
					Console.ReadKey();
				} while (FileAddress != @"\q");
			}
		}
	}
}
