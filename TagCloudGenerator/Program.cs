using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Autofac;

namespace TagCloudGenerator
{
	static class Program
	{
		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<TagCloudGenerator>().As<ITagCloudGenerator>();
			builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().WithParameter("center", default(Point));
			builder.RegisterType<ArchimedianSpiral>().As<ISpiral>();
			builder.RegisterType<TxtReader>().As<IFileReader>();
			builder.RegisterType<CloudImageSaver>().As<IImageSaver>();
			builder.RegisterType<ConsoleUserInterface>().As<IUserInterface>()
				.WithParameter("reader", Console.In)
				.WithParameter("writer", Console.Out)
				.WithParameter("imageFormat", ImageFormat.Png);

			var container = builder.Build();

			var ui = container.Resolve<IUserInterface>();
			ui.Run();
		}
	}
}