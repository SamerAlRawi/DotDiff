using System;

namespace DotDiff.Console
{
    internal class User
    {
        [Audit]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public long Id { get; set; }
        public DateTime LastLogin { get; set; }
    }
}