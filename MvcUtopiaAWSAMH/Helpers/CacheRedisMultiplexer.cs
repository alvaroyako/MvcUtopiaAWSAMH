
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
                return ConnectionMultiplexer.Connect("cache-utopia.5srfja.ng.0001.use1.cache.amazonaws.com:6379");
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
