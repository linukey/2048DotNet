using System.Windows;
using System.Windows.Input;

namespace P2048.UI
{
    /// <summary>
    /// Interaction logic for MaxScore.xaml
    /// </summary>
    public partial class MaxScore : Window
    {
        #region 构造函数

        public MaxScore()
        {
            InitializeComponent();
        }

        public MaxScore(int score)
        {
            InitializeComponent();
            ibnScore.Text = score.ToString();
        }

        #endregion

        #region 窗体处理

        //窗体拖放
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion

        #region 菜单处理

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            Setting window = new Setting();
            window.Show();
            this.Close();
        }

        #endregion
    }
}
