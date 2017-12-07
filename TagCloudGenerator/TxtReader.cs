using System.IO;

namespace TagCloudGenerator
{
	class TxtReader : IFileReader
	{
		public string ReadFile(FileInfo fileInfo)
		{
			return fileInfo.OpenText().ReadToEnd();
		}
	}
}
