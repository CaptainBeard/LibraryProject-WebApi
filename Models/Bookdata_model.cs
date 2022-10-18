using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Bookdata
    {
        public string name { get; set; }
        public string author { get; set; }
        public string language { get; set; }
        public int year { get; set; }
        public string isbn { get; set; }
        public string status { get; set; }

        internal Database Db { get; set; }

        public Bookdata()
        {
        }

        internal Bookdata(Database db)
        {
            Db = db;
        }

        public async Task<List<Bookdata>> GetAllBookData()
        {
            using var cmd = Db.Connection.CreateCommand();
            
            cmd.CommandText = @"SELECT name, author, language, year, isbn,
            if(strcmp(loan.id_book,'$'), 'Loaned', 'Available') as Status
            from book left join loan on book.id_book = loan.id_book;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@status",
                DbType = DbType.String,
                Value = status,
            });
            
            return await ReturnAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Bookdata> FindOneAsync(int id_book)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM book WHERE id_book = @id_book";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_book",
                DbType = DbType.Int32,
                Value = id_book,
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

        private async Task<List<Bookdata>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Bookdata>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Bookdata(Db)
                    {
                        name = reader.GetString(0),
                        author = reader.GetString(1),
                        language = reader.GetString(2),
                        year = reader.GetInt32(3),
                        isbn = reader.GetString(4),
                        status = reader.GetString(5)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}