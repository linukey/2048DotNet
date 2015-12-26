using System.Windows;
using P2048.UI;
using System.Windows.Controls;
using System;
using System.Data;
using System.Media;
using System.Windows.Input;
using System.Collections;

namespace P2048
{
    public partial class MainWindow : Window
    {
        #region 构造信息

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public MainWindow(LUserInfoDataSet ds)
        {
            InitializeComponent();
            Init();

            //信息初始化
            this.ds = ds;
            DataRow row = ds.Tables["userinfo_t"].Rows[0];
            ibnUsername.Text = row["Name"].ToString();
        }

        #endregion

        #region 窗体处理
        //窗体拖动
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            int MaxScore = Convert.ToInt32(ds.Tables["userinfo_t"].Rows[0]["MaxScore"]);
            int CurScore = Convert.ToInt32(ibnScore.Text);
            if (CurScore > MaxScore)
            {
                ds.Tables["userinfo_t"].Rows[0]["MaxScore"] = Convert.ToInt32(ibnScore.Text);
                ds.Tables["userinfo_t"].Rows[0]["Flag"] = true;
                ds.WriteXml("UserInfo.xml");
            }

            Setting window = new Setting();
            window.Show();
            this.Close();
        }

        private void ibnQuit_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void ibnQuit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        #region 字段

