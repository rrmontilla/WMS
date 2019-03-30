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

namespace WMS
{
    public partial class CanvassPrep_frm : Form
    {
        SignatoryController lSignatoryCtrl;
        UserController user = new UserController();
        SupplierController sup = new SupplierController();
        RequestOrderController ro = new RequestOrderController();
        CanvassController canvass = new CanvassController();
        public DataSet RO_Table = new DataSet();
        public DataTable Canvass_Table = new DataTable();
        public DataSet RO_ReqData = new DataSet();
        public DataSet DSSupplier = new DataSet();
        public int Canvass_Counter = 0;
        public DataSet ROItemsDetails = new DataSet();
        public string frm_type = "";
        string ROuser = "";
        string RODateReq = "";
        string RODept = "";

        DataTable dtItems = new DataTable();

        public CanvassPrep_frm(string type)
        {
           frm_type = type;
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        DataSet dsCanvassData = new DataSet();
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox5.Text != "")
                {
                    GetRORequestor();
                    string data = string.Empty;
                    var query = Canvass_Table.AsEnumerable()
                                 .Where(p => p.Field<string>("CanvassNumber") == comboBox5.Text)
                                 ;
                    if (query.Any())
                    {
                        dsCanvassData = canvass.GetPrintableReport("CANVASS " + comboBox5.Text, dateTimePicker1.Value
                                        , dateTimePicker1.Value, "0");

                        if (query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("APPROVED") == true
                            || query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("PO") == true)
                        {
                            data = comboBox5.Text + " $ " +
                                            comboBox1.Text + " $ " +
                                            DateTime.Now.ToShortDateString() + " $ " +
                                            ROuser + " $ " +
                                            DateTime.Parse(RODateReq).ToShortDateString() + " $ " +
                                            textBox5.Text + " $ SGD " +
                                            textBox3.Text + " $ SGD " +
                                            comboBox6.Text + " $ " +
                                            RODept + " $ " +
                                            dateTimePicker2.Value.ToShortDateString() + " $ " +
                                            textBox2.Text + " $ SGD " +
                                            comboBox4.Text;
                        }
                        else if (query.CopyToDataTable().Rows[0]["Status"].ToString().Contains("NOTED") == true)
                        {
                            data = comboBox5.Text + " $ " +
                                            comboBox1.Text + " $ " +
                                            DateTime.Now.ToShortDateString() + " $ " +
                                            ROuser + " $ " +
                                            DateTime.Parse(RODateReq).ToShortDateString() + " $ " +
                                            textBox5.Text + " $ SGD " +
                                            textBox3.Text + " $ SGD " +
                                            comboBox6.Text + " $ " +
                                            RODept + " $ " +
                                            dateTimePicker2.Value.ToShortDateString() + " $ " +
                                            textBox2.Text + " $ " +
                                            "-";
                        }
                        else
                        {
                            data = comboBox5.Text + " $ " +
                                            comboBox1.Text + " $ " +
                                            DateTime.Now.ToShortDateString() + " $ " +
                                            ROuser + " $ " +
                                            DateTime.Parse(RODateReq).ToShortDateString() + " $ " +
                                            textBox5.Text + " $ SGD " +
                                            textBox3.Text + " $ " +
                                            "-" + " $ " +
                                            RODept + " $ " +
                                            dateTimePicker2.Value.ToShortDateString() + " $ " +
                                            textBox2.Text + " $ " +
                                            "-";
                        }
                    }

                    UI_Report.Report_RO rro = new UI_Report.Report_RO("CANVASSING $ " + data, dsCanvassData, 3);
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

        private void CanvassPrep_frm_Load(object sender, EventArgs e)
        {
            try
            {
                label18.Text = "Request Summary: " + 0 + " Request";
                fill_userID();
                fill_supplier();
                textBox3.Text = Program.loginfrm.fullname;
                getCanvass_Table();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                    this.BeginInvoke(new MethodInvoker(Close));
            }
        }
        public void getCanvass_Table()
        {
            if (frm_type.Trim() != "")
            {
                DataSet ds = new DataSet();
                if (frm_type.Trim() == "Preparation")
                {
                    label21.Hide();
                    textBox11.Hide();
                    button2.Hide();

                    EnAbleBTN();
                    this.Text = "Canvass Preparation";
                    label16.Text = "Canvass Preparation";
                    button1.Text = "Submit for note";
                    ds = canvass.getCanvasData(int.Parse(Program.loginfrm.userid), "PENDING");
                    button8.Hide();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        button1.Enabled = false;
                        button7.Hide();
                    }
                }
                else if (frm_type == "Endorse")
                {
                    lSignatoryCtrl = new SignatoryController();
                    lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.Canvass);
                    label21.Hide();
                    textBox11.Hide();
                    button2.Hide();

                    this.Text = "Canvass Endorsement";
                    label16.Text = "Canvass Endorsement";
                    DisableBTN();
                    button1.Text = "Submit for Approve";
                    ds = canvass.getCanvasData(int.Parse(Program.loginfrm.userid), "CANVASS-PENDING");

                }
                else if (frm_type == "Approved")
                {
                    lSignatoryCtrl = new SignatoryController();
                    lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Approver, TransactionType.Canvass);
                    label21.Hide();
                    textBox11.Hide();
                    button2.Hide();

                    this.Text = "Canvass Approval";
                    //--Commented By Ren | 2019-03-28 I don't know what the purpose of this dialog box
                    //MessageBox.Show("Please select one supplier per item in order to approved.\n\n Thank you!");
                    //--ENd
                    DisableBTN();
                    label16.Text = "Canvass Approval";
                    button1.Text = "Approved";
                    ds = canvass.getCanvasData(int.Parse(Program.loginfrm.userid), "CANVASS-NOTED");
                }
                else if (frm_type == "Print")
                {
                    if (textBox11.Text.Length != 0)
                    {
                        ds = ro.Print_Transaction(int.Parse(Program.loginfrm.userid), textBox11.Text, "CANVASS");

                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("No Canvass Number found!\n\n Thank you!", "ERROR!");
                            return;
                        }
                    }
                    else
                    {
                        label21.Show();
                        textBox11.Show();
                        button2.Show();

                        button1.Hide();
                        button4.Hide();
                        button6.Hide();
                        button7.Hide();
                        button8.Hide();
                    }
                }


