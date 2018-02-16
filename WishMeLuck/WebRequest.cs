using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WishMeLuck
{
    public class WebReq
    {
        public static string WebRq(string postData, string method, string phpFile, string contentType)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);

            //WebRequest request = WebRequest.Create("http://192.168.10.191/test/webservice/" + phpFile); //Markus Laptop
            WebRequest request = WebRequest.Create("http://192.168.10.202/webservice/" + phpFile); //Pi
            request.Method = method;
            if (contentType == "json")
            {
                request.ContentType = "application/json";
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            request.ContentLength = data.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            WebResponse response = request.GetResponse();
            stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);
            string jsonStr = sr.ReadToEnd();

            return jsonStr;
        }
    }
}
