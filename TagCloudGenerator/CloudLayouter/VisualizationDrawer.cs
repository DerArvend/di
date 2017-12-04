using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudGenerator
{
	class VisualizationDrawer
	{
		public static Bitmap GetRectanglesVisualization(IEnumerable<Rectangle> rectangles)
		{
			var rand = new Random();
			var bitmap = new Bitmap(1500, 1500, PixelFormat.Format24bppRgb);
			var graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
			foreach (var rectangle in rectangles)
			{
				graphics.FillRectangle(GetRandomBrush(rand), rectangle);
				graphics.DrawRectangle(Pens.Black, rectangle);
			}
			return bitmap;
		}

		private static Brush GetRandomBrush(Random rand)
		{
			return new SolidBrush(Color.FromArgb(rand.Next(50, 200), rand.Next(50, 200), rand.Next(50, 200)));
		}
	}
}