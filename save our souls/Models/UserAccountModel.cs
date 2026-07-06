using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace save_our_souls.Models
{
    public class UserAccountModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Photo { get; set; }
    }
}
