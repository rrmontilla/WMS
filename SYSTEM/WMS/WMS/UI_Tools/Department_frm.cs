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

namespace WMS
{
    public partial class Department_frm : Form
    {
        DepartmentController dept = new DepartmentController();
        DataSet dsTemp;
        public Department_frm()
        {
            InitializeComponent();
        }

        private void Department_frm_Load(object sender, EventArgs e)
        {
            displayData();
        }

        public void displayData()
        {
            DataSet ds = dept.getDepartment();
            dsTemp = null;
            dsTemp = ds;
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Department name is empty.");
            }
            else if (comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Status for this department is empty.");
            }
            else
            {
                DepartmentModel model = new DepartmentModel();
                model.DeptName = textBox1.Text.Trim();
                model.Status = comboBox1.Text.Trim();


                if (button1.Text.Trim() == "Save" || button1.Text.Trim() == "&Save")
                {
                    string response = dept.InsertDept(model);
                    if (response.Trim() == "SUCCESS")
                    {
                        MessageBox.Show("New department added.");
                    }
                }
                else if (button1.Text.Trim() == "Update" || button1.Text.Trim() == "&Update")
                {
                    model.ID = int.Parse(textBox2.Text.Trim());
                    string response = dept.UpdateDept(model);
                    if (response.Trim() == "SUCCESS")
                    {
                        MessageBox.Show("New department added.");
                    }
                }
                displayData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];

                textBox1.Text = Convert.ToString(selectedrow.Cells["DeptName"].Value);
                textBox2.Text = Convert.ToString(selectedrow.Cells["ID"].Value);

                button1.Text = "Update";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox1.Text = "";
            button1.Text = "Save";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("You were about to print the list of department's.\n\nClick 'Yes' to print the selected department.\n\nClick 'No' to print all the department's.\n\nClick 'Cancel' to cancel printing.", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                var query = dsTemp.Tables[0].AsEnumerable()
                    .Where(p => p.Field<int>("ID") == int.Parse(textBox2.Text))
                    ;

                if (query.Any())
                {
                    DataSet dst = new DataSet();
                    dst.Tables.Add(query.CopyToDataTable());
                    UI_Report.Report_RO rpt = new UI_Report.Report_RO("DEPT", dst, 0);
                    rpt.ShowDialog();
                }
            }
            if (res == DialogResult.No)
            {
                UI_Report.Report_RO rpt = new UI_Report.Report_RO("DEPT", dsTemp, 0);
                rpt.ShowDialog();
            }
        }
    }
}
