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
                if (update.message.text == "GET")
                {
                    using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
                    {
                        DataRepository repository = new DataRepository();
                        var data =  repository.GetAllUsers().ToList().Select(c => modelFactory.Create(c));
                        StringBuilder sb = new StringBuilder();
                        sb.Append(data.ElementAt(0).username);
                        //JsonConvert.SerializeObject(new { sb = sb });
                        SendMessage(update.sender.id, sb.ToString());
                    }
                }
                else if(update.message.text == "fuck")
                {
                    SendMessage(update.sender.id, "You!");
                }
                else SendMessage(update.sender.id, "Hello world (message event)!!!");
            }
            else
            {

            }

            return "";
        }

        public string SendMessage(string tmpRecipient, string tmpMessage)
        {
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
    }
}
