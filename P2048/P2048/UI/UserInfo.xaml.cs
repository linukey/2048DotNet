using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Text.RegularExpressions;

namespace P2048.UI
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        #region 构造函数

        public UserInfo()
        {
            InitializeComponent();
        }

        public UserInfo(LUserInfoDataSet ds)
        {
            InitializeComponent();
            this.ds = ds;

            //初始化用户信息
            DataRow row = ds.Tables["userinfo_t"].Rows[0];
            ibnUsername.Text = row["Name"].ToString();
            ibnEmail.Text = row["Email"].ToString();
            ibnOffice.Text = row["Office"].ToString();
            ibnPhono.Text = row["Phono"].ToString();
        }

        #endregion

        #region 字段

        LUserInfoDataSet ds = null;

        #endregion

        #region 窗体处理

        //窗体拖动
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //光标失去与得到效果处理
        private void ibnUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            ImageButton ibn = (ImageButton)sender;
            if (ibn.Name == "ibnUsername") { if (ibn.Text == "用户名") ibn.Text = ""; }
            else if (ibn.Name == "ibnEmail") { if (ibn.Text == "Email") ibn.Text = ""; }
            else if (ibn.Name == "ibnPhono") { if (ibn.Text == "Phono") ibn.Text = ""; }
            else if (ibn.Name == "ibnOffice") { if (ibn.Text == "Office") ibn.Text = ""; }
        }
        private void ibnUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageButton ibn = (ImageButton)sender;
            if (ibn.Name == "ibnUsername") { if (ibn.Text == "") ibn.Text = "用户名"; }
            else if (ibn.Name == "ibnEmail")
            {
                if (ibn.Text == "") ibn.Text = "Email";
                else
                {
                    Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
                    if (r.IsMatch(ibn.Text) == false)
                    {
                        MessageBox.Show("请输入正确的邮箱格式！");
                        ibn.Text = "Email";
                    }
                }
            }
            else if (ibn.Name == "ibnPhono") { if (ibn.Text == "") ibn.Text = "Phono"; }
            else if (ibn.Name == "ibnOffice") { if (ibn.Text == "") ibn.Text = "Office"; }
        }

        #endregion

        #region 菜单处理

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkChange()) //如果信息改变的话则进行修改
            {
                DataRow row = ds.Tables["userinfo_t"].Rows[0];
                row["Name"] = ibnUsername.Text;
                row["Email"] = ibnEmail.Text;
                row["Office"] = ibnOffice.Text;
                row["Phono"] = ibnPhono.Text;
                row["Flag"] = true;
                ds.WriteXml("UserInfo.xml");
            }
            Setting window = new Setting();
            window.Show();
            this.Close();
        }

        private bool checkChange()
        {
            DataRow row = ds.Tables["userinfo_t"].Rows[0];
            if (row["Name"].ToString() != ibnUsername.Text || row["Email"].ToString() != ibnEmail.Text ||
                row["Office"].ToString() != ibnOffice.Text || row["Phono"].ToString() != ibnPhono.Text)
                return true;
            return false;
        }

        #endregion

    }
}