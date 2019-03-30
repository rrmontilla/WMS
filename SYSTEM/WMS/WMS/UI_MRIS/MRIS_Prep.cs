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


namespace WMS.UI_MRIS
{
    public partial class MRIS_Prep : Form
    {
        UserController user = new UserController();
        ItemsController item = new ItemsController();
        MRISController mris = new MRISController();
        string frm_type = "";
        int MRIS_counter = 0;
        int MRIS_ID = 0;
        DataTable MRIS_Table = new DataTable();
        public MRIS_Prep(string type)
        {
            frm_type = type;
            InitializeComponent();
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
            //if (comboBox5.Text.Trim() == "")
            //{
            //    MessageBox.Show("Select Urgent level!");
            //}
            //else if (textBox3.Text.Trim() == "")
            //{
            //    MessageBox.Show("Requested by is empty");
            //}
            //else if (comboBox1.Text.Trim() == "")
            //{
            //    MessageBox.Show("Endorser is empty!");
            //}
            //else if (comboBox3.Text.Trim() == "")
            //{
            //    MessageBox.Show("Recommender is empty!");
            //}
            //else 
           
                if (frm_type == "Preparation")
                {
                    submit_request();
                }
                else if (frm_type == "Approved")
                {
                    
                    string response = mris.Approved_MRIS(MRIS_ID, frm_type);
                }
                else if (frm_type == "Issued")
                {
                    string response = mris.Approved_MRIS(MRIS_ID, frm_type);
                }

            
        }

        public void submit_request()
        {
            try
            {
                DataTable container = new DataTable();
                container.Columns.Add("UserID");
                container.Columns.Add("PositionID");
                container.Columns.Add("RequestingOffice");
                container.Columns.Add("DateRequested");
                container.Columns.Add("DateNeeded");
                container.Columns.Add("ApproverID");
                //container.Columns.Add("DateRequested");
                container.Columns.Add("DateApproved");
                container.Columns.Add("Issuer");
                container.Columns.Add("DateIssued");
                container.Columns.Add("Purpose");


                container.Rows.Add(Program.loginfrm.userid,
                                   Program.loginfrm.posID,
                                   Program.loginfrm.deptID, //requesting office
                                   dateTimePicker1.Value.ToString(),
                                   dateTimePicker2.Value.ToShortDateString(),
                                   0,
                                   //(cbApproveBy.SelectedItem as ComboBoxItem).Value.ToString(),
                                   //dateTimePicker1.Value.ToString(),//"DateRequested",
                                   DateTime.Now,
                                   0,
                                   //(cbIssuedBy.SelectedItem as ComboBoxItem).Value.ToString(),
                                   DateTime.Now,
                                   textBox8.Text);

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

                string response = mris.SubmitMRIS(data_summary, data_items);
                if (response == "SUCCESS")
                {
                    getMRIS_Table();
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
            //Commented By Ren | 2019-03-30 Useless
            // if (cbIssuedBy.Text.Trim() == "")
            //{
            //    MessageBox.Show("Approver is empty!");
            //}
            // else if (txtRONo.Text != "")
            // {
            //     MessageBox.Show("Create a new request before submission!");
            // }
            // else
            // {
            //     try
            //     {
            //         DataTable container = new DataTable();
            //         container.Columns.Add("UserID");
            //         container.Columns.Add("PositionID");
            //         container.Columns.Add("RequestingOffice");
            //         container.Columns.Add("DateRequested");
            //         container.Columns.Add("DateNeeded");
            //         container.Columns.Add("ApproverID");
            //         //container.Columns.Add("DateRequested");
            //         container.Columns.Add("DateApproved");
            //         container.Columns.Add("Issuer");
            //         container.Columns.Add("DateIssued");
            //         container.Columns.Add("Purpose");


            //         container.Rows.Add(Program.loginfrm.userid,
            //                            Program.loginfrm.posID,
            //                            Program.loginfrm.deptID, //requesting office
            //                            dateTimePicker1.Value.ToString(),
            //                            dateTimePicker2.Value.ToShortDateString(),
            //                            (cbApproveBy.SelectedItem as ComboBoxItem).Value.ToString(),
            //             //dateTimePicker1.Value.ToString(),//"DateRequested",
            //                            DateTime.Now,
            //                            (cbIssuedBy.SelectedItem as ComboBoxItem).Value.ToString(),
            //                            DateTime.Now,
            //                            textBox8.Text);

            //         DataTable container1 = new DataTable();
            //         container1.Columns.Add("ItemID");
            //         container1.Columns.Add("ItemCode");
            //         container1.Columns.Add("ItemName");
            //         container1.Columns.Add("Qty");
            //         container1.Columns.Add("Unit");

            //         foreach (DataGridViewRow row in dataGridView1.Rows)
            //         {
            //             container1.Rows.Add(row.Cells["itemID"].Value.ToString(),
            //                              row.Cells["ItemCode"].Value.ToString(),
            //                              row.Cells["Description"].Value.ToString(),
            //                              row.Cells["Quantity"].Value.ToString(),
            //                              row.Cells["Unit"].Value.ToString()
            //                              );
            //         }

            //         string data_summary = ToXML.Toxml(container);
            //         string data_items = ToXML.Toxml(container1);

            //         string response = mris.SubmitMRIS(data_summary, data_items);
            //         if (response == "SUCCESS")
            //         {
            //             getMRIS_Table();
            //             MessageBox.Show("SuccessFully submitted!");
            //         }
            //         else
            //         {
            //             MessageBox.Show(response);
            //         }

            //     }
            //     catch (Exception ex)
            //     {
            //         MessageBox.Show(ex.Message);
            //     }
            // }
        }
        private void MRIS_Prep_Load(object sender, EventArgs e)
        {
            fill_items();
            fill_userID();
            getMRIS_Table();
        }
        public void getMRIS_Table()
        {
            if (frm_type == "Preparation")
            {
                label16.Text = "MRIS Preparation";
                button1.Text = "Submit Request";
                button2.Enabled = true;
                
            }
            else if (frm_type == "Approved")
            {
                label16.Text = "MRIS Approved";
                button1.Text = "Approved";
                button2.Enabled = false;
                
            }
            else if (frm_type == "Issued")
            {
                label16.Text = "MRIS Issuance";
                button1.Text = "Issue";
                button2.Enabled = false;
            }
            InitializeForm(frm_type);

            MRIS_Table = mris.getMRIS(int.Parse(Program.loginfrm.userid),frm_type);

            if (MRIS_Table.Rows.Count > 0)
            {
                MRIS_counter = MRIS_Table.Rows.Count - 1;
                DataTable dt = mris.MRISCount(MRIS_Table, MRIS_Table.Rows.Count - 1);
                retrieve_request(dt);
            }
        }
        public void retrieve_request(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                txtRONo.Text = dt.Rows[0]["MRISNo"].ToString();
                //getCurrentStat(int.Parse(dt.Rows[0]["ROID"].ToString()));
                dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["DateRequested"].ToString());
                dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["DateNeeded"].ToString());
                dateTimePicker2.Enabled = false;
                textBox8.Text = dt.Rows[0]["Purpose"].ToString();
                textBox8.ReadOnly = true;
                textBox8.BackColor = System.Drawing.Color.PaleGoldenrod;
               // comboBox5.Enabled = false;
                button6.Enabled = false;
                button4.Enabled = false;
                cbApproveBy.Enabled = false;
                cbIssuedBy.Enabled = false;
                txtRequestor.Text = dt.Rows[0]["Department"].ToString();
                txtRequestingOffice.Text = dt.Rows[0]["Department"].ToString();
                cbApproveBy.Text = dt.Rows[0]["Approver"].ToString();
                // comboBox3.Text = dt.Rows[0]["Recommender"].ToString();
                cbIssuedBy.Text = dt.Rows[0]["IssuerName"].ToString();
                label9.Text = "Request Status: " + dt.Rows[0]["Status"].ToString();
                DataTable details = new DataTable();

                details = mris.getMRIS_Details(int.Parse(dt.Rows[0]["MRISID"].ToString()));

                MRIS_ID = int.Parse(dt.Rows[0]["MRISID"].ToString());  //for approval and issued
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
        public void fill_items()
        {
            DataSet ds = new DataSet();
            ds = item.selectItems();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["ItemName"].ToString() + " ~ " + row["itemCode"].ToString();
                        items.Value = row["ID"].ToString();
                        comboBox2.Items.Add(items);
                    }
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
                        cbApproveBy.Items.Add(items);
                        cbIssuedBy.Items.Add(items);

                    }
                }
            }
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
            //else if (comboBox5.Text.Trim() == "")
            //{
            //    MessageBox.Show("Select Urgent Level!");

            //}
            else if (txtRONo.Text != "")
            {
                MessageBox.Show("Create a new request before submission!");
               
            }
            else
            {
                string itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();
                dataGridView1.Rows.Add(false,itemID, textBox2.Text,
                                       comboBox2.Text.Split('~')[1],
                                       comboBox2.Text.Split('~')[0],
                                       textBox1.Text.Trim());
                comboBox2.Text = "";
               // textBox8.Text = "";
                textBox2.Text = "";
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dataGridView1.Rows.Clear();
            cbApproveBy.Text = "";
            //comboBox3.Text = "";
            cbIssuedBy.Text = "";
            txtRONo.Text = "";
            dateTimePicker2.Enabled = true;
            button4.Enabled = true;
            textBox8.ReadOnly = false;
            textBox8.BackColor = System.Drawing.Color.White;

           // comboBox5.Enabled = true;
            button6.Enabled = true;

            //Commented By Ren | 2019-03-30 is useless
            //cbApproveBy.Enabled = true;
            //cbIssuedBy.Enabled = true;
            txtRequestingOffice.Text = Program.loginfrm.deptName;
            label9.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MRIS_Table.Rows.Count > 0)
            {
                MRIS_counter--;

                if (MRIS_counter == 0)
                {
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter < 0)
                {
                    MRIS_counter++;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter == MRIS_Table.Rows.Count)
                {
                    MRIS_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter > MRIS_Table.Rows.Count)
                {
                    MRIS_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else
                {
                    //RO_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                    //MessageBox.Show("No more data to show!");
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (MRIS_Table.Rows.Count > 0)
            {
                MRIS_counter++;

                if (MRIS_counter == 0)
                {
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter == MRIS_Table.Rows.Count)
                {
                    MRIS_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter > MRIS_Table.Rows.Count - 1)
                {
                    MRIS_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else if (MRIS_counter < 0)
                {
                    MRIS_counter++;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
                else
                {
                    // RO_counter--;
                    DataTable dt = mris.MRISCount(MRIS_Table, MRIS_counter);
                    retrieve_request(dt);
                }
            }
        }

        private void InitializeForm(string pFormType)
        {
            lblRequestor.Visible = pFormType == "Preparation" ? false : true;
            txtRequestor.Visible = pFormType == "Preparation" ? false : true;
            lblApproveBy.Visible = pFormType == "Preparation" ? true : false;
            cbApproveBy.Visible = pFormType == "Preparation" ? true : false;
            lblIssuedBy.Visible = pFormType == "Preparation" ? true : false;
            cbIssuedBy.Visible = pFormType == "Preparation" ? true : false;
            cbApproveBy.Enabled = pFormType == "Preparation" ? false : true;
            cbIssuedBy.Enabled = pFormType == "Preparation" ? false : true;
        }

    }
}
