using System;
using System.Collections.Generic;
using System.Text;

namespace save_our_souls
{
    public static class Constants
    {
        public const string DatabaseFilename = "SaveOurSoulsSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create;
        
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
