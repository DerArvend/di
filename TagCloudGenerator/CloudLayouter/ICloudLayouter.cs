using System.Drawing;

namespace TagCloudGenerator
{
	public interface ICloudLayouter
	{
		Rectangle PutNextRectangle(Size rectangleSize);
		Point Center { get; set; }
	}
}