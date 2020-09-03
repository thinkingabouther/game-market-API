using System;
using System.Threading.Tasks;

namespace game_market_API.Services.ExceptionLoggingService
{
    public interface IExceptionLoggingService
    {
        public Task<bool> Set(string key, Exception exception);

        public Task<string> Get(string key);
        public Task<string> GetExceptionLog();

        public void Connect();

    }
}