using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;


namespace game_market_API.Services.ExceptionLoggingService
{
    public class RedisLoggingService : IExceptionLoggingService
    {
        private readonly string _redisConnectionString;
        private ConnectionMultiplexer _redis;

        public RedisLoggingService(IConfiguration config)
        {
            _redisConnectionString = GetConnectionString(config);
        }

        private string GetConnectionString(IConfiguration config)
        {
            var redisUrlString = Environment.GetEnvironmentVariable("REDIS_URL");
            if (string.IsNullOrEmpty(redisUrlString))
            {
                var redisHost = config["Settings:Redis:Host"];
                var redisPort = Convert.ToInt32(config["Settings:Redis:Port"]);
                return $"{redisHost}:{redisPort}";
            }
            var redisUri = new Uri(redisUrlString);
            var userInfo = redisUri.UserInfo.Split(":");
            return $"{redisUri.Host}:{redisUri.Port}, password={userInfo[1]}";
        }
        
        public void Connect()
        {
            try
            {
                _redis = ConnectionMultiplexer.Connect(_redisConnectionString);
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
            var server = _redis.GetServer(_redisConnectionString);

            var keys = server.Keys();
            foreach (var key in keys)
            {
                result.Add(new KeyValuePair<string, string>(key, await Get(key)));
            }
            return string.Join('\n', result);
            
        }
    }
}