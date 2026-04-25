using StackExchange.Redis;
using System.Text.Json;

namespace Backend.Services
{
    public class RedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public void SetData(string key, object value, int expireMinutes = 10)
        {
            var json = JsonSerializer.Serialize(value);
            _db.StringSet(key, json, TimeSpan.FromMinutes(expireMinutes));
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
