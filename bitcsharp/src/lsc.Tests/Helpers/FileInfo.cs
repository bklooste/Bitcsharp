
namespace lsc.Tests
{
    public class FileInfo
    {
        public string Path, Description;

        public override string ToString()
        {
            return Path + " - " + Description;
        }
    }
}
