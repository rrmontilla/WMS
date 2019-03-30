using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WMS.Controller;

namespace Uploading.UI
{
    public partial class ViewConstruction : Form
    {
        ConstructionController cons = new ConstructionController();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int stat = 0;
        public ViewConstruction()
        {
            InitializeComponent();
        }

        private void ViewConstruction_Load(object sender, EventArgs e)
        {
            Fill_Data();
        }

        private void Fill_Data()
        {
            ds = cons.GetAllConstruction("Construction", "");
            var query1 = ds.Tables[0].AsEnumerable()
                        .GroupBy(r => new { GroupCode = r["ConstructionCode"] })
                        .Select(g => g.OrderBy(r => r["ConstructionCode"]).First())
                        ;

            if (query1.Any())
            {
                comboBox1.DataSource = query1.CopyToDataTable();
                comboBox1.DisplayMember = "ConstructionCode";
                comboBox1.ValueMember = "ConstructionCode";
                comboBox1.Enabled = true;
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables[0];
            stat = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var query1 = ds.Tables[0].AsEnumerable()
                                .Where(p => p.Field<string>("ConstructionCode") == comboBox1.Text)
                                ;
                stat = 1;
                if (query1.Any())
                {
                    dataGridView1.DataSource = query1.CopyToDataTable();
                } 

            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;
                DataSet dsNew = new DataSet();

                if (stat == 1)
                {
                    DataTable newDT = new DataTable();
                    newDT = (DataTable)dataGridView1.DataSource;
                    dsNew.Tables.Add(newDT);
                    UI.Report rpt = new UI.Report("CONSTRUCTION", dsNew);
                    rpt.Show();

                }
                else
                {
                    //dsNew.Tables.Add(dt);
                    UI.Report rpt = new UI.Report("CONSTRUCTION", ds);
                    rpt.Show();
                }

                Cursor.Current = Cursors.WaitCursor;
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
