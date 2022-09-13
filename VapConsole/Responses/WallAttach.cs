namespace Alika.Libs.VK.Responses;

public class Response
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

public class RootWall
{
    public List<Response> response { get; set; }
}

public class Size
{
    public int height { get; set; }
    public string url { get; set; }
    public string type { get; set; }
    public int width { get; set; }
}