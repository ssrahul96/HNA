


using System;
using System.Data;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HNA_UI
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        static Dbconnect db;
        public Reports()
        {
            db = new Dbconnect();
            InitializeComponent();
            this.Icon = BitmapFrame.Create(System.Windows.Application.GetResourceStream(new Uri("Hna_icon.ico", UriKind.RelativeOrAbsolute)).Stream);
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            DataTable dt = db.getData();
            dataGrid.IsHitTestVisible = false;
            dataGrid.ItemsSource = dt.DefaultView;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