        private ImageButton[,] board = new ImageButton[4, 4];
        LUserInfoDataSet ds = null;
        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
            }
        }

        private struct Pos
        {
            public int X { set; get; }
            public int Y { set; get; }
            public Pos(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }//用来判断是否失败的时候保存坐标信息

        #endregion

        #region 算法处理

        //初始化游戏
        private void Init()
        {
            Random r = new Random();
            int x1, x2, y1, y2;
            do
            {
                x1 = r.Next(0, 4);
                x2 = r.Next(0, 4);
                y1 = r.Next(0, 4);
                y2 = r.Next(0, 4);
            } while (x1 == x2 && y1 == y2);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (x1 == i && y1 == j) board[x1, y1] = new ImageButton(r.Next(0, 4) % 2 == 0 ? 2 : 4);
                    else if (x2 == i && y2 == j) board[x2, y2] = new ImageButton(r.Next(0, 4) % 2 == 0 ? 2 : 4);
                    else board[i, j] = new ImageButton();
                    board[i, j].Template = (ControlTemplate)FindResource("n");
                    board[i, j].MouseLeftButtonDown += Window_MouseLeftButtonDown;
                    board[i, j].Focusable = false;
                    Grid.SetColumn(board[i, j], j);
                    Grid.SetRow(board[i, j], i);
                    Board.Children.Add(board[i, j]);
                }
            }
        }
        //侦听键盘
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            SoundPlayer sound = new SoundPlayer("Audio/1.wav");
            sound.Play();

            if (CheckFalse())
            {
                MessageBox.Show("您已经走投无路！");
                return;
            }

            ImageButton[,] bbegin = new ImageButton[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    bbegin[i, j] = new ImageButton();
                    bbegin[i, j].Text = board[i, j].Text;
                }
            int indexone, indextwo;
            for (int i = 0; i < 4; i++)
            {
                if (e.Key.ToString() == "Up")
                {
                    indexone = 0;
                    indextwo = 1;
                    while (indextwo < 4) Excute(board[indexone, i], board[indextwo, i], ref indexone, ref indextwo, true);
                }
                else if (e.Key.ToString() == "Down")
                {
                    indexone = 3;
                    indextwo = 2;
                    while (indextwo >= 0) Excute(board[indexone, i], board[indextwo, i], ref indexone, ref indextwo, false);
                }
                else if (e.Key.ToString() == "Left")
                {
                    indexone = 0;
                    indextwo = 1;
                    while (indextwo < 4) Excute(board[i, indexone], board[i, indextwo], ref indexone, ref indextwo, true);
                }
                else if (e.Key.ToString() == "Right")
                {
                    indexone = 3;
                    indextwo = 2;
                    while (indextwo >= 0) Excute(board[i, indexone], board[i, indextwo], ref indexone, ref indextwo, false);
                }
            }
            ImageButton[,] eend = new ImageButton[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    eend[i, j] = new ImageButton();
                    eend[i, j].Text = board[i, j].Text;
                }
            bool judge = false;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (bbegin[i, j].Text != eend[i, j].Text) { judge = true; break; }
            if (judge) //只有当数据有改动时才产生新的数据
                CreateNew();
        }
        //代码复用，四个分支
        private void Excute(ImageButton bone, ImageButton btwo, ref int ione, ref int itwo, bool judge)
        {
            if (bone.Text == "" && btwo.Text != "") { bone.Text = btwo.Text; btwo.Text = ""; itwo = judge ? itwo + 1 : itwo - 1; }
            else if (bone.Text == "" && btwo.Text == "") itwo = judge ? itwo + 1 : itwo - 1;
            else if (bone.Text != "" && btwo.Text == "") itwo = judge ? itwo + 1 : itwo - 1;
            else if (bone.Text != "" && btwo.Text == bone.Text)
            {
                ibnScore.Text = (Score += Convert.ToInt32(bone.Text = (Convert.ToInt32(bone.Text) + Convert.ToInt32(btwo.Text)).ToString())).ToString();
                if (ibnScore.Text == "2048") MessageBox.Show("哇，好厉害，您已经成功到达2048！");
                else if (ibnScore.Text == "4096") MessageBox.Show("您已经接近神，继续加油哦！！！");
                else if (ibnScore.Text == "8192") MessageBox.Show("您已经超神！！！！");
                btwo.Text = "";
                itwo = judge ? itwo + 1 : itwo - 1;
                ione = judge ? ione + 1 : ione - 1;
            }
            else if (bone.Text != "" && btwo.Text != "" && bone.Text != btwo.Text)
            {
                if (ione == itwo - 1 && judge) { ione = judge ? ione + 1 : ione - 1; itwo = judge ? itwo + 1 : itwo - 1; }
                else if (ione == itwo + 1 && !judge) { ione = judge ? ione + 1 : ione - 1; itwo = judge ? itwo + 1 : itwo - 1; }
                else ione = judge ? ione + 1 : ione - 1;
            }
        }
        //当数据有改动时，产生新的数字
        private void CreateNew()
        {
            Random r = new Random();
            int x, y;
            do
            {
                x = r.Next(0, 4);
                y = r.Next(0, 4);
            } while (board[x, y].Text != "");
            board[x, y].Text = (r.Next(0, 4) % 2 == 0 ? 2 : 4).ToString();
        }
        //判断游戏是否失败
        private bool CheckFalse()
        {
            //满足两个条件：1.键盘全满 2.相邻两个之间没有相同的(BFS搜索一下即可)
            
            bool[,] flag = new bool[4, 4];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j].Text == "")
                        return false;
                    flag[i, j] = false;
                }

            Queue q = new Queue();
            q.Enqueue(new Pos(0, 0));

            while(q.Count != 0)
            {
                Pos p = (Pos)q.Dequeue();
                if(p.X+1 < 4 && !flag[p.X+1,p.Y])
                {
                    if (board[p.X, p.Y].Text == board[p.X + 1, p.Y].Text) return false;
                    q.Enqueue(new Pos(p.X + 1, p.Y));
                    flag[p.X + 1, p.Y] = true;
                }
                if (p.X - 1 >= 0 && !flag[p.X - 1, p.Y])
                {
                    if (board[p.X, p.Y].Text == board[p.X - 1, p.Y].Text) return false;
                    q.Enqueue(new Pos(p.X + 1, p.Y));
                    flag[p.X - 1, p.Y] = true;
                }
                if (p.Y + 1 < 4 && !flag[p.X, p.Y + 1])
                {
                    if (board[p.X, p.Y].Text == board[p.X, p.Y + 1].Text) return false;
                    q.Enqueue(new Pos(p.X, p.Y + 1));
                    flag[p.X, p.Y + 1] = true;
                }
                if (p.Y - 1 >= 0 && !flag[p.X, p.Y - 1])
                {
                    if (board[p.X, p.Y].Text == board[p.X, p.Y - 1].Text) return false;
                    q.Enqueue(new Pos(p.X, p.Y - 1));
                    flag[p.X, p.Y - 1] = true;
                }
            }

            return true;
        }

        #endregion

    }
}