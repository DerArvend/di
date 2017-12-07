using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudGenerator
{
	public interface IImageSaver
	{
		void SaveImage(Bitmap image, ImageFormat format, string path);
	}
}
