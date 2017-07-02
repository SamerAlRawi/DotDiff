using System;
using static System.Console;

namespace DotDiff.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = GetUser("name@domain.com", "Passw0rd", "user1", 32333, true);
            var user2 = GetUser("another_email@domain.com", "Secret123", "user2", 443455, false);
            //XML
            var xml = new XmlAuditBuilder<User>()
                .Audit(user1, user2)
                //.Include(_ => _.Email) not needed because it have [Audit] attribute
                .Include(_ => _.Password)
                .Include(_ => _.UserName)
                .Include(_ => _.Id)
                .Include(_ => _.Enabled)
                .Include(_ => _.LastLogin)
                .Serialize();
            ForegroundColor = ConsoleColor.Green;
            WriteLine(xml);

            //JSON
            var json = new JsonAuditBuilder<User>()
                .Audit(user1, user2)
                //.Include(_ => _.Email) not needed because it have [Audit] attribute
                .Include(_ => _.Password)
                .Include(_ => _.UserName)
                .Include(_ => _.Id)
                .Include(_ => _.Enabled)
                .Include(_ => _.LastLogin)
                .Serialize();
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine(json);

            ReadLine();
        }

        private static User GetUser(string email, string password, string userName, long id, bool enabled)
        {
            return new User
            {
                Email = email,
                Password = password,
                UserName = userName,
                Id = id,
                Enabled = enabled,
                LastLogin = DateTime.Now
            };
        }
    }
}
