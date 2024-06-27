using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        public string FileRename(string path, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = NameOperation.CharacterRegulatory(oldName);
            string newFileName = $"{regulatedFileName}{extension}";

            string fullPath = Path.Combine(path, newFileName);
            int iteration = 1;

            while (File.Exists(fullPath))
            {
                newFileName = $"{regulatedFileName}-{iteration}{extension}";
                fullPath = Path.Combine(path, newFileName);
                iteration++;
            }

            return newFileName;
        }
    }
}
