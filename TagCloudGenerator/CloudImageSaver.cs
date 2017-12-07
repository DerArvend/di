using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudGenerator
{
	public class CloudImageSaver : IImageSaver
	{
		public void SaveImage(Bitmap image, ImageFormat format, string path)
		{
			image.Save(path, format);
		}
	}
}