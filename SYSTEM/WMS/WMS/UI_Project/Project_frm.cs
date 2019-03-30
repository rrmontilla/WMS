using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Uploading.UI
{
    public partial class Project_frm : Form
    {
        //Connection.Converter con = new Connection.Converter();
        //Connection.CRUD crud = new Connection.CRUD();

        DataSet ds = new DataSet();
        DataTable dtPN = new DataTable();
        public Project_frm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Project_frm_Load(object sender, EventArgs e)
        {
            Fill_Data();
            comboBox2.Text = "Industrial";
            textBox1.Text = DateTime.Now.ToString("F");
        }

        private void Fill_Data()
        {
            try
            {
                //foreach (DataRow row in con.ToDataTable(crud.GetAllItems()).Rows)
                //{
                //    Model.ConstructionModel items = new Model.ConstructionModel();
                //    items.Description = row["Description"].ToString() + " ~ " + row["ItemCode"].ToString();
                //    //items.Value = row["ID"].ToString();
                //    comboBox5.Items.Add(items.Description);
                //}

                //ds = con.ToDataSet(crud.GetAllData("Construction", ""));

                //var query1 = ds.Tables[0].AsEnumerable()
                //            .GroupBy(r => new { GroupCode = r["ConstructionCode"] })
                //            .Select(g => g.OrderBy(r => r["ConstructionCode"]).First())
                //            ;

                //if (query1.Any())
                //{
                //    comboBox1.DataSource = query1.CopyToDataTable();
                //    comboBox1.DisplayMember = "ConstructionCode";
                //    comboBox1.ValueMember = "ConstructionCode";
                //    comboBox1.Enabled = true;
                //}
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var query1 = ds.Tables[0].AsEnumerable()
                                .Where(p => p.Field<string>("ConstructionCode") == comboBox1.Text)
                                ;

                if (query1.Any())
                {
                    foreach(DataRow row in query1.CopyToDataTable().Rows)
                    {
                        dataGridView1.Rows.Add(comboBox1.Text, row["ItemCode"].ToString().Trim()
                            , row["Description"].ToString().Trim(), row["Quantity"].ToString().Trim()
                            , row["Unit"].ToString().Trim());
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Length == 0 || textBox6.Text.Length == 0)
            {
                MessageBox.Show("SOMETHING WENT WRONG! PLEASE FILL IN ALL THE DATA INFORMATION!", "ERROR!");
            }
            else
            {
                if (int.Parse(textBox7.Text) > 1)
                {
                    textBox6.Text = textBox6.Text + "s";
                }
                dataGridView1.Rows.Add("", comboBox5.Text.Split('~')[1].Trim(),
                                           comboBox5.Text.Split('~')[0].Trim(),
                                           textBox7.Text,
                                           textBox6.Text);
                textBox7.Text = "";
            }
        }
    }
}
