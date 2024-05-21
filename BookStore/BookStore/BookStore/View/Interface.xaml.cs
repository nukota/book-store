using System.Windows;
using System.Windows.Input;
using BookStore.ViewModel;
using BookStore;

namespace BookStore.View
{

    public partial class Interface : Window
    {
        public Interface()
        {
            InitializeComponent();
        }
        public Interface(string loaitaikhoan)
        {
            App.Me.loaiTK = loaitaikhoan;
            MessageBox.Show("Đăng nhập thành công", "Thông báo"); 
            InitializeComponent();
        }
        #region Window Tittle Bar
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        
    }
}
