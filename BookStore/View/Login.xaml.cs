using BookStore.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    
    public partial class Login : Window
    {
        QuanLySachEntities context = new QuanLySachEntities();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        /*
        private string _username;
        private string _password;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value;}
        }*/
       
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtbUser.Text == "" || passwordBox.Password == "")
            {
                MessageBox.Show("Hãy nhập thông tin đăng nhập");
            } else
            {
                TAIKHOAN _taikhoan = new TAIKHOAN();
                _taikhoan.tentaikhoan = txtbUser.Text;
                _taikhoan = (from m in context.TAIKHOAN
                                    where m.tentaikhoan == txtbUser.Text
                                    select m).FirstOrDefault();
                if (_taikhoan.matkhau == null || _taikhoan.matkhau != passwordBox.Password)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!");
                }
                else
                {
                    Interface home = new Interface(_taikhoan.loaitaikhoan);
                    App.Me.loaiTK = _taikhoan.loaitaikhoan;
                    home.Show();
                    this.Hide();
                } 
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
