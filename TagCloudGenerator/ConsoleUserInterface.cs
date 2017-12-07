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
			var textFile = new FileInfo(parameters.filepath);
			var cloud = generator.GenerateCloud(fileReader.ReadFile(textFile), parameters.maxCloudItems);
			var outputPath = $"{textFile.FullName}.cloud.{imageFormat}";
			imageSaver.SaveImage(cloud, imageFormat, outputPath);
			writer.WriteLine($"Cloud saved to {outputPath}");
		}

		private (string filepath, Font font, Color color, int maxCloudItems) GetParameters()
		{
			writer.WriteLine("Enter path to file with text");
			var filepath = reader.ReadLine();
			writer.WriteLine("Enter font name");
			var fontName = reader.ReadLine();
			writer.WriteLine("Enter font size");
			var stringFontSize = reader.ReadLine();
			if (!float.TryParse(stringFontSize, out var fontSize))
				throw new ArgumentException("Invalid color size");
			writer.WriteLine("Enter maximum cloud items count");
			var stringMaxCloudItems = reader.ReadLine();
			if (!int.TryParse(stringMaxCloudItems, out var maxCloudItems) || maxCloudItems <= 0)
				throw new ArgumentException("Maximum cloud items count should be positive integer");
			writer.WriteLine("Enter color");
			var stringColor = reader.ReadLine();

			return (filepath: filepath, font: new Font(fontName, fontSize), color: Color.FromName(stringColor),
				maxCloudItems: maxCloudItems);
		}
	}
}