using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace save_our_souls.Models
{
    class UserAccount
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
