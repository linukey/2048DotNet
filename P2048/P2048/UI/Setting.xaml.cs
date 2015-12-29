using System.Windows;
using System.Windows.Input;
using P2048.Model;
using System.Data;
using System;
using P2048.BLL;
using System.IO;
using System.Windows.Media.Animation;
using System.Media;

namespace P2048.UI
{
    public partial class Setting : Window
    {
        #region 构造函数

        public Setting()
        {
            InitializeComponent();

            //检测用户本地数据源，有的话则读取，没有的话初始化
            if (File.Exists("./UserInfo.xml")) InitUserInfo();
            else CreateUserInfo();
        }

        #endregion

        #region 字段
        //用xml架构生成DataSet类
        LUserInfoDataSet ds = null;

        #endregion

        #region 初始化数据

        private void InitUserInfo() //读取用户信息
        {
            ds = new LUserInfoDataSet();
            ds.ReadXml("UserInfo.xml");
        }
        private void CreateUserInfo() //创建新用户信息
        {
            ds = new LUserInfoDataSet();
            DataRow row = ds.Tables["userinfo_t"].NewRow();//写入本地数据源
            row["Id"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff");
            row["Name"] = "用户名";
            row["Email"] = "Email";
            row["Phono"] = "Phono";
            row["Office"] = "Office";
            row["MaxScore"] = "0";
            row["Flag"] = false;
            ds.Tables["userinfo_t"].Rows.Add(row);
            ds.WriteXml("UserInfo.xml");

            MRUserInfo m = new MRUserInfo(); //写入远程数据源
            m.MName = "用户名";
            m.MEmail = "Email";
            m.MPhono = "Phono";
            m.MOffice = "Office";
            m.MMaxScore = "0";
            m.MUid = ds.Tables["userinfo_t"].Rows[0]["Id"].ToString();
            new RUserInfoBLL().Add(m).ToString();
        }

        #endregion

        #region 窗体处理

        //窗体拖动
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //改变鼠标手势
        private void ibnQuit_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundPlayer sound = new SoundPlayer("Audio/2.wav");
            sound.Play();
            Cursor = Cursors.Hand;
        }
        private void ibnQuit_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        #region 菜单处理
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn.Name == "ibnQuit") //退出
            {
                DataRow row = ds.Tables["userinfo_t"].Rows[0];
                if ((bool)row["Flag"])
                {
                    MRUserInfo m = new MRUserInfo();
                    m.MName = row["Name"].ToString();
                    m.MEmail = row["Email"].ToString();
                    m.MPhono = row["Phono"].ToString();
                    m.MOffice = row["Office"].ToString();
                    m.MMaxScore = row["MaxScore"].ToString();
                    m.MUid = row["Id"].ToString();
                    if (new RUserInfoBLL().Update(m)) //更新成功才修改Flag标示列，不然的话不修改，下次继续尝试修改
                    {
                        row["Flag"] = false;
                    }
                }

                this.Close();
            }
            else if (btn.Name == "ibnMaxScore") //最高分
            {
                MaxScore window = new MaxScore(Convert.ToInt32(ds.Tables["userinfo_t"].Rows[0]["MaxScore"]));
                window.Show();
                this.Close();
            }
            else if (btn.Name == "ibnStart") //开始游戏
            {
                MainWindow window = new MainWindow(ds);
                window.Show();
                this.Close();
            }
            else if (btn.Name == "ibnUserinfo") //个人信息
            {
                UserInfo window = new UserInfo(ds);
                window.Show();
                this.Close();
            }
        }
        #endregion
    }
}