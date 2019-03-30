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
using WMS.UI_Report;

namespace WMS.UI_RO
{
    public partial class ROPrep_frm : Form
    {
        ItemsController item = new ItemsController();
        RequestOrderController ro = new RequestOrderController();
        UserController user = new UserController();

        DataTable RO_Table = new DataTable();
        DataTable dtItems = new DataTable();
        int RO_counter = 0;
        string frm_type = string.Empty;
        public ROPrep_frm(string ttl)
        {
            frm_type = ttl;
            InitializeComponent();
            this.Text = "Request Order - Preparation";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.Trim() == "")
            {
                MessageBox.Show("Description is empty!");
            }
            else if (textBox8.Text.Trim() == "")
            {
                MessageBox.Show("Purpose is empty!");

            }
            else if (textBox2.Text.Trim() == "")
            {   
                MessageBox.Show("Quantity is empty!");

            }
            else if (comboBox5.Text.Trim() == "")
            {
                MessageBox.Show("Select Urgent Level!");

            }
            else if (comboBox3.Text != "")
            {
                MessageBox.Show("Create a new request before submission!");
            }
            else
            {
                string itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();
                string itemCode = string.Empty;
                var query = dtItems.AsEnumerable()
                            .Where(p => p.Field<int>("ID") == int.Parse(itemID))
                            ;
                
                if (query.Any())
                {
                    itemCode = query.CopyToDataTable().Rows[0]["ItemCode"].ToString().Trim();
                }
                
                dataGridView1.Rows.Add(false, itemID, textBox2.Text,
                                       itemCode,
                                       comboBox2.Text.Split('~')[0],
                                       textBox1.Text.Trim());
                comboBox2.Text = "";
                // textBox8.Text = "";
                textBox2.Text = "";
            }
        }

