using App.CustomConsole;

namespace App.ServerScripts
{
    public class User
    {
        public int id;
        public string name;
    }

    public static class Scripts
    {
        public static List<User> users = new List<User>();

        public static User GetUser(int id)
        {
            foreach(User user in users)
            {
                if(user.id == id)
                    return user;
            }

            return null;
        }

        public static void OnConnect(int id)
        {
            MyConsole.Info($"User [ID:{ id }] has connected.");
            User user = new User();
            user.id = id;
            users.Add(new User());
        }

        public static void OnChangeName(int id, string newName, string oldName)
        {
            GetUser(id).name = newName;
            MyConsole.Info($"User [ID:{ id }] has changed name to { newName }");
        }

        public static void OnSendMessage(int id, string message)
        {
            MyConsole.PlayerMessage(GetUser(id).name + "> ", message);
        }

        public static void OnCommand(int id, string cmd, string args)
        {
            switch(cmd)
            {
                case "test":
                    MyConsole.WriteLine($"{GetUser(id).name} is testing: { args }");
                    break;
            }
        }
    }
}
