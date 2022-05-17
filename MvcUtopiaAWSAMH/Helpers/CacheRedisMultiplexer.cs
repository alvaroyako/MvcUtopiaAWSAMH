
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUtopiaAWSAMH.Helpers
{
    public static class CacheRedisMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("cacheproyectoamh.redis.cache.windows.net:6380,password=GU8zwZyBfLRR42OcbBMBJEjA8KwPNf0uUAzCaD2XHWw=,ssl=True,abortConnect=False");
            });

        public static ConnectionMultiplexer GetConnection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
