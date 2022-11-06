using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Introducao.Models;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Introducao
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString
                = "Server=localhost,1433; Database=balta; User ID=SA; Password=Mcro@093246; TrustServerCertificate=true;";

            // Forma mais burocratica, precisa de varios comandos
            // ExecutarSql(connectionString);

            // Utilizando dapper
            ExecuteSqlViaDapper(connectionString, false);

            Console.ReadKey();
        }

        static void ExecuteSqlViaDapper(string connectionString, bool insert = false)
        {
            var category = new Category();
            string insertSql = string.Empty;

            if (insert)
            {
                category.Id = Guid.NewGuid();
                category.Title = "Amazon AWS";
                category.Url = "amazon";
                category.Description = "Categoria destinada a serviços do AWS";
                category.Order = 8;
                category.Summary = "AWS Cloud";
                category.Featured = false;

                insertSql = @"
                INSERT INTO
                    [Category]
                VALUES (
                    @Id,
                    @Title,
                    @Url,
                    @Summary,
                    @Order,
                    @Description,
                    @Featured)";
            }
            
            using (var connection = new SqlConnection(connectionString))
            {
                if (insert)
                {
                    connection.Execute(insertSql, new
                    {
                        Id = category.Id,
                        Title = category.Title,
                        Url = category.Url,
                        Summary = category.Summary,
                        Order = category.Order,
                        Description = category.Description,
                        Featured = category.Featured
                    });
                }

                // Dapper é uma extensão para executar comandos no banco
                var categories = connection.Query<Category>("SELECT [Id], [Title], [Url], [Summary], [Order], [Description], [Featured] FROM [Category]");

                if (categories.Any())
                {
                    foreach (var item in categories)
                    {
                        Console.WriteLine($"{item.Id} - {item.Title} - {item.Order} - {item.Summary}");
                    }
                }
            }
        }

        static void ExecutarSql(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado");
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT [Id], [Title] FROM [Category]";

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }
                }
            }
        }
    }
}

