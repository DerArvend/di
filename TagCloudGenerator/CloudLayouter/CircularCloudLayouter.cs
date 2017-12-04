using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudGenerator
{
	public class CircularCloudLayouter : ICloudLayouter
	{
		private Point center;
		public Point Center
		{
			get => center;
			set { center = value;
				spiral.Center = value;
			}
		}

		public List<Rectangle> Rectangles { get; }
		private readonly ISpiral spiral;

		public CircularCloudLayouter(Point center, ISpiral spiral)
		{
			if (center.X < 0 || center.Y < 0)
				throw new ArgumentOutOfRangeException("Center coordinates should be non-negative numbers");
			
			Rectangles = new List<Rectangle>();
			this.spiral = spiral;
			Center = center;
		}


		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
				throw new ArgumentException("Size must not be empty");

			var newRectangleLocation = spiral.GetNextPoint();
			while (!RectangleCanBePlacedAt(newRectangleLocation, rectangleSize))
				newRectangleLocation = spiral.GetNextPoint();

			var rectangle = new Rectangle(newRectangleLocation, rectangleSize);
			Rectangles.Add(rectangle);
			return rectangle;
		}

		public Rectangle PutNextRectangle(int width, int heigth)
		{
			return PutNextRectangle(new Size(width, heigth));
		}

		private bool RectangleCanBePlacedAt(Point p, Size size)
		{
			return (p.X >= 0 && p.Y >= 0 && !IsRectangleAt(p, size));
		}

		private bool IsRectangleAt(Point p, Size size)
		{
			var rectangleToPlace = new Rectangle(p, size);
			return Rectangles.Any(r => r.IntersectsWith(rectangleToPlace));
		}
	}
}