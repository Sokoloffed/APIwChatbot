using DBAccess;
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
using System.Text.RegularExpressions;

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
                //SendMessage(update.sender.id, "Hello, newcomer!");
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
            #region Message
            else 
            if (update.@event == "message")
            {
                if (Logic.isFilter && Logic.isFilterParameter)
                {

                    if (update.message.text == "Back")
                    {
                        /*if (Logic.isGetUsers)
                        {
                            Logic.isGetUsers = false;
                        }
                        else if (Logic.isGetBranches)
                        {
                            Logic.isGetBranches = false;
                        }
                        else if (Logic.isGetTasks)
                        {
                            Logic.isGetTasks = false;
                        }*/
                        Logic.isFilter = false;
                        Logic.isFilterParameter = false;
                        return "";
                    }
                    else
                    {
                        Regex rxUID = new Regex(@"(ID)\s.*");
                        Regex rxUs = new Regex(@"(Username)\s.*");
                        Regex rxBN = new Regex(@"(Branchname)\s.*");
                        Regex rxCA = new Regex(@"(CreatedAt)\s.*");
                        Regex rxCID = new Regex(@"(CreatorId)\s.*");
                        using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
                        {
                            Match m1 = rxUID.Match(update.message.text);
                            Match m2 = rxUs.Match(update.message.text);
                            Match m3 = rxBN.Match(update.message.text);
                            Match m4 = rxCA.Match(update.message.text);
                            Match m5 = rxCID.Match(update.message.text);
                            DataRepository data = new DataRepository();
                            string text = update.message.text.Replace(" ", String.Empty);
                            if (m1.Success)
                            {
                                string param = text.Substring(2, text.Length - 2);
                                int ID = Convert.ToInt32(param);
                                string message = ConvertUsers(modelFactory.Create(entities.Users.Where(i => i.id == ID).FirstOrDefault()));
                                SendMessage(update.sender.id, message);
                            }
                            else if (m2.Success)
                            {
                                string param = text.Substring(8, text.Length - 8);
                                string message = ConvertUsers(modelFactory.Create(entities.Users.Where(i => i.username == param).FirstOrDefault()));
                                SendMessage(update.sender.id, message);
                            }
                            else if (m3.Success)
                            {
                                string param = text.Substring(10, text.Length - 10);
                                string message = ConvertBranches(modelFactory.Create(entities.Branches.Where(i => i.branchname == param).FirstOrDefault()));
                                SendMessage(update.sender.id, message);
                            }
                            else if (m4.Success)
                            {
                                string param = text.Substring(9, text.Length - 9);

                                if (Logic.isGetTasks)
                                {
                                    string message = ConvertTasks(data.GetTaskViaBeginTime(Convert.ToDateTime(param)).ToList().Select(c => modelFactory.Create(c)));
                                    SendMessage(update.sender.id, message);
                                }
                                else if (Logic.isGetBranches)
                                {
                                    string message = ConvertBranches(data.GetBranchesViaBeginTime(Convert.ToDateTime(param)).ToList().Select(c => modelFactory.Create(c)));
                                    SendMessage(update.sender.id, message);
                                }
                                
                            }
                            else if (m5.Success)
                            {
                                string param = text.Substring(9, text.Length - 9);
                                int ID = Convert.ToInt32(param);
                                if (Logic.isGetTasks)
                                {
                                    string message = ConvertTasks(data.GetTaskViaCreatorID(ID).ToList().Select(c => modelFactory.Create(c)));
                                    SendMessage(update.sender.id, message);
                                }
                                else if (Logic.isGetBranches)
                                {
                                    string message = ConvertBranches(data.GetBranchesViaCreatorID(ID).ToList().Select(c => modelFactory.Create(c)));
                                    SendMessage(update.sender.id, message);
                                }
                                
                            }

                        }
                        SendMessage(update.sender.id, "Print Back to return");
                        return "";
                    }
                    
                   
                }

                else if((Logic.isGetUsers || Logic.isGetBranches || Logic.isGetTasks) && Logic.isFilter)
                {
                    string s = "";
                    string tmp = "";
                    if (Logic.isGetUsers)
                    {
                        s = "Users";
                        tmp = "ID, Username";
                    }
                    else if (Logic.isGetBranches)
                    {
                        s = "Branches";
                        tmp = "Branchname, CreatedAt, CreatorId";
                    }
                    else if (Logic.isGetTasks)
                    {
                        s = "Tasks";
                        tmp = "CreatorId, CreatedAt, FinishAt, Description, Status, ExecutorId";
                    }
                    Logic.isFilterParameter = true;
                    SendMessage(update.sender.id, "Possible parameters for filtering are: " + tmp + "\\n" 
                        + "Print Back to return, or enter the name of parameter, " + "\\n" 
                        + "Print name and parameter and value (w whitespace) to search");

                    

                }

                else if (Logic.isGet && Logic.isGetTasks)
                {
                    if (update.message.text == "All")
                    {
                        DataRepository repository = new DataRepository();
                        var data = repository.GetAllTasks().ToList().Select(c => modelFactory.Create(c));
                        //sb.Append(data.ElementAt(0).username);
                        //JsonConvert.SerializeObject(new { sb = sb });
                        string message = ConvertTasks(data);
                        SendMessage(update.sender.id, message);
                        return "";
                    }
                    else if (update.message.text == "Filter")
                    {
                        //Logic.isFilterParameter = true;
                        SendMessage(update.sender.id, "Oops");
                        return "";
                    }
                    else if (update.message.text == "Back")
                    {
                        Logic.isGetTasks = false;
                        Logic.isGet = false;
                        SendMessage(update.sender.id, "If you want to get data from database, print Get + Users|Branches|Tasks; " + "\\n"
                                                + "if you want to Post new data into database, print Post + Users|Branches|Tasks; " + "\\n"
                                                + "if you need help, print Help; ");
                        return "";
                    }
                    else
                    {
                        SendMessage(update.sender.id, "Print All to get all data from Branches table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
                }

                else if (Logic.isGet && Logic.isGetBranches)
                {
                    if (update.message.text == "All")
                    {
                        DataRepository repository = new DataRepository();
                        var data = repository.GetAllBranches().ToList().Select(c => modelFactory.Create(c));
                        //sb.Append(data.ElementAt(0).username);
                        //JsonConvert.SerializeObject(new { sb = sb });
                        string message = ConvertBranches(data);
                        SendMessage(update.sender.id, message);
                        return "";
                    }
                    else if (update.message.text == "Filter")
                    {
                        Logic.isFilter = true;
                        SendMessage(update.sender.id, "Press any key");
                        return "";
                    }
                    else if (update.message.text == "Back")
                    {
                        Logic.isGetBranches = false;
                        Logic.isGet = false;
                        SendMessage(update.sender.id, "If you want to get data from database, print Get + Users|Branches|Tasks; " + "\\n"
                                                + "if you want to Post new data into database, print Post + Users|Branches|Tasks; " + "\\n"
                                                + "if you need help, print Help; ");
                        return "";
                    }
                    else
                    {
                        SendMessage(update.sender.id, "Print All to get all data from Branches table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
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
                        Logic.isFilter = true;
                        SendMessage(update.sender.id, "Press any key");
                        return "";
                    }
                    else if (update.message.text == "Back")
                    {
                        Logic.isGetUsers = false;
                        Logic.isGet = false;
                        SendMessage(update.sender.id, "If you want to get data from database, print Get + Users|Branches|Tasks; " + "\\n"
                                                + "if you want to Post new data into database, print Post + Users|Branches|Tasks; " + "\\n"
                                                + "if you need help, print Help; ");
                        return "";
                    }
                    else
                    {
                        SendMessage(update.sender.id, "Print All to get all data from Users table, print Filter to start filtering, print Back to return ");
                        return "";
                    }

                }

                /*else if (Logic.isGet)
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
                }*/

                else if (!Logic.isGet && !Logic.isPost)
                {
                    Regex mGUser = new Regex(@"(Get)\s(Users)");
                    Regex mGBranches = new Regex(@"(Get)\s(Branches)");
                    Regex mGTasks = new Regex(@"(Get)\s(Tasks)");
                    Regex mPUser = new Regex(@"(Post)\s(Users)");
                    Regex mPBranches = new Regex(@"(Post)\s(Branches)");
                    Regex mPTasks = new Regex(@"(Post)\s(Tasks)");

                    Match mUg = mGUser.Match(update.message.text);
                    Match mBg = mGBranches.Match(update.message.text);
                    Match mTg = mGTasks.Match(update.message.text);
                    Match mUp = mPUser.Match(update.message.text);
                    Match mBp = mPBranches.Match(update.message.text);
                    Match mTp = mPTasks.Match(update.message.text);

                    if (mUg.Success)
                    {
                        Logic.isGetUsers = true;
                    }
                    else if (mBg.Success)
                    {
                        Logic.isGetBranches = true;
                    }
                    else if (mTg.Success)
                    {
                        Logic.isGetTasks = true;
                    }
                    if(mUg.Success || mBg.Success || mTg.Success)
                    {
                        Logic.isGet = true;
                        SendMessage(update.sender.id, "Print All to get all data from table, print Filter to start filtering, print Back to return ");
                        return "";
                    }
                    else if(mUp.Success || mBp.Success || mTp.Success)
                    {
                        Logic.isPost = true;
                        return "";
                    }
                    else SendMessage(update.sender.id, "If you want to get data from database, print Get + Users|Branches|Tasks; " + "\\n"
                        + "if you want to Post new data into database, print Post + Users|Branches|Tasks; " + "\\n"
                        + "if you need help, print Help; ");
                    return "";
                }
                else return "";
                
                //else SendMessage(update.sender.id, "Wrong command, please, print HELP to get info about posible actions. ");
            }
            #endregion Message
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

        public string ConvertUsers(UsersModel data)
        {
            return $"ID is: " + data.id + "Username is: " + data.username + "\\n";
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

        public string ConvertBranches(BranchesModel data)
        {
            return "Branchname is: " + data.branchname + ", ID is: " + data.id + ", description is: " + data.description + ", created at"
                    + data.created_date + ", ID of creator: " + data.creator_id + "\\n";
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

        public string ConvertTasks(TasksModel data)
        {
            return "Taskname is: " + data.taskname + " ,ID is: " + data.id + " , description is: " + data.description + " , status is: " + data.status
                    + ", created at: " + data.date_begin + " , deadline at: " + data.date_end + "\\n";
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
        public static bool isFilter { get; set; }
        public static bool isFilterParameter { get; set; }


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
