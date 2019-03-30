using GlobalObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.Class;
using WMS.Controller;
using WMS.Model;

namespace WMS.UI_RI
{
    public partial class RI_frm : Form
    {
        SignatoryController lSignatoryCtrl;
        ReturnInventoryController ri = new ReturnInventoryController();
        UserController user = new UserController();
        string frm_type = "";
        int RI_counter = 0;
        int RI_ID = 0;
        DataTable RI_Table = new DataTable();
        DataTable MRISDetails_Table = new DataTable();
        public RI_frm(string type)
        {
            frm_type = type;
            InitializeComponent();
        }

        private void RI_frm_Load(object sender, EventArgs e)
        {
            try
            {
                txtRequestor.Text = Program.loginfrm.fullname;
                fill_MRIS();
                fill_userID();
                getRI_Table();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                    this.BeginInvoke(new MethodInvoker(Close));
            }
        }
          public void getRI_Table()
        {
            if (frm_type == "RI_Prep")
            {
                label16.Text = "RI Preparation";
                button1.Text = "Submit Request";
                button2.Enabled = true;
            }
            else if (frm_type == "Noted")
            {
                lSignatoryCtrl = new SignatoryController();
                lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.RequestOrder);
                label16.Text = "RI Noted";
                button1.Text = "Approved";
                button2.Enabled = false;
                
            }
            else if (frm_type == "Confirm")
            {
                lSignatoryCtrl = new SignatoryController();
                lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.RequestOrder);
                label16.Text = "RI Confirmation";
                button1.Text = "Issue";
                button2.Enabled = false;
            }

            RI_Table = ri.getRI(int.Parse(Program.loginfrm.userid), frm_type);

