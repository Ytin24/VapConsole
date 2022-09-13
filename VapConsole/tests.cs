using System.Net;
using Alika.Libs.VK.Responses;
using Asceils;
using Newtonsoft.Json;
using RestSharp;

namespace VkautoposterConsole;

public class tests
{
    public List<long> TEST1()
    {
        var test = new List<long>();
        for (var i = 0; i < 1; i++)
        {
            test.Add(-208919014);
            test.Add(-208918948);
            test.Add(-208759212);
        }

        return test;
    }


    public void TEST2()
    {
        for (var i = 0; i < 100; i++)
        {
            Console.WriteLine("TEST{0}", i);
            var client = new RestClient("https://api.vk.com");
            var request = new RestRequest("method/groups.create");
            request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
            request.AddParameter("title", "TESTTT" + i, ParameterType.QueryString);
            request.AddParameter("v", "5.131", ParameterType.QueryString);
            var response = client.PostAsync(request).Result;
            var answer = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<IItemsResponse>>>(response.Content);

            if (answer.Error == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(answer.Response.post_id);
                Console.WriteLine("|\tOK!\t|");
                //Console.WriteLine(a.Count+ "| \t |"+ Attachments);
                Console.ResetColor();
            }

            else if (answer.Error.CaptchaImg != null)
            {
                string captcha = null;
                var lastansw = answer;
                while (true)
                {
                    Console.WriteLine(
                        "Введите капчу (если не получается прочитать то напишите RESET для смены капчи\n");
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
                    Console.WriteLine();
                    captcha = Console.ReadLine();
                    if (captcha != "RESET" || captcha != "")
                    {
                        client = new RestClient("https://api.vk.com");
                        request = new RestRequest("method/groups.create");

                        request.AddParameter("captcha_sid", lastansw.Error.CaptchaSid, ParameterType.QueryString);
                        request.AddParameter("captcha_key", captcha, ParameterType.QueryString);
                        request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
                        request.AddParameter("title", "TESTTT" + i, ParameterType.QueryString);
                        request.AddParameter("v", "5.131", ParameterType.QueryString);
                        response = client.PostAsync(request).Result;
                        answer =
                            JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<IItemsResponse>>>(
                                response.Content);

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
            }
        }
    }

    public async void TEST3()
    {
        var a = "aaaa";
        List<UserData.group> b = new()
            { new UserData.group { Id = 111, Name = "TEST" }, new UserData.group { Id = 111, Name = "TEST2" } };
        UserData.Save(a, b, "TEST");
        var c = UserData.Load().Result;
        Console.WriteLine("TESTDATA");
        foreach (var d in c.Groups)
        {
            Console.WriteLine(d.Name);
            Console.WriteLine(d.Id);
        }


        Console.WriteLine(c.Token);

        Console.WriteLine(c.Message);
    }
}