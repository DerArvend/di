using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TagCloudGenerator
{
	public class TagCloudGenerator : ITagCloudGenerator
	{
		private static HashSet<string> stopWords = new HashSet<string>(Properties.Resources.Stopwords.Replace("\r", "").Split('\n'));
		private Func<string, bool> includingRule = s => false;
		private Func<string, bool> excludingRule = s => false;
		private Func<string, string> wordChanger = s => s;

		private Font font = new Font(SystemFonts.DefaultFont.Name, 72);
		private Func<string, Color> colorSelector = s => Color.Black;

		private ICloudLayouter layouter;

		public TagCloudGenerator(ICloudLayouter layouter)
		{
			this.layouter = layouter;
		}

		public Bitmap GenerateCloud(string text, int wordsCount)
		{
			var data = GetMostFrequentWords(text, wordsCount).ToList();
			var maxCount = data.First().count;
			var sizes = data.Select(x => TextRenderer.MeasureText(
				x.word, new Font(font.Name, GetWordSize(x.count, maxCount))));

			var imageSize = GetImageSize(sizes);

			layouter.Center = new Point(imageSize.Width / 2, imageSize.Height / 2);

			var rectangles = sizes.Select(layouter.PutNextRectangle).ToList();
			var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
			var g = Graphics.FromImage(bitmap);
			
			g.FillRectangle(Brushes.White, g.ClipBounds);
			for (int i = 0; i < data.Count; i++)
			{
				g.DrawString(data[i].word, 
					new Font(font.Name, GetWordSize(data[i].count, maxCount)),
					new SolidBrush(colorSelector(data[i].word)), rectangles[i]);
			}
			
			return bitmap;
		}

		private float GetWordSize(int count, int maxCount)
		{
			return Math.Min(font.Size * count / maxCount, font.Size / 2);
		}

		#region configuration

		public TagCloudGenerator Excluding(Func<string, bool> wordExcludingRule)
		{
			excludingRule += wordExcludingRule;
			return this;
		}

		public TagCloudGenerator Including(Func<string, bool> wordIncludingRule)
		{
			includingRule += wordIncludingRule;
			return this;
		}

		public TagCloudGenerator Using(Func<string, string> wordChanger)
		{
			this.wordChanger = wordChanger;
			return this;
		}

		public TagCloudGenerator WithFont(Font font)
		{
			this.font = font;
			return this;
		}

		public TagCloudGenerator SetColorSelector(Func<string, Color> colorSelector)
		{
			this.colorSelector = colorSelector;
			return this;
		}

		#endregion

		private IEnumerable<(string word, int count)> GetMostFrequentWords(string text, int count)
		{
			return Regex.Split(text.ToLower(), @"\W+") //split text to words in lowercase
				.Where(word => !string.IsNullOrEmpty(word))
				.Where(word => !stopWords.Contains(word))
				.Where(word => !excludingRule(word) || includingRule(word))
				.Select(word => wordChanger(word))
				.GroupBy(word => word)
				.OrderByDescending(group => group.Count())
				.ThenBy(g => g.Key)
				.Take(count)
				.Select(g => (word: g.Key, count: g.Count()));
		}

		private Size GetImageSize(IEnumerable<Size> rectangleSizes)
		{
			var sumArea = rectangleSizes
				.Select(size => size.Width * size.Height)
				.Sum();
			var diameter =  2 * (int) Math.Sqrt(sumArea / Math.PI);

			return new Size(2 * diameter, 2 * diameter);
		}
	}
}