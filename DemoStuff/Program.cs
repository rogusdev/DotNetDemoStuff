﻿using System;

//using DotNetEnv;
//using StackExchange.Redis;
//using Npgsql;
using Dapper;

namespace DemoStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello DemoStuff World!");

            DotNetEnv.Env.Load();
            var pgConnString = System.Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
            Console.WriteLine("POSTGRES_CONNECTION : {0}", pgConnString);

            // https://stackexchange.github.io/StackExchange.Redis/Basics
            Console.WriteLine("playing with redis");
            var redisConn = StackExchange.Redis.ConnectionMultiplexer.Connect("localhost");
            var redisDb = redisConn.GetDatabase();
            redisDb.StringSet("key1", "value1");
            redisDb.StringIncrement("key2", 3);
            Console.WriteLine("redis key1: {0}", redisDb.StringGet("key1"));
            Console.WriteLine("redis key2: {0}", redisDb.StringGet("key2"));
            Console.WriteLine("redis key3: {0}", redisDb.StringGet("key3"));

            // http://www.npgsql.org/doc/index.html
            Console.WriteLine("playing with npgsql");
            using (var dbConn = new Npgsql.NpgsqlConnection(pgConnString))
            {
                dbConn.Open();

                var now = DateTime.UtcNow;

                // Insert some data
                using (var cmd = new Npgsql.NpgsqlCommand())
                {
                    cmd.Connection = dbConn;
                    cmd.CommandText = "INSERT INTO data (name) VALUES (@Name)";
                    cmd.Parameters.AddWithValue("Name", $"Hello world {now}");
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("inserted into postgres: {0}", now);

                // Retrieve all rows
                using (var cmd = new Npgsql.NpgsqlCommand("SELECT name FROM data", dbConn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        Console.WriteLine(reader.GetString(0));
                Console.WriteLine("read all postgres");
            }

            // https://github.com/StackExchange/Dapper
            // http://dapper-tutorial.net/dapper
            Console.WriteLine("playing with dapper");
            using (var dbConn = new Npgsql.NpgsqlConnection(pgConnString))
            {
                dbConn.Open();
                // # https://stackoverflow.com/questions/8902674/manually-map-column-names-with-class-properties/34536863#34536863
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var now = DateTime.UtcNow;
                var id = Guid.NewGuid();

                // Insert some data
                var newThing = new Thing()
                {
                    Id = id,
                    Name = $"Hello world {now}",
                    Enabled = true,
                    CreatedAt = now.AddMinutes(-10),
                    UpdatedAt = now.AddMinutes(10),
                };
                dbConn.Execute(
                    "INSERT INTO things" +
                    " (id, name, enabled, created_at, updated_at)" +
                    " VALUES (@Id, @Name, @Enabled, @CreatedAt, @UpdatedAt)",
                    newThing
                );
                Console.WriteLine("inserted via dapper: {0}", now);

                // Retrieve all rows
                var things = dbConn.Query<Thing>("SELECT * FROM things");
                foreach (var thing in things)
                    Console.WriteLine(thing);
                Console.WriteLine("read all dapper");

                var addedThing = dbConn.QuerySingleOrDefault<Thing>(
                    "SELECT * FROM things WHERE id = @Id",
                    new { Id = id }
                );
                Console.WriteLine("added: {0}", addedThing);
            }
        }
    }

    public class Thing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Enabled)}: {Enabled}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(UpdatedAt)}: {UpdatedAt}";
        }
    }
}
