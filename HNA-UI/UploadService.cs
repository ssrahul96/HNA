using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;

namespace HNA_UI
{
    public class UploadService
    {
        static Dbconnect db;
        string mtime;

        public void upload(string message, string path, int counttime)
        {

            NotifyService ns = new NotifyService();
            db = new Dbconnect();

            string mdate = DateTime.Now.ToString("dd-MM-yyyy");
            mtime = DateTime.Now.ToString("HH-mm-ss");

            Debug.WriteLine(mdate + "\t" + mtime);
            //Console.ReadKey();
            string address = "https://hna-test.firebaseio.com/" + mdate + "/" + mtime + ".json";

            db.putdata(mdate, mtime, message, path);

            Mdata md = new Mdata();
            md.mdate = mdate;
            md.mtime = mtime;
            md.mmessage = message;
            md.mpath = path;
            string data = JsonConvert.SerializeObject(md);

            //Debug.WriteLine("\n" + md);
            using (var client = new System.Net.WebClient())
            {
                client.UploadData(address, "PUT", Encoding.ASCII.GetBytes(data));
            }

            if (counttime == 0)
            {
                ns.notify(path);
                counttime = 1;
                Debug.WriteLine(mtime + "  " + counttime);
            }
            else
            {
                Debug.WriteLine(mtime + "  " + counttime);
            }
        }
    }
}