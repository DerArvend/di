using System.Drawing;

namespace TagCloudGenerator
{
	public interface ISpiral
	{
		Point GetNextPoint();
		Point Center { get; set; }
	}
}
