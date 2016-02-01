using IniParser;
using IniParser.Model;
using ReportingTool.Core.Validation;
using ReportingTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ReportingTool.Controllers
{
    public enum Answer { NotExists, IsEmpty, NotValid, Exists, Created, NotCreated };
    public class ConfigurationController : Controller
    {
        private string FILE_NAME = @"C:\Configurations.ini"; //TODO
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

        public string FileName
        {
            get
            {
                return FILE_NAME;
            }
            set
            {
                FILE_NAME = value;
            }
        }

        /// <summary>
        /// Checks if configuration file exists, empty and valid
        /// </summary>
        /// <returns>Answer</returns>
        [HttpGet]
        public ActionResult SetConfigurations()
        {
            Answer answer;
            if (!System.IO.File.Exists(FILE_NAME))
            {
                answer = Answer.NotExists;
            }
            else if (new FileInfo(FILE_NAME).Length == 0)
            {
                answer = Answer.IsEmpty;
            }
            else
            {
                FileIniDataParser fileIniData = new FileIniDataParser();
                try
                {
                    IniData parsedData = fileIniData.ReadFile(FILE_NAME);

                    KeyDataCollection section = parsedData[SECTION];
                    if (section == null)
                    {
                        answer = Answer.IsEmpty;
                    }
                    else
                    {
                        if (parsedData[SECTION].ContainsKey(SERVEL_URL_KEY) && parsedData[SECTION].ContainsKey(PROJECT_NAME_KEY))
                        {
                            if (!string.IsNullOrEmpty(parsedData[SECTION][SERVEL_URL_KEY]) && (!string.IsNullOrEmpty(parsedData[SECTION][PROJECT_NAME_KEY])))
                            {
                                answer = Answer.Exists;
                            }
                            else
                            {
                                answer = Answer.IsEmpty;
                            }
                        }
                        else
                        {
                            answer = Answer.IsEmpty;
                        }

                    }
                }
                catch (Exception e)
                {
                    answer = Answer.NotValid;
                }
            }
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates or full in configuration file. Returns answer whether file was created or no
        /// </summary>
        /// <param name="model">Model that contains input user parameters of server url and project name</param>
        /// <returns>Answer</returns>
        [HttpPost]
        public ActionResult SetConfigurations(ConfigurationParametersModel model)
        {
            Answer answer;
            if (model == null)
            {
                answer = Answer.NotCreated;
            }
            else
            {
                string serverUrl = model.ServerUrl;
                string projectName = model.ProjectName;
                if (ConfigurationHelper.ValidateParametes(serverUrl, projectName))
                {
                    FileIniDataParser fileIniData = new FileIniDataParser();

                    if (!System.IO.File.Exists(FILE_NAME) || (new FileInfo(FILE_NAME).Length == 0))
                    {
                        IniData newData = ConfigurationHelper.CreateINIData(serverUrl, projectName);
                        newData = ConfigurationHelper.AddSectionDBConfigurationINIData(newData);
                        fileIniData.WriteFile(FILE_NAME, newData);
                        answer = Answer.Created;
                    }
                    else
                    {
                        try
                        {
                            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
                            KeyDataCollection section = parsedData[SECTION];
                            if (section == null)
                            {
                                parsedData = ConfigurationHelper.AddSectionINIData(serverUrl, projectName, parsedData);
                            }
                            else
                            {
                                if (parsedData[SECTION].ContainsKey(SERVEL_URL_KEY) && parsedData[SECTION].ContainsKey(PROJECT_NAME_KEY))
                                {
                                    if (!string.IsNullOrEmpty(parsedData[SECTION][SERVEL_URL_KEY]) && (!string.IsNullOrEmpty(parsedData[SECTION][PROJECT_NAME_KEY])))
                                    {
                                        answer = Answer.NotCreated;
                                        return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                                    }
                                    else
                                    {
                                        parsedData = ConfigurationHelper.UpdateKeysINIData(serverUrl, projectName, parsedData);
                                    }
                                }
                                else
                                {
                                    parsedData = ConfigurationHelper.UpdateSectionINIData(serverUrl, projectName, parsedData);

                                }
                            }
                            parsedData = ConfigurationHelper.AddSectionDBConfigurationINIData(parsedData);
                            fileIniData.WriteFile(FILE_NAME, parsedData);
                            answer = Answer.Created;
                        }
                        catch (Exception e)
                        {
                            answer = Answer.NotCreated;
                        }
                    }
                }
                else
                {
                    answer = Answer.NotCreated;
                }
            }
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }
    }
}