                if (ds.Tables.Count > 0)
                {
                    Canvass_Table = ds.Tables[0];
                    if (Canvass_Table.Rows.Count > 0)
                    {
                        label18.Text = "Request Summary: " + Canvass_Table.Rows.Count + " Request";
                        Canvass_Counter = Canvass_Table.Rows.Count;
                        //DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Table.Rows.Count - 1);
                        retrieve_Canvass(Canvass_Table);
                    }
                    else
                    {
                        label18.Text = "Request Summary: " + 0 + " Request";
                        newTransaction();
                    }
                }
                else
                {
                    if (frm_type.Trim() == "Preparation")
                    {
                        label18.Text = "Request Summary: " + 0 + " Request";
                        newTransaction();
                    }
                    {
                        return;
                    }
                }
                InitializeForm(frm_type);
                if (comboBox5.Text.Length != 0)
                {
                    textBox5.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox6.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                    textBox10.Enabled = false;
                }

            }
        
        }

        public void DisableBTN() //Disable unnecessary button for noter and approver
        {
            comboBox1.Enabled = false;
            button4.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button3.Hide();
            //button9.Visible = false;
            button8.Visible = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            textBox7.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox10.ReadOnly = true;

            textBox7.BackColor = System.Drawing.Color.PaleGoldenrod;
            textBox9.BackColor = System.Drawing.Color.PaleGoldenrod;
            textBox8.BackColor = System.Drawing.Color.PaleGoldenrod;
            textBox10.BackColor = System.Drawing.Color.PaleGoldenrod;
        }

        public void EnAbleBTN() //Enable BTn for canvasser
        {
            comboBox1.Enabled = true;
            button4.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            //button9.Visible = true;
            button8.Visible = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            textBox7.ReadOnly = false;
            textBox9.ReadOnly = false;
            textBox8.ReadOnly = false;
            textBox10.ReadOnly = false;

            textBox7.BackColor = System.Drawing.Color.White;
            textBox9.BackColor = System.Drawing.Color.White;
            textBox8.BackColor = System.Drawing.Color.White;
            textBox10.BackColor = System.Drawing.Color.White; 
        }
        public void retrieve_Canvass(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                comboBox5.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    ComboBoxItem items = new ComboBoxItem();
                    items.Text = row["CanvassNumber"].ToString();
                    items.Value = row["ID"].ToString();
                    comboBox5.Items.Add(items);
                }

                comboBox5.Text = dt.Rows[0]["CanvassNumber"].ToString();                
            }
        }
        public void fill_ROItemsDetails(int ROID)
        {
            DataSet ds = new DataSet();
            ds = ro.getApprovedDetails(ROID);
            ROItemsDetails = ds;
            comboBox2.Items.Clear();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["ItemName"].ToString();
                        items.Value = row["ItemID"].ToString();
                        comboBox2.Items.Add(items);

                    }
                }
            }
        }
        public void fill_ROReference()
        {
            //DataSet ds = new DataSet();
            RO_Table = ro.getApprovedRO();
            if (RO_Table.Tables.Count > 0)
            {
                if (RO_Table.Tables[0].Rows.Count > 0)
                {
                    comboBox1.Items.Clear();
                    comboBox1.Text = "";
                    comboBox1.Enabled = true;
                    foreach (DataRow row in RO_Table.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["RONumber"].ToString();
                        items.Value = row["RONumber"].ToString();
                        comboBox1.Items.Add(items);

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
                        comboBox6.Items.Add(items);
                        comboBox4.Items.Add(items);

                    }
                }
            }
        }
        public void fill_supplier()
        {
            DataSet ds = new DataSet();
            ds = sup.SelectSupplier();
            DSSupplier = ds;
            comboBox3.Items.Clear();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["SupplierName"].ToString();
                        items.Value = row["ID"].ToString();
                        comboBox3.Items.Add(items);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Trim() == "")
            {
                MessageBox.Show("");
            }
            else if (textBox9.Text.Trim() == "")
            {
                MessageBox.Show("");
            }else if(textBox1.Text.Trim() == "")
            {
                MessageBox.Show("");
            }else if(textBox4.Text.Trim() == "")
            {
                MessageBox.Show("");
            }
            else if (textBox8.Text.Trim() == "")
            {
                MessageBox.Show("");
            }
            else if (textBox10.Text.Trim() == "")
            {
                MessageBox.Show("");
            }
            else
            {
                string itemCode = string.Empty;
                string supplierCode = string.Empty;
                var query = ROItemsDetails.Tables[0].AsEnumerable()
                            .Where(p => p.Field<string>("ItemName") == comboBox2.Text)
                            ;

                if (query.Any())
                {
                    itemCode = query.CopyToDataTable().Rows[0]["ItemCode"].ToString().Trim();
                }

                var query1 = DSSupplier.Tables[0].AsEnumerable()
                            .Where(p => p.Field<string>("SupplierName") == comboBox3.Text)
                            ;

                if (query1.Any())
                {
                    supplierCode = query1.CopyToDataTable().Rows[0]["SupplierCode"].ToString().Trim();
                }

                if (dataGridView1.Rows.Count > 0)
                {
                    string itemID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();
                    string suppID = (comboBox3.SelectedItem as ComboBoxItem).Value.ToString();
                    int count = 0;
                    int count_supp = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (itemID.Trim() == row.Cells["ItemID"].Value.ToString().Trim())
                        {
                            count++;
                        }

                        if (itemID.Trim() == row.Cells["ItemID"].Value.ToString().Trim() && suppID.Trim() == row.Cells["SupplierID"].Value.ToString().Trim())
                        {
                            count_supp++;
                        }
                    }

                    if (count < 3 && count_supp < 1)
                    {
                        dataGridView1.Rows.Add(false, itemCode,
                                       (comboBox2.SelectedItem as ComboBoxItem).Value.ToString(),
                                       comboBox2.Text,
                                       textBox1.Text,
                                       (comboBox3.SelectedItem as ComboBoxItem).Value.ToString(),
                                       comboBox3.Text,
                                       supplierCode,
                                       textBox7.Text,
                                       textBox9.Text,
                                       textBox4.Text,
                                       textBox10.Text,
                                       textBox8.Text);
                    }
                    else
                    {
                        MessageBox.Show("Unable to add data! It might be the item exceed to more than three(3) Supplier.");
                        return;
                    }
                }
                else
                {
                    dataGridView1.Rows.Add(false, itemCode,
                                   (comboBox2.SelectedItem as ComboBoxItem).Value.ToString(),
                                   comboBox2.Text,
                                   textBox1.Text,
                                   (comboBox3.SelectedItem as ComboBoxItem).Value.ToString(),
                                   comboBox3.Text,
                                   supplierCode,
                                   textBox7.Text,
                                   textBox9.Text,
                                   textBox4.Text,
                                   textBox10.Text,
                                   textBox8.Text);
                }

                textBox7.Text = "";
                textBox9.Text = "";
                //textBox1.Text = "";
                //textBox4.Text = "";
                textBox8.Text = "";
                textBox10.Text = "";
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Changing this option will cancel all your current transaction!. \n Would you like to proceed?", "Warning!!", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    NewTransaction();
                }
                else if (dialog == DialogResult.No)
                {

                }
            }
            else
            {
                NewTransaction();
            //    if (RO_Table.Tables.Count > 0)
            //    {
            //        if (RO_Table.Tables[0].Rows.Count > 0)
            //        {
            //            foreach (DataRow row in RO_Table.Tables[0].Rows)
            //            {
            //                if (row["RONumber"].ToString().Trim() == comboBox1.Text)
            //                {
            //                    textBox2.Text = row["TargetDate"].ToString();
            //                    fill_ROItemsDetails(int.Parse(row["ID"].ToString()));
            //                    DataTable dt = user.selectUserByID(int.Parse(row["UserID"].ToString()));
            //                    textBox5.Text = row["Remarks"].ToString();
            //                  //  textBox3.Text = dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["middleName"].ToString() + " " + dt.Rows[0]["lastName"].ToString();
            //                }
            //            }
            //        }
            //    }
            }
        }

        public void NewTransaction()
        {
            if (RO_Table.Tables.Count > 0)
            {
                if (RO_Table.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    comboBox5.Text = "";
                    foreach (DataRow row in RO_Table.Tables[0].Rows)
                    {
                        if (row["RONumber"].ToString().Trim() == comboBox1.Text)
                        {
                            textBox2.Text = row["TargetDate"].ToString();
                            fill_ROItemsDetails(int.Parse(row["ID"].ToString()));
                            DataTable dt = user.selectUserByID(int.Parse(row["UserID"].ToString()));
                            textBox5.Text = row["Remarks"].ToString();
                            ROuser = row["UserID"].ToString();
                            RODateReq = row["DateRequested"].ToString();
                            //textBox3.Text = dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["middleName"].ToString() + " " + dt.Rows[0]["lastName"].ToString();
                        }
                    }
                }
            }
        }

        public void GetRORequestor()
        {
            RO_ReqData = ro.GetROByRONum(comboBox1.Text);
            if (RO_ReqData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in RO_ReqData.Tables[0].Rows)
                {
                    //if (row["RONumber"].ToString().Trim() == comboBox1.Text)
                    //{
                        //textBox2.Text = row["TargetDate"].ToString();
                        //fill_ROItemsDetails(int.Parse(row["ID"].ToString()));
                        //DataTable dt = user.selectUserByID(int.Parse(row["UserID"].ToString()));
                        //textBox5.Text = row["Remarks"].ToString();
                    ROuser = row["RequestorName"].ToString();
                    RODateReq = row["DateRequested"].ToString();
                    RODept = row["DeptName"].ToString();
                        //textBox3.Text = dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["middleName"].ToString() + " " + dt.Rows[0]["lastName"].ToString();
                   // }
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ROItemsDetails.Tables.Count > 0)
            {
                if (ROItemsDetails.Tables[0].Rows.Count > 0)
                {
                    string ID = (comboBox2.SelectedItem as ComboBoxItem).Value.ToString();

                    foreach (DataRow row in ROItemsDetails.Tables[0].Rows)
                    {
                        if (row["ItemID"].ToString().Trim() == ID)
                        {
                            textBox1.Text = row["QTY"].ToString().Trim();
                            textBox4.Text = row["Unit"].ToString().Trim();
                        }
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ProcessData();
        }

        private void button7_Click(object sender, EventArgs e)
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
        private void button6_Click(object sender, EventArgs e)
        {
            newTransaction();
        }
        public void newTransaction()
        {
            dateTimePicker1.Value = DateTime.Now;
            fill_ROReference();
            // fill_supplier();
            comboBox6.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox5.Enabled = false;
            dataGridView1.Rows.Clear();

            textBox5.Enabled = false;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            dateTimePicker2.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox10.Enabled = true;
            comboBox4.Enabled = false;
            comboBox6.Enabled = false;

            textBox5.Text = string.Empty;
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            comboBox4.Text = string.Empty;
            comboBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;
            textBox10.Text = string.Empty;
            button1.Enabled = true;
            button7.Show();

            label21.Hide();
            textBox11.Hide();
            button2.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Canvass_Table.Rows.Count > 0)
            {
                Canvass_Counter--;

                if (Canvass_Counter == 0)
                {
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter < 0)
                {
                    Canvass_Counter++;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter == Canvass_Table.Rows.Count)
                {
                    Canvass_Counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter > Canvass_Table.Rows.Count)
                {
                    Canvass_Counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else
                {
                    //RO_counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                    //MessageBox.Show("No more data to show!");
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (Canvass_Table.Rows.Count > 0)
            {
                Canvass_Counter++;

                if (Canvass_Counter == 0)
                {
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter == Canvass_Table.Rows.Count)
                {
                    Canvass_Counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter > Canvass_Table.Rows.Count - 1)
                {
                    Canvass_Counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else if (Canvass_Counter < 0)
                {
                    Canvass_Counter++;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
                else
                {
                    // RO_counter--;
                    DataTable dt = canvass.CanvassCount(Canvass_Table, Canvass_Counter);
                    retrieve_Canvass(dt);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (frm_type.Trim() == "Preparation")
            {
                ProcessData();
            }
            else
            {
                int CanvassID = 0;

                DataTable dt = Canvass_Table; 

                if (Canvass_Table.Rows.Count > 0)
                {
                    foreach (DataRow row in Canvass_Table.Rows)
                    {
                        if (row["RONumber"].ToString().Trim() == comboBox1.Text)
                        {
                            CanvassID = int.Parse(row["ID"].ToString().Trim());
                        }
                    }
                }



                DataTable container2 = new DataTable();
                container2.Columns.Add("CanvassID");
                container2.Columns.Add("SupplierID");
                container2.Columns.Add("ItemID");
                container2.Columns.Add("Qty");
                container2.Columns.Add("Unit");
                container2.Columns.Add("Terms");
                container2.Columns.Add("DeliveryDate");
                container2.Columns.Add("Warranty");
                container2.Columns.Add("UnitCost");

                if (CanvassID != 0)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (bool.Parse(row.Cells["Select"].Value.ToString()))
                        {
                            container2.Rows.Add(CanvassID,
                              row.Cells["SupplierID"].Value.ToString(),
                              row.Cells["ItemID"].Value.ToString(),
                              row.Cells["Qty"].Value.ToString(),
                              row.Cells["Unit"].Value.ToString(),
                              row.Cells["Terms"].Value.ToString(),
                              row.Cells["DeliveryDate"].Value.ToString(),
                              row.Cells["warranty"].Value.ToString(),
                              row.Cells["UnitCost"].Value.ToString());
                        }
                    }

                    // string data_summary = ToXML.Toxml(container);
                    //string data_items = ToXML.Toxml(container2);

                    // string response = canvass.SubmitCanvass(data_summary, data_items);
                    if (frm_type == "Approved")
                    {
                        if (container2.Rows.Count == 0)
                        {
                            MessageBox.Show("Please select a supplier per item in order to approved.\n\n Thank you!", "ERROR!");
                            return;
                        }
                    }

                    string response = canvass.ApproveCanvassDetails(container2, frm_type);
                    if (response == "SUCCESS")
                    {
                        comboBox5.Items.Clear();
                        comboBox5.Text = "";
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                        comboBox3.Text = "";
                        textBox1.Text = "";
                        textBox4.Text = "";
                        textBox3.Text = "";
                        getCanvass_Table();
                        MessageBox.Show("Successfully submitted!", "CONFIRMED!");
                    }
                    else
                    {
                        MessageBox.Show(response);
                    }
                }
                else
                {
                    MessageBox.Show("Save in draft for printing!");
                }
            }           
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var query = Canvass_Table.AsEnumerable()
                             .Where(p => p.Field<string>("CanvassNumber") == comboBox5.Text)
                             ;

                if (query.Any())
                {
                    comboBox1.Enabled = false;
                    comboBox1.Text = query.CopyToDataTable().Rows[0]["RONumber"].ToString();
                    textBox5.Text = query.CopyToDataTable().Rows[0]["Remarks"].ToString();
                    dateTimePicker1.Value = DateTime.Parse(query.CopyToDataTable().Rows[0]["DatePrepared"].ToString());
                    textBox2.Text = query.CopyToDataTable().Rows[0]["TargetDate"].ToString();
                    comboBox5.Text = query.CopyToDataTable().Rows[0]["CanvassNumber"].ToString();
                    comboBox6.Text = query.CopyToDataTable().Rows[0]["Noter"].ToString();
                    textBox3.Text = query.CopyToDataTable().Rows[0]["Canvassor"].ToString();
                    comboBox4.Text = query.CopyToDataTable().Rows[0]["Approver"].ToString();
                    var _roid = string.IsNullOrEmpty(query.CopyToDataTable().Rows[0]["ROID"].ToString()) ? "0" : query.CopyToDataTable().Rows[0]["ROID"].ToString();
                    fill_ROItemsDetails(int.Parse(_roid));

                    DataSet ds = canvass.getCanvassDetails1(int.Parse(query.CopyToDataTable().Rows[0]["ID"].ToString()));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dataGridView1.Rows.Clear();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                dataGridView1.Rows.Add(false,
                                         row["ItemCode"].ToString().Trim(),
                                         row["ItemID"].ToString().Trim(),
                                         row["Description"].ToString().Trim(),
                                         row["Qty"].ToString().Trim(),
                                         row["SupplierID"].ToString().Trim(),
                                         row["SupplierName"].ToString().Trim(),
                                         row["SupplierCode"].ToString().Trim(),
                                         row["Terms"].ToString().Trim(),
                                         row["Warranty"].ToString().Trim(),
                                         row["Unit"].ToString().Trim(),
                                         row["UnitCost"].ToString().Trim(),
                                         row["DeliveryDate"].ToString().Trim());
                            }
                            dataGridView1.Columns["ItemID"].Visible = false;
                        }
                    }
 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMENTHING WENT WRONG!", "ERROR");
                return;
            }
        }

        public void ProcessData()
        {
            DataTable container = new DataTable();
            container.Columns.Add("RONumber");
            container.Columns.Add("PositionID");
            container.Columns.Add("BranchID");
            container.Columns.Add("DeptID");
            container.Columns.Add("Urgent");
            container.Columns.Add("CanvassorID");
            container.Columns.Add("DatePrepared");
            container.Columns.Add("EndorserID");
            container.Columns.Add("DateEndorse");
            container.Columns.Add("ApproverID");
            container.Columns.Add("Remarks");
            container.Columns.Add("LDR");

            int posID = 0;
            int branchID = 0;
            int deptID = 0;
            int urgent = 0;
            int NotedBy = 0;
            int ApprovedBy = 0;
            int CanvassID = 0;
            bool RO_flag = false; // true for newly created else for update details
            RO_Table = ro.getApprovedRO();
            if (RO_Table.Tables.Count > 0)
            {
                if (RO_Table.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RO_Table.Tables[0].Rows)
                    {
                        if (row["RONumber"].ToString().Trim() == comboBox1.Text)
                        {
                            posID = int.Parse(row["PositionID"].ToString().Trim());
                            branchID = int.Parse(row["BranchID"].ToString().Trim());
                            deptID = int.Parse(row["DepartmentID"].ToString().Trim());
                            urgent = int.Parse(row["Urgent"].ToString().Trim());
                            NotedBy = 0;
                            ApprovedBy = 0;
                            //--Commented By Ren | 2019-03-28
                            //NotedBy = int.Parse((comboBox6.SelectedItem as ComboBoxItem).Value.ToString());
                            //ApprovedBy = int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString());
                            //--End
                            RO_flag = true;
                        }
                    }
                }
            }

            if (Canvass_Table.Rows.Count > 0 && RO_flag == false)
            {
                foreach (DataRow row in Canvass_Table.Rows)
                {
                    if (row["RONumber"].ToString().Trim() == comboBox1.Text)
                    {
                        posID = int.Parse(row["PositionID"].ToString().Trim());
                        branchID = int.Parse(row["BranchID"].ToString().Trim());
                        deptID = int.Parse(row["DeptID"].ToString().Trim());
                        urgent = int.Parse(row["Urgent"].ToString().Trim());
                        NotedBy = int.Parse(row["EndorserID"].ToString().Trim());
                        ApprovedBy = int.Parse(row["ApproverID"].ToString().Trim());
                        CanvassID = int.Parse(row["ID"].ToString().Trim());
                    }
                }
            }


            container.Rows.Add(comboBox1.Text,
                               posID,
                               branchID,
                               deptID,
                               urgent,
                               Program.loginfrm.userid,
                               dateTimePicker1.Value.ToString(),
                               NotedBy,
                               dateTimePicker1.Value.ToString(),
                               ApprovedBy,
                               textBox5.Text,
                               dateTimePicker2.Value.ToString());

            DataTable container2 = new DataTable();
            container2.Columns.Add("SupplierID");
            container2.Columns.Add("ItemID");
            container2.Columns.Add("Qty");
            container2.Columns.Add("Unit");
            container2.Columns.Add("Terms");
            container2.Columns.Add("DeliveryDate");
            container2.Columns.Add("Warranty");
            container2.Columns.Add("UnitCost");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                container2.Rows.Add(row.Cells["SupplierID"].Value.ToString(),
                                 row.Cells["ItemID"].Value.ToString(),
                                 row.Cells["Qty"].Value.ToString(),
                                 row.Cells["Unit"].Value.ToString(),
                                 row.Cells["Terms"].Value.ToString(),
                                 row.Cells["DeliveryDate"].Value.ToString(),
                                 row.Cells["warranty"].Value.ToString(),
                                 row.Cells["UnitCost"].Value.ToString());
            }

            string data_summary = ToXML.Toxml(container);
            string data_items = ToXML.Toxml(container2);

            string response = canvass.SubmitCanvass(data_summary, data_items, RO_flag, CanvassID);

            if (response == "SUCCESS")
            {
                comboBox5.Items.Clear();
                comboBox5.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                textBox1.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                getCanvass_Table();
                MessageBox.Show("Successfully saved and ready for noted signatory!", "CONFIRM!");
            }
            else
            {
                MessageBox.Show(response);
            }
            #region Commented By Ren | 2019-03-28
            //if (comboBox6.Text.Trim() == "")
            //{
            //    MessageBox.Show("Noted By is empty!");
            //}
            //else if (comboBox4.Text.Trim() == "")
            //{
            //    MessageBox.Show("Approved By is empty!");
            //}
            //else
            //{
            //    int posID = 0;
            //    int branchID = 0;
            //    int deptID = 0;
            //    int urgent = 0;
            //    int NotedBy = 0;
            //    int ApprovedBy = 0;
            //    int CanvassID = 0;
            //    bool RO_flag = false; // true for newly created else for update details
            //    RO_Table = ro.getApprovedRO();
            //    if (RO_Table.Tables.Count > 0)
            //    {
            //        if (RO_Table.Tables[0].Rows.Count > 0)
            //        {
            //            foreach (DataRow row in RO_Table.Tables[0].Rows)
            //            {
            //                if (row["RONumber"].ToString().Trim() == comboBox1.Text)
            //                {
            //                    posID = int.Parse(row["PositionID"].ToString().Trim());
            //                    branchID = int.Parse(row["BranchID"].ToString().Trim());
            //                    deptID = int.Parse(row["DepartmentID"].ToString().Trim());
            //                    urgent = int.Parse(row["Urgent"].ToString().Trim());
            //                    NotedBy = int.Parse((comboBox6.SelectedItem as ComboBoxItem).Value.ToString());
            //                    ApprovedBy = int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString());
            //                    RO_flag = true;
            //                }
            //            }
            //        }
            //    }

            //    if (Canvass_Table.Rows.Count > 0 && RO_flag == false)
            //    {
            //        foreach (DataRow row in Canvass_Table.Rows)
            //        {
            //            if (row["RONumber"].ToString().Trim() == comboBox1.Text)
            //            {
            //                posID = int.Parse(row["PositionID"].ToString().Trim());
            //                branchID = int.Parse(row["BranchID"].ToString().Trim());
            //                deptID = int.Parse(row["DeptID"].ToString().Trim());
            //                urgent = int.Parse(row["Urgent"].ToString().Trim());
            //                NotedBy = int.Parse(row["EndorserID"].ToString().Trim());
            //                ApprovedBy = int.Parse(row["ApproverID"].ToString().Trim());
            //                CanvassID = int.Parse(row["ID"].ToString().Trim());
            //            }
            //        }
            //    }


            //    container.Rows.Add(comboBox1.Text,
            //                       posID,
            //                       branchID,
            //                       deptID,
            //                       urgent,
            //                       Program.loginfrm.userid,
            //                       dateTimePicker1.Value.ToString(),
            //                       NotedBy,
            //                       dateTimePicker1.Value.ToString(),
            //                       ApprovedBy,
            //                       textBox5.Text,
            //                       dateTimePicker2.Value.ToString());

            //    DataTable container2 = new DataTable();
            //    container2.Columns.Add("SupplierID");
            //    container2.Columns.Add("ItemID");
            //    container2.Columns.Add("Qty");
            //    container2.Columns.Add("Unit");
            //    container2.Columns.Add("Terms");
            //    container2.Columns.Add("DeliveryDate");
            //    container2.Columns.Add("Warranty");
            //    container2.Columns.Add("UnitCost");

            //    foreach (DataGridViewRow row in dataGridView1.Rows)
            //    {
            //        container2.Rows.Add(row.Cells["SupplierID"].Value.ToString(),
            //                         row.Cells["ItemID"].Value.ToString(),
            //                         row.Cells["Qty"].Value.ToString(),
            //                         row.Cells["Unit"].Value.ToString(),
            //                         row.Cells["Terms"].Value.ToString(),
            //                         row.Cells["DeliveryDate"].Value.ToString(),
            //                         row.Cells["warranty"].Value.ToString(),
            //                         row.Cells["UnitCost"].Value.ToString());
            //    }

            //    string data_summary = ToXML.Toxml(container);
            //    string data_items = ToXML.Toxml(container2);

            //    string response = canvass.SubmitCanvass(data_summary, data_items, RO_flag, CanvassID);

            //    if (response == "SUCCESS")
            //    {
            //        comboBox5.Items.Clear();
            //        comboBox5.Text = "";
            //        comboBox1.Text = "";
            //        comboBox2.Text = "";
            //        comboBox3.Text = "";
            //        textBox1.Text = "";
            //        textBox4.Text = "";
            //        textBox3.Text = "";
            //        getCanvass_Table();
            //        MessageBox.Show("Successfully saved and ready for noted signatory!", "CONFIRM!");
            //    }
            //    else
            //    {
            //        MessageBox.Show(response);
            //    }
            //}
            #endregion
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            getCanvass_Table();
        }

        private void InitializeForm(string FormType)
        {
            label11.Visible = FormType == "Preparation" ? false : true;
            textBox3.Visible = FormType == "Preparation" ? false : true;
            label10.Visible = FormType == "Preparation" ? true : false;
            comboBox6.Visible = FormType == "Preparation" ? true : false;
            label8.Visible = FormType == "Preparation" ? true : false;
            comboBox4.Visible = FormType == "Preparation" ? true : false;
        }
    }
}
