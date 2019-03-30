﻿using GlobalObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.Controller;
using WMS.Model;
using WMS.UI_Report;

namespace WMS.UI_RO
{
    public partial class ROEndorse_frm : Form
    {
        RequestOrderController ro = new RequestOrderController();
        UserController user = new UserController();
        SignatoryController lSignatoryCtrl;
        DataTable RO_Table = new DataTable();
        public string ROID = "";
        int RO_counter = 0;
        //string title = "";
        public ROEndorse_frm()
        {
            //title = ttle;
            InitializeComponent();
            this.Text = "Request Order - Noted Form";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ROID == "0" || ROID == "")
            {
                MessageBox.Show("Failed!");
            }
            else
            {
                string response = ro.SubmitEndorse(int.Parse(ROID), int.Parse(Program.loginfrm.userid));
                if (response == "SUCCESS")
                {
                    textBox2.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    textBox10.Text = "";
                    comboBox1.Text = "";
                    dataGridView1.Rows.Clear();
                    getRO_Table();
                    comboBox1.Items.Clear();
                    MessageBox.Show("Request has been noted and submitted for approval!");
                }
                else
                {
                    MessageBox.Show("Failed!");
                }
            }
        }

        private void ROEndorse_frm_Load(object sender, EventArgs e)
        {
            try
            {
                lSignatoryCtrl = new SignatoryController();
                lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.RequestOrder);

                // getEndorser();
                this.Text = "Noted By Form";
                label2.Text = "Request Summary: " + 0 + " Request";
                getRO_Table();
                button5.Hide();

                //switch (title)
                //{
                //    case "Endorse":
                //        button1.Text = "Endorsed";
                //        label16.Text = "To Endorse Request Order";
                //        break;
                //    case "Recommend":
                //        button1.Text = "Recommended";
                //        label16.Text = "To Recommend Request Order";
                //        break;
                //    case "Approve":
                //        button1.Text = "Approved";
                //        label16.Text = "To Approve Request Order";
                //        break;
                //}
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                    this.BeginInvoke(new MethodInvoker(Close));
            }

        }

        //public void getEndorser()
        //{
        //   DataTable dt = ro.getEndorser(int.Parse(Program.loginfrm.userid));
        //   if (dt.Rows.Count > 0)
        //   {
        //       textBox2.Text = dt.Rows[0]["DateRequested"].ToString();
        //       textBox4.Text = dt.Rows[0]["TargetDate"].ToString();
        //       if (dt.Rows[0]["Urgent"].ToString().Trim() == "1")
        //       {
        //           textBox5.Text = "Yes";
        //       }
        //       else
        //       {
        //           textBox5.Text = "No";               
        //       }


        //       textBox8.Text = dt.Rows[0][""].ToString();
        //   }
        //}
        public void getRO_Table()
        {
            RO_Table = ro.getEndorser(int.Parse(Program.loginfrm.userid));
            if (RO_Table.Rows.Count > 0)
            {
                foreach (DataRow row in RO_Table.Rows)
                {
                    ComboBoxItem items = new ComboBoxItem();
                    items.Text = row["RONumber"].ToString();
                    items.Value = row["ID"].ToString();
                    comboBox1.Items.Add(items);
                }

                label2.Text = "Request Summary: " + RO_Table.Rows.Count + " Request";
                RO_counter = RO_Table.Rows.Count - 1;
                //DataTable dt = ro.EndorseCount(RO_Table, RO_Table.Rows.Count - 1);
                retrieve_request(RO_Table);
            }
            else
            {
                label2.Text = "Request Summary: " + 0 + " Request";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            RO_counter--;

            if (RO_counter == 0)
            {
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter < 0)
            {
                RO_counter++;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter == RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter > RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else
            {
                //RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
                //MessageBox.Show("No more data to show!");
            }
        }
        public void retrieve_request(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                comboBox1.Text = dt.Rows[0]["RONumber"].ToString();
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            RO_counter++;

            if (RO_counter == 0)
            {
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter == RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter > RO_Table.Rows.Count - 1)
            {
                RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter < 0)
            {
                RO_counter++;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else
            {
                // RO_counter--;
                DataTable dt = ro.EndorseCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "")
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

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        dtTemp.Rows.Add(comboBox1.Text, textBox2.Text.Substring(0, 10), row.Cells["Quantity"].Value.ToString(),
                            row.Cells["ItemCode"].Value.ToString() + " " + row.Cells["Description"].Value.ToString(), row.Cells["Purpose"].Value.ToString(), textBox3.Text, textBox6.Text, textBox7.Text);
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

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var query = RO_Table.AsEnumerable()
                        .Where(p => p.Field<string>("RONumber") == comboBox1.Text)
                        ;

            if (query.Any())
            {
                string purpose = string.Empty;
                ROID = query.CopyToDataTable().Rows[0]["ID"].ToString();
                comboBox1.Text = query.CopyToDataTable().Rows[0]["RONumber"].ToString();
                textBox2.Text = query.CopyToDataTable().Rows[0]["DateRequested"].ToString();
                textBox3.Text = query.CopyToDataTable().Rows[0]["Requestor"].ToString();
                textBox4.Text = query.CopyToDataTable().Rows[0]["TargetDate"].ToString();
                purpose = query.CopyToDataTable().Rows[0]["Remarks"].ToString();
                if (query.CopyToDataTable().Rows[0]["Urgent"].ToString().Trim() == "1")
                {
                    textBox5.Text = "Yes";
                }
                else
                {
                    textBox5.Text = "No";
                }

                textBox6.Text = query.CopyToDataTable().Rows[0]["Endorser"].ToString();
                // textBox1.Text = dt.Rows[0]["Recommender"].ToString();
                textBox7.Text = query.CopyToDataTable().Rows[0]["Approver"].ToString();

                DataTable user_details = new DataTable();
                user_details = user.selectUserByID(int.Parse(query.CopyToDataTable().Rows[0]["RequestorID"].ToString()));


                textBox8.Text = user_details.Rows[0]["DeptName"].ToString();
                textBox9.Text = user_details.Rows[0]["PositionName"].ToString();
                textBox10.Text = user_details.Rows[0]["BranchName"].ToString();

                DataTable details = new DataTable();
                details = ro.getRO_Details(int.Parse(query.CopyToDataTable().Rows[0]["ID"].ToString()));
                if (details.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow row in details.Rows)
                    {
                        dataGridView1.Rows.Add(row["ID"].ToString(),
                                               row["Qty"].ToString() + " " + row["Unit"].ToString(),
                                               row["ItemCode"].ToString(),
                                               row["ItemName"].ToString(),
                                               purpose);
                    }
                }
            }
        }
    }
}