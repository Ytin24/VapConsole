using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class BasicResponse<Type>
{
    [JsonProperty("error")] public Error Error;
    [JsonProperty("response")] public Type Response;
}

public class ItemsResponse<Type> : IItemsResponse
{
    [JsonProperty("count")] public int Count;

    [JsonProperty("items")] public List<Type> Items;

    public int post_id { get; set; }

    [JsonProperty("profiles")] public List<User> Profiles { get; set; }

    [JsonProperty("groups")] public List<Group> Groups { get; set; }
}

public interface IItemsResponse
{
    public List<User> Profiles { get; set; }
    public List<Group> Groups { get; set; }
}

public class Error
{
    [JsonProperty("captcha_img")] public string CaptchaImg;

    [JsonProperty("captcha_sid")] public string CaptchaSid;
    [JsonProperty("error_code")] public int Code;

    [JsonProperty("error_msg")] public string Message;

    [JsonProperty("request_params")] public List<Dictionary<string, string>> RequestParams;
}