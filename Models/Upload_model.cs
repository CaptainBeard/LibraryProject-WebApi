using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Upload
    {

        public string username { get; set; }
        public string image { get; set; }

        internal Database Db { get; set; }

        public Upload()
        {
        }

        internal Upload(Database db)
        {
            Db = db;
        }

        public async Task<int> UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE user SET image = @image
            WHERE username = @username;";
            Console.WriteLine("username: " + username);
            int returnValue = await cmd.ExecuteNonQueryAsync();
            return returnValue;
        }


        private async Task<List<Userdata>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Userdata>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Userdata(Db)
                    {
                        username = reader.GetString(0),
                        image = reader.GetString(1)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}