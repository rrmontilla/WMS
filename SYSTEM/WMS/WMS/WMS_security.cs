using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WMS.Controller;

namespace WMS
{
    public partial class WMS_security : Form
    {
        PasswordEncryptor enc = new PasswordEncryptor();
        int TogMove;
        int MValX;
        int MValY;
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);


        public string fullname = "";
        public string userid = "";
        public string posID = "";
        public string deptID = "";
        public string deptName = "";
        public string branchID = "";
        public string uname = "";

        wms_service.Service1 wms = new wms_service.Service1();
        public WMS_security()
        {
            InitializeComponent();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }
        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
            this.Close();
            this.Dispose();
        }

        private void WMS_security_Load(object sender, EventArgs e)
        {
            SendMessage(txtUserName.Handle, EM_SETCUEBANNER, 0, "Username");
            SendMessage(txtPassword.Handle, EM_SETCUEBANNER, 0, "Password");
            lblLoginNotification.Text = "";
            this.ActiveControl = txtUserName;
            txtUserName.Focus();
            txtUserName.SelectAll();
        }
        public void UserName_Focus()
        {
            this.ActiveControl = txtUserName;
            txtUserName.Focus();
            txtUserName.SelectAll();
        }
        private void btn_login_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == "")
            {
                lblLoginNotification.Text = "Username is empty.";
            }
            else if (txtPassword.Text.Trim() == "")
            {
                lblLoginNotification.Text = "Password is empty.";
            }
            else
            {
                try
                {

                    string result = wms.VerifyUserLogin(txtUserName.Text.Trim(), enc.encrypt(txtPassword.Text.Trim()));
                    if (result.Trim() == "SUCCESS")
                    {
                        DataSet ds = wms.SelectUserByUserName(txtUserName.Text.Trim());
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                fullname = ds.Tables[0].Rows[0]["firstname"].ToString() + " " + ds.Tables[0].Rows[0]["middlename"].ToString() + " " + ds.Tables[0].Rows[0]["lastName"].ToString();
                                userid = ds.Tables[0].Rows[0]["userid"].ToString();
                                posID = ds.Tables[0].Rows[0]["positionID"].ToString();
                                deptID = ds.Tables[0].Rows[0]["departmentID"].ToString();
                                branchID = ds.Tables[0].Rows[0]["branchID"].ToString();
                                uname = ds.Tables[0].Rows[0]["username"].ToString();
                                deptName = ds.Tables[0].Rows[0]["DeptName"].ToString();

                            }
                        }

                        txtPassword.Text = "";
                        Program.mainfrm.Show();
                        Program.mainfrm.displayname();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Username or password is incorrect!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_login.PerformClick();
            }
        }
    }
}
