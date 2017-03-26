using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace HNA_UI
{
    public class NotifyService
    {

        public void notify(string path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;
            request.ContentType = "application/json";
            request.Timeout = 100000;

            request.Headers.Add("Authorization:key=AAAAAKHqqw4:APA91bHQ9-cvLTIYQ_izV2sLD3QUTNq5O3CnrV438N2lTjHTYOt6RfEzaMnWQE1FecktuqN7ncqgFigIK2pmVgmNZbY7TH7i5J5FwW_xRTDjx9H1Pp34ykBYhHMpLgWzQDln9Od8BWDV");
            
            String uid = "fb69uwWe9YA:APA91bG6BeTaNnXLHpWRgEyxSj7YNA66i3Cg3dsKPAqjVLTCuAJgW_quNSKS370uNYcPu8gBZXEgrxpg4ZTjAlYW_QmAIf6LWMXUufDXszbOpWfNweNLsd2f4MaxZ2WXLqnHdsKjEPij";
            Jsonobject1 j1 = new Jsonobject1();
            Jsonobject2 j2 = new Jsonobject2();

            j1.body = "tap for details";
            j1.title = "File Access";

            j2.priority = "normal";
            j2.to = uid;
            j2.notification = j1;


            string postData = JsonConvert.SerializeObject(j2).ToString();
            Console.WriteLine(postData);
            Debug.WriteLine(postData);
            //Console.ReadKey();

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(postData);
            request.ContentLength = byte1.Length;
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(byte1, 0, byte1.Length);
            reqStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string txtResponse = reader.ReadToEnd();

            Console.WriteLine(txtResponse);
            Debug.WriteLine(txtResponse);
            //Console.ReadKey();
        }
    }
}