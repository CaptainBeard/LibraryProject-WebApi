using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Userdata
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int identity { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string streetaddress { get; set; }
        public string postalcode { get; set; }
        public string image { get; set; }

        internal Database Db { get; set; }

        public Userdata()
        {
        }

        internal Userdata(Database db)
        {
            Db = db;
        }

        public async Task<Userdata> FindOneAsync(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM user WHERE username = @username";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username,
            });
            var result = await ReturnAllAsync(await cmd.ExecuteReaderAsync());
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM user";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }


        public async Task<int> UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE user SET username = @username, password = @password, firstname = @firstname, lastname = @lastname, 
            phone = @phone, streetaddress = @streetaddress, postalcode = @postalcode, image = @image
            WHERE username = @username;";
            Console.WriteLine("username: " + username);
            int returnValue = await cmd.ExecuteNonQueryAsync();
            return returnValue;
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM user WHERE username = @username;";
            await cmd.ExecuteNonQueryAsync();
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
                        id_user = reader.GetInt32(0),
                        username = reader.GetString(1),
                        password = reader.GetString(2),
                        identity = reader.GetInt32(3),
                        firstname = reader.GetString(4),
                        lastname = reader.GetString(5),
                        phone = reader.GetString(6),
                        streetaddress = reader.GetString(7),
                        postalcode = reader.GetString(8),
                        image = reader.GetString(9)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}