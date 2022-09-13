using System.Text.RegularExpressions;
using Alika.Libs.VK.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using VkautoposterConsole;
using Group = Alika.Libs.VK.Responses.Group;
#pragma warning disable CS8602

namespace vkautopost;

internal class main
{
    private const string version = "VapC 0.3.6";

    public static string path = Environment.CurrentDirectory + "/";

    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(version);
        Console.ResetColor();

        BasicResponse<ItemsResponse<Group>> GetGroups;
        List<UserData.group> Ids = new();
        var message = "";
        var NoData = 0;
        Console.WriteLine(@"Использовать данные из настроек? * Y\n *");
        if (Console.ReadLine().ToLower() != "n")
        {
            var Data = UserData.Load().Result;
            var groups = new groups();
            if (Data == null)
            {
                Console.WriteLine("Настройки не найдены!");
                NoData = 1;
            }
            else
            {
                if (Data.Token == null || Data.Token == "")
                {
                    Console.WriteLine("Токен ненайден!");
                    new auth();
                }
                
                foreach (var groupID in Data.Groups)
                        Ids.Add(groupID);


                message = Data.Message;
                if (Data.Groups != null)
                {
                    Console.WriteLine("Список групп:");
                    var i = 1;
                    foreach (var group in Data.Groups.GroupBy(x=>x.Id).Select(x=>x.First()).ToList())
                        Console.WriteLine(i++ +
                                          @" ||| " +
                                          group.Id +
                                          '\t' +
                                          group.Name);
                }
                else
                {
                    Console.WriteLine("Группы не найдены");
                }

                Console.WriteLine(@"Хотите изменить список групп? * Y\n *");
                if (Console.ReadLine().ToLower() != "n")
                {
                    GetGroups = groups.get();
                    Ids.AddRange(Menu(GetGroups,Ids));
                }

                Console.WriteLine("Текст сообщения:");
                Console.WriteLine($"||||\n{message}\n||||");
                Console.WriteLine(@"Хотите изменить текст сообщения? * Y\n *");

                if (Console.ReadLine().ToLower() != "n")
                {
                    Console.WriteLine("* чтобы завершить ввод введите END *\nВведите текст:");
                    message = "";
                    while (true)
                    {
                        var text = Console.ReadLine();
                        if (text == "END") break;

                        message += text + '\n';
                    }

                    if (message != "") message.Remove(message.Length - 1);
                }

                StartSend(ref groups, Ids.GroupBy(x=>x.Id).Select(x=>x.First()).ToList(), message);
            }
        }
        else
        {
            NoData = 1;
        }

        if (NoData == 1)
        {
            Console.WriteLine("* чтобы завершить ввод введите END *\nВведите текст:");
            while (true)
            {
                var text = Console.ReadLine();
                if (text == "END") break;

                message += text + '\n';
            }

            if (message != "") message.Remove(message.Length - 1);
            new auth();
            var groups = new groups();
            while (true)
            {
                GetGroups = groups.get();
                if (GetGroups.Error == null)
                {
                    GetGroups = groups.get();
                    break;
                }
                else
                {
                    new gettoken().FailledToken();
                }
            }

            Console.WriteLine("Всего групп: {0}", GetGroups.Response.Count);
            Ids.AddRange(Menu(GetGroups));
            StartSend(ref groups, Ids.GroupBy(x=>x.Id).Select(x=>x.First()).ToList(), message);
        }


