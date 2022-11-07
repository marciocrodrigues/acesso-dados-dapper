using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Introducao.Models;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;

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
            ExecuteSqlViaDapper(connectionString, false, true, false);

            Console.ReadKey();
        }

        static void ExecuteSqlViaDapper(string connectionString, bool insert = false, bool consulta = false, bool many = false)
        {
            if (insert)
                ExecuteInsertCategory(connectionString);

            if (consulta)
                ExecuteListCategory(connectionString);

            if (many)
                ExecuteCreateManyCategory(connectionString);
        }

        static void ExecuteListCategory(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
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

        static void ExecuteCreateManyCategory(string connectionString)
        {
            var category = new Category();
            var category2 = new Category();

            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS 2";
            category.Url = "amazon 2";
            category.Description = "Categoria destinada a serviços do AWS 2";
            category.Order = 9;
            category.Summary = "AWS Cloud 2";
            category.Featured = false;

            category2.Id = Guid.NewGuid();
            category2.Title = "Amazon AWS 3";
            category2.Url = "amazon 3";
            category2.Description = "Categoria destinada a serviços do AWS 3";
            category2.Order = 10;
            category2.Summary = "AWS Cloud 3";
            category2.Featured = false;

            var insertSql = @"
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

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(insertSql, new[]
                {
                    new {
                    Id = category.Id,
                    Title = category.Title,
                    Url = category.Url,
                    Summary = category.Summary,
                    Order = category.Order,
                    Description = category.Description,
                    Featured = category.Featured
                },
                new
                {
                    Id = category2.Id,
                    Title = category2.Title,
                    Url = category2.Url,
                    Summary = category2.Summary,
                    Order = category2.Order,
                    Description = category2.Description,
                    Featured = category2.Featured
                }
                });

            }
        }

        static void ExecuteInsertCategory(string connectionString)
        {
            var category = new Category();
            
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"
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

            using (var connection = new SqlConnection(connectionString))
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

        static void ExecuteProcedure(string connecionString, Guid idStudent)
        {
            using (var connection = new SqlConnection(connecionString))
            {
                var procedure = "spDeleteStudent";
                var pars = new { StudentId = idStudent };
                connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