            if (RI_Table.Rows.Count > 0)
            {
                RI_counter = RI_Table.Rows.Count - 1;
                DataTable dt = ri.RICount(RI_Table, RI_Table.Rows.Count - 1);
                retrieve_request(dt);
            }
        }
          public void retrieve_request(DataTable dt)
          {
              if (dt.Rows.Count > 0)
              {
                  txtRONo.Text = dt.Rows[0]["RINo"].ToString();
                  //getCurrentStat(int.Parse(dt.Rows[0]["ROID"].ToString()));
                
                  // comboBox5.Enabled = false;
                  button6.Enabled = false;
                  button4.Enabled = false;
                  comboBox1.Enabled = false;
                  comboBox4.Enabled = false;
                  txtRequestor.Text = dt.Rows[0]["ReceivedBYName"].ToString();
                  comboBox5.Text = dt.Rows[0]["ReturnedBYName"].ToString();
                  comboBox1.Text = dt.Rows[0]["NotedBYName"].ToString();
                  // comboBox3.Text = dt.Rows[0]["Recommender"].ToString();
                  comboBox4.Text = dt.Rows[0]["ConfirmedBYName"].ToString();
                  label9.Text = "Request Status: " + dt.Rows[0]["Status"].ToString();
                  DataTable details = new DataTable();

                  details = ri.getRI_Details(int.Parse(dt.Rows[0]["RIID"].ToString()));

                  RI_ID = int.Parse(dt.Rows[0]["RIID"].ToString());  //for approval and issued
                  if (details.Rows.Count > 0)
                  {
                      dataGridView1.Rows.Clear();
                      foreach (DataRow row in details.Rows)
                      {
                          dataGridView1.Rows.Add(false, row["ItemID"].ToString(),
                                                 row["Qty"].ToString(),
                                                 row["ItemCode"].ToString(),
                                                 row["ItemName"].ToString(),
                                                 row["Unit"].ToString());
                      }
                  }
                  else
                  {
                      dataGridView1.Rows.Clear();
                  }


              }
          }
        public void fill_userID()
        {
            DataSet ds = new DataSet();
            ds = user.getAllUser2();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["firstName"].ToString() + " " + row["middleName"].ToString() + " " + row["lastName"].ToString();
                        items.Value = row["UserID"].ToString();
                        //comboBox3.Items.Add(items);
                        comboBox1.Items.Add(items);
                        comboBox4.Items.Add(items);
                        comboBox5.Items.Add(items);

                    }
                }
            }
        }
        public void fill_MRIS()
        {
            DataTable ds = new DataTable();
            ds = ri.getMRIS(int.Parse(Program.loginfrm.userid), frm_type);
           
                if (ds.Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["MRISNo"].ToString() ;
                        items.Value = row["ID"].ToString();
                        comboBox3.Items.Add(items);
                    }
              }

        }
        public void get_MRISDetails(int MRIS_ID)
        {
           // DataTable ds =new DataTable();
            MRISDetails_Table = ri.getMRIS_Details(MRIS_ID);

            if (MRISDetails_Table.Rows.Count > 0)
                {
                    comboBox2.Items.Clear();
                    foreach (DataRow row in MRISDetails_Table.Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["ItemName"].ToString() + " ~ " + row["itemCode"].ToString();
                        items.Value = row["itemID"].ToString();
                        comboBox2.Items.Add(items);
                    }
                }
            

        }
        public bool checkItemQuantity(int itemID,string input_qty)
        {
            bool result = false;
            double total_qty = double.Parse(textBox3.Text);
            double totalQty_input = double.Parse(input_qty);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["itemID"].Value.ToString().Trim() == itemID.ToString())
                {
                    totalQty_input += double.Parse(row.Cells["Quantity"].Value.ToString());
                }
                //container1.Rows.Add(row.Cells["itemID"].Value.ToString(),
                //                 row.Cells["ItemCode"].Value.ToString(),
                //                 row.Cells["Description"].Value.ToString(),
                //                 row.Cells["Quantity"].Value.ToString(),
                //                 row.Cells["Unit"].Value.ToString()
                //                 );
            }

            if (total_qty >= totalQty_input)
            {
                result = true;
            }

            return result;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text.Trim() == "")
            {
                MessageBox.Show("Please select MRIS No!");
            }
            else if (comboBox3.Text.Trim() == "")
            {
                MessageBox.Show("Select Item");

            }
            else if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Quantity is empty!");

            }
            else if (txtRONo.Text != "")
            {
                MessageBox.Show("Create a new request before submission!");

            }
            else if (!checkItemQuantity(int.Parse((comboBox2.SelectedItem as ComboBoxItem).Value.ToString()),textBox2.Text))
            {
                MessageBox.Show("Item Exceed with the total quantity from this MRISNo!");
            }
            else
            {
                string itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();
                dataGridView1.Rows.Add(false, itemID, textBox2.Text,
                                       comboBox2.Text.Split('~')[1],
                                       comboBox2.Text.Split('~')[0],
                                       textBox1.Text.Trim());
                comboBox2.Text = "";
                // textBox8.Text = "";
                textBox2.Text = "";
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            int MRIS_ID = int.Parse((comboBox3.SelectedItem as ComboBoxItem).Value.ToString());
            get_MRISDetails(MRIS_ID);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            comboBox1.Text = "";
            //comboBox3.Text = "";
            comboBox4.Text = "";
            txtRONo.Text = "";

            button4.Enabled = true;


            // comboBox5.Enabled = true;
            button6.Enabled = true;

            comboBox1.Enabled = true;
            comboBox4.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            removeFromdatagrid(dataGridView1);
        }
        public void removeFromdatagrid(DataGridView data)
        {
            try
            {
                for (int i = data.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)data.Rows[i].Cells[0].FormattedValue)
                    {
                        data.Rows.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frm_type == "RI_Prep")
            {
                submit_request();
            }
            else if (frm_type == "Noted")
            {

                string response = ri.Approved_RI(RI_ID, frm_type);
            }
            else if (frm_type == "Confirmed")
            {
                string response = ri.Approved_RI(RI_ID, frm_type);
            }
        }
        public void submit_request()
        {
            if (comboBox4.Text.Trim() == "")
            {
                MessageBox.Show("Approver is empty!");
            }
            else if (txtRONo.Text != "")
            {
                MessageBox.Show("Create a new request before submission!");
            }
            else
            {
                try
                {
                    DataTable container = new DataTable();

                    container.Columns.Add("MRISNumber");
                    container.Columns.Add("userID");
                    container.Columns.Add("positionID");
                    container.Columns.Add("branchID");
                    container.Columns.Add("departmentID");
                    container.Columns.Add("returnedBy");
                    container.Columns.Add("retdepartmentID");
                    container.Columns.Add("returnedBYDate");
                    container.Columns.Add("receivedBy");
                    container.Columns.Add("receivedByDate");
                    container.Columns.Add("notedBy");
                    container.Columns.Add("notedByDate");
                    container.Columns.Add("confirmedBy");
                    container.Columns.Add("confirmedDate");

                    int ret_user = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString());
                    DataTable return_user = user.selectUserByID(ret_user);
                     int dept_retUser = int.Parse(return_user.Rows[0]["departmentID"].ToString());

                    container.Rows.Add(comboBox3.Text,//"MRISNumber"
                                         Program.loginfrm.userid,
                                        Program.loginfrm.posID,
                                        Program.loginfrm.branchID,
                                        Program.loginfrm.deptID,
                                        ret_user,//"returnedBy"
                                        dept_retUser,//"retdepartmentID"
                                        DateTime.Now,//"returnedBYDate"
                                        Program.loginfrm.userid,// "receivedBy",
                                        DateTime.Now,//"receivedByDate"
                                        (comboBox1.SelectedItem as ComboBoxItem).Value.ToString(),//"notedBy"
                                        DateTime.Now,//"notedByDate"
                                        (comboBox4.SelectedItem as ComboBoxItem).Value.ToString(),//"confirmedBy"
                                        DateTime.Now//"confirmedDate"
                                        );

                  

                    DataTable container1 = new DataTable();
                    container1.Columns.Add("ItemID");
                    container1.Columns.Add("ItemCode");
                    container1.Columns.Add("ItemName");
                    container1.Columns.Add("Qty");
                    container1.Columns.Add("Unit");

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        container1.Rows.Add(row.Cells["itemID"].Value.ToString(),
                                         row.Cells["ItemCode"].Value.ToString(),
                                         row.Cells["Description"].Value.ToString(),
                                         row.Cells["Quantity"].Value.ToString(),
                                         row.Cells["Unit"].Value.ToString()
                                         );
                    }

                    string data_summary = ToXML.Toxml(container);
                    string data_items = ToXML.Toxml(container1);

                    string response = ri.SubmitRI(data_summary, data_items);
                    if (response == "SUCCESS")
                    {
                        getRI_Table();
                        MessageBox.Show("SuccessFully submitted!");
                    }
                    else
                    {
                        MessageBox.Show(response);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (RI_Table.Rows.Count > 0)
            {
                RI_counter--;

                if (RI_counter == 0)
                {
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter < 0)
                {
                    RI_counter++;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter == RI_Table.Rows.Count)
                {
                    RI_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter > RI_Table.Rows.Count)
                {
                    RI_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else
                {
                    //RO_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                    //MessageBox.Show("No more data to show!");
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (RI_Table.Rows.Count > 0)
            {
                RI_counter++;

                if (RI_counter == 0)
                {
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter == RI_Table.Rows.Count)
                {
                    RI_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter > RI_Table.Rows.Count - 1)
                {
                    RI_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else if (RI_counter < 0)
                {
                    RI_counter++;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
                else
                {
                    // RO_counter--;
                    DataTable dt = ri.RICount(RI_Table, RI_counter);
                    retrieve_request(dt);
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (MRISDetails_Table.Rows.Count > 0)
            {
                double quantity = 0.0;
                foreach (DataRow row in MRISDetails_Table.Rows)
                {
                    string current_itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString().Trim();
                    if (row["itemID"].ToString().Trim() == current_itemID)
                    {
                        quantity += double.Parse(row["Qty"].ToString());
                    }
                }

                textBox3.Text = quantity.ToString("N2");
            }
        }
    }
}
