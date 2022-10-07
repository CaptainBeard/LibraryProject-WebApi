using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace library_project
{
    public class Book
    {
        public int id_book { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string language { get; set; }
        public int year { get; set; }
        public string isbn { get; set; }
        public string image { get; set; }


        internal Database Db { get; set; }

        public Book()
        {
        }

        internal Book(Database db)
        {
            Db = db;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM book;";
            return await ReturnAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Book> FindOneAsync(int id_book)
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


        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO book (name, author, language, year, isbn, image)
            VALUES (@name, @author, @language, @year, @isbn, @image);";
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
            cmd.CommandText = @"UPDATE book SET name = @name, author = @author, language = @language, year = @year, isbn = @isbn, image = @image
            WHERE id_book = @id_book;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM book WHERE id_book = @id_book;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<Book>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Book>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Book(Db)
                    {
                        id_book = reader.GetInt32(0),
                        name = reader.GetString(1),
                        author = reader.GetString(2),
                        language = reader.GetString(3),
                        year = reader.GetInt32(4),
                        isbn = reader.GetString(5),
                        image = reader.GetString(6)
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
                ParameterName = "@id_book",
                DbType = DbType.Int32,
                Value = id_book,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@author",
                DbType = DbType.String,
                Value = author,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@language",
                DbType = DbType.String,
                Value = language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@year",
                DbType = DbType.Int32,
                Value = year,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@isbn",
                DbType = DbType.String,
                Value = isbn,
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