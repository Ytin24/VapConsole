using Newtonsoft.Json;
using vkautopost;

namespace VkautoposterConsole;

public static class UserData
{
    public static void Save(string token, List<group> groups, string message)
    {
        Console.WriteLine("Идет сохранение");
        var data = new UserDataJson
        {
            Token = token,
            Groups = groups,
            Message = message
        };
        var json = JsonConvert.SerializeObject(data);
        FileStream file;
        if (!File.Exists(main.path + "UserData.json"))
        {
            file = File.Create(main.path + "UserData.json");
        }
        else
        {
            File.Delete(main.path + "UserData.json");
            file = File.Create(main.path + "UserData.json");
        }

        file.Close();
        File.WriteAllText(main.path + "UserData.json", json);
        file.Close();
        file.Dispose();
        Console.WriteLine("Сохраненено!");
    }

    public static async Task<UserDataJson> Load()
    {
        if (File.Exists(main.path + "UserData.json"))
        {
            var json = JsonConvert.DeserializeObject<UserDataJson>(File.ReadAllText(main.path + "UserData.json"));
            auth._Token = json.Token;
            return json;
        }

        return null;
    }

    public class group
    {
        public string Name { get; set; }
        public long Id { get; set; }

    }

    public class UserDataJson
    {
        public string Token { get; set; }

        public List<group> Groups { get; set; }

        public string Message { get; set; }
    }
}