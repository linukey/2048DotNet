using System.ComponentModel;
using System.Windows.Controls;

namespace P2048.UI
{
    public class ImageButton : Button, INotifyPropertyChanged
    {
        #region 构造函数

        public ImageButton(int n = 0)
        {
            if (n != 0)
                text = n.ToString();
            else
                text = "";
        }

        public ImageButton()
        {
            text = "";
        }

        #endregion

        #region 字段

        public event PropertyChangedEventHandler PropertyChanged;

        private string imagePath;
        
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
            }
        }

        private string text;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        #endregion
    }
}
