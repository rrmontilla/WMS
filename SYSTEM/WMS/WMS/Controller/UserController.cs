using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
    public class UserController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public DataSet getAllUser()
        {
            DataSet ds = new DataSet();
            ds = wms.SelectAllUser();
            return ds;
        }
        public DataSet getAllUser2()
        {
            DataSet ds = new DataSet();
            ds = wms.SelectAllUserV2();
            return ds;
        }

        public string registerUser(UserModel model)
        { 
            string result = "";
           result = wms.RegisterUserAccount(model.FName,model.MiddleName,model.LastName,model.Address,model.City,model.MobileNumber,model.UserName,model.Password,model.position,model.Department,model.Branch,model.Signature);

            return result;
        }

        public string updateUser(UserModel model)
        {
            string result = "";
            result = wms.UpdateUser(model.MobileNumber, model.position, model.Department, model.Branch, model.Signature, model.ID, model.UserName);

            return result;
        }
        public DataTable getUserByID()
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public DataTable selectUserByID(int userid)
        {
            DataTable dt = new DataTable();
            dt = wms.SelectUserByUserID(userid).Tables[0];
            return dt;
        }

    }
}
