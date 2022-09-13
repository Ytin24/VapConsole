using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class Group
{
    private int _id;

    [JsonProperty("is_closed")] public int IsClosed;

    [JsonProperty("name")] public string Name;

    [JsonProperty("photo_100")] public string Photo100;

    [JsonProperty("photo_200")] public string Photo200;

    [JsonProperty("photo_50")] public string Photo50;

    [JsonProperty("screen_name")] public string ScreenName;

    [JsonProperty("verified")] public int Verified;

    [JsonProperty("id")]
    public int Id
    {
        get => _id;
        set => _id = value < 0 ? value : -value;
    }
}

public class GroupsResponse
{
    [JsonProperty("groups")] public List<Group> Groups;
}