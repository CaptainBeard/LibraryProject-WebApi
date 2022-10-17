using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Loandata
    {
        public int id_loan { get; set; }
        public int id_book { get; set; }
        public int id_user { get; set; }
        public DateTime loan_date { get; set; }
        public DateTime loan_end { get; set; }

        internal Database Db { get; set; }

        public Loandata()
        {
        }

        internal Loandata(Database db)
        {
            Db = db;
        }

        public async Task<Loandata> FindOneAsync(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM loan WHERE username = @username";
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

        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO loan (id_loan, id_book, id_user, loan_date, loan_end)
            VALUES (@id_loan, @id_book, @id_user, @loan_date, @loan_end);";
            BindParams(cmd);
            try
            {
                await cmd.ExecuteNonQueryAsync();
                int lastInsertId = 1;
                return lastInsertId; 
            }
            catch (System.Exception)
            {   
                return 0;
            } 
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE loan SET id_loan = @id_loan, id_book = @id_book, id_user = @id_user, loan_date = @loan_date, loan_end = @loan_end
            WHERE username = @username;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM loan WHERE id_book = @id_book;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<Loandata>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Loandata>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Loandata(Db)
                    {
                        id_loan = reader.GetInt32(0),
                        id_book = reader.GetInt32(1),
                        id_user = reader.GetInt32(2),
                        loan_date = reader.GetDateTime(4),
                        loan_end = reader.GetDateTime(5),
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
                ParameterName = "@id_loan",
                DbType = DbType.Int32,
                Value = id_loan,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {            

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_book",
                DbType = DbType.Int32,
                Value = id_book,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@loan_date",
                DbType = DbType.DateTime,
                Value = loan_date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@loan_end",
                DbType = DbType.DateTime,
                Value = loan_end,
            });
        }
    }
}