using BookStore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore.View
{
    /// <summary>
    /// Interaction logic for ImportReceipt.xaml
    /// </summary>
    public partial class ImportReceipt : Window
    {
        public ImportReceipt()
        {
            InitializeComponent();
            Load();
        }
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        QuanLySachEntities context = new QuanLySachEntities();

        private void Load()
        {
            NgayLap.Text = DateTime.Now.ToString();
            List<PHIEUNHAPSACH> list = (from b in context.PHIEUNHAPSACH
                                        select b).ToList();
            int i = 0; bool kt = false;
            foreach (PHIEUNHAPSACH c in list)
            {
                if (c.SoPNS != i)
                {
                    SoPhieu.Text = i.ToString();
                    kt = true;
                    break;
                }
                i++;
            }
            if (!kt) SoPhieu.Text = i.ToString();
        }

        private void AddImportReceipt(object sender, RoutedEventArgs e)
        {
            if (SoPhieu.Text == "" || NgayLap.Text == null || NgayLap.Text == "")
            {
                MessageBox.Show("Hãy điền thông tin phiếu nhập", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
            {
                PHIEUNHAPSACH _phieunhapsach = new PHIEUNHAPSACH();
                _phieunhapsach.SoPNS = int.Parse(SoPhieu.Text);
                _phieunhapsach.NgayNhap = DateTime.Parse(NgayLap.Text);
                context.PHIEUNHAPSACH.Add(_phieunhapsach);
                context.SaveChanges();
                Application.Current.MainWindow.Close();
            }
        }
    }
}
