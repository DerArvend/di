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
			var color = GetColor();

			return (file: file, font: new Font(fontName, fontSize), color: color, maxCloudItems: maxCloudItems);
		}

		private int GetMaxCloudItems()
		{
			writer.WriteLine("Enter maximum cloud items count");
			while (true)
			{
				var max = ParametersParser.ParseMaxCloudItems(reader.ReadLine());
				if (max.IsSuccess)
					return max.Value;
				writer.WriteLine(max.Error);
			}
		}

		private FileInfo GetTextFile()
		{
			writer.WriteLine("Enter path to file with text");
			while (true)
			{
				var fileResult = ParametersParser.ParseFileInfo(reader.ReadLine());
				if (fileResult.IsSuccess)
					return fileResult.Value;
				writer.WriteLine(fileResult.Error);
			}
		}

		private string GetFontName()
		{
			writer.WriteLine("Enter font");
			while (true)
			{
				var fontNameResult = ParametersParser.ValidateFontName(reader.ReadLine());
				if (fontNameResult.IsSuccess)
					return fontNameResult.Value;
				writer.WriteLine(fontNameResult.Error);
			}
			
		}

		private float GetFontSize()
		{
			writer.WriteLine("Enter font size");
			while (true)
			{
				var fontSizeResult = ParametersParser.ParseFontSize(reader.ReadLine());
				if (fontSizeResult.IsSuccess)
					return fontSizeResult.Value;
				writer.WriteLine(fontSizeResult.Error);
			}
		}

		private Color GetColor()
		{
			writer.WriteLine("Enter font color");
			while (true)
			{
				var colorResult = ParametersParser.ParseColor(reader.ReadLine());
				if (colorResult.IsSuccess)
					return colorResult.Value;
				writer.WriteLine(colorResult.Error);
			}
		}
	}
}