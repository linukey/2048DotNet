using System.Data.Common;
using System.Data;

namespace P2048.DAL
{
    public class RUserInfoDAL
    {
        #region Add
        public int Add(string cmdStr, params DbParameter[] paras)
        {
            return DbHelper.ExcuteNonQuery(cmdStr, paras);
        }
        #endregion

        #region Update
        public int Update(string cmdStr, params DbParameter[] paras)
        {
            return DbHelper.ExcuteNonQuery(cmdStr, paras);
        }
        #endregion

        #region Select

        public DataSet Select(string cmdStr, params DbParameter[] paras)
        {
            return DbHelper.ExcuteSetData(cmdStr, paras);
        }

        #endregion
    }
}