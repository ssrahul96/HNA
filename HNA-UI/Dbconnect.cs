using System.Diagnostics;
using System;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace HNA_UI
{
    public class Dbconnect
    {
        static MySqlConnection conn = null;
        string cs = @"server=localhost;port=3306;userid=ssrahul96;password=rahul1234;database=hna";
        public Dbconnect()
        {
            conn = new MySqlConnection(cs);
            conn.Open();
            Debug.WriteLine("conn state " + conn.State);
        }

        internal void putdata(string mdate, string mtime, string message, string path)
        {

            try
            {                
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO records(ddate,dtime,dmessage,dpath,dstatus) VALUES(@mdate, @mtime,@message,@path,'P')";
                cmd.Parameters.AddWithValue("@mdate", mdate);
                cmd.Parameters.AddWithValue("@mtime", mtime);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@path", path);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex.ToString());
            }finally
            {
                //conn.Close();
                Debug.WriteLine("conn state " + conn.State);
            }

        }

        public DataTable getData()
        {
            string getquery = @"select * from records";
            MySqlCommand cmd = new MySqlCommand(getquery, conn);
            DataTable dt=null;
            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                dt = new DataTable("Reports");
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                //conn.Close();
                Debug.WriteLine("conn state " + conn.State);
            }
            return dt;
        }

        public void update(Int16 id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Update records set dstatus='C' where id ="+id;
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error: {0}", e.ToString());
            }
        }
    }
}