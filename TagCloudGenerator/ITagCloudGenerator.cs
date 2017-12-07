using System;
using System.Drawing;

namespace TagCloudGenerator
{
	public interface ITagCloudGenerator
	{
		Bitmap GenerateCloud(string text, int wordsCount);
		TagCloudGenerator Excluding(Func<string, bool> wordExcludingRule);
		TagCloudGenerator Including(Func<string, bool> wordIncludingRule);
	}
}
