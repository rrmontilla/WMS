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
    public partial class Position_frm : Form
    {

        PositionController pos = new PositionController();
        DataSet dsTemp = new DataSet();
        public Position_frm()
        {
            InitializeComponent();
        }
        public void displayData()
        {
            DataSet ds = pos.getPosition();
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
                MessageBox.Show("Position Name is Empty.");
            }
            else if (comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Status is Empty.");

            }
            else
            {
                PositionModel model = new PositionModel();
                model.PositionName = textBox1.Text.Trim();
                model.Status = comboBox1.Text.Trim();

                if (button1.Text.Trim() == "Save" || button1.Text.Trim() == "&Save")
                {
                    string response = pos.InsertPosition(model);
                    if (response.Trim() == "SUCCESS")
                    {
                        MessageBox.Show("Branch successfully added.");
                    }
                }
                else if (button1.Text.Trim() == "Update" || button1.Text.Trim() == "&Update")
                {
                    model.ID = int.Parse(textBox2.Text);
                    string response = pos.UpdatePosition(model);
                    if (response.Trim() == "SUCCESS")
                    {
                        MessageBox.Show("Branch successfully added.");
                    }
                }
                displayData();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];

                textBox1.Text = Convert.ToString(selectedrow.Cells["PositionName"].Value);
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

        private void Position_frm_Load(object sender, EventArgs e)
        {
            displayData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("You were about to print the list of position's.\n\nClick 'Yes' to print the selected position.\n\nClick 'No' to print all the position's.\n\nClick 'Cancel' to cancel printing.", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                var query = dsTemp.Tables[0].AsEnumerable()
                    .Where(p => p.Field<int>("ID") == int.Parse(textBox2.Text))
                    ;

                if (query.Any())
                {
                    DataSet dst = new DataSet();
                    dst.Tables.Add(query.CopyToDataTable());
                    UI_Report.Report_RO rpt = new UI_Report.Report_RO("POST", dst, 0);
                    rpt.ShowDialog();
                }
            }
            if (res == DialogResult.No)
            {
                UI_Report.Report_RO rpt = new UI_Report.Report_RO("POST", dsTemp, 0);
                rpt.ShowDialog();
            }
        }
    }
}
