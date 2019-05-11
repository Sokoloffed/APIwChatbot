﻿using DBAccess;
using RESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace RESTApi.Controllers
{
    public class ChatbotController : ApiController
    {
        public static string API_TOKEN = "4845464947a7d3a8-c64c104170e9a147-b439a07e29c30513";

        public string URLToSendMessage = @"https://chatapi.viber.com/pa/send_message";
        public string URLToGetUserProfile = @"https://chatapi.viber.com/pa/get_user_details";
        public static string API_HEADER_CONST = "X-Viber-Auth-Token";

        public string BotAvatar = "https://wcf.plan.net.ua/public/ava100.jpg";
        public string BotName = "Danil tester";

        public delegate void metaEvent(string type);

        //public event metaEvent get;
        //public event metaEvent help;


        ModelFactory modelFactory = new ModelFactory();


        [Route("api/Chatbot/Get")]
        [HttpPost]
        public string Get(CallbackData update)
        {

            if (update.@event == "webhook")
            {

            }
            else if (update.@event == "conversation_started")
            {
                SendMessage(update.sender.id, "Hello, your conversation with bot has just started!");
            }
            else if (update.@event == "subscribed")
            {

            }
            else if (update.@event == "unsubscribed")
            {

            }
            else if (update.@event == "delivered")
            {

            }
            else if (update.@event == "seen")
            {

            }
            else if (update.@event == "failed")
            {

            }
            else if (update.@event == "message")
            {
                if (update.message.text == "HELP")
                {

                    string help = "Print GET ALL USERS to get info about all users, " + "\\n"
                        + "Print GET ALL BRANCHES to get info about all branches, " + "\\n"
                        + "Print GET ALL TASKS to get info about all tasks " + "\n";
                    SendMessage(update.sender.id, help);
                }

                else if (Logic.isGet && Logic.isGetUsers)
                {
                    if (update.message.text == "All")
                    {
                        DataRepository repository = new DataRepository();
                        var data = repository.GetAllUsers().ToList().Select(c => modelFactory.Create(c));
                        //sb.Append(data.ElementAt(0).username);
                        //JsonConvert.SerializeObject(new { sb = sb });
                        string message = ConvertUsers(data);
                        SendMessage(update.sender.id, message);
                        return "";
                    }
                    else if (update.message.text == "Filter")
                    {
                        SendMessage(update.sender.id, "Temporary unavaliable");
                        return "";
                    }
                    else if (update.message.text == "Back")
                    {
                        Logic.isGetUsers = false;
                        SendMessage(update.sender.id, "Please, print what kind of data do you want to work with: Users, Branches, Tasks ");
                        return "";
                    }
                    else
                    {
                        SendMessage(update.sender.id, "Print All to get all data from Users table, print Filter to start filtering, print Back to return ");
                        return "";
                    }

                }

                else if (Logic.isGet)
                {
                    

                    if (update.message.text == "Users")
                    {
                        Logic.isGetUsers = true;
                        SendMessage(update.sender.id, "Print All to get all data from Users table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
                    else if (update.message.text == "Branches")
                    {
                        Logic.isGetBranches = true;
                        SendMessage(update.sender.id, "Print All to get all data from Branches table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
                    else if (update.message.text == "Tasks")
                    {
                        Logic.isGetTasks = true;
                        SendMessage(update.sender.id, "Print All to get all data from Tasks table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
                    else if (update.message.text == "Back")
                    {
                        Logic.isGet = false;
                        return "";
                    }


                    else SendMessage(update.sender.id, "Please, print what kind of data do you want to work with: Users, Branches, Tasks, or print Back to get back");
                }

                else if (!Logic.isGet && !Logic.isPost)
                {
                    if (update.message.text == "Get")
                    {
                        Logic.isGet = true;
                        SendMessage(update.sender.id, "Please, print what kind of data do you want to work with: Users, Branches, Tasks ");
                        return "";
                    }

                    else SendMessage(update.sender.id, "If you want to get data from database, print Get; " + "\\n"
                        + "if you want to Post new data into database, print Post; " + "\\n"
                        + "if you need help, print Help; ");
                    return "";
                }

                





                else if (update.message.text == "GET ALL USERS")
                {
                    using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
                    {
                        DataRepository repository = new DataRepository();
                        var data = repository.GetAllUsers().ToList().Select(c => modelFactory.Create(c));
                        //sb.Append(data.ElementAt(0).username);
                        //JsonConvert.SerializeObject(new { sb = sb });
                        string message = ConvertUsers(data);
                        SendMessage(update.sender.id, message);
                    }
                }
                else if (update.message.text == "GET ALL BRANCHES")
                {
                    DataRepository repository = new DataRepository();
                    var data = repository.GetAllBranches().ToList().Select(c => modelFactory.Create(c));
                    string message = ConvertBranches(data);
                    SendMessage(update.sender.id, message);

                }

                else if (update.message.text == "GET ALL TASKS")
                {
                    DataRepository repository = new DataRepository();
                    var data = repository.GetAllTasks().ToList().Select(c => modelFactory.Create(c));
                    string message = ConvertTasks(data);
                    SendMessage(update.sender.id, message);
                }
                else return "";
                
                //else SendMessage(update.sender.id, "Wrong command, please, print HELP to get info about posible actions. ");
            }
            else
            {

            }

            return "";
        }

        public string SendMessage(string tmpRecipient, string tmpMessage)
        {
            string log = tmpMessage;
            string tmpMessagePattern = "{\"receiver\":\"" + tmpRecipient + "\",\"min_api_version\":1,\"sender\":{\"name\":\"" + BotName + "\",\"avatar\":\"" + BotAvatar + "\"},\"tracking_data\":\"tracking data\",\"type\":\"text\",\"text\":\"" + tmpMessage + "\"}";

            var response = RequestHelper(HttpMethod.Post, new Uri(URLToSendMessage), tmpMessagePattern);

            response.Wait();
            var result = response.Result;

            Task<string> content;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                content = result.Content.ReadAsStringAsync();
                content.Wait();

                return content.Result;
            }
            
            else
            {
                return "";
            }
        }

        public static Task<HttpResponseMessage> RequestHelper(HttpMethod method, Uri uri, string json = null)
        {
            var client = new HttpClient();
            var msg = new HttpRequestMessage(method, uri);
            msg.Headers.Add(API_HEADER_CONST, API_TOKEN);
            if (json != null)
            {
                msg.Content = new StringContent(
                    json,
                    UnicodeEncoding.UTF8,
                    "application/json");
            }
            return client.SendAsync(msg);
        }

        public DateTime ConvertFromUnixTimestampMiliseconds(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(Math.Floor(timestamp / 1000));
        }

        public double ConvertToUnixTimestampMiliseconds(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds) * 1000;
        }

        public string ConvertUsers(IEnumerable<UsersModel> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach(UsersModel m in data)
            {
                sb.Append("ID is: " + m.id + "Username is: " + m.username + "\\n");
            }
            return sb.ToString();
        }

        public string ConvertBranches(IEnumerable<BranchesModel> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach(BranchesModel m in data)
            {
                sb.Append("Branchname is: " + m.branchname + ", ID is: " + m.id + ", description is: " + m.description + ", created at"
                    + m.created_date + ", ID of creator: " + m.creator_id + "\\n");
            }
            return sb.ToString();
        }

        public string ConvertTasks(IEnumerable<TasksModel> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach(TasksModel m in data)
            {
                sb.Append("Taskname is: " + m.taskname + " ,ID is: " + m.id + " , description is: " + m.description + " , status is: " + m.status
                    + ", created at: " + m.date_begin + " , deadline at: " + m.date_end + "\\n");
            }
            return sb.ToString();
        }
    }

    public static class Logic
    {
        public static bool isBegin { get; set; }

        public static bool isGetUsers { get; set; }
        public static bool isGetBranches { get; set; }
        public static bool isGetTasks { get; set; }
        public static bool isGetChoosing { get; set; }

        public static bool isGet { get; set; }


        public static bool isPostUsers { get; set; }
        public static bool isPostBranches { get; set; }
        public static bool isPostTasks { get; set; }

        public static bool isPost { get; set; }


        public static bool isNew { get; set; }

        public static void setTablesFalse()
        {
            isGetUsers = false;
            isGetBranches = false;
            isGetTasks = false;
            isGetChoosing = false;

            isPostUsers = false;
            isPostBranches = false;
            isPostTasks = false;

        }



        

    }
}
