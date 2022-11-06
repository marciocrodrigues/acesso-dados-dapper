﻿using System;
using System.Data;
using Microsoft.Data.SqlClient;

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
            ExecuteSqlViaDapper(connectionString);

            Console.ReadKey();
        }

        static void ExecuteSqlViaDapper(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                
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

