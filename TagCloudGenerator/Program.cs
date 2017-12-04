using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Autofac;

namespace TagCloudGenerator
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<TagCloudGenerator>().As<ITagCloudGenerator>();
			builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().WithParameter("center", default(Point));
			builder.RegisterType<ArchimedianSpiral>().As<ISpiral>();

			var generator = builder.Build().Resolve<ITagCloudGenerator>();
			var image = generator.GenerateCloud(Properties.Resources.constitution, 100);

			image.Save("example.png", ImageFormat.Png);
		}
	}
}