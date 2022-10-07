using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class User
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

        public User()
        {
        }

        internal User(Database db)
        {
            Db = db;
        }

        public async Task<List<User>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM user ;";
            return await ReturnAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<User> FindOneAsync(int id_user)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM user WHERE id_user = @id_user";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
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
            cmd.CommandText = @"DELETE FROM user ";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }


        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO user (username, password, identity, firstname, lastname, phone, streetaddress, postalcode, image)
            VALUES (@username, @password, 1, @firstname, @lastname, @phone, @streetaddress, @postalcode, @image);";
            BindParams(cmd);
            try
            {
                await cmd.ExecuteNonQueryAsync();
                int LastInsertedId = (int) cmd.LastInsertedId;
                return LastInsertedId; 
            }
            catch (System.Exception ex)
            {   
                Console.WriteLine(ex);
                return 0;
            } 
        }

        public async Task<int> UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE user SET username = @username, password = @password, firstname = @firstname, lastname = @lastname, 
            phone = @phone, streetaddress = @streetaddress, postalcode = @postalcode, image = @image;";
            BindParams(cmd);
            BindId(cmd);
            Console.WriteLine("id" + id_user);
            int returnValue = await cmd.ExecuteNonQueryAsync();
            return returnValue;
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM user WHERE id_user = @id_user;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<User>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new User(Db)
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

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@identity",
                DbType = DbType.Int32,
                Value = identity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstname",
                DbType = DbType.String,
                Value = firstname,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastname",
                DbType = DbType.String,
                Value = lastname,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone",
                DbType = DbType.String,
                Value = phone,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@streetaddress",
                DbType = DbType.String,
                Value = streetaddress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@postalcode",
                DbType = DbType.String,
                Value = postalcode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@image",
                DbType = DbType.String,
                Value = image,
            });
        }
    }
}