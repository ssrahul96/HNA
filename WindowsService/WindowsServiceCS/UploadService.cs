using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace WindowsServiceCS
{
    public class UploadService
    {
        static Dbconnect db;
        static JObject temp;

        public void upload(string mtime, string mdate, string message, string path)
        {
            temp = new JObject();

            string address = "https://hna-test.firebaseio.com/" + mdate + "/" + mtime + ".json";

            Mdata md = new Mdata();
            md.mdate = mdate;
            md.mtime = mtime;
            md.mmessage = message;
            md.mpath = path;

            //temp.Add("mdate", mdate);
            //temp.Add("mtime", mtime);
            //temp.Add("message", message);
            //temp.Add("path", path);


            string data = JsonConvert.SerializeObject(md);

            Debug.WriteLine("\n" + md);
            using (var client = new System.Net.WebClient())
            {
                client.UploadData(address, "PUT", Encoding.ASCII.GetBytes(data));
            }


        }
        public void WriteToFile(string text)
        {
            string path = "D:\\UploadLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                //writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.WriteLine(text);
                writer.Close();
            }
        }
    }


}