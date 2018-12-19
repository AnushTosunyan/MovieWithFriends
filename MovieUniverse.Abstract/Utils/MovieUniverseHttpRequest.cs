using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MovieUniverse.Abstract.Utils
{
    public static class MovieUniverseHttpRequest
    {
        public static string GetRensponse(string webAddress, string contentType, string methodType, string request = "",Encoding encoding = null, List<Cookie> cookies = null)
        {
            if (methodType == MethodType.Get && request != "")
            {
                webAddress += "?" + request;
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddress);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = methodType;
            httpWebRequest.UserAgent = UserAgentType.Fiddler;
            
            httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            if (cookies != null)
            {
                httpWebRequest.CookieContainer = new CookieContainer();
                foreach (Cookie cookie in cookies)
                {
                    httpWebRequest.CookieContainer.Add(cookie);
                }
            }

            if (methodType == MethodType.Post)
            {
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(request);
                    streamWriter.Flush();
                }
            }
            string result;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            encoding = encoding ?? Encoding.UTF8;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
