using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.Result;

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
			var file = ReadParameter("Enter file path", ParametersParser.ParseFileInfo);
			var fontName = ReadParameter("Enter font name", ParametersParser.ValidateFontName);
			var fontSize = ReadParameter("Enter font size", ParametersParser.ParseFontSize);
			var maxCloudItems = ReadParameter("Enter max cloud items", ParametersParser.ParseMaxCloudItems);
			var color = ReadParameter("Enter color", ParametersParser.ParseColor);

			return (file: file, font: new Font(fontName, fontSize), color: color, maxCloudItems: maxCloudItems);
		}


		private T ReadParameter<T>(string message, Func<string, Result<T>> validator)
		{
			writer.WriteLine(message);
			while (true)
			{
				var result = validator(reader.ReadLine());
				if (result.IsSuccess)
					return result.Value;
				writer.WriteLine(result.Error);
			}
		}
	}
}