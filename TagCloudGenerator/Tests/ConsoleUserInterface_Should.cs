using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace TagCloudGenerator.Tests
{
	[TestFixture]
	public class ConsoleUserInterface_Should
	{
		private ConsoleUserInterface ui;

		[SetUp]
		public void SetUp()
		{
			var saver = new Mock<IImageSaver>();
		}

		[Test]
		public void SaveCloud_ToOriginalFolder()
		{
			var saver = new Mock<IImageSaver>();
			var reader = new Mock<TextReader>();
			reader.Setup(x => x.ReadLine()).Returns("1");
			var fileReader = new Mock<IFileReader>();
			fileReader.Setup(fr => fr.ReadFile(It.IsAny<FileInfo>())).Returns("1");
			var ui = new ConsoleUserInterface(reader.Object, TextWriter.Null, ImageFormat.Png,
				new TagCloudGenerator(new CircularCloudLayouter(new Point(50, 50), new ArchimedianSpiral())), fileReader.Object, 
				saver.Object);
			ui.Run();
			saver.Verify(s => s.SaveImage(It.IsAny<Bitmap>(), It.IsAny<ImageFormat>(), new FileInfo("1").FullName + ".cloud.Png"), Times.Once);
		}
	}
}