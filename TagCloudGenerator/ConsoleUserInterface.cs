using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudGenerator
{
	class ConsoleUserInterface : IUserInterface
	{
		private readonly TextReader reader;
		private readonly TextWriter writer;
		private readonly ImageFormat imageFormat;
		private readonly ITagCloudGenerator generator;
		private readonly IFileReader fileReader;
		private readonly IImageSaver imageSaver;

		public ConsoleUserInterface(TextReader reader, TextWriter writer, ImageFormat imageFormat,
			ITagCloudGenerator generator, IFileReader fileReader, IImageSaver imageSaver)
		{
			this.reader = reader;
			this.writer = writer;
			this.imageFormat = imageFormat;
			this.generator = generator;
			this.fileReader = fileReader;
			this.imageSaver = imageSaver;
		}

		public void Run()
		{
			var parameters = GetParameters();
			var cloud = generator.GenerateCloud(fileReader.ReadFile(parameters.file), parameters.maxCloudItems);
			var outputPath = $"{parameters.file.FullName}.cloud.{imageFormat}";
			imageSaver.SaveImage(cloud, imageFormat, outputPath);
			writer.WriteLine($"Cloud saved to {outputPath}");
		}

		private (FileInfo file, Font font, Color color, int maxCloudItems) GetParameters()
		{
			var file = GetTextFile();
			var fontName = GetFontName();
			var fontSize = GetFontSize();
			var maxCloudItems = GetMaxCloudItems();

			writer.WriteLine("Enter color");
			var stringColor = reader.ReadLine();

			return (file: file, font: new Font(fontName, fontSize), color: Color.FromName(stringColor),
				maxCloudItems: maxCloudItems);
		}

		private int GetMaxCloudItems()
		{
			writer.WriteLine("Enter maximum cloud items count");
			while (true)
			{
				var stringMaxCloudItems = reader.ReadLine();
				if (int.TryParse(stringMaxCloudItems, out var maxCloudItems) && maxCloudItems > 0)
					return maxCloudItems;
				writer.WriteLine("Maximum cloud items count should be positive integer");
			}
		}

		private FileInfo GetTextFile()
		{
			writer.WriteLine("Enter path to file with text");
			while (true)
			{
				var filepath = reader.ReadLine();
				if (File.Exists(filepath))
					return new FileInfo(filepath);
				writer.WriteLine("Incorrect file path");
			}
		}

		private string GetFontName()
		{
			writer.WriteLine("Enter font");
			while (true)
			{
				var name = reader.ReadLine();
				try
				{
					new Font(name, 12);
					return name;
				}
				catch
				{
					writer.WriteLine("Incorrect font name");
				}
			}
		}

		private float GetFontSize()
		{
			writer.WriteLine("Enter font size");
			while (true)
			{
				var wasParsed = float.TryParse(reader.ReadLine(), out var fontSize);
				if (wasParsed && fontSize >= 6)
					return fontSize;
				writer.WriteLine("Size must be 6 or more");
			}
		} 
	}
}