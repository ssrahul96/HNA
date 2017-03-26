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
                cmd.CommandText = "INSERT INTO records(ddate,dtime,dmessage,dpath) VALUES(@mdate, @mtime,@message,@path)";
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

        internal DataTable getData()
        {
            string getquery = @"select * from records";
            MySqlCommand cmd = new MySqlCommand(getquery, conn);
            DataTable dt=null;
            try
            {
                //MySqlDataReader rdr = cmd.ExecuteReader();

                //Debug.WriteLine("{0} {1} {2} {3} {4}",rdr.GetName(0), rdr.GetName(1), rdr.GetName(2), rdr.GetName(3), rdr.GetName(4));

                //while (rdr.Read())
                //{
                //    Debug.WriteLine("{0} {1} {2} {3} {4}", rdr.GetString(0),rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4));
                //}
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
    }
}