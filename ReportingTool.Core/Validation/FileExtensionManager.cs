using System.IO;
using IniParser;
using IniParser.Model;
using ReportingTool.Core.Validation.Interfaces;

namespace ReportingTool.Core.Validation
{
    public class FileExtensionManager : IFileExtensionManager
    {
        public bool IsExists(string path)
        {
            return File.Exists(path);
        }

        public bool IsEmpty(string path)
        {
            return new FileInfo(path).Length == 0;
        }

        public IniData ReadFile(FileIniDataParser fileIniData, string path)
        {
            return fileIniData.ReadFile(path);
        }

        public void WriteFile(FileIniDataParser fileIniData, string path, IniData parsedData)
        {
            fileIniData.WriteFile(path, parsedData);
        }
    }
}