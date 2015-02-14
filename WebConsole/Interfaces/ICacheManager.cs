using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConsole.Interfaces
{
    interface ICacheManager
    {
        void CacheAdd<T>(string key, T item, int expirationMinutes);

        void CacheAdd<T>(string key, T item);

        T CacheLoad<T>(string key);

        void Remove(string key);

        bool TryCacheLoad<T>(string key, ref T item);
    }
}
