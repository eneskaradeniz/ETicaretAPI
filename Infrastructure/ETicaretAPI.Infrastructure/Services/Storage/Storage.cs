using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected string FileRename(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = NameOperation.CharacterRegulatory(oldName);
            string newFileName = $"{regulatedFileName}{extension}";

            int iteration = 1;

            while (hasFileMethod(pathOrContainerName, newFileName))
            {
                newFileName = $"{regulatedFileName}-{iteration}{extension}";
                iteration++;
            }

            return newFileName;
        }
    }
}
