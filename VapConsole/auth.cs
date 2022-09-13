using System.Diagnostics;
using Alika.Libs.VK.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace VkautoposterConsole;

internal class auth
{
    public auth()
    {
        _Token = gettoken.Get();
        _UserId = new getUser().getUserId();
    }

    public static string _Token { get; set; }
    public static int _UserId { get; set; }
}

internal class gettoken
{
    private static readonly string url =
        "https://oauth.vk.com/authorize?client_id=7985481&redirect_uri=https://oauth.vk.com/blank.html&scope=offline,groups,wall,photos&response_type=token&v=5.131&https://oauth.vk.com/blank.html";


    public static string Get()
    {
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        Console.WriteLine("введите AccesToken из браузерной строки:");
        var _token = Console.ReadLine();
        return _token;
    }

    public void FailledToken()
    {
        new auth();
    }
}

internal class getUser
{
    private readonly RestClient client = new();

    public int getUserId()
    {
        var request = new RestRequest($"https://api.vk.com/method/account.getProfileInfo?access_token={auth._Token}&v=5.131");
        var response = client.GetAsync(request).Result;
        var User = JsonConvert.DeserializeObject<BasicResponse<User>>(response.Content);
        try
        {
            return User.Response.UserId;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}