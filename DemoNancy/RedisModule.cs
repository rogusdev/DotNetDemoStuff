using System;
using System.Collections.Generic;
using Nancy;

namespace DemoNancy
{
    public class RedisModule : NancyModule
    {
        public RedisModule() : base("/redis")
        {
            // this is very bad REST, I know
            Get("/{key}/{value}", args =>
            {
                var key = args.key.ToString();
                var value = args.value.ToString();

                var redisConn = StackExchange.Redis.ConnectionMultiplexer.Connect("localhost");
                var redisDb = redisConn.GetDatabase();
                redisDb.StringSet(key, value);
                var dict = new Dictionary<string, string>
                {
                    {args.key, redisDb.StringGet(key)},
                    {"ticks", DateTime.UtcNow.Ticks.ToString()},
                };
                return dict;
            });
        }
    }
}
