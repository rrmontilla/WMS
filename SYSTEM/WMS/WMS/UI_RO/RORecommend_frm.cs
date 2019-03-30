using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.Controller;

namespace WMS.UI_RO
{
    public partial class RORecommend_frm : Form
    {
        RequestOrderController ro = new RequestOrderController();
        UserController user = new UserController();
        DataTable RO_Table = new DataTable();
        public string ROID = "";
        int RO_counter = 0;

        public RORecommend_frm()
        {
            InitializeComponent();
        }

        private void RORecommend_frm_Load(object sender, EventArgs e)
        {
            getRO_Table();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ROID == "0" || ROID == "")
            {
                MessageBox.Show("Failed!");
            }
            else
            {
                string response = ro.SubmitRecommend(int.Parse(ROID));
                if (response == "SUCCESS")
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    textBox10.Text = "";
                    txtRONo.Text = "";
                    textBox11.Text = "";
                    dataGridView1.Rows.Clear();
                    getRO_Table();
                    MessageBox.Show("Request has been endorse for recommendation!");
                }
                else
                {
                    MessageBox.Show("Failed!");
                }
            }

        }
        public void getRO_Table()
        {
            RO_Table = ro.getForReccomendation(int.Parse(Program.loginfrm.userid));
            if (RO_Table.Rows.Count > 0)
            {
                RO_counter = RO_Table.Rows.Count - 1;
                DataTable dt = ro.RecommendCount(RO_Table,RO_Table.Rows.Count - 1);
                retrieve_request(dt);
            }
        }
        public void retrieve_request(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                ROID = dt.Rows[0]["ROID"].ToString();
                txtRONo.Text = dt.Rows[0]["RONumber"].ToString();
                textBox2.Text = dt.Rows[0]["RequestDate"].ToString();
                textBox3.Text = dt.Rows[0]["Requestor"].ToString();
                textBox4.Text = dt.Rows[0]["TargetDate"].ToString();
                if (dt.Rows[0]["Urgent"].ToString().Trim() == "1")
                {
                    textBox5.Text = "Yes";
                }
                else
                {
                    textBox5.Text = "No";
                }

                textBox6.Text = dt.Rows[0]["Endorser"].ToString();
                textBox1.Text = dt.Rows[0]["Recommender"].ToString();
                textBox7.Text = dt.Rows[0]["Approver"].ToString();
                textBox11.Text = dt.Rows[0]["DateEndorse"].ToString();

                DataTable user_details = new DataTable();
                user_details = user.selectUserByID(int.Parse(dt.Rows[0]["RequestorID"].ToString()));


                textBox8.Text = user_details.Rows[0]["DeptName"].ToString();
                textBox9.Text = user_details.Rows[0]["PositionName"].ToString();
                textBox10.Text = user_details.Rows[0]["BranchName"].ToString();

                DataTable details = new DataTable();
                details = ro.getRO_Details(int.Parse(dt.Rows[0]["ROID"].ToString()));
                if (details.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow row in details.Rows)
                    {
                        dataGridView1.Rows.Add(row["ID"].ToString(),
                                               row["Qty"].ToString(),
                                               row["ItemCode"].ToString(),
                                               row["ItemName"].ToString(),
                                               row["Unit"].ToString());
                    }
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RO_counter--;

            if (RO_counter == 0)
            {
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter < 0)
            {
                RO_counter++;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter == RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter > RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else
            {
                //RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
                //MessageBox.Show("No more data to show!");
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            RO_counter++;

            if (RO_counter == 0)
            {
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter == RO_Table.Rows.Count)
            {
                RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter > RO_Table.Rows.Count - 1)
            {
                RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else if (RO_counter < 0)
            {
                RO_counter++;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
            else
            {
                // RO_counter--;
                DataTable dt = ro.RecommendCount(RO_Table,RO_counter);
                retrieve_request(dt);
            }
        }
    }
}
