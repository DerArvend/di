using System.IO;

namespace TagCloudGenerator
{
	public interface IFileReader
	{
		string ReadFile(FileInfo fileInfo);
	}
}
