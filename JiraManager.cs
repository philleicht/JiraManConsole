using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace JiraManConsole
{
    public class JiraProject
    {
        public string id { get; set; }
        public string key { get; set;  }
        public string name { get; set; }
    }

    public class JiraComponent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string project { get; set; }
        public string projectkey { get; set; }
        public string projectid { get; set; }
    }

    public class JiraIssue 
    {
        public string id { get; set; }
        public string key { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string[] issuetype { get; set; } //TODO: Add setter and include extract of issuetype "Name" (issuetype[name])
        public string[] reporter { get; set; } //TODO: Add setter to only consider displayName of reporter (reporter[displayName])
        public string[] status { get; set; } //TODO: Add setter to only consider status[Name] (i.e. resolved)
        public string[] priority { get; set; } //TODO: Add setter to only consider priority[name] (i.e. mayor)
        //public string[] project { get; set; } //TODO: Add setter to separate project[id], project[key], project[name]
    }

    public enum JiraResource
    {
        project,
        issue
    }

    public class JiraManager
    {
        private string m_BaseUrl;
        private string m_Username;
        private string m_Password;

        public JiraManager(string baseurl, string username, string password)
        {
            m_BaseUrl = baseurl;
            m_Username = username;
            m_Password = password;
        }

        public JiraProject[] RunQuery(JiraResource resource, string argument = null, string data = null, string method = "GET")
        {
            string url = string.Format("{0}{1}/", m_BaseUrl, resource.ToString());

            if (argument != null)
            {
                url = string.Format("{0}{1}/", url, argument);
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = method;

            if (data != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(data);
                }
            }

            if (m_Username != null && m_Password != null)
            {
                string base64Credentials = GetEncodedCredentials();
                request.Headers.Add("Authorization", "Basic " + base64Credentials);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string result = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            JiraProject[] results = js.Deserialize<JiraProject[]>(result);
            /*
            Console.WriteLine(results);
            Console.WriteLine(results[0].id);
            Console.WriteLine(results[0].name);
            Console.WriteLine(results[0].key);

            Console.WriteLine(results[1].id);
            Console.WriteLine(results[1].name);
            Console.WriteLine(results[1].key);
            */
              
            foreach(JiraProject proj in results){
                Console.WriteLine(proj.id);
                Console.WriteLine(proj.key);
                Console.WriteLine(proj.name);
            }

            return results;
            //Console.WriteLine(result);
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", m_Username, m_Password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }
}
