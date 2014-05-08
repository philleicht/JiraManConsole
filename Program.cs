using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
//using TechTalk.JiraRestClient;
using System.Net;

namespace JiraManConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello and welcome to a Jira Example application!");
            // Used to ignore "Certificate doesn't correspond with URL"
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            //Console.Write("Username: ");
            //string username = Console.ReadLine();

            //Console.Write("Password: ");
            //string password = Console.ReadLine();

            //FOR PUBLIC JIRA
            JiraManager manager = new JiraManager("https://jira.atlassian.com/rest/api/2/", null, null);
            
            manager.RunQuery(JiraResource.project, "BON/components");


            Console.Read();
            
              


        }
    }
}
