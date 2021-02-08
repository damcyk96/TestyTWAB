using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TestyDB
{
    class Program
    {
        const string ConnectionString =
            "Server=localhost;Database=northwind;Uid=postgres;Pwd=top123;";

        const string Sql = "SELECT * FROM order_details;";

        static readonly Func<NorthwindContext, IEnumerable<OrderDetails>> CompiledQuery =
            EF.CompileQuery((NorthwindContext ctx) =>
                ctx.OrderDetails.Select(x => x));

        static async Task Main(string[] args)
        {
            await Task.WhenAll(
                AdoNET(),
                Dapper(),
                Ef(),
                Compiled(),
                RawSql());
        }

        static async Task AdoNET()
        {
            await using var sqlConnection = new NpgsqlConnection(ConnectionString);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var command = new NpgsqlCommand(Sql, sqlConnection);
            command.Connection?.Open();
            await command.ExecuteReaderAsync();

            stopWatch.Stop();
            Console.WriteLine("ADO.NET:");
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);
        }

        static async Task Dapper()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            connection.Query(Sql);
            stopWatch.Stop();
            Console.WriteLine("Dapper");
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);

        }

        private static async Task Ef()
        {
            await using var context = new NorthwindContext();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await context.OrderDetails.AsNoTracking().Select(x => x).ToListAsync();

            Console.WriteLine("Entity Framework:");
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);
        }

        private static async Task Compiled()
        {
            await using var context = new NorthwindContext();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            CompiledQuery.Invoke(context);

            Console.WriteLine("Compiled Query:");
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);
        }

        private static async Task RawSql()
        {
            await using var context = new NpgsqlConnection(ConnectionString);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await context.QueryAsync<OrderDetails>(Sql, commandType: CommandType.Text);

            Console.WriteLine("RAW SQL:");
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);
        }
    }
}