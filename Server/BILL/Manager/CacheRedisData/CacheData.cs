using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace BILL.CacheRedisData
{
    public sealed class CacheData
    {
        //Việc sử dụng hàm delegate () => new CacheData() trong trường hợp này chỉ định rõ quy trình khởi tạo đối tượng CacheData khi cần thiết
        private static readonly Lazy<CacheData> instance = new Lazy<CacheData>(() => new CacheData());
        private ConnectionMultiplexer _ConnectionMultiplexer;
        private IDatabase _Database;
        private CacheData()
        {
            _ConnectionMultiplexer = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
        }
        public static CacheData Instance
        {
            get
            {
                return instance.Value;
            }
        }
        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                return _ConnectionMultiplexer;
            }
        }
        public IDatabase Database
        {
            get
            {
                if(_Database == null)
                {
                    _Database = ConnectionMultiplexer.GetDatabase();
                }
                return _Database;   
            }
        }
        public async Task<bool> IsKeyExists(string key)
        {
            var keyvalue = key;
            return await Database.KeyExistsAsync(keyvalue);
        }

        public async Task<bool> SetString(string key, string value, int MinExpiress = 30)
        {
            var cache = Instance.Database;
            // var cache = Database;
            TimeSpan expiress = TimeSpan.FromMinutes(MinExpiress);
            if (await cache.StringSetAsync(key, value, expiress))
            {
                return true;
            }
            return false;
        }
        public async Task<string> GetString(string key, bool checkCache = true)
        {


            RedisValue _value =await Database.StringGetAsync(key);
            if (_value.HasValue)
            {
                return _value;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> SetObj(string key,object value,int MinExpiress = 30)
        {
            var cache = Instance.Database;
            TimeSpan expiress = TimeSpan.FromMinutes(MinExpiress);
            string json = JsonConvert.SerializeObject(value, Formatting.Indented);
            if (await cache.StringSetAsync(key, json, expiress))
            {
                return true;
            }
            return false;
        }
        public async Task<T> GetObj<T>(string key)  where T : new()
        {
            RedisValue redisValue =await Database.StringGetAsync(key);
            if (redisValue.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(redisValue);
            }
            else
            {
                return new T();
            }
        }
        public T GetObjs<T>(string key) where T : new()
        {
            RedisValue redisValue = Database.StringGet(key);
            if (redisValue.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(redisValue);
            }
            else
            {
                return new T();
            }
        }
        public string GenerateKey(object input, bool GenMD5 = true)
        {
            if (!GenMD5)
                return JsonConvert.SerializeObject(input);
            else
            {
                string str = JsonConvert.SerializeObject(input);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

                StringBuilder sbHash = new StringBuilder();

                foreach (byte b in bHash)
                {
                    sbHash.Append(String.Format("{0:x2}", b));
                }
                return sbHash.ToString();
            }
        }

        public void RemoveKeys(params string[] Keys)
        {
            foreach (var _key in Keys)
            {
                Database.KeyDelete(_key, CommandFlags.HighPriority);
            }
        }
        public async Task RemoveCacheResponseAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("null");
            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                await Database.KeyDeleteAsync(key);
            }
        }
        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("null");
            //connectionMultiplexer.GetEndPoints() là phương thức của đối tượng IConnectionMultiplexer trả về danh sách các EndPoint
            //trong Redis Cluster mà đối tượng này đang kết nối đến.

            // Mỗi endpoint đại diện cho một Redis server hoặc một Redis cluster node. Trong Redis cluster,
            // một endpoint có thể đại diện cho nhiều node nếu chúng được cấu hình để chia sẻ endpoint.
            foreach (var endpoint in _ConnectionMultiplexer.GetEndPoints())
            {
                //endpoint: là địa chỉ IP hoặc tên miền của một Redis instance được sử dụng trong Redis cluster

                var Server = _ConnectionMultiplexer.GetServer(endpoint);
                //Vòng lặp foreach được sử dụng để lấy từng key trong danh sách các keys tìm được
                foreach (var key in Server.Keys(pattern: pattern))
                {
                    // yield return được sử dụng để trả về giá trị của key đó dưới dạng một phần tử của một IAsyncEnumerable<string>
                    // yield return còn dùng để trả về 1 phần tử khi có yêu cầu thay vì trả về 1 danh sách
                    yield return key.ToString();
                }
            }
        }
    }
}
