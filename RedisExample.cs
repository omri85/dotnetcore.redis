using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redis.NetCore;
using Redis.NetCore.Abstractions;
using Redis.NetCore.Configuration;

namespace dotnet
{
    class RedisExample
    {
        static IRedisClient Client = null;
        static void Main(string[] args)
        {
            Client = RedisClient.CreateClient(new RedisConfiguration(){Endpoints = new string[]{"127.0.0.1:6379"}});
            // Verify connection
            if (Client.PingAsync().Result == "PONG")
            {
                Console.WriteLine("Connected to Redis");
            }
            // Put a value with TTL of 60 seconds
            Task.WaitAll(Client.SetStringAsync("name", "Kylie", 60));
            // Get a value
            var name = Client.GetStringAsync("name").Result;
            Console.WriteLine($"The name is {name}");

            // Set many values
            Task.WaitAll(SetMultiStrings(new Dictionary<string,string>{{"a", "A"}, {"b", "B"}}));
        }

        private static async Task SetMultiStrings(IDictionary<string, string> values)
        {
            // Setting many values in cache
            await Task.WhenAll(values.Select(kv => Client.SetStringAsync(kv.Key, kv.Value)).ToList());
        }
    }
}
