using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Data;

namespace HNA_UI
{
    public partial class MainWindow : Window
    {
        static string path;
        Thread startthread;
        Boolean includesd;
        Boolean sd;
        static Dbconnect db;

        public MainWindow()
        {
            db = new Dbconnect();
            InitializeComponent();
            this.Icon = BitmapFrame.Create(System.Windows.Application.GetResourceStream(new Uri("Hna_icon.ico", UriKind.RelativeOrAbsolute)).Stream);
            Hide();
            Boolean b = false;

            while (b != true)
            {
                b = ldisplay();
                Debug.Write(b);
            }

            if (b)
            {
                Show();
            }
        }

        private bool ldisplay()
        {
            string strUsername = "";
            string strPassword = "";

            CredentialDialog crDiag = new CredentialDialog();
            crDiag.Content = "";
            crDiag.MainInstruction = "Please enter your username and password";
            crDiag.Target = "SampleUI";

            crDiag.ShowDialog();

            strUsername = crDiag.UserName;
            strPassword = crDiag.Password;
            Debug.Write(strUsername + strPassword);

            if (strUsername.Equals("admin") && strPassword.Equals("admin"))
            {
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid Login", "Oops");
                return false;
            }
        }

        private void button_Browse(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.ShowDialog();
            path = dlg.SelectedPath;
            Debug.WriteLine("path = " + path);
            textBox.AppendText(path);
        }

        private void subdriectory_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton rb = sender as System.Windows.Controls.RadioButton;
            Debug.Write("sd " + rb.Content);
            if (rb.Content.Equals("Yes"))
            {
                includesd = true;
            }
            
        }

        private void shutwin_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton rb = sender as System.Windows.Controls.RadioButton;
            Debug.Write("ws " + rb.Content);
            if (rb.Content.Equals("Yes"))
            {
                sd = true;
            }
            
        }

        private void button_Start(object sender, RoutedEventArgs e)
        {

            progressbar.IsIndeterminate = true;
            startthread = new Thread(startmonitor);
            startthread.Start();
            Debug.WriteLine(startthread.ThreadState);
            //listView.Items.Add("started");
        }

        private void button_Stop(object sender, RoutedEventArgs e)
        {
            progressbar.IsIndeterminate = false;
            startthread.Abort();
            Debug.WriteLine(startthread.ThreadState);

        }

        void startmonitor()
        {
     

            this.Dispatcher.Invoke(() =>
            {
                pathbox.AppendText(path + "\n");
                textBox.Text = "";
            });

            FileSystemWatcher fswatch = new FileSystemWatcher();
            fswatch.Path = path;
            fswatch.IncludeSubdirectories = includesd;
            fswatch.EnableRaisingEvents = true;


            fswatch.NotifyFilter = NotifyFilters.Attributes
               | NotifyFilters.CreationTime
               | NotifyFilters.DirectoryName
               | NotifyFilters.FileName
               | NotifyFilters.LastAccess
               | NotifyFilters.LastWrite
               | NotifyFilters.Security
               | NotifyFilters.Size;


            fswatch.Created += new FileSystemEventHandler(OnChanged);
            fswatch.Deleted += new FileSystemEventHandler(OnChanged);
            fswatch.Changed += new FileSystemEventHandler(OnChanged);
            fswatch.Renamed += new RenamedEventHandler(OnRenamed);


        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            WatcherChangeTypes wct = e.ChangeType;
            Debug.WriteLine("File {0} {1}", e.FullPath, wct.ToString());

            db.putdata(DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"), wct.ToString(), path);

            this.Dispatcher.Invoke(() =>
            {
                listView.Items.Add(new ListViewObject() { File= wct.ToString() , Path = e.FullPath,Oldname="" });
            });

            if (sd)
            {
                shutdown();
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {

            WatcherChangeTypes wct = e.ChangeType;
            Debug.WriteLine("File {0} {1} {2}", e.FullPath, wct.ToString(), e.OldName);

            db.putdata(DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"), wct.ToString(), path);
            //us.upload(e.OldName + " " + wct.ToString() + " " + e.Name, e.FullPath, countitme);

            this.Dispatcher.Invoke(() =>
            {
                listView.Items.Add(new ListViewObject() { File = wct.ToString(), Path = e.FullPath, Oldname = e.OldName });
            });
            

            if (sd)
            {
                shutdown();
            }
        }


        void shutdown()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void tab_changed(object sender, SelectionChangedEventArgs e)
        {
            Dbconnect db1 = new Dbconnect();
            DataTable dt = db1.getData();
            //dataGrid.IsHitTestVisible = false;
            dataGrid.ItemsSource = dt.DefaultView;
            NotifyService ns = new NotifyService();
            ns.notify("hi");
        }        
    }

    
}