        Console.WriteLine(@"Сохраниить настройки? * Y\n *");
        if (Console.ReadLine().ToLower() != "n") UserData.Save(auth._Token, Ids, message);
        Console.WriteLine("Для завершения нажмите Enter...");
        Console.ReadLine();
    }

    public static void StartSend(ref groups groups, List<UserData.group> Ids, string message)
    {
        Console.Clear();
        Console.WriteLine("Задайте таймаут в секундах после публикации поста (больше лучше *желательно 5*)");
        double.TryParse(Console.ReadLine(), out var timeout);
        if (Ids.Count != 0) groups.CreatePost(message, Ids.Distinct().ToList(), timeout);
    }

    public static List<UserData.group> Menu(BasicResponse<ItemsResponse<Group>> GetGroups, List<UserData.group> Ids = null)
    {
        if (Ids == null)
        {
            Ids = new List<UserData.group>();
        }

        Ids = Ids.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        var DeleteGroup = new Regex("^DEL [0-9]*$");
        var AddGroupID = new Regex("^ADD -[0-9]*$");
        var AddGroupName = new Regex("^ADD [a-z,A-Z,а-я,А-Я,0-9]*$");
        var StartPost = new Regex("^START$");
        var ViewAll = new Regex("^ALL$");
        var Help =
            "Чтобы вызвать подсказку еще раз напишите ?\n* Чтобы прекратить ввод введите START *\n* Введите 0 чтобы сделать пост на своей стене *\nВведите ADD {id} (группы начинаются с минуса: -208919014) или введите название группы\nЧтобы увидеть список введите ALL \nЧтобы удалить группу из списка напишите DEL {позиция}";

        Console.WriteLine(Help);
        while (true)
        {
            var data = Console.ReadLine();
            if (data == null)
            {
                data = "null";
            }

            int tp;
            if (int.TryParse(data, out tp) && tp == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Beep();
                Console.WriteLine("В ПЛАНАХ!");
                Console.ResetColor();
                //Ids.Add(auth._UserId);
            }
            else if (DeleteGroup.IsMatch(data))
            {
                try
                {
                    var index = Convert.ToInt32(data.Substring(3));
                    Ids.RemoveAt(index - 1);
                    Console.WriteLine("Готово\nнапишите снова ALL чтобы увидеть список");
                }
                catch (Exception)
                {
                    Console.WriteLine("Неправильная позиция или команда!");
                }
            }
            else if (ViewAll.IsMatch(data))
            {
                var i = 1;
                if (Ids.Count != 0)
                    foreach (var ID in Ids)
                    {
                        try
                        {
                            Console.WriteLine(i +
                                              @" ||| " +
                                              ID.Id +
                                              '\t' +
                                              GetGroups.Response.Items.FirstOrDefault(x => x.Id == ID.Id)!.Name);
                        }
                        catch (Exception ex)
                        {
                            var client = new RestClient("https://api.vk.com");
                            var request = new RestRequest("method/groups.getById");

                            request.AddParameter("group_id", ID.Id * -1, ParameterType.QueryString);
                            request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
                            request.AddParameter("v", "5.131", ParameterType.QueryString);
                            var response = client.PostAsync(request).Result;
                            JObject json = JObject.Parse(response.Content);
                            Console.WriteLine(i +
                                              @" ||| " +
                                              ID.Id +
                                              '\t' +
                                              (string)json["response"][0]["screen_name"]);
                        }

                        i++;
                    }
            }

            else if (AddGroupName.IsMatch(data))
            {
                var nothing = 0;
                foreach (var group in GetGroups.Response.Items)
                    if (group.Name.Contains(data.Substring(4)))
                    {
                        nothing++;
                        Console.WriteLine(group.Name + '\t' + group.Id);
                        Ids.Add(new UserData.group
                        {
                            Name = group.Name,
                            Id = group.Id
                        });
                    }

                if (nothing == 0) Console.WriteLine("Ничего не найдено!");
            }
            else if (AddGroupID.IsMatch(data))
            {
                var id = Convert.ToInt32(data.Substring(3));
                try
                {
                    Ids.Add(new UserData.group
                    {
                        Name = GetGroups.Response.Items.FirstOrDefault(x => x.Id == id).Name,
                        Id = id
                    });
                }
                catch (Exception ex)
                {
                    var client = new RestClient("https://api.vk.com");
                    var request = new RestRequest("method/groups.getById");

                    request.AddParameter("group_id", id * -1, ParameterType.QueryString);
                    request.AddParameter("access_token", auth._Token, ParameterType.QueryString);
                    request.AddParameter("v", "5.131", ParameterType.QueryString);
                    var response = client.PostAsync(request).Result;
                    JObject json = JObject.Parse(response.Content);
                    Ids.Add(new UserData.group
                    {
                        Name = (string)json["response"][0]["screen_name"],
                        Id = id
                    });
                }
            }
            else if (StartPost.IsMatch(data))
            {
                Ids = Ids.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                break;
            }
            else if (data == "?")
            {
                Console.WriteLine(Help);
            }
            else
            {
                Console.WriteLine("Что???");
            }

            Ids = Ids.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }

        return Ids;
    }
}