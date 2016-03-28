using IniParser;
using IniParser.Model;

namespace ReportingTool.Core.Validation.Interfaces
{
    public interface IFileExtensionManager
    {
        bool IsExists(string path);
        bool IsEmpty(string path);
        IniData ReadFile(FileIniDataParser fileIniData, string path);
        void WriteFile(FileIniDataParser fileIniData, string path, IniData parsedData);
    }
}