using System.IO;
using System.Threading.Tasks;

namespace APIBase.Common.FileFunctions
{
    public static class FileReader
    {
        public static async Task<string> ReadFileAsStringAsync(
            string pathFile)
        {
            using var fileStreamReader = new StreamReader(pathFile);

            var fileAsString = await fileStreamReader.ReadToEndAsync();

            return fileAsString;
        }

        public static string ReadFileAsString(
            string pathFile)
        {
            using var fileStreamReader = new StreamReader(pathFile);

            var fileAsString = fileStreamReader.ReadToEnd();

            return fileAsString;
        }
    }
}
