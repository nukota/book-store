using BookStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore.View
{
    /// <summary>
    /// Interaction logic for SellReceipt.xaml
    /// </summary>
    public partial class SellReceipt : Window
    {
        public SellReceipt(string tenkhachhang)
        {
            InitializeComponent();
            Load(tenkhachhang);
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

        private void AddSellReceipt(object sender, RoutedEventArgs e)
        {
            if (MaHD.Text == "" || MaKH.Text == "" || NgayLap.Text == "")
            {
                MessageBox.Show("Hãy điền thông tin hóa đơn", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                HOADON _hoadon = new HOADON();
                _hoadon.MaKhachHang = int.Parse(MaKH.Text);
                _hoadon.SoHD = int.Parse(MaHD.Text);
                _hoadon.NgayLap = DateTime.Parse(NgayLap.Text);
                context.HOADON.Add(_hoadon);
                context.SaveChanges();
                Application.Current.MainWindow.Close();
            }
        }
        QuanLySachEntities context = new QuanLySachEntities();
        private void Load(string tenkhachhang)
        {
            KHACHHANG khachhang = (from b in context.KHACHHANG
                                   where b.TenKhachHang.Equals(tenkhachhang)
                                   select b).FirstOrDefault();
            MaKH.Text = khachhang.MaKhachHang.ToString();
            NgayLap.Text = DateTime.Now.ToString();
            List<HOADON> list = (from b in context.HOADON
                                 select b).ToList();
            int i = 1; bool kt = false;
            foreach (HOADON c in list)
            {
                if (c.SoHD != i)
                {
                    MaHD.Text = i.ToString();
                    kt = true;
                    break;
                }
                i++;
            }
            if (!kt) MaHD.Text = i.ToString();
        }
    }
}