        private void ROPrep_frm_Load(object sender, EventArgs e)
        {
            fill_items();
            fill_userID();
            getRO_Table();
           // getCurrentStat();
            textBox3.Text = Program.loginfrm.fullname;

            comboBox2.Enabled = false;
            textBox2.Enabled = false;
        }
        public void getCurrentStat(int  ROID)
        {
            DataTable dt = ro.getROByRONumber(ROID.ToString());
           
            if (dt.Rows.Count > 0)
            {
               
                if (dt.Rows[0]["Status"].ToString().Trim() == "CANVASS")
                {
                    label2.Text = "Current Status:  For Canvass-noted";
                }
                else if (dt.Rows[0]["Status"].ToString().Trim() == "CANVASS-NOTED")
                {
                    label2.Text = "Current Status:  For Canvass Approved";
                }
                else if (dt.Rows[0]["Status"].ToString().Trim() == "CANVASS-APPROVED")
                {
                    label2.Text = "Current Status:  For PO";
                }
                else if (dt.Rows[0]["Status"].ToString().Trim() == "PENDING")
                {
                    label2.Text = "Current Status:  For RO NOTED";
                }
                else if (dt.Rows[0]["Status"].ToString().Trim() == "RO-NOTED")
                {
                    label2.Text = "Current Status:  For RO APPROVED";
                }
                else if (dt.Rows[0]["Status"].ToString().Trim() == "RO-APPROVED")
                {
                    label2.Text = "Current Status:  For Canvass";
                }
                else
                {
                    label2.Text = "Current Status:  Unknown";
                }
            }
        }
        public void getRO_Table()
        {
            if (frm_type != "Print")
            {
                RO_Table = ro.getRO_Requestor(int.Parse(Program.loginfrm.userid));
                label11.Hide();
                textBox4.Hide();
                button3.Hide();
            }
            else
            {
                if (textBox4.Text.Length != 0)
                {
                    RO_Table = ro.Print_Transaction(int.Parse(Program.loginfrm.userid), textBox4.Text, "RO").Tables[0];

                    if (RO_Table.Rows.Count == 0)
                    {
                        MessageBox.Show("No Request Order Number found!\n\n Thank you!", "ERROR!");
                        return;
                    }
                }
                else
                {
                    label11.Show();
                    textBox4.Show();
                    button3.Show();
                    return;
                }
            }

            if (RO_Table.Rows.Count > 0)
            {
                comboBox3.Items.Clear();
                foreach (DataRow row in RO_Table.Rows)
                {
                    ComboBoxItem items = new ComboBoxItem();
                    items.Text = row["RONumber"].ToString();
                    items.Value = row["ID"].ToString();
                    comboBox3.Items.Add(items);
                }

                RO_counter = RO_Table.Rows.Count;
                //DataTable dt = ro.RequestOrderCount(RO_Table,RO_Table.Rows.Count - 1);
                retrieve_request(RO_Table);
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

                    }
                }
            }
        }


        public void fill_items()
        {
            DataSet ds = new DataSet();
           ds = item.selectItems();
           dtItems = ds.Tables[0];
           if (ds.Tables.Count > 0)
           {
               if (ds.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow row in ds.Tables[0].Rows)   
                   {
                       ComboBoxItem items = new ComboBoxItem();
                       items.Text = row["Description"].ToString();
                       items.Value = row["ID"].ToString();
                       comboBox2.Items.Add(items);
                   }
               }
           }

        }

        public void filter_textbox_decimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                         (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {   
            filter_textbox_decimal(sender,e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text.Trim() == "")
            {
                MessageBox.Show("Please select an urgent level! Thank you!", "ERROR!");
            } 
            //==> Commented By Ren | 2019-03-18 due to unnecessary
            //else if (textBox3.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please select a requestor. Thank you!", "ERROR!");
            //}
            //else if (comboBox1.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please select a signatory for Noted by! Thank you!", "ERROR!");
            //}
            //==> End
            //else if (comboBox3.Text.Trim() == "")
            //{
            //    MessageBox.Show("Recommender is empty!");
            //}
            //==> Commented By Ren | 2019-03-18 due to unnecessary
            //else if (comboBox4.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please select a signatory fro Approved by! Thank you!", "ERROR!");
            //}
            //==> End
            else if(comboBox3.Text != "")
            {
                MessageBox.Show("Please create a new request before submission! Thank you!", "ERROR!");              
            }
            else
            {
                try
                {
                    DataTable container = new DataTable();
                    container.Columns.Add("UserID");
                    container.Columns.Add("PositionID");
                    container.Columns.Add("BranchID");
                    container.Columns.Add("DepartmentID");
                    container.Columns.Add("Urgent");
                    container.Columns.Add("TargetDate");
                    container.Columns.Add("DateRequested");
                    container.Columns.Add("EndorserID");
                    container.Columns.Add("DateEndorse");
                    container.Columns.Add("RecommendersID");
                    container.Columns.Add("DateRecommended");
                    container.Columns.Add("ApproverID");
                    container.Columns.Add("DateApproved");
                    container.Columns.Add("Remarks");

                    int urgent = 0;
                    if (comboBox5.Text.Trim() == "Yes")
                    {
                        urgent = 1;
                    }

                    container.Rows.Add(Program.loginfrm.userid,
                                       Program.loginfrm.posID,
                                       Program.loginfrm.branchID,
                                       Program.loginfrm.deptID,
                                       urgent,
                                       dateTimePicker2.Value.ToString(),
                                       dateTimePicker1.Value.ToString(),
                                       0,
                                       //==> Commented By Ren | 2019-03-18 due to create new Request Order
                                       //(comboBox1.SelectedItem as ComboBoxItem).Value.ToString(),
                                       //==> End
                                       "",
                                       "0",
                                       "",
                                       0,
                                       //==> Commented By Ren | 2019-03-18 due to create new Request Order
                                       //(comboBox4.SelectedItem as ComboBoxItem).Value.ToString(),
                                       //==> End
                                       "",
                                       textBox8.Text.Trim());
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

                    string response = ro.SubmitRO(data_summary, data_items);
                    if (response == "SUCCESS")
                    {
                        getRO_Table();
                        comboBox3.Enabled = true;
                        MessageBox.Show("Your request has been successfully submitted and subject for approval! Thank you!", "CONFIRM!");
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (RO_Table.Rows.Count > 0)
            {
                RO_counter--;

                if (RO_counter == 0)
                {
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter < 0)
                {
                    RO_counter++;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter == RO_Table.Rows.Count)
                {
                    RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter > RO_Table.Rows.Count)
                {
                    RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else
                {
                    //RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                    //MessageBox.Show("No more data to show!");
                }
            }
        }

        public void retrieve_request(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                comboBox3.Text = dt.Rows[0]["RONumber"].ToString();
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (RO_Table.Rows.Count > 0)
            {
                RO_counter++;

                if (RO_counter == 0)
                {
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter == RO_Table.Rows.Count)
                {
                    RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter > RO_Table.Rows.Count - 1)
                {
                    RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else if (RO_counter < 0)
                {
                    RO_counter++;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
                else
                {
                    // RO_counter--;
                    DataTable dt = ro.RequestOrderCount(RO_Table, RO_counter);
                    retrieve_request(dt);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dataGridView1.Rows.Clear();
            comboBox1.Text = "";
            button1.Enabled = true;
            //comboBox3.Text = "";
            comboBox4.Text = "";
           // txtRONo.Text = "";
            dateTimePicker2.Enabled = true;
            button4.Enabled = true;
            textBox8.ReadOnly = false;
            textBox8.BackColor = System.Drawing.Color.White;

            comboBox5.Enabled = true;
            button6.Enabled = true;
            //==> Ren Add new code | 2019-03-18
            label6.Visible = false;
            label8.Visible = false;
            comboBox1.Visible = false;
            comboBox4.Visible = false;
            //==> End
            comboBox1.Enabled = true;
            comboBox4.Enabled = true;
            textBox8.Text = string.Empty;
            comboBox2.Enabled = true;
            textBox2.Enabled = true;
            label2.Text = "Current Status: Preparation";
            comboBox3.Items.Clear();
            comboBox3.Enabled = false;
            comboBox3.Text = string.Empty;
            label11.Hide();
            textBox4.Hide();
            button3.Hide();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            string itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();

            DataSet dsItem = item.getItemByID(int.Parse(itemID));
            if (dsItem.Tables.Count > 0)
            {
                if (dsItem.Tables[0].Rows.Count > 0)
                {
                    textBox1.Text = dsItem.Tables[0].Rows[0]["Unit"].ToString();  
                }
            }

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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.Text != "")
                {
                    DataTable dtTemp = new DataTable();
                    DataSet dsTemp = new DataSet();
                    dtTemp.Columns.Add("RONo", typeof(string));
                    dtTemp.Columns.Add("Date", typeof(string));
                    dtTemp.Columns.Add("Quantity", typeof(string));
                    dtTemp.Columns.Add("Description", typeof(string));
                    dtTemp.Columns.Add("Purpose", typeof(string));
                    dtTemp.Columns.Add("Requestor", typeof(string));
                    dtTemp.Columns.Add("Endorsed", typeof(string));
                    dtTemp.Columns.Add("Approved", typeof(string));
                    dtTemp.Columns.Add("ItemCode", typeof(string));

                    var query = RO_Table.AsEnumerable()
                                 .Where(p => p.Field<string>("RONumber") == comboBox3.Text)
                                 ;
                    if (query.Any())
                    {
                        if (query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("APPROVED") == true
                            || query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("CANVASS") == true)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                dtTemp.Rows.Add(comboBox3.Text, dateTimePicker1.Value.ToShortDateString().Substring(0, 10), " " + row.Cells["Quantity"].Value.ToString() + " " + row.Cells["Unit"].Value.ToString(),
                                    " " + row.Cells["Description"].Value.ToString(), textBox8.Text, "SGD " + textBox3.Text,
                                    "SGD " + comboBox1.Text, "SGD " + comboBox4.Text, row.Cells["ItemCode"].Value.ToString());
                            }
                        }
                        else if (query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("NOTED") == true)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                dtTemp.Rows.Add(comboBox3.Text, dateTimePicker1.Value.ToShortDateString().Substring(0, 10), " " + row.Cells["Quantity"].Value.ToString() + " " + row.Cells["Unit"].Value.ToString(),
                                    " " + row.Cells["Description"].Value.ToString(), textBox8.Text, "SGD " + textBox3.Text,
                                    "SGD " + comboBox1.Text, " ", row.Cells["ItemCode"].Value.ToString());
                            }

                        }
                        else
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                dtTemp.Rows.Add(comboBox3.Text, dateTimePicker1.Value.ToShortDateString().Substring(0, 10), " " + row.Cells["Quantity"].Value.ToString() + " " + row.Cells["Unit"].Value.ToString(),
                                    " " + row.Cells["Description"].Value.ToString(), textBox8.Text, "SGD " + textBox3.Text, "", " ", row.Cells["ItemCode"].Value.ToString());
                            }
 
                        }
                    }

                    dsTemp.Tables.Add(dtTemp);

                    UI_Report.Report_RO rro = new Report_RO("Request Order Monitoring", dsTemp, 1);
                    rro.ShowDialog();
                }
                else
                {
                    MessageBox.Show("NO DATA TO PRINT!", "ERROR!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var query = RO_Table.AsEnumerable()
                             .Where(p => p.Field<string>("RONumber") == comboBox3.Text)
                             ;

                if (query.Any())
                {
                    getCurrentStat(int.Parse(query.CopyToDataTable().Rows[0]["ID"].ToString()));
                    dateTimePicker1.Value = DateTime.Parse(query.CopyToDataTable().Rows[0]["DateRequested"].ToString());
                    dateTimePicker2.Value = DateTime.Parse(query.CopyToDataTable().Rows[0]["TargetDate"].ToString());
                    dateTimePicker2.Enabled = false;
                    textBox8.Text = query.CopyToDataTable().Rows[0]["Remarks"].ToString();
                    textBox8.ReadOnly = true;
                    textBox8.BackColor = System.Drawing.Color.PaleGoldenrod;
                    comboBox5.Enabled = false;
                    button6.Enabled = false;
                    button4.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox4.Enabled = false;
                    button1.Enabled = false;

                    if (query.CopyToDataTable().Rows[0]["Urgent"].ToString().Trim() == "0")
                    {
                        comboBox5.Text = "No";
                    }
                    else if (query.CopyToDataTable().Rows[0]["Urgent"].ToString().Trim() == "1")
                    {
                        comboBox5.Text = "Yes";
                    }

                    comboBox1.Text = query.CopyToDataTable().Rows[0]["Endorser"].ToString();
                    // comboBox3.Text = dt.Rows[0]["Recommender"].ToString();
                    comboBox4.Text = query.CopyToDataTable().Rows[0]["Approver"].ToString();
                    DataTable details = new DataTable();

                    details = ro.getRO_Details(int.Parse(query.CopyToDataTable().Rows[0]["ID"].ToString()));
                    if (details.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        foreach (DataRow row in details.Rows)
                        {
                            dataGridView1.Rows.Add(false, row["ID"].ToString(),
                                                    row["Qty"].ToString(),
                                                    row["ItemCode"].ToString(),
                                                    row["ItemName"].ToString(),
                                                    row["Unit"].ToString());
                        }
                    } 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!"); 
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            getRO_Table();
        }
    }
}
