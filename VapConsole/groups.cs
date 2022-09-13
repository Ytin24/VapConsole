using System.Net;
using System.Text;
using Alika.Libs.VK.Responses;
using Asceils;
using Newtonsoft.Json;
using RestSharp;
using vkautopost;

namespace VkautoposterConsole;

internal class groups
{
    public BasicResponse<ItemsResponse<Group>> get()
    {
        var client = new RestClient("https://api.vk.com");
        var request = new RestRequest("method/groups.get");
        request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
        request.AddParameter("extended", "1", ParameterType.QueryString);
        request.AddParameter("v", "5.131", ParameterType.QueryString);
        var response = client.GetAsync(request).Result;
        var Groups = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<Group>>>(response.Content);
        try
        {
            return Groups;
        }
        catch (NullReferenceException)
        {
            new gettoken().FailledToken();
            return null;
        }
    }

    public void CreatePost(string msg, List<UserData.group> GroupsArr, double timeout)
    {
        var Photos = GetUploadServer(GroupsArr[0].Id);

        for (var g = 0; g < GroupsArr.Count; g++)
        {
            var client = new RestClient("https://api.vk.com");
            var request = new RestRequest("method/wall.post");
            var Attachments = "";

            request.AddParameter("message", msg, ParameterType.QueryString);
            if (Photos != null)
            {
                for (var i = 0; i < Photos.Count(); i++)
                    Attachments += $"photo{Photos[i].response[0].owner_id}_{Photos[i].response[0].id},";
                request.AddParameter("attachments", Attachments, ParameterType.QueryString);
            }

            request.AddParameter("owner_id", GroupsArr[g].Id, ParameterType.QueryString);
            request.AddParameter("signed", 1, ParameterType.QueryString);
            request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
            request.AddParameter("v", "5.131", ParameterType.QueryString);

            var response = client.PostAsync(request).Result;
            var answer = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<IItemsResponse>>>(response.Content);

            if (answer.Error == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(answer.Response.post_id);
                Console.WriteLine("|\tOK!\t|");
                Console.ResetColor();
            }

            else if (answer.Error.CaptchaImg != null)
            {
                var captcha = "";
                var lastansw = answer;
                while (true)
                {
                    Console.WriteLine("Введите капчу (если не получается прочитать то напишите RESET для смены капчи");
                    var _url = answer.Error.CaptchaImg;
                    var _HttpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
                    _HttpWebRequest.AllowWriteStreamBuffering = true;
                    _HttpWebRequest.Timeout = 20000;
                    var _WebResponse = _HttpWebRequest.GetResponse();
                    var _WebStream = _WebResponse.GetResponseStream();
                    var ANCII = PicToAscii.CreateDefault.Convert(_WebStream);
                    _WebResponse.Close();
                    _WebResponse.Close();
                    foreach (var tape in ANCII)
                    {
                        Console.ForegroundColor = tape.ForeColor;
                        Console.Write(tape.Chunk);
                    }

                    Console.ResetColor();
                    captcha = Console.ReadLine();
                    if (captcha != "RESET" || captcha != "")
                    {
                        client = new RestClient("https://api.vk.com");
                        request = new RestRequest("method/wall.post");
                        request.AddParameter("message", msg, ParameterType.QueryString);
                        if (Attachments != "")
                            request.AddParameter("attachments", Attachments, ParameterType.QueryString);
                        request.AddParameter("captcha_sid", lastansw.Error.CaptchaSid, ParameterType.QueryString);
                        request.AddParameter("captcha_key", captcha, ParameterType.QueryString);
                        request.AddParameter("owner_id", GroupsArr[g].Id, ParameterType.QueryString);
                        request.AddParameter("signed", 1, ParameterType.QueryString);
                        request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
                        request.AddParameter("v", "5.131", ParameterType.QueryString);
                        response = client.PostAsync(request).Result;
                        answer = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<IItemsResponse>>>(response.Content);

                        if (answer.Error == null) break;

                        if (answer.Error.CaptchaImg == null) break;

                        if (answer.Error.Message.ToLower() == "captcha needed")
                        {
                            lastansw = answer;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Beep();
                            Console.WriteLine(answer.Error.Message);
                            Console.ResetColor();
                            break;
                        }
                    }
                }

                Console.Clear();
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Beep();
                Console.WriteLine(answer.Error.Message);
                Console.ResetColor();
            }

            Thread.Sleep(TimeSpan.FromSeconds(timeout));
        }
    }


    private List<RootWall> GetUploadServer(long group)
    {
        var client = new RestClient("https://api.vk.com");
        var request = new RestRequest("method/photos.getWallUploadServer");
        request.AddParameter("group_id", group * -1, ParameterType.QueryString);
        request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
        request.AddParameter("v", "5.131", ParameterType.QueryString);
        var response = client.GetAsync(request).Result;
        var answer = JsonConvert.DeserializeObject<BasicResponse<UploadServers.UploadServerBase>>(response.Content);
        return saveWallPhoto(answer.Response.UploadUrl, group);
    }

    private List<RootWall> saveWallPhoto(string url, long group)
    {
        var path = main.path + "/photos/";
        var SavedPhotos = new List<RootWall>();
        List<string> photo = null;
        try
        {
            photo = Directory.EnumerateFiles(path, "*.jpg").ToList();
        }
        catch (Exception ex)
        {
        }

        if (photo == null)
        {
            Console.WriteLine("Фото не найдены! Использую отправку только текста");
            return null;
        }

        for (var i = 0; i < 10; i++)
            try
            {
                var pos = i + 1;
                var myWebClient = new WebClient();
                var aBytes = myWebClient.UploadFile(url, photo[i]);
                Console.WriteLine(pos + " Фото \n| " + photo[i] + " |\nЗагружено!\n");
                var jsonResponse = Encoding.UTF8.GetString(aBytes);
                var answer = JsonConvert.DeserializeObject<UploadServers.PhotoMessages.UploadResult>(jsonResponse);
                var client = new RestClient("https://api.vk.com");
                var request = new RestRequest("method/photos.saveWallPhoto");
                request.AddParameter("photo", answer.Photo, ParameterType.QueryString);
                request.AddParameter("server", answer.Server, ParameterType.QueryString);
                request.AddParameter("hash", answer.Hash, ParameterType.QueryString);
                request.AddParameter("group_id", group * -1, ParameterType.QueryString);
                request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
                request.AddParameter("v", "5.131", ParameterType.QueryString);
                var response = client.GetAsync(request).Result;
                var answer2 = JsonConvert.DeserializeObject<RootWall>(response.Content);
                SavedPhotos.Add(answer2);
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }

        Console.WriteLine("Все Фото загружены!");
        Thread.Sleep(1000);
        Console.Clear();
        return SavedPhotos;
    }
}