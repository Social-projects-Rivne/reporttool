﻿using System;
using System.Collections.Generic;

namespace ReportingTool.DAL.Entities
{
    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
        public string summary { get; set; }
        public Worklog worklog { get; set; }
        public Status status { set; get; }
        public Assignee assignee { set; get; }
    }

    public class Assignee
    {
        public bool active { set; get; }
        public Avatarurls avatarUrls { set; get; }
        public string displayName { get; set; }
        public string emailAddress { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string self { get; set; }
        public string timeZone { get; set; }
    }

    public class Status
    {
        public  string description { set; get; }
        public string iconUrl { set; get; }
        public int id { set; get; }
        public string name { set; get; }
        public string self { set; get; }
        public StatusCategory statusCategory { set; get; }
    }

    public class StatusCategory
    {
        public string colorName { set; get; }
        public int id { set; get; }
        public string key { set; get; }
        public string name { set; get; }
        public string self { set; get; }

    }

    public class Worklog
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<WorklogDetails> worklogs { get; set; }
    }

    public class WorklogDetails
    {
        public string self { get; set; }
        public Author author { get; set; }
        public Updateauthor updateAuthor { get; set; }
        public string comment { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public DateTime started { get; set; }
        public string timeSpent { get; set; }
        public int timeSpentSeconds { get; set; }
        public string id { get; set; }
    }

    public class Author
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public Avatarurls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
    }

    public class Avatarurls
    {
        public string _16x16 { get; set; }
        public string _24x24 { get; set; }
        public string _32x32 { get; set; }
        public string _48x48 { get; set; }
    }

    public class Updateauthor
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public Avatarurls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
    }



}
