using System;
using System.Drawing;
using System.IO;
using TagCloudGenerator.Result;

namespace TagCloudGenerator
{
	public static class ParametersParser
	{
		public static Result<int> ParseMaxCloudItems(string raw)
		{
			if (int.TryParse(raw, out var maxCloudItems) && maxCloudItems > 0)
				return maxCloudItems.AsResult();
			return Result.Result.Fail<int>("Maximum cloud items count should be positive integer");
		}

		public static Result<FileInfo> ParseFileInfo(string filepath)
		{
			if (File.Exists(filepath))
				return new FileInfo(filepath).AsResult();
			return Result.Result.Fail<FileInfo>("Incorrect file path");
		}

		public static Result<string> ValidateFontName(string raw)
		{
			try
			{
				var font = new Font(raw, 12);
				return raw.AsResult();
			}
			catch
			{
				return Result.Result.Fail<string>("Incorrect font name");
			}
		}

		public static Result<float> ParseFontSize(string raw)
		{
			var wasParsed = float.TryParse(raw, out var fontSize);
			if (wasParsed && fontSize >= 6)
				return fontSize.AsResult();
			return Result.Result.Fail<float>("Size must be 6 or more");
		}

		public static Result<Color> ParseColor(string raw)
		{
			if (!Enum.TryParse(raw, out KnownColor color))
				return Result.Result.Fail<Color>("Incorrect color");
			return Color.FromKnownColor(color).AsResult();
		}
	}
}