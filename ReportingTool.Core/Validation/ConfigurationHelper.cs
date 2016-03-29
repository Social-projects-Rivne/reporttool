using IniParser.Model;
using System.Text.RegularExpressions;

namespace ReportingTool.Core.Validation
{
    /// <summary>
    /// Represents configuration helper class
    /// </summary>
    public static class ConfigurationHelper
    {
        private const string SECTION = "GeneralConfiguration";
        private const string SERVEL_URL_KEY = "ServerUrl";
        private const string PROJECT_NAME_KEY = "ProjectName";
        private const string DB_SECTION = "DataBaseConfiguration";
        private const string DB_ID_KEY = "UserId";
        private const string DB_PASSWORD_KEY = "Password";
        private const string DB_DATABASE_KEY = "Database";
        private const string DB_ID_VALUE = "postgres";
        private const string DB_PASSWORD_VALUE = "postgres";
        private const string DB_DATABASE_VALUE = "rtdb";

        /// <summary>
        /// Returns full data of configuration file, when section wasn't exists
        /// </summary>
        /// <param name="serverUrl">Server url of the project</param>
        /// <param name="projectName">Project name</param>
        /// <returns>Configuration data</returns>
        public static IniData CreateINIData(string serverUrl, string projectName)
        {
            IniData newData = new IniData();
            return AddSectionINIData(serverUrl, projectName, newData);
        }

        /// <summary>
        ///  Returns full data of configuration file, when section was empty 
        /// </summary>
        /// <param name="serverUrl">Server url of the project</param>
        /// <param name="projectName">Project name</param>
        /// <param name="iniData">Existing data in the file</param>
        /// <returns>Configuration data</returns>
        public static IniData AddSectionINIData(string serverUrl, string projectName, IniData iniData)
        {
            iniData.Sections.AddSection(SECTION);
            return UpdateSectionINIData(serverUrl, projectName, iniData);
        }

        /// <summary>
        /// Returns full data of configuration file, when keys were not exists
        /// </summary>
        /// <param name="serverUrl">Server url of the project</param>
        /// <param name="projectName">Project name</param>
        /// <param name="iniData">Existing data in the file</param>
        /// <returns>Configuration data</returns>
        public static IniData UpdateSectionINIData(string serverUrl, string projectName, IniData iniData)
        {
            iniData[SECTION].AddKey(SERVEL_URL_KEY, serverUrl);
            iniData[SECTION].AddKey(PROJECT_NAME_KEY, projectName);
            return iniData;
        }

        /// <summary>
        /// Returns full data of configuration file, when keys values were empty 
        /// </summary>
        /// <param name="serverUrl">Server url of the project</param>
        /// <param name="projectName">Project name</param>
        /// <param name="iniData">Existing data in the file</param>
        /// <returns>Configuration data</returns>
        public static IniData UpdateKeysINIData(string serverUrl, string projectName, IniData iniData)
        {
            iniData[SECTION][SERVEL_URL_KEY] = serverUrl;
            iniData[SECTION][PROJECT_NAME_KEY] = projectName;
            return iniData;
        }

        /// <summary>
        /// Add section database configurations
        /// </summary>
        /// <param name="iniData">Existing data in the file</param>
        /// <returns>Configuration data</returns>
        public static IniData AddSectionDBConfigurationINIData(IniData iniData)
        {
            if (iniData[DB_SECTION] == null)
            {
                iniData.Sections.AddSection(DB_SECTION);
                iniData[DB_SECTION].AddKey(DB_ID_KEY, DB_ID_VALUE);
                iniData[DB_SECTION].AddKey(DB_PASSWORD_KEY, DB_PASSWORD_VALUE);
                iniData[DB_SECTION].AddKey(DB_DATABASE_KEY, DB_DATABASE_VALUE);
            }
            return iniData;
        }

        /// <summary>
        /// Checks if input user parameters are valid
        /// </summary>
        /// <param name="serverUrl">Server url of the project</param>
        /// <param name="projectName">Project name</param>
        /// <returns>Configuration data</returns>
        public static bool ValidateParametes(string serverUrl, string projectName)
        {
            bool answer = true;
            if (serverUrl == null || projectName == null)
            {
                answer = false;
            }
            else
            {
                Regex rgxUrl = new Regex(@"^(http|https)\:\/\/(www\.)?(([a-zA-Z\d]|[a-zA-Z\d][a-zA-Z\d\-]*[a-zA-Z\d])\.)(([a-zA-Z\d]|[a-zA-Z\d][a-zA-Z\d\-]*[a-zA-Z\d])\.){0,2}([A-Za-z\d]|[A-Za-z\d][A-Za-z\d\-]*[A-Za-z\d])$");
                Regex rgxProject = new Regex(@"^((([A-Z\d](\.|\-)?)*[A-Z\d]))$");
                if (!rgxUrl.IsMatch(serverUrl) && !rgxProject.IsMatch(projectName))
                {
                    answer = false;
                }
            }
            return answer;
        }
    }
}