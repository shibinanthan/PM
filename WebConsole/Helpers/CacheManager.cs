using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using WebConsole.Interfaces;

namespace WebConsole.Helpers
{
    public class CacheManager : ICacheManager
    {
        public CacheManager()
        {
        }

        public void CacheAdd<T>(string key, T item)
        {
          this.AddToCache<T>(key, item, 0);
        }

        public void CacheAdd<T>(string key, T item, int expirationMinutes)
        {
          this.AddToCache<T>(key, item, expirationMinutes);
        }

        public bool TryCacheLoad<T>(string key, ref T item)
        {
          object obj = HttpContext.Current.Cache[key];
          if (obj == null)
            return false;
          item = (T) obj;
          return true;
        }

        public T CacheLoad<T>(string key)
        {
          if (HttpContext.Current.Items.Contains((object) key))
            return (T) HttpContext.Current.Cache[key];
          else
            return default (T);
        }

        public void Remove(string key)
        {
          HttpContext.Current.Cache.Remove(key);
        }

        private void AddToCache<T>(string key, T item, int expirationMinutes)
        {
          DateTime absoluteExpiration = expirationMinutes <= 0 ? Cache.NoAbsoluteExpiration : DateTime.Now.AddMinutes((double) expirationMinutes);
          if (HttpContext.Current.Cache[key] != null)
            return;
          HttpContext.Current.Cache.Add(key, (object) item, (CacheDependency) null, absoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, (CacheItemRemovedCallback) null);
        }
    }
}