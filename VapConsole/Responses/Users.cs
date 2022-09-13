using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class User
{
    [JsonProperty("first_name")] public string FirstName;

    [JsonProperty("last_name")] public string LastName;

    [JsonProperty("online")] public int Online;

    [JsonProperty("online_info")] public UserOnlineInfo OnlineInfo;

    [JsonProperty("online_mobile")] public int OnlineMobile;

    [JsonProperty("photo_100")] public string Photo100;

    [JsonProperty("photo_200")] public string Photo200;

    [JsonProperty("photo_50")] public string Photo50;

    [JsonProperty("screen_name")] public string ScreenName;
    [JsonProperty("id")] public int UserId;

    [JsonProperty("verified")] public int Verified;

    public class UserOnlineInfo
    {
        [JsonProperty("is_mobile")] public bool IsMobile;

        [JsonProperty("is_online")] public bool IsOnline;

        [JsonProperty("last_seen")] public int LastSeen;
        [JsonProperty("visible")] public bool Visible;
    }
}