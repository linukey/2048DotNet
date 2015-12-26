namespace P2048.Model
{
    public class MLUserInfo
    {
        #region 构造函数

        public MLUserInfo()
        { }

        public MLUserInfo(int id, string name, string email, string phono, string office, string maxscore, bool flag)
        {
            this.mId = id;
            this.MName = name;
            this.mEmail = email;
            this.MPhono = phono;
            this.mOffice = office;
            this.mMaxScore = maxscore;
            this.MFlag = flag;
        }

        #endregion

        #region 字段

        private int mId;
        public int MId
        {
            get { return mId; }
            set { mId = value; }
        }

        private string mName;
        public string MName
        {
            get { return mName; }
            set { mName = value; }
        }

        private string mEmail;
        public string MEmail
        {
            get { return mEmail; }
            set { mEmail = value; }
        }

        private string mPhono;
        public string MPhono
        {
            get { return mPhono; }
            set { mPhono = value; }
        }

        private string mOffice;
        public string MOffice
        {
            get { return mOffice; }
            set { mOffice = value; }
        }

        private string mMaxScore;
        public string MMaxScore
        {
            get { return mMaxScore; }
            set { mMaxScore = value; }
        }

        private bool mFlag; //判断本地信息是否有改动
        public bool MFlag
        {
            get { return mFlag; }
            set { mFlag = value; }
        }

        #endregion
    }
}
