using GlobalObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Controller
{
    public class SignatoryController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public DataSet GetSignatory()
        {
            try
            {
                return wms.GetSignatory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetSignatoryByUserId(int pUserId, SignatoryType pSignatoryType, TransactionType pTransactionType)
        {
            try
            {
                int _hasRow = wms.GetSignatoryByUserId(pUserId, pTransactionType.ToString(), pSignatoryType.ToString()).Tables[0].Rows.Count;
                if (_hasRow.Equals(0))
                    throw new Exception("You have no permission of this module");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertSignatory(int pUserId, string pTransType, string pSignType)
        {
            try
            {
                return wms.InsertSignatory(pUserId, pTransType, pSignType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateSignatory(int pSignatoryId, int pUserId, string pTransType, string pSignType)
        {
            try
            {
                return wms.UpdateSignatory(pSignatoryId, pUserId, pTransType, pSignType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteSignatory(int pSignatoryId)
        {
            try
            {
                return wms.DeleteSignatory(pSignatoryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
