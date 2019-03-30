using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.Controller;
using WMS.Model;

namespace WMS
{
    public partial class User_frm : Form
    {
        public string image_url = "";
        UserController user = new UserController();
        DepartmentController dept = new DepartmentController();
        PositionController pos = new PositionController();
        BranchController branch = new BranchController();
        PasswordEncryptor enc = new PasswordEncryptor();
        DataSet dsTemp;
        public User_frm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            
            if(openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = openfile.FileName;
                image_url = file;
                System.Drawing.Image img = System.Drawing.Image.FromFile(file, true);
                //Bitmap map = new Bitmap(file); 
                //byte[] imgbytes = Encoding.Unicode.GetBytes(file);
                //MemoryStream mstream = new MemoryStream(imgbytes);

                //PictureBox pic = new PictureBox();
                //pic.Image{file}

                pictureBox1.Image = img;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFName.Text.Trim() == "")
            {
                MessageBox.Show("First Name is Empty");
            }
            else if (txtMiddleName.Text.Trim() == "")
            {
                MessageBox.Show("Middle Name is Empty");
            }
            else if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("last Name is Empty");
            }
            else if (txtAddress.Text.Trim() == "")
            {
                MessageBox.Show("Address is Empty");
            }
            else if (txtCity.Text.Trim() == "")
            {
                MessageBox.Show("City is Empty");
            }
            else if (txtMobileNumber.Text.Trim() == "")
            {
                MessageBox.Show("Mobile Number is Empty");
            }
            else if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("UserName is Empty");
            }
            else if (cmbPosition.Text.Trim() == "")
            {
                MessageBox.Show("Position is Empty");
            }
            else if (cmbDepartment.Text.Trim() == "")
            {
                MessageBox.Show("Department is Empty");
            }
            else if (cmbBranch.Text.Trim() == "")
            {
                MessageBox.Show("Branch is Empty");
            }
            else
            { 
                UserModel model = new UserModel();
                model.FName = txtUsername.Text.Trim();
                model.LastName = txtLastName.Text.Trim();
                model.MiddleName = txtMiddleName.Text.Trim(); 
                model.Address = txtAddress.Text.Trim();
                model.City = txtCity.Text.Trim();
                model.MobileNumber = txtMobileNumber.Text.Trim();
                model.UserName = txtUsername.Text;
                model.Password = enc.encrypt(txtPassword.Text.Trim());
                model.position = int.Parse((cmbPosition.SelectedItem as ComboBoxItem).Value.ToString());
                model.Department = int.Parse((cmbDepartment.SelectedItem as ComboBoxItem).Value.ToString());
                model.Branch  = int.Parse((cmbBranch.SelectedItem as ComboBoxItem).Value.ToString());
                
                if (image_url == "")
                {
                    model.Signature = "";
                }
                else
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(image_url, true);
                    string base64 = ImgToBase64(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                    model.Signature = imagetobase64(img);
                }
                
               
              
                if (button1.Text == "Save")
                {
                    if (txtPassword.Text.Trim() == "")
                    {
                        MessageBox.Show("Password is Empty");
                    }
                    else
                    {
                        string response = user.registerUser(model);
                        if (response == "SUCCESS")
                        {
                            getAllUser();
                            MessageBox.Show("Users successfully added.");
                        }
                        else
                        {
                            MessageBox.Show(response, "ERROR!");
                            return;
                        }
                    }
                }
                else if(button1.Text == "Update")
                {
                    model.ID = int.Parse(textBox1.Text.Trim());
                    string response = user.updateUser(model);
                    if (response == "SUCCESS")
                    {
                        getAllUser();
                        MessageBox.Show("Users successfully updated.");
                    }
                    else
                    {
                        MessageBox.Show(response, "ERROR!");
                        return;
                    }
                }

               
            }
            
        }

        public string imagetobase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Bitmap map = new Bitmap(image);
                //map.SetPixel(300,300,Color.White);
               // map.SetResolution(300, 300);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imagebyte = ms.ToArray();
                string base64 = Convert.ToBase64String(imagebyte);
                return base64;
            }
            
        }

        public Image base64toImage(string base64)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
            imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            image.Save(@"B:\sample\asdsa.jpg");
            return image;
        }

        public string ImgToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public Image Base64ToImg(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
            imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            // saving image to drive
           // image.Save(@"B:\image_saving\sample.jpg");
            return image;
        }

        private void User_frm_Load(object sender, EventArgs e)
        {
            getPosition();
            getBranch();
            getDepartment();
            getAllUser();
        }
        public void getAllUser()
        {
             DataSet ds = user.getAllUser();
             dsTemp = null;
             dsTemp = ds;
             if (ds.Tables.Count > 0)
             {
                 dataGridView1.DataSource = ds.Tables[0];
             }
            
        }
        public void getPosition()
        {
            DataSet ds = pos.getPosition();
            if(ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows.Count > 0)
                {
                   foreach(DataRow row in ds.Tables[0].Rows)
                   {
                       ComboBoxItem item = new ComboBoxItem();
                       item.Text = row["PositionName"].ToString();
                       item.Value = row["ID"].ToString();
                       cmbPosition.Items.Add(item);
                   }
                }
            }
          
           // (cmbPosition.SelectedItem as ComboBox).Value.ToString();
        }

        public void getDepartment()
        {

            DataSet ds = dept.getDepartment();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Text = row["DeptName"].ToString();
                        item.Value = row["ID"].ToString();
                        cmbDepartment.Items.Add(item);
                    }
                }
            }

            // (cmbPosition.SelectedItem as ComboBox).Value.ToString();
        }

        public void getBranch()
        {

            DataSet ds = branch.getBranch();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Text = row["BranchName"].ToString();
                        item.Value = row["ID"].ToString();
                        cmbBranch.Items.Add(item);
                    }
                }
            }

            // (cmbPosition.SelectedItem as ComboBox).Value.ToString();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];
                textBox1.Text = Convert.ToString(selectedrow.Cells["UserID"].Value);
                txtFName.Text = Convert.ToString(selectedrow.Cells["firstName"].Value);
                txtFName.ReadOnly = true;
                txtMiddleName.Text = Convert.ToString(selectedrow.Cells["middleName"].Value);
                txtMiddleName.ReadOnly = true;
                txtLastName.Text = Convert.ToString(selectedrow.Cells["lastName"].Value);
                txtLastName.ReadOnly = true;
                txtAddress.Text = Convert.ToString(selectedrow.Cells["address"].Value);
                txtAddress.ReadOnly = true;
                txtCity.Text = Convert.ToString(selectedrow.Cells["city"].Value);
                txtCity.ReadOnly = true;
                txtMobileNumber.Text = Convert.ToString(selectedrow.Cells["mobileNumber"].Value);//
                txtUsername.Text = Convert.ToString(selectedrow.Cells["username"].Value);//
                txtPassword.Text = "";
                cmbPosition.Text = Convert.ToString(selectedrow.Cells["PositionName"].Value);//
                cmbDepartment.Text = Convert.ToString(selectedrow.Cells["DeptName"].Value);//
                cmbBranch.Text = Convert.ToString(selectedrow.Cells["BranchName"].Value);//
                string signature = selectedrow.Cells["Signature"].Value.ToString().Trim();
                if ( signature != "")
                {
                    Image img = Base64ToImg(selectedrow.Cells["Signature"].Value.ToString());
                    pictureBox1.Image = img;
                    //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
               
                button1.Text = "Update";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            txtUsername.Text = "";
            txtMiddleName.Text ="";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtMobileNumber.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            cmbPosition.Text = "";
            cmbDepartment.Text ="";
            cmbBranch.Text = "";

            txtUsername.ReadOnly = false;
            txtMiddleName.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtCity.ReadOnly = false;
            txtFName.ReadOnly = false;
            txtLastName.ReadOnly = false;
             button1.Text = "Save";

             //string path = @"C:\Users\regie\Pictures\Camera Roll\samp.jpg";
             //System.Drawing.Image img = System.Drawing.Image.FromFile(path, true);
             //// Pass image parameter to Convert in Base64
             //string b64 = ImgToBase64(img, System.Drawing.Imaging.ImageFormat.Jpeg);


             //Base64ToImg(b64);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("You were about to print the list of user's.\n\nClick 'Yes' to print the selected user.\n\nClick 'No' to print all the user's.\n\nClick 'Cancel' to cancel printing.", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                var query = dsTemp.Tables[0].AsEnumerable()
                    .Where(p => p.Field<int>("UserID") == int.Parse(textBox1.Text))
                    ;

                if (query.Any())
                {
                    DataSet dsT = new DataSet();
                    dsT.Tables.Add(query.CopyToDataTable());
                    UI_Report.Report_RO rpt = new UI_Report.Report_RO("USER", dsT, 0);
                    rpt.ShowDialog();  
                }
            }
            if (res == DialogResult.No)
            {
                UI_Report.Report_RO rpt = new UI_Report.Report_RO("USER", dsTemp, 0);
                rpt.ShowDialog();
            }
        }
    }
}
