using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace wsWMS
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string Version()
        {
            return "1.0";
        }

        public DataTable ToDataTable(string xml)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            //XML Back to DataTable
            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);
            dt = ds.Tables[0]; // <--- Converted Xml to DataTable thru ds

            return dt;
        }

        public string ToXML(DataTable dt)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                using (MemoryStream memorystream = new MemoryStream())
                {
                    using (TextWriter steamwriter = new StreamWriter(memorystream))
                    {
                        XmlSerializer xmlserializer = new XmlSerializer(typeof(DataSet));
                        xmlserializer.Serialize(steamwriter, ds);
                        string xml = Encoding.UTF8.GetString(memorystream.ToArray());
                        return xml;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception(ex.Message);
            }
        }

        #region VerifyUserLogin
        [WebMethod]
        public string VerifyUserLogin(string username, string password)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.usp_ValidateUserLogin", conn);
                cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 20).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 200).Value = password;
                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();
                int ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                if(ret == 100)
                {
                    msg = "SUCCESS";
                }
                else
                {
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region SelectDepartment
        [WebMethod]
        public DataSet SelectDepartment()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.usp_SelectDepartment", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region InsertDepartment
        [WebMethod]
        public string InsertDepartment(string branchName, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.usp_InsertDepartment", conn);
                cmd.Parameters.Add("@branchName", SqlDbType.NVarChar, 50).Value = branchName;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 200).Value = status;
                cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region UpdateDepartment
        [WebMethod]
        public string UpdateDepartment(int id, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateDepartment]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region SelectBranch
        [WebMethod]
        public DataSet SelectBranch()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectBranch]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region InsertBranch
        [WebMethod]
        public string InsertBranch(string branchName, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertBranch]", conn);
                cmd.Parameters.Add("@branchName", SqlDbType.NVarChar, 50).Value = branchName;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region UpdateBranch
        [WebMethod]
        public string UpdateBranch(int id, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateBranch]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region SelectPosition
        [WebMethod]
        public DataSet SelectPosition()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectPosition]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region InsertPosition
        [WebMethod]
        public string InsertPosition(string branchName, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertPosition]", conn);
                cmd.Parameters.Add("@PosName", SqlDbType.NVarChar, 50).Value = branchName;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region UpdatePosition
        [WebMethod]
        public string UpdatePosition(int id, string status)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdatePosition]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region RegisterUserAccount
        [WebMethod]
        public string RegisterUserAccount(
            string fName,
			string mName,
			string lName,
			string addr,
			string city,
			string mNumber, 
			string username,
			string password,
			int     posID,
			int     depID,
			int     brID,
			string signature)
        {
            string msg = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertUser]", conn);
                cmd.Parameters.Add("@fName", SqlDbType.NVarChar, 100).Value =     fName;
			    cmd.Parameters.Add("@mName", SqlDbType.NVarChar, 100).Value =     mName;
			    cmd.Parameters.Add("@lName", SqlDbType.NVarChar, 100).Value =     lName;
			    cmd.Parameters.Add("@addr", SqlDbType.NVarChar, 500).Value =      addr;
			    cmd.Parameters.Add("@city", SqlDbType.NVarChar, 100).Value =      city;
			    cmd.Parameters.Add("@mNumber", SqlDbType.NVarChar, 100).Value =   mNumber; 
			    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value =  username;
			    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 500).Value =  password;
			    cmd.Parameters.Add("@posID", SqlDbType.Int).Value =      posID;
                cmd.Parameters.Add("@depID", SqlDbType.Int).Value = depID;
                cmd.Parameters.Add("@brID", SqlDbType.Int).Value = brID;
                cmd.Parameters.Add("@signature", SqlDbType.NVarChar).Value = signature;
                //cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                //cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                //cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                SqlParameter paramRetVal = new SqlParameter("@retval", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();
                int ret = Convert.ToInt32(cmd.Parameters["@retval"].Value);
                if (ret == 100)
                {
                    msg = "SUCCESS";
                }
                else if(ret == -100)
                {
                    msg = "SOMETHING WENT WRONG";
                }
                else
                {
                    msg = "USERNAME IS ALREADY EXISTS! PLEASE USE ANOTHER USERNAME. THANK YOU!";
                }

                return msg;
            }
        }
        #endregion

        #region SelectUserByUserName
        [WebMethod]
        public DataSet SelectUserByUserName(string username)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectUserByUsername]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region SelectUserByUserID
        [WebMethod]
        public DataSet SelectUserByUserID(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectUserByUserID]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region SelectAllUser
        [WebMethod]
        public DataSet SelectAllUser()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectUser]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region SelectAllUserV2
        [WebMethod]
        public DataSet SelectAllUserV2()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectUserV2]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region InsertAccessModule
        [WebMethod]
        public string InsertAccessModule(string Desc)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertAccessModule]", conn);
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = Desc;
                cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region InsertUserAccessRights
        [WebMethod]
        public string InsertUserAccessRights(int userID, int ModuleID)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertUserAccessRights]", conn);
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
                cmd.Parameters.Add("@TDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region DeleteUserAccessRights
        [WebMethod]
        public string DeleteUserAccessRights(int userID, int ModuleID)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteUserAccessRights]", conn);
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region SelectUserAccessModule
        [WebMethod]
        public DataSet SelectUserAccessModule(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectUserAccessModules]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region UpdateUser
        [WebMethod]
        public string UpdateUser(string mobileNum, int posID, int deptID, int brID, string sign, int id, string username)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateUser]", conn);
                cmd.Parameters.Add("@mobileNumber", SqlDbType.NVarChar, 15).Value = mobileNum;
                cmd.Parameters.Add("@posID", SqlDbType.Int).Value = posID;
                cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;
                cmd.Parameters.Add("@brID", SqlDbType.Int).Value = brID;
                cmd.Parameters.Add("@sign", SqlDbType.NVarChar).Value = sign;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 20).Value = username;
                //cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;

                SqlParameter paramRetVal = new SqlParameter("@retval", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();
                int ret = Convert.ToInt32(cmd.Parameters["@retval"].Value);
                if (ret == 100)
                {
                    msg = "SUCCESS";
                }
                else if (ret == -100)
                {
                    msg = "SOMETHING WENT WRONG. UNABLE TO UPDATE USER.";
                }
                else
                {
                    msg = "SOMETHING WENT WRONG.";
                }
            }
            return msg;
        }
        #endregion

        #region UpdateUserPassword
        [WebMethod]
        public string UpdateUserPassword(string NewPass, string oldPass, int id, string username)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateUserPassword]", conn);
                cmd.Parameters.Add("@newPass", SqlDbType.NVarChar, 500).Value = NewPass;
                cmd.Parameters.Add("@oldPass", SqlDbType.NVarChar, 500).Value = oldPass;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 20).Value = username;
                cmd.Parameters.Add("@UDT", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region InsertSupplier
        [WebMethod]
        public int InsertSupplier(string data)
        {
            int intqry = 0;
            int idnum = 0;
            DataTable dt = ToDataTable(data);

            idnum = GetLastCode("Supplier", "GetLastRecord");

            foreach (DataRow row in dt.Rows)
            {
                idnum++;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[usp_InsertSupplier]";

                    cmd.Parameters.Add("@supplierCode", SqlDbType.NVarChar, 20).Value = GenerateCode("SP", idnum.ToString());
                    cmd.Parameters.Add("@supplierCodeTag", SqlDbType.NVarChar, 20).Value = row["SUPPLIER CODE"].ToString().Trim();
                    cmd.Parameters.Add("@supplierName", SqlDbType.NVarChar, 500).Value = row["SUPPLIER NAME"].ToString().Trim();
                    cmd.Parameters.Add("@businessAddress", SqlDbType.NVarChar, 1000).Value = row["ADDRESS"].ToString().Trim();
                    cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 25).Value = row["TIN #"].ToString().Trim();
                    cmd.Parameters.Add("@cellNumber", SqlDbType.NVarChar, 30).Value = row["CELLPHONE NUMBER"].ToString().Trim();
                    cmd.Parameters.Add("@telNumber", SqlDbType.NVarChar, 30).Value = row["CONTACT #"].ToString().Trim();
                    cmd.Parameters.Add("@contactPerson", SqlDbType.NVarChar, 100).Value = row["CONTACT PERSON"].ToString().Trim();
                    cmd.Parameters.Add("@productsAvailed", SqlDbType.NVarChar, 500).Value = row["PRODUCTS AVAILED"].ToString().Trim();
                    cmd.Parameters.Add("@pt", SqlDbType.NVarChar, 20).Value = row["PT"].ToString().Trim();
                    cmd.Parameters.Add("@country", SqlDbType.NVarChar, 50).Value = row["COUNTRY"].ToString().Trim();
                    cmd.Parameters.Add("@supplierCurrency", SqlDbType.NVarChar, 20).Value = row["CURRENCY"].ToString().Trim();
                    SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);
                }
            }

            return intqry;
        }
        #endregion

        #region InsertAccounts
        [WebMethod]
        public int InsertAccounts(string data)
        {
            int intqry = 0;

            DataTable dt = ToDataTable(data);

            foreach (DataRow row in dt.Rows)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[usp_InsertAccounts]";

                    cmd.Parameters.Add("@acode", SqlDbType.NVarChar, 20).Value = row["ACCOUNT CODE"].ToString().Trim();
                    cmd.Parameters.Add("@pcc", SqlDbType.NVarChar, 20).Value = row["PCC"].ToString().Trim();
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, 250).Value = row["DESCRIPTION"].ToString().Trim();
                    cmd.Parameters.Add("@debitCredit", SqlDbType.Int).Value = int.Parse(row["DEBIT CREDIT"].ToString().Trim());
                    cmd.Parameters.Add("@acodeid", SqlDbType.Int).Value = int.Parse(row["ID"].ToString().Trim());
                    cmd.Parameters.Add("@sl", SqlDbType.NVarChar, 20).Value = row["SL"].ToString().Trim();
                    cmd.Parameters.Add("@groupBy", SqlDbType.NVarChar, 50).Value = row["GROUP"].ToString().Trim();
                    SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);
                }
            }

            return intqry;
        }
        #endregion

        #region InsertItems
        [WebMethod]
        public int InsertItems(string data)
        {
            int intqry = 0;
            int idnum = 0;
            DataTable dt = ToDataTable(data);

            idnum = GetLastCode("Items", "GetLastRecord");

            foreach (DataRow row in dt.Rows)
            {
                idnum++;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[usp_InsertItems]";

                    cmd.Parameters.Add("@itemCode", SqlDbType.NVarChar, 20).Value = GenerateCode("IT", idnum.ToString());
                    cmd.Parameters.Add("@itemCodeTag", SqlDbType.NVarChar, 20).Value = row["ITEM CODE"].ToString().Trim();
                    cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 500).Value = row["ITEM NAME"].ToString().Trim();
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, 1000).Value = row["DESCRIPTION"].ToString().Trim();
                    cmd.Parameters.Add("@brand", SqlDbType.NVarChar, 250).Value = row["BRAND"].ToString().Trim();
                    cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 20).Value = row["UOM"].ToString().Trim();
                    cmd.Parameters.Add("@supplierName", SqlDbType.NVarChar, 250).Value = row["SUPPLIER"].ToString().Trim();
                    cmd.Parameters.Add("@ssLevel", SqlDbType.NVarChar, 20).Value = row["SAFETY LVL"].ToString().Trim();
                    cmd.Parameters.Add("@LTDelivery", SqlDbType.NVarChar, 20).Value = row["LEAD DELIVERY"].ToString().Trim();
                    cmd.Parameters.Add("@Inventory", SqlDbType.NVarChar, 2).Value = row["INVENTORY"].ToString().Trim();
                    SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);
                }
            }

            return intqry;
        }
        #endregion

        #region SelectSupplier
        [WebMethod]
        public DataSet SelectSupplier()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectSuppliers]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region UpdateSupplier
        [WebMethod]
        public int UpdateSupplier(
            int id					
            ,string supplierCode		
            ,string supplierCodeTag	
            ,string supplierName		
            ,string businessAddress	
            ,string tin				
            ,string cellNumber		
            ,string telNumber			
            ,string contactPerson		
            ,string productsAvailed	
            ,string pt				
            ,string country			
            ,string supplierCurrency	
            )
        {
            int intqry = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_UpdateSupplier]";
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@supplierCode", SqlDbType.NVarChar, 20).Value = supplierCode;
                cmd.Parameters.Add("@supplierCodeTag", SqlDbType.NVarChar, 20).Value = supplierCodeTag;
                cmd.Parameters.Add("@supplierName", SqlDbType.NVarChar, 500).Value = supplierName;
                cmd.Parameters.Add("@businessAddress", SqlDbType.NVarChar, 1000).Value = businessAddress;
                cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 25).Value = tin;
                cmd.Parameters.Add("@cellNumber", SqlDbType.NVarChar, 30).Value = cellNumber;
                cmd.Parameters.Add("@telNumber", SqlDbType.NVarChar, 30).Value = telNumber;
                cmd.Parameters.Add("@contactPerson", SqlDbType.NVarChar, 100).Value = contactPerson;
                cmd.Parameters.Add("@productsAvailed", SqlDbType.NVarChar, 500).Value = productsAvailed;
                cmd.Parameters.Add("@pt", SqlDbType.NVarChar, 20).Value = pt;
                cmd.Parameters.Add("@country", SqlDbType.NVarChar, 50).Value = country;
                cmd.Parameters.Add("@supplierCurrency", SqlDbType.NVarChar, 20).Value = supplierCurrency;
                SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                conn.Open();

                cmd.ExecuteNonQuery();
                intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);

                conn.Close();
            }

            return intqry;
        }
        #endregion

        #region UpdateAccount
        [WebMethod]
        public int UpdateAccount(
            int id				
            ,string acode			
            ,string pcc			
            ,string description	
            ,int debitCredit	
            ,int acodeid		
            ,string sl			
            ,string groupBy	
            )
        {
            int intqry = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_UpdateAccounts]";
                cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@acode", SqlDbType.NVarChar, 20).Value = acode;
                cmd.Parameters.Add("@pcc", SqlDbType.NVarChar, 20).Value = pcc;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar, 250).Value = description;
                cmd.Parameters.Add("@debitCredit", SqlDbType.Int).Value = debitCredit;
                cmd.Parameters.Add("@acodeid", SqlDbType.Int).Value = acodeid;
                cmd.Parameters.Add("@sl", SqlDbType.NVarChar, 20).Value = sl;
                cmd.Parameters.Add("@groupBy", SqlDbType.NVarChar, 50).Value = groupBy;
                SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                conn.Open();

                cmd.ExecuteNonQuery();
                intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);

                conn.Close();
            }
            
            return intqry;
        }
        #endregion

        #region SelectAllAccount
        [WebMethod]
        public DataSet SelectAllAccount()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectAccounts]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }

        #endregion

        #region SelectAllItem
        [WebMethod]
        public DataSet SelectAllItem()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectItems]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region SelectItemPerID
        [WebMethod]
        public DataSet SelectItemPerID(int id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectItemsPerID]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region UpdateItem
        [WebMethod]
        public int UpdateItem(
            int id				
            ,string itemCode		
            ,string itemCodeTag	
            ,string itemName		
            ,string description	
            ,string brand			
            ,string unit			
            ,string supplierName	
            ,string ssLevel		
            ,string LTDelivery	
            ,string Inventory
            )
        {
            int intqry = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_UpdateItems]";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@itemCode", SqlDbType.NVarChar, 20).Value = itemCode;
                cmd.Parameters.Add("@itemCodeTag", SqlDbType.NVarChar, 20).Value = itemCodeTag;
                cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 500).Value = itemName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar, 1000).Value = description;
                cmd.Parameters.Add("@brand", SqlDbType.NVarChar, 250).Value = brand;
                cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 20).Value = unit;
                cmd.Parameters.Add("@supplierName", SqlDbType.NVarChar, 250).Value = supplierName;
                cmd.Parameters.Add("@ssLevel", SqlDbType.NVarChar, 20).Value = ssLevel;
                cmd.Parameters.Add("@LTDelivery", SqlDbType.NVarChar, 20).Value = LTDelivery;
                cmd.Parameters.Add("@Inventory", SqlDbType.NVarChar, 2).Value = Inventory;
                SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                paramRetVal.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramRetVal);

                conn.Open();

                cmd.ExecuteNonQuery();
                intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);
            }

            return intqry;
        }
        #endregion

        #region GetLastCode
        public int GetLastCode(string title, string data)
        {
            int intqry = 0;
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SelectSuppliers]";
                cmd.Parameters.Add("@title", SqlDbType.NVarChar, 20).Value = title;
                cmd.Parameters.Add("@ttype", SqlDbType.NVarChar, 20).Value = data;

                SqlDataAdapter adp;

                adp = new SqlDataAdapter(cmd);

                adp.Fill(ds);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                intqry = int.Parse(ds.Tables[0].Rows[0]["code"].ToString());
            }
            return intqry;
        }
        #endregion

        #region GenerateCode
        public string GenerateCode(string title, string data)
        {
            string code = "";
            code = title + "-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + data.ToString().PadLeft(5, '0');
            return code;
        }
        #endregion

        #region InsertRequestOrder
        [WebMethod]
        public string InsertRequestOrder(string xml1, string xml2)
        {
            string msg = "";
            string ROnum = "";
            int roID = 0;
            int ret = 0;
            
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectRONUM]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = int.Parse(row[0].ToString().Split('-')[2]) + 1;
                            string txtyear = DateTime.Now.Year.ToString().Substring(2, 2);
                            ROnum = "RO-" +  txtyear + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        ROnum = "RO-" + DateTime.Now.Year.ToString().Substring(2,2) + "-00001";
                    }
                    
                }



                foreach(DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("dbo.[usp_InsertRequestOrder]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@ROnum", SqlDbType.NVarChar, 50).Value = ROnum;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(data["UserID"].ToString());
                    cmd.Parameters.Add("@PosID", SqlDbType.Int).Value = int.Parse(data["PositionID"].ToString());
                    cmd.Parameters.Add("@BrID", SqlDbType.Int).Value = int.Parse(data["BranchID"].ToString());
                    cmd.Parameters.Add("@DepID", SqlDbType.Int).Value = int.Parse(data["DepartmentID"].ToString());
                    cmd.Parameters.Add("@stat", SqlDbType.NVarChar, 50).Value = "PENDING";
                    cmd.Parameters.Add("@Urgent", SqlDbType.Int).Value = int.Parse(data["Urgent"].ToString());
                    cmd.Parameters.Add("TargetDate", SqlDbType.DateTime).Value = DateTime.Parse(data["TargetDate"].ToString());
                    cmd.Parameters.Add("DateRequested", SqlDbType.DateTime).Value = DateTime.Parse(data["DateRequested"].ToString());
                    cmd.Parameters.Add("@EndorseID", SqlDbType.Int).Value = int.Parse(data["EndorserID"].ToString());
                    cmd.Parameters.Add("@RecomID", SqlDbType.Int).Value = int.Parse(data["RecommendersID"].ToString());
                    cmd.Parameters.Add("@apprID", SqlDbType.Int).Value = int.Parse(data["ApproverID"].ToString());
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 50).Value = data["Remarks"].ToString();
                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {
                        
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                roID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }
                if(ret == 100)
                {
                    foreach(DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertRequestOrderDetail]", conn);
                        cmd3.Parameters.Add("@roID", SqlDbType.Int).Value = roID;
                        cmd3.Parameters.Add("@itemID", SqlDbType.Int).Value = int.Parse(data["ItemID"].ToString());
                        cmd3.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 50).Value = data["ItemCode"].ToString();
                        cmd3.Parameters.Add("@ItemName", SqlDbType.NVarChar, 50).Value = data["ItemName"].ToString();
                        cmd3.Parameters.Add("@qty", SqlDbType.Int).Value = int.Parse(data["Qty"].ToString());
                        cmd3.Parameters.Add("@unit", SqlDbType.NVarChar, 50).Value = data["Unit"].ToString();
                        
                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }
                }
                
                conn.Close();
            }
            return msg;
        }
        #endregion

        #region Get_RO_Requestor
        [WebMethod]
        public DataSet Get_RO_Requestor(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Print_Transaction
        [WebMethod]
        public DataSet Print_Transaction(int userID, string RONum, string title)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_PrintTransaction]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@RONum", SqlDbType.NVarChar, 20).Value = RONum;
                    cmd.Parameters.Add("@stat", SqlDbType.NVarChar, 20).Value = title;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_BY_RONumber
        [WebMethod]
        public DataSet Get_RO_BY_RONumber(string ronum)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GETRONUM]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@ronum", SqlDbType.NVarChar, 20).Value = ronum;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_Details
        [WebMethod]
        public DataSet Get_RO_Details(int requestOrderID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = requestOrderID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_Endorser
        [WebMethod]
        public DataSet Get_RO_Endorser(int endorserID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = endorserID;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_Recommenders
        [WebMethod]
        public DataSet Get_RO_Recommenders(int recomID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = recomID;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_Approver
        [WebMethod]
        public DataSet Get_RO_Approver(int ApprID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = ApprID;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 5;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_RO_ByRONumber
        [WebMethod]
        public DataSet Get_RO_ByRONumber(int ROID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetRequest]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ROID", SqlDbType.Int).Value = ROID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 6;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region RO_UpdateForEndorse
        [WebMethod]
        public string RO_UpdateForEndorse(string ROID, int pUserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_RO_UpdateRequestOrder]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ROID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@pUserId", SqlDbType.Int).Value = pUserId;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region RO_UpdateForRecommend
        [WebMethod]
        public string RO_UpdateForRecommend(string ROID)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_RO_UpdateRequestOrder]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ROID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region RO_UpdateForApproved
        [WebMethod]
        public string RO_UpdateForApproved(string ROID, int pUserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_RO_UpdateRequestOrder]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ROID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@pUserId", SqlDbType.Int).Value = pUserId;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Select_RO_Approve
        [WebMethod]
        public DataSet Select_RO_Approve()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_RO_GetApproved]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region InsertCanvass
        [WebMethod]
        public string InsertCanvass(string xml1, string xml2)
        {
            string msg = "";
            string cnum = "";
            int cID = 0;
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopCanvasNo]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = 0;
                            try
                            {
                                asd = int.Parse(row[0].ToString().Split('-')[2]) + 1; // -00001
                            }
                            catch (Exception ex)
                            {
                                asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
                            }
                            
                            cnum = "CV-" + DateTime.Now.Year.ToString().Substring(2,2) + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        cnum = "CV-" + DateTime.Now.Year.ToString().Substring(2, 2) + "00001";
                    }

                }

                foreach (DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertCanvass]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@CanvassNumber", SqlDbType.NVarChar, 50).Value = cnum; 
                    cmd.Parameters.Add("@RONumber", SqlDbType.NVarChar, 50).Value = data["RONumber"].ToString();
                    cmd.Parameters.Add("@PositionID", SqlDbType.Int).Value = int.Parse(data["PositionID"].ToString());
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = int.Parse(data["BranchID"].ToString());
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = int.Parse(data["DeptID"].ToString());
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = "PENDING-CANVASS";//"PENDING"; --Commented by Ren | PENDING IS NOT EXIST IN CANVASS BUSINESS LOGIC
                    cmd.Parameters.Add("@Urgent", SqlDbType.Int).Value = int.Parse(data["Urgent"].ToString());
                    cmd.Parameters.Add("@CanvassorID", SqlDbType.Int).Value = int.Parse(data["CanvassorID"].ToString());
                    string ssss = data["DatePrepared"].ToString();
                    cmd.Parameters.Add("@DatePrepared", SqlDbType.DateTime).Value = DateTime.Parse(ssss);
                    cmd.Parameters.Add("@EndorserID", SqlDbType.Int).Value = int.Parse(data["EndorserID"].ToString());
                    cmd.Parameters.Add("@RecommenderID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ApproverID", SqlDbType.Int).Value = int.Parse(data["ApproverID"].ToString());
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 50).Value = data["Remarks"].ToString();
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = 0.00m;
                    cmd.Parameters.Add("@LastPrice", SqlDbType.Decimal).Value = 0.00m;
                    string ldr = data["LDR"].ToString();
                    cmd.Parameters.Add("@LDPurchase", SqlDbType.DateTime).Value = DateTime.Parse(ldr);

                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                cID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }
                if (ret == 100)
                {
                    foreach (DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertCanvassDetails]", conn);
                        cmd3.Parameters.Add("@CanvassID", SqlDbType.Int).Value = cID;
                        cmd3.Parameters.Add("@SupplierID", SqlDbType.Int).Value = int.Parse(data["SupplierID"].ToString());
                        cmd3.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["ItemID"].ToString());
                        cmd3.Parameters.Add("@Qty", SqlDbType.Int).Value = int.Parse(data["Qty"].ToString());
                        cmd3.Parameters.Add("@Unit", SqlDbType.NVarChar, 50).Value = data["Unit"].ToString();
                        cmd3.Parameters.Add("@Terms", SqlDbType.NVarChar, 50).Value = data["Terms"].ToString();
                        cmd3.Parameters.Add("@DeliveryDate", SqlDbType.NVarChar, 50).Value = data["DeliveryDate"].ToString();
                        cmd3.Parameters.Add("@Warranty", SqlDbType.NVarChar, 50).Value = data["Warranty"].ToString();
                        cmd3.Parameters.Add("@UnitCost", SqlDbType.Decimal).Value = decimal.Parse(data["UnitCost"].ToString());
                        cmd3.Parameters.Add("@Approved", SqlDbType.NVarChar, 50).Value = "NOT APPROVED";

                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }
                }

                conn.Close();
            }
            return msg;
        }
        #endregion

        #region InsertCanvassDetails
        [WebMethod]
        public string InsertCanvassDetails(string xml, int canvassID)
        {
            string msg = "";
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();

                foreach (DataRow data in ToDataTable(xml).Rows)
               {
                   SqlCommand cmd = new SqlCommand("dbo.[usp_InsertCanvassDetails]", conn);
                   cmd.Parameters.Add("@CanvassID", SqlDbType.Int).Value = canvassID;
                   cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = int.Parse(data["SupplierID"].ToString());
                   cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["ItemID"].ToString());
                   cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = int.Parse(data["Qty"].ToString());
                   cmd.Parameters.Add("@Unit", SqlDbType.NVarChar, 50).Value = data["Unit"].ToString();
                   cmd.Parameters.Add("@Terms", SqlDbType.NVarChar, 50).Value = data["Terms"].ToString();
                   cmd.Parameters.Add("@DeliveryDate", SqlDbType.NVarChar, 50).Value = data["DeliveryDate"].ToString();
                   cmd.Parameters.Add("@Warranty", SqlDbType.NVarChar, 50).Value = data["Warranty"].ToString();
                   cmd.Parameters.Add("@UnitCost", SqlDbType.Decimal).Value = decimal.Parse(data["UnitCost"].ToString());
                   cmd.Parameters.Add("@Approved", SqlDbType.NVarChar, 50).Value = "NOT APPROVED";

                   SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                   paramRetVal.Direction = ParameterDirection.Output;
                   cmd.Parameters.Add(paramRetVal);

                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.ExecuteNonQuery();
                   ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                   if (ret == 100)
                   {
                       msg = "SUCCESS";
                   }
                   else
                   {
                       msg = "FAILED";
                   }
               }
               

                conn.Close();
            }
            return msg;
        }
        #endregion

        #region Approve_CanvassItems
        [WebMethod]
        public string Approve_CanvassItems(int canID, int ItemID,int SupplierID)        //public string Approve_CanvassItems(int canID, int canvassDetailID)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCanvassDetails]", conn);
                cmd.Parameters.Add("@canvassID", SqlDbType.Int).Value = canID;
                cmd.Parameters.Add("@canvassDetailsID", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
                cmd.Parameters.Add("@SupID", SqlDbType.Int).Value = SupplierID;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Get_Canvass
        [WebMethod]
        public DataSet Get_Canvass(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetCanvass]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_Canvass_for_Noted
        [WebMethod]
        public DataSet Get_Canvass_for_Noted(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetCanvass]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_Canvass_for_Approved
        [WebMethod]
        public DataSet Get_Canvass_for_Approved(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetCanvass]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_CanvassDetailsNotApproved
        [WebMethod]
        public DataSet Get_CanvassDetailsNotApproved(int canvassID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectCanvasDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@canvasID", SqlDbType.Int).Value = canvassID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_CanvassDetailsApproved
        [WebMethod]
        public DataSet Get_CanvassDetailsApproved(int canvassID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectCanvasDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@canvasID", SqlDbType.Int).Value = canvassID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_CanvassDetailsAll
        [WebMethod]
        public DataSet Get_CanvassDetailsAll(int canvassID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectCanvasDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@canvasID", SqlDbType.Int).Value = canvassID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_CanvassDetailsAll
        [WebMethod]
        public DataSet Get_CanvassDetailsAll1(int canvassID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectCanvasDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@canvasID", SqlDbType.Int).Value = canvassID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region GetAllItems
        [WebMethod] 
        public string GetAllItems()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[usp_SelectItems]";

                SqlDataAdapter adp;

                adp = new SqlDataAdapter(cmd);

                adp.Fill(dt);
            }

            return ToXML(dt);
        }
        #endregion

        #region GetAllData
        [WebMethod]
        public string GetAllData(string title, string ttype)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SelectSuppliersReport]";
                cmd.Parameters.Add("@title", SqlDbType.NVarChar, 20).Value = title;
                cmd.Parameters.Add("@ttype", SqlDbType.NVarChar, 20).Value = ttype;

                SqlDataAdapter adp;

                adp = new SqlDataAdapter(cmd);

                adp.Fill(dt);
            }

            return ToXML(dt);
        }
        #endregion

        #region InsertConstruction
        [WebMethod]
        public int InsertConstruction(string data)
        {
            int intqry = 0;
            int idnum = 0;

            string code = "";
            string oldcode = "";
            DataTable dt = ToDataTable(data);

            idnum = GetLastCode("Construction", "GetLastRecord");
            idnum++;
            code = GenerateCode("CC", idnum.ToString());

            foreach (DataRow row in dt.Rows)
            {
                if (oldcode == "")
                {

                }
                else if (oldcode != row["ItemCode"].ToString().Trim())
                {
                    idnum++;
                    code = GenerateCode("CC", idnum.ToString());
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
                {

                    
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[InsertConstructions]";

                    cmd.Parameters.Add("@code", SqlDbType.NVarChar, 20).Value = code.ToString().Trim();
                    cmd.Parameters.Add("@itemCode", SqlDbType.NVarChar, 20).Value = row["ItemCode"].ToString().Trim();
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, 500).Value = row["Description"].ToString().Trim();
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = int.Parse(row["Quantity"].ToString().Trim());
                    cmd.Parameters.Add("@unit", SqlDbType.NVarChar, (15)).Value = row["Unit"].ToString().Trim();
                    cmd.Parameters.Add("@groupCode", SqlDbType.NVarChar, 20).Value = row["GroupCode"].ToString().Trim();
                    SqlParameter paramRetVal = new SqlParameter("@retVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    intqry = Convert.ToInt32(cmd.Parameters["@retVal"].Value);
                }

                oldcode = row["ItemCode"].ToString();
            }
             
            return intqry;
        }
        #endregion

        #region Update_Canvass_Noted
        [WebMethod]
        public string Update_Canvass_Noted(int canvassID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCanvass]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = canvassID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = UserId;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Update_Canvass_Approved
        [WebMethod]
        public string Update_Canvass_Approved(int canvassID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCanvass]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = canvassID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = UserId;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Select_Canvass_Approved
        [WebMethod]
        public DataSet Select_Canvass_Approved()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_CANVASS_GetApproved]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        //#region InsertPurchaseOrder
        //[WebMethod]
        //public string InsertPurchaseOrder(string xml1, string xml2)
        //{
        //    string msg = "";
        //    string POnum = "";
        //    int poID = 0;
        //    int ret = 0;

        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopPONo]", conn))
        //        using (var da1 = new SqlDataAdapter(cmd2))
        //        {
        //            DataSet ds = new DataSet();
        //            cmd2.CommandType = CommandType.StoredProcedure;
        //            da1.Fill(ds);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow row in ds.Tables[0].Rows)
        //                {
        //                    int asd = 0;
        //                    try
        //                    {
        //                        asd = int.Parse(row[0].ToString().Split('-')[1]) + 1; // -00001
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
        //                    }

        //                    POnum = "PO" + DateTime.Now.Year.ToString().Substring(2,2) + asd.ToString().PadLeft(5, '0');
        //                }
        //            }
        //            else
        //            {
        //                POnum = "PO" + DateTime.Now.Year.ToString().Substring(2, 2) + "00001";
        //            }

        //        }

        //        foreach (DataRow data in ToDataTable(xml1).Rows)
        //        {
        //            SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertPurchaseOrder]", conn);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            cmd.Parameters.Add("@PONumber", SqlDbType.NVarChar, 50).Value = POnum;
        //            cmd.Parameters.Add("@canvassNumber", SqlDbType.NVarChar, 50).Value = data["canvassNumber"].ToString();
        //            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(data["userID"].ToString());
        //            cmd.Parameters.Add("@positionID", SqlDbType.Int).Value = int.Parse(data["positionID"].ToString());
        //            cmd.Parameters.Add("@branchID", SqlDbType.Int).Value = int.Parse(data["branchID"].ToString());
        //            cmd.Parameters.Add("@departmentID", SqlDbType.Int).Value = int.Parse(data["departmentID"].ToString());
        //            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = "PENDING";
        //            cmd.Parameters.Add("@urgent", SqlDbType.NVarChar, 50).Value = data["urgent"].ToString();
        //            cmd.Parameters.Add("@deliveryVia", SqlDbType.NVarChar, 50).Value = data["deliveryVia"].ToString();
        //            cmd.Parameters.Add("@paymentCondition", SqlDbType.NVarChar, 50).Value = data["paymentCondition"].ToString();
        //            cmd.Parameters.Add("@DeliveryTo", SqlDbType.NVarChar, 150).Value = data["DeliveryTo"].ToString();
        //            cmd.Parameters.Add("@requestorID", SqlDbType.Int).Value = null;
        //            cmd.Parameters.Add("@DateRequested", SqlDbType.DateTime).Value = int.Parse(data["DateRequested"].ToString());
        //            cmd.Parameters.Add("@endorserID", SqlDbType.Int).Value = int.Parse(data["endorserID"].ToString());
        //            cmd.Parameters.Add("@recommendersID", SqlDbType.Int).Value = null;
        //            cmd.Parameters.Add("@approversID", SqlDbType.Int).Value = int.Parse(data["approversID"].ToString());
        //            cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 50).Value = data["remarks"].ToString();

        //            SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
        //            paramRetVal.Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add(paramRetVal);

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            //cmd.ExecuteNonQuery();
        //            DataSet ds = new DataSet();
        //            da.Fill(ds);
        //            ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
        //            if (ret == 100)
        //            {

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow row in ds.Tables[0].Rows)
        //                    {
        //                        poID = int.Parse(row[0].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        if (ret == 100)
        //        {
        //            foreach (DataRow data in ToDataTable(xml2).Rows)
        //            {
        //                SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertPurchaseOrderDetails]", conn);
        //                cmd3.Parameters.Add("@POID", SqlDbType.Int).Value = poID;
        //                cmd3.Parameters.Add("@canvassNumber", SqlDbType.NVarChar, 50).Value = data["canvassNumber"].ToString();
        //                cmd3.Parameters.Add("@supplierID", SqlDbType.Int).Value = int.Parse(data["supplierID"].ToString());
        //                cmd3.Parameters.Add("@itemID", SqlDbType.Int).Value = int.Parse(data["itemID"].ToString());
        //                cmd3.Parameters.Add("@quantity", SqlDbType.Int).Value = int.Parse(data["quantity"].ToString());
        //                cmd3.Parameters.Add("@unitMeasure", SqlDbType.NVarChar, 50).Value = data["unitMeasure"].ToString();
        //                cmd3.Parameters.Add("@terms", SqlDbType.NVarChar, 50).Value = data["terms"].ToString();
        //                cmd3.Parameters.Add("@DeliveryDate", SqlDbType.NVarChar, 50).Value = data["DeliveryDate"].ToString();
        //                cmd3.Parameters.Add("@Warranty", SqlDbType.NVarChar, 100).Value = data["Warranty"].ToString();
        //                cmd3.Parameters.Add("@unitCost", SqlDbType.Decimal).Value = decimal.Parse(data["unitCost"].ToString());
        //                cmd3.Parameters.Add("@approved", SqlDbType.Int).Value = int.Parse(data["approved"].ToString());

        //                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
        //                paramRetVal.Direction = ParameterDirection.Output;
        //                cmd3.Parameters.Add(paramRetVal);

        //                cmd3.CommandType = CommandType.StoredProcedure;
        //                cmd3.ExecuteNonQuery();
        //                ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
        //                if (ret == 100)
        //                {
        //                    msg = "SUCCESS";
        //                }
        //                else
        //                {
        //                    msg = "FAILED";
        //                }
        //            }
        //        }

        //        conn.Close();
        //    }
        //    return msg;
        //}
        //#endregion

        #region SelectSummaryReport
        [WebMethod]
        public DataSet SelectSummaryReport(string title, DateTime tdt1, DateTime tdt2, string id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectSummaryReport]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@title", SqlDbType.NVarChar, 50).Value = title;
                    cmd.Parameters.Add("@tdt1", SqlDbType.DateTime).Value = tdt1;
                    cmd.Parameters.Add("@tdt2", SqlDbType.DateTime).Value = tdt2;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        //Added by regie new
        #region InsertPurchaseOrder
        [WebMethod]
        public string InsertPurchaseOrder(string xml1, string xml2)
        {
            string msg = "";
            string POnum = "";
            int poID = 0;
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopPONo]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = 0;
                            try
                            {
                                asd = int.Parse(row[0].ToString().Split('-')[2]) + 1; // -00001
                            }
                            catch (Exception ex)
                            {
                                asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
                            }

                            POnum = "PO-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        POnum = "PO-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-00001";
                    }

                }

                foreach (DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertPurchaseOrder]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@PONumber", SqlDbType.NVarChar, 50).Value = POnum;
                    cmd.Parameters.Add("@canvassNumber", SqlDbType.NVarChar, 50).Value = data["canvassNumber"].ToString();
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(data["userID"].ToString());
                    cmd.Parameters.Add("@positionID", SqlDbType.Int).Value = int.Parse(data["positionID"].ToString());
                    cmd.Parameters.Add("@branchID", SqlDbType.Int).Value = int.Parse(data["branchID"].ToString());
                    cmd.Parameters.Add("@departmentID", SqlDbType.Int).Value = int.Parse(data["departmentID"].ToString());
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = "PENDING";
                    cmd.Parameters.Add("@urgent", SqlDbType.NVarChar, 50).Value = data["urgent"].ToString();
                    cmd.Parameters.Add("@deliveryVia", SqlDbType.NVarChar, 50).Value = data["deliveryVia"].ToString();
                    cmd.Parameters.Add("@paymentCondition", SqlDbType.NVarChar, 50).Value = data["paymentCondition"].ToString();
                    cmd.Parameters.Add("@DeliveryTo", SqlDbType.NVarChar, 150).Value = data["DeliveryTo"].ToString();
                    cmd.Parameters.Add("@requestorID", SqlDbType.Int).Value = int.Parse(data["RequestorID"].ToString()); // equivalent to userid
                    string sss = data["DateRequested"].ToString();
                    cmd.Parameters.Add("@DateRequested", SqlDbType.DateTime).Value = DateTime.Parse(sss);
                    cmd.Parameters.Add("@endorserID", SqlDbType.Int).Value = int.Parse(data["endorserID"].ToString()); //noted by
                    cmd.Parameters.Add("@recommendersID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@approversID", SqlDbType.Int).Value = int.Parse(data["approversID"].ToString()); //approver
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 50).Value = data["remarks"].ToString();

                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                poID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }
                if (ret == 100)
                {
                    foreach (DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertPurchaseOrderDetails]", conn);
                        cmd3.Parameters.Add("@POID", SqlDbType.Int).Value = poID;
                        cmd3.Parameters.Add("@canvassNumber", SqlDbType.NVarChar, 50).Value = data["canvassNumber"].ToString();
                        cmd3.Parameters.Add("@supplierID", SqlDbType.Int).Value = int.Parse(data["supplierID"].ToString());
                        cmd3.Parameters.Add("@itemID", SqlDbType.Int).Value = int.Parse(data["itemID"].ToString());
                        cmd3.Parameters.Add("@quantity", SqlDbType.Int).Value = int.Parse(data["quantity"].ToString());
                        cmd3.Parameters.Add("@unitMeasure", SqlDbType.NVarChar, 50).Value = data["unitMeasure"].ToString();
                        cmd3.Parameters.Add("@terms", SqlDbType.NVarChar, 50).Value = data["terms"].ToString();
                        cmd3.Parameters.Add("@DeliveryDate", SqlDbType.NVarChar, 50).Value = data["DeliveryDate"].ToString();
                        cmd3.Parameters.Add("@Warranty", SqlDbType.NVarChar, 100).Value = data["Warranty"].ToString();
                        cmd3.Parameters.Add("@unitCost", SqlDbType.Decimal).Value = decimal.Parse(data["unitCost"].ToString());
                        cmd3.Parameters.Add("@approved", SqlDbType.NVarChar, 50).Value = data["approved"].ToString();

                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }
                }

                conn.Close();
            }
            return msg;
        }
        #endregion
        #region Get_PO
        [WebMethod]
        public DataSet Get_PO(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetPurchaseOrder]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Get_Pending_PO
        [WebMethod]
        public DataSet Get_Pending_PO(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetPurchaseOrder]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Get_PO_for_Noted
        [WebMethod]
        public DataSet Get_PO_for_Noted(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetPurchaseOrder]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Get_PO_for_Approved
        [WebMethod]
        public DataSet Get_PO_for_Approved(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetPurchaseOrder]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_PODetailsAll
        [WebMethod]
        public DataSet Get_PODetailsAll(int POID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_SelectPODetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@canvasID", SqlDbType.Int).Value = POID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Update_PO_Noted
        [WebMethod]
        public string Update_PO_Noted(int POID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdatePO]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = POID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion
        #region Update_PO_Approved
        [WebMethod]
        public string Update_PO_Approved(int POID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdatePO]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = POID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        //Insert MRIS 
        #region  Insert MRIS
        [WebMethod]
        public string InsertMRIS(string xml1, string xml2)
        {
            string msg = "";
            string MRISnum = "";
            int MRISID = 0;
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopMRISNo]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = 0;
                            try
                            {
                                asd = int.Parse(row[0].ToString().Split('-')[2]) + 1; // -00001
                            }
                            catch (Exception ex)
                            {
                                asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
                            }

                            MRISnum = "MRIS-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        MRISnum = "MRIS-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-00001";
                    }

                }

                foreach (DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertMRIS]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@MRISNumber", SqlDbType.NVarChar, 50).Value = MRISnum;
                    //cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 50).Value = data["canvassNumber"].ToString();
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(data["userID"].ToString());
                    cmd.Parameters.Add("@positionID", SqlDbType.Int).Value = int.Parse(data["positionID"].ToString());
                    cmd.Parameters.Add("@RequestingOffice", SqlDbType.Int).Value = int.Parse(data["RequestingOffice"].ToString());
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = "PENDING";
                    cmd.Parameters.Add("@DateRequested", SqlDbType.DateTime).Value = DateTime.Parse(data["DateRequested"].ToString());
                    cmd.Parameters.Add("@DateNeeded", SqlDbType.DateTime).Value = DateTime.Parse(data["DateNeeded"].ToString());
                    cmd.Parameters.Add("@ApproverID", SqlDbType.Int).Value = int.Parse(data["ApproverID"].ToString()); //noted by
                    cmd.Parameters.Add("@DateApproved", SqlDbType.DateTime).Value = DateTime.Parse(data["DateApproved"].ToString());
                    cmd.Parameters.Add("@Issuer", SqlDbType.Int).Value = int.Parse(data["Issuer"].ToString()); //noted by
                    cmd.Parameters.Add("@DateIssued", SqlDbType.DateTime).Value = DateTime.Parse(data["DateIssued"].ToString());
                    cmd.Parameters.Add("@Purpose", SqlDbType.NVarChar, 50).Value = data["Purpose"].ToString();

                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                MRISID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }
                if (ret == 100)
                {
                    foreach (DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertMRISDetails]", conn);
                        cmd3.Parameters.Add("@MRISID", SqlDbType.Int).Value = MRISID;
                        cmd3.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["ItemID"].ToString());
                        cmd3.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 20).Value = data["ItemCode"].ToString();
                        cmd3.Parameters.Add("@ItemName", SqlDbType.NVarChar, 500).Value = data["ItemName"].ToString();
                        cmd3.Parameters.Add("@Qty", SqlDbType.Int).Value = int.Parse(data["Qty"].ToString());
                        cmd3.Parameters.Add("@Unit", SqlDbType.NVarChar, 50).Value = data["Unit"].ToString();

                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }
                }

                conn.Close();
            }
            return msg;
        }
        #endregion
        #region Get_MRIS
        [WebMethod]
        public DataSet Get_MRIS(int userID,string type)
        {
            DataSet ds = new DataSet();
           
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
                {
                    using (var cmd = new SqlCommand("[dbo].[usp_GetMRIS]", conn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                        cmd.Parameters.Add("@stat", SqlDbType.Int).Value = type;
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(ds);
                    }
                }
            

            return ds;
        }
        #endregion
        #region Get_MRISDetailsAll
        [WebMethod]
        public DataSet Get_MRISDetails(int MRISID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetMRISDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@MRISID", SqlDbType.Int).Value = MRISID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Update_MRIS
        [WebMethod]
        public string Update_MRIS(int MRISID,int type)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateMRIS]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = MRISID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = type;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Select_PO_Approved
        [WebMethod]
        public DataSet Select_PO_Approved()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_PO_GetApproved]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Get_Pending_RR
        [WebMethod]
        public DataSet Get_Pending_RR(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetReceivingReport]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region  Insert RI
        [WebMethod]
        public string InsertRI(string xml1, string xml2)
        {
            string msg = "";
            string RInum = "";
            int RIID = 0;
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopRINo]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = 0;
                            try
                            {
                                asd = int.Parse(row[0].ToString().Split('-')[2]) + 1; // -00001
                            }
                            catch (Exception ex)
                            {
                                asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
                            }

                            RInum = "RI-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        RInum = "RI-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-00001";
                    }

                }

                foreach (DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertRI]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    cmd.Parameters.Add("@RINumber", SqlDbType.NVarChar, 50).Value = RInum;
                    cmd.Parameters.Add("@MRISNumber", SqlDbType.NVarChar, 50).Value = data["MRISNumber"].ToString();
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(data["userID"].ToString());
                    cmd.Parameters.Add("@positionID", SqlDbType.Int).Value = int.Parse(data["positionID"].ToString());
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = int.Parse(data["branchID"].ToString());
                    cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = int.Parse(data["departmentID"].ToString());
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = "PENDING";
                    cmd.Parameters.Add("@ReturnedBy", SqlDbType.Int).Value = int.Parse(data["returnedBy"].ToString());
                    cmd.Parameters.Add("@RetDepID", SqlDbType.Int).Value = int.Parse(data["retdepartmentID"].ToString());
                    cmd.Parameters.Add("@ReturnedDate", SqlDbType.DateTime).Value = DateTime.Parse(data["returnedBYDate"].ToString());
                    cmd.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = int.Parse(data["receivedBy"].ToString());
                    cmd.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = DateTime.Parse(data["receivedByDate"].ToString());
                    cmd.Parameters.Add("@NotedBy", SqlDbType.Int).Value = int.Parse(data["notedBy"].ToString());
                    cmd.Parameters.Add("@NotedDate", SqlDbType.DateTime).Value = DateTime.Parse(data["notedByDate"].ToString());
                    cmd.Parameters.Add("@ConfirmedBy", SqlDbType.Int).Value = int.Parse(data["confirmedBy"].ToString());
                    cmd.Parameters.Add("@ConfirmeDate", SqlDbType.DateTime).Value = DateTime.Parse(data["confirmedDate"].ToString());


                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                RIID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }
                if (ret == 100)
                {
                    foreach (DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertRIDetails]", conn);
                        cmd3.Parameters.Add("@RIID", SqlDbType.Int).Value = RIID;
                        cmd3.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["ItemID"].ToString());
                        cmd3.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 20).Value = data["ItemCode"].ToString();
                        cmd3.Parameters.Add("@ItemName", SqlDbType.NVarChar, 500).Value = data["ItemName"].ToString();
                        cmd3.Parameters.Add("@Qty", SqlDbType.Int).Value = int.Parse(data["Qty"].ToString());
                        cmd3.Parameters.Add("@Unit", SqlDbType.NVarChar, 50).Value = data["Unit"].ToString();

                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }
                }

                conn.Close();
            }
            return msg;
        }
        #endregion
        #region Get_RI
        [WebMethod]
        public DataSet Get_RI(int userID, string type)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetRI]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = type;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }


            return ds;
        }
        #endregion
        #region Get_RIDetailsAll
        [WebMethod]
        public DataSet Get_RIDetails(int RIID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetRIDetails]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@RIID", SqlDbType.Int).Value = RIID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 3;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion
        #region Update_RI
        [WebMethod]
        public string Update_RI(int RIID, int type)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateRI]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = RIID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = type;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region InsertReceivingReport
        [WebMethod]
        public string InsertReceivingReport(string xml1, string xml2)
        {
            string msg = "";
            string RRnum = "";
            string POnum = "";
            int poID = 0;
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd2 = new SqlCommand("[dbo].[usp_SelectTopRRNo]", conn))
                using (var da1 = new SqlDataAdapter(cmd2))
                {
                    DataSet ds = new DataSet();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int asd = 0;
                            try
                            {
                                asd = int.Parse(row[0].ToString().Split('-')[2]) + 1; // -00001
                            }
                            catch (Exception ex)
                            {
                                asd = int.Parse(row[0].ToString()) + 1;   //return = 00001
                            }

                            RRnum = "RR-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + asd.ToString().PadLeft(5, '0');
                        }
                    }
                    else
                    {
                        RRnum = "RR-" + DateTime.Now.Year.ToString().Substring(2, 2) + "-00001";
                    }

                }

                foreach (DataRow data in ToDataTable(xml1).Rows)
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertRR]", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@RRNo", SqlDbType.NVarChar, 50).Value = RRnum;
                    cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 50).Value = data["PONo"].ToString();
                    POnum = data["PONo"].ToString();
                    cmd.Parameters.Add("@Prepared", SqlDbType.Int).Value = int.Parse(data["Prepared"].ToString());
                    cmd.Parameters.Add("@Noted", SqlDbType.Int).Value = int.Parse(data["Noted"].ToString());
                    cmd.Parameters.Add("@Confirmed", SqlDbType.Int).Value = int.Parse(data["Confirmed"].ToString());                   
                    cmd.Parameters.Add("@User", SqlDbType.Int).Value = int.Parse(data["User"].ToString());
                    cmd.Parameters.Add("@Position", SqlDbType.Int).Value = int.Parse(data["Position"].ToString());
                    cmd.Parameters.Add("@Branch", SqlDbType.Int).Value = int.Parse(data["Branch"].ToString());
                    cmd.Parameters.Add("@Department", SqlDbType.Int).Value = int.Parse(data["Department"].ToString());
                    cmd.Parameters.Add("@Received", SqlDbType.Int).Value = int.Parse(data["Received"].ToString()); // equivalent to userid

                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramRetVal);

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ret = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                poID = int.Parse(row[0].ToString());
                            }
                        }
                    }
                }

                    foreach (DataRow data in ToDataTable(xml2).Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertRRDetails]", conn);
                        cmd3.Parameters.Add("@RRID", SqlDbType.Int).Value = poID;
                        cmd3.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["itemID"].ToString());
                        cmd3.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 20).Value = data["itemCode"].ToString();
                        cmd3.Parameters.Add("@Quantity", SqlDbType.Int).Value = int.Parse(data["Quantity"].ToString());
                        cmd3.Parameters.Add("@PONo", SqlDbType.NVarChar, 50).Value = POnum;
                        cmd3.Parameters.Add("@SupplierID", SqlDbType.Int).Value = int.Parse(data["supplierID"].ToString());
                        
                        SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        paramRetVal.Direction = ParameterDirection.Output;
                        cmd3.Parameters.Add(paramRetVal);

                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.ExecuteNonQuery();
                        ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                        if (ret == 100)
                        {
                            msg = "SUCCESS";
                        }
                        else
                        {
                            msg = "FAILED";
                        }
                    }

                conn.Close();
            }
            return msg;
        }
        #endregion

        #region InsertCanvassDetails
        [WebMethod]
        public string InsertRRDetails(string xml, int RRID)
        {
            string msg = "";
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();

                foreach (DataRow data in ToDataTable(xml).Rows)
                {
                    SqlCommand cmd3 = new SqlCommand("dbo.[usp_InsertRRDetails]", conn);
                    cmd3.Parameters.Add("@RRID", SqlDbType.Int).Value = RRID;
                    cmd3.Parameters.Add("@ItemID", SqlDbType.Int).Value = int.Parse(data["itemID"].ToString());
                    cmd3.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 20).Value = data["itemCode"].ToString();
                    cmd3.Parameters.Add("@Quantity", SqlDbType.Int).Value = int.Parse(data["Quantity"].ToString());
                    cmd3.Parameters.Add("@PONo", SqlDbType.NVarChar, 50).Value = RRID;
                    cmd3.Parameters.Add("@SupplierID", SqlDbType.Int).Value = int.Parse(data["supplierID"].ToString());

                    SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                    paramRetVal.Direction = ParameterDirection.Output;
                    cmd3.Parameters.Add(paramRetVal);

                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.ExecuteNonQuery();
                    ret = Convert.ToInt32(cmd3.Parameters["@RetVal"].Value);
                    if (ret == 100)
                    {
                        msg = "SUCCESS";
                    }
                    else
                    {
                        msg = "FAILED";
                    }
                }


                conn.Close();
            }
            return msg;
        }
        #endregion

        #region Get_RR_for_Noted
        [WebMethod]
        public DataSet Get_RR_for_Noted(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetReceivingReport]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Update_RR_Noted
        [WebMethod]
        public string Update_RR_Noted(int RRID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateRR]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = RRID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion
        #region Update_RR_Approved
        [WebMethod]
        public string Update_RR_Approved(int RRID, int UserId)
        {
            string msg = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateRR]", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = RRID;
                cmd.Parameters.Add("@udate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    msg = "SUCCESS";
                }
                catch (Exception)
                {

                    conn.Close();
                    msg = "FAILED";
                }
            }
            return msg;
        }
        #endregion

        #region Get_RR_for_Approved
        [WebMethod]
        public DataSet Get_RR_for_Approved(int userID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[usp_GetReceivingReport]", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@stat", SqlDbType.Int).Value = 2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }

            return ds;
        }
        #endregion

        #region Signatory
        [WebMethod]
        public DataSet GetSignatory()
        {
            var _ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("[dbo].[usp_GetSignatory]"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    using (var da = new SqlDataAdapter(cmd))
                        da.Fill(_ds);
                }
            }

            return _ds;
        }
        [WebMethod]
        public DataSet GetSignatoryByUserId(int pUserId, string pTransactionType, string pSignatoryType)
        {
            var _ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("[dbo].[usp_GetSignatoryByUserId]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pUserId", SqlDbType.Int).Value = pUserId;
                    cmd.Parameters.Add("@pTransactionType", SqlDbType.NVarChar, 150).Value = pTransactionType;
                    cmd.Parameters.Add("@pSignatoryType", SqlDbType.NVarChar, 150).Value = pSignatoryType;
                    cmd.Connection = conn;

                    using (var da = new SqlDataAdapter(cmd))
                        da.Fill(_ds);
                }
                
            }

            return _ds;
        }
        [WebMethod]
        public bool InsertSignatory(int pUserId, string pTransType, string pSignType)
        {
            bool _isError = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var _trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand("[dbo].[usp_InsertSignatory]", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pUserId", SqlDbType.Int).Value = pUserId;
                            cmd.Parameters.Add("@pTransactionType", SqlDbType.NVarChar, 100).Value = pTransType;
                            cmd.Parameters.Add("@pSignatoryType", SqlDbType.NVarChar, 100).Value = pSignType;
                            cmd.Connection = _trans.Connection;
                            cmd.Transaction = _trans;
                            cmd.ExecuteNonQuery();
                        }

                        _trans.Commit();
                    }
                    catch (Exception)
                    {
                        _trans.Rollback();
                        _isError = true;
                    }
                }
            }
            return _isError;
        }
        [WebMethod]
        public bool UpdateSignatory(int pSignId, int pUserId, string pTransType, string pSignType)
        {
            bool _isError = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var _trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand("[dbo].[usp_UpdateSignatory]", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pSignatoryId", SqlDbType.Int).Value = pSignId;
                            cmd.Parameters.Add("@pUserId", SqlDbType.Int).Value = pUserId;
                            cmd.Parameters.Add("@pTransactionType", SqlDbType.NVarChar, 100).Value = pTransType;
                            cmd.Parameters.Add("@pSignatoryType", SqlDbType.NVarChar, 100).Value = pSignType;
                            cmd.Connection = _trans.Connection;
                            cmd.Transaction = _trans;
                            cmd.ExecuteNonQuery();
                        }

                        _trans.Commit();
                    }
                    catch (Exception)
                    {
                        _trans.Rollback();
                        _isError = true;
                    }
                }
            }
            return _isError;
        }
        [WebMethod]
        public bool DeleteSignatory(int pSignId)
        {
            bool _isError = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["wsWMS"].ConnectionString))
            {
                conn.Open();
                using (var _trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand("[dbo].[usp_DeleteSignatory]", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pSignatoryId", SqlDbType.Int).Value = pSignId;
                            cmd.Connection = _trans.Connection;
                            cmd.Transaction = _trans;
                            cmd.ExecuteNonQuery();
                        }

                        _trans.Commit();
                    }
                    catch (Exception)
                    {
                        _trans.Rollback();
                        _isError = true;
                    }
                }
            }
            return _isError;
        }
        #endregion
    }
}