using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class UploadServers
{
    public class PhotoMessages : UploadServerBase
    {
        [JsonProperty("album_id")] public int AlbumId;

        [JsonProperty("group_id")] public int GroupId;

        [JsonProperty("user_id")] public int UserId;

        public class UploadResult
        {
            [JsonProperty("hash")] public string Hash;

            [JsonProperty("photo")] public string Photo;
            [JsonProperty("server")] public int Server;
        }
    }

    public class wallphoto
    {
        public int album_id { get; set; }
        public int date { get; set; }
        public int id { get; set; }
        public int owner_id { get; set; }
        public string access_key { get; set; }
        public List<Size> sizes { get; set; }
        public string text { get; set; }
        public bool has_tags { get; set; }
    }

    public class Size
    {
        public int height { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public int width { get; set; }
    }

    public class DocumentMessages : UploadServerBase
    {
        public class UploadResult
        {
            [JsonProperty("file")] public string File;
        }

        public class SaveResult
        {
            [JsonProperty("audio_message")] public Attachment.AudioMessageAtt AudioMessage;

            [JsonProperty("doc")] public Attachment.DocumentAtt Document;

            [JsonProperty("graffiti")] public Attachment.GraffitiAtt Graffiti;
            [JsonProperty("type")] public string Type;
        }
    }

    public class UploadServerBase
    {
        [JsonProperty("upload_url")] public string UploadUrl;
    }
}