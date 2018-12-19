using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieUniverse.Abstract.Utils
{
    public static class ContentType
    {
        public const string Json = "application/json; charset=utf-8";
        public const string Html = "text/html; charset=utf-8";
        public const string Plain = "text/plain; charset=utf-8";
        public const string UrlEncoded = "application/x-www-form-urlencoded; charset=UTF-8";
    }
    public static class MethodType
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "Put";
        public const string Delete = "Delete";
    }

    public static class UserAgentType
    {
        public const string Fiddler = "Mozilla";
    }

    public static class Extations
    {
        public static string GetCorrectString(this string str)
        {
            return string.Join(" ", str.Split(' ').Where(x => !string.IsNullOrEmpty(x))).ToLower().Replace("\n","").Trim();
        }

        public static void AddOrUpdate(this ConcurrentDictionary<long, string> dic, long userId, string connectionId)
        {
            if (dic.ContainsKey(userId))
            {
                dic[userId] = connectionId;
            }
            else
                dic.TryAdd(userId, connectionId);
        }
        
    }
    
}
