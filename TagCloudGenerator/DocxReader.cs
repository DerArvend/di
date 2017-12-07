using System.IO;
using Xceed.Words.NET;

namespace TagCloudGenerator
{
	class DocxReader : IFileReader
	{
		public string ReadFile(FileInfo fileInfo)
		{
			return DocX.Load(fileInfo.FullName).Text;
		}
	}
}
