using System.Data.Common;
using P2048.Model;
using P2048.DAL;

namespace P2048.BLL
{
    public class RUserInfoBLL
    {
        #region Add
        public bool Add(MRUserInfo m)
        {
            string cmdStr = "insert into userinfo_t(Name, Email, Phono, Office, MaxScore, Uid) values(@name,@email,@phono,@office,@maxscore,@uid)";
            DbProviderFactory factory = DbProviderFactories.GetFactory(DbHelper.providerName);
            DbParameter pName = factory.CreateParameter();
            pName.ParameterName = "@name";
            pName.Value = m.MName;
            DbParameter pEmail = factory.CreateParameter();
            pEmail.ParameterName = "@email";
            pEmail.Value = m.MEmail;
            DbParameter pPhono = factory.CreateParameter();
            pPhono.ParameterName = "@phono";
            pPhono.Value = m.MPhono;
            DbParameter pOffice = factory.CreateParameter();
            pOffice.ParameterName = "@office";
            pOffice.Value = m.MOffice;
            DbParameter pMaxScore = factory.CreateParameter();
            pMaxScore.ParameterName = "@maxscore";
            pMaxScore.Value = m.MMaxScore;
            DbParameter pUid = factory.CreateParameter();
            pUid.ParameterName = "@uid";
            pUid.Value = m.MUid;
            if (new RUserInfoDAL().Add(cmdStr, pName, pEmail, pPhono, pOffice, pMaxScore, pUid) == 1)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Update
        public bool Update(MRUserInfo m)
        {
            string cmdStr = "update userinfo_t set Name=@name, Email=@email, Phono=@phono, Office=@office, MaxScore=@maxscore where Uid=@uid";
            DbProviderFactory factory = DbProviderFactories.GetFactory(DbHelper.providerName);
            DbParameter pName = factory.CreateParameter();
            pName.ParameterName = "@name";
            pName.Value = m.MName;
            DbParameter pEmail = factory.CreateParameter();
            pEmail.ParameterName = "@email";
            pEmail.Value = m.MEmail;
            DbParameter pPhono = factory.CreateParameter();
            pPhono.ParameterName = "@phono";
            pPhono.Value = m.MPhono;
            DbParameter pOffice = factory.CreateParameter();
            pOffice.ParameterName = "@office";
            pOffice.Value = m.MOffice;
            DbParameter pMaxScore = factory.CreateParameter();
            pMaxScore.ParameterName = "@maxscore";
            pMaxScore.Value = m.MMaxScore;
            DbParameter pUid = factory.CreateParameter();
            pUid.ParameterName = "@uid";
            pUid.Value = m.MUid;
            if(new RUserInfoDAL().Update(cmdStr, pName, pEmail, pPhono, pOffice, pMaxScore, pUid) == 1)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}