using BookStore.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BookStore.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ExistReport : UserControl
    {
        public ExistReport()
        {
            InitializeComponent();
            Load();
        }
        QuanLySachEntities context = new QuanLySachEntities();
        private ObservableCollection<BAOCAOTON> getBaoCaoTon()
        {
            return new ObservableCollection<BAOCAOTON>(context.BAOCAOTON);
        }
        private void LapBaoCao(object sender, RoutedEventArgs e)
        {
            if (thang.Text != "" && nam.Text != "")
            {
                ObservableCollection<BAOCAOTON> a = getBaoCaoTon();
                ObservableCollection<BAOCAOTON> b = new ObservableCollection<BAOCAOTON>();
                foreach (BAOCAOTON c in a)
                {
                    if (c.Thang == int.Parse(thang.Text) && c.Nam == int.Parse(nam.Text))
                    {
                        b.Add(c);
                    }
                }
                dataBaoCaoTon.ItemsSource = b.ToArray();
                if (b.Count == 0)
                    MessageBox.Show("Không có báo cáo tồn trong tháng " + thang.Text + " năm " + nam.Text + "!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Hãy chọn khoảng thời gian lập báo cáo!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void Load()
        {
            dataBaoCaoTon.ItemsSource = getBaoCaoTon();
        }
        private void xuatExcel(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = show_save_dialog("Báo cáo tồn");
                if (path == "")
                {
                    throw new Exception("Đường dẫn không hợp lệ");
                }
                SaveToPng(solieu, path);
                MessageBox.Show("Xuất báo cáo tồn thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong quá trình thực hiện!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string show_save_dialog(string savename)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = savename; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|*.png|JPG (.jpg)|*.jpg"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                return filename;
            }
            return "";
        }
        private void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, fileName, encoder);
        }

        private static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
        }
    }
}
