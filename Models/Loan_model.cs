using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Loan
    {
        public string name { get; set; }
        public DateTime loan_date { get; set; }
        public DateTime loan_end { get; set; }


        internal Database Db { get; set; }

        public Loan()
        {
        }

        internal Loan(Database db)
        {
            Db = db;
        }

        public async Task<List<Loan>> GetAllLoans(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select name, loan_date, loan_end from book
            inner join loan on book.id_book = loan.id_book
            inner join user on loan.id_user = user.id_user
            where username = @username";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username,
            });
            return await ReturnAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Loan>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Loan>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Loan(Db)
                    {
                        name = reader.GetString(0),
                        loan_date = reader.GetDateTime(1),
                        loan_end = reader.GetDateTime(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}