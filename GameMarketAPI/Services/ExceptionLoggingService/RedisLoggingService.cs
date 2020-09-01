using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;


namespace game_market_API.Services.ExceptionLoggingService
{
    public class RedisLoggingService : IExceptionLoggingService
    {
        private readonly string _redisHost;
        private readonly int _redisPort;
        private ConnectionMultiplexer _redis;

        public RedisLoggingService(IConfiguration config)
        {
            _redisHost = config["Settings:Redis:Host"];
            _redisPort = Convert.ToInt32(config["Settings:Redis:Port"]);
        }
        
        public void Connect()
        {
            try
            {
                var configString = $"{_redisHost}:{_redisPort}";
                _redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (RedisConnectionException err)
            {
                throw err;
            }
        }
        
        public async Task<bool> Set(string key, Exception exception)
        {
            var db = _redis.GetDatabase();
            var exceptionLog = $"{exception.GetType().Name}\n{exception.Message}\n{exception.StackTrace}"; 
            return await db.StringSetAsync(key, exceptionLog);
        }
        
        public async Task<string> Get(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<string> GetExceptionLog()
        {
            var result = new List<KeyValuePair<string, string>>();
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer($"{_redisHost}:{_redisPort}");

            var keys = server.Keys();
            foreach (var key in keys)
            {
                result.Add(new KeyValuePair<string, string>(key, await Get(key)));
            }
            return string.Join('\n', result);
        }
    }
}