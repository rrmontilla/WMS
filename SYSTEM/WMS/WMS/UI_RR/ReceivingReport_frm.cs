using GlobalObject;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WMS.Class;
using WMS.Controller;
using WMS.Model;

namespace WMS.UI_RR
{
    public partial class ReceivingReport_frm : Form
    {
        SignatoryController lSignatoryCtrl;
        PurchaseOrderController po = new PurchaseOrderController();
        ReceivingReportController rr = new ReceivingReportController();
        UserController user = new UserController();
        DataTable PO_Table = new DataTable();
        DataTable RR_Table = new DataTable();
        DataTable dtTR = new DataTable();

        DataSet ds = new DataSet();
        DataSet dsPrint = new DataSet();

        int RR_Counter = 0;
        int supID = 0;
        string frm_type = string.Empty;
        int qty = 0;
        int rrid = 0;
        int supplier = 0;
        int itemName = 0;
        string itemCode = string.Empty;
        public ReceivingReport_frm(string title)
        {
            frm_type = title;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataSet dsP = new DataSet();
            DataTable dt = new DataTable();
            DataSet dsToPrint = new DataSet();
            string dReq = string.Empty;
            dt.Columns.Add("Supplier");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Description");
            dt.Columns.Add("UnitPrice");
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("PONo");
            dt.Columns.Add("RONo");
            dt.Columns.Add("Address");
            dt.Columns.Add("PreparedBy");
            dt.Columns.Add("ApprovedBy");
            dt.Columns.Add("TIN");

            foreach (DataRow row in dsPrint.Tables[0].Rows)
            {
                dt.Rows.Add(row["SupplierName"].ToString()
                , row["Quantity"].ToString() + " " + row["Unit"].ToString()
                , row["Description"].ToString()
                , row["unitCost"].ToString()
                , decimal.Parse(row["unitCost"].ToString()) * decimal.Parse(row["Quantity"].ToString())
                , comboBox5.Text
                //, textBox3.Text
                , row["BusinessAddress"].ToString()
                , row["Prepared"].ToString() //PreparedBy
                , row["Approver"].ToString() //ApprovedBy
                , row["TIN"].ToString());

                dReq = DateTime.Parse(row["DatePrepared"].ToString().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim();
            }

            dsToPrint.Tables.Add(dt);
                
            UI_Report.Report_RO rro = new UI_Report.Report_RO("RECEIVING REPORT", dsToPrint, 7);//ds -> Commented by Ren | 2019-03-30 What is the purpose of this code.(ds)
            rro.ShowDialog();
        }

        private void ReceivingReport_frm_Load(object sender, EventArgs e)
        {
            try
            {
                label10.Text = "Request Summary: " + 0 + " Request";
                fill_userID();
                getRR_Table();
                //textBox3.Text = Program.loginfrm.fullname;
                getPOApproved();

                if (comboBox7.Text.Length == 0)
                {
                    button1.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox5.Enabled = false;
                    button4.Enabled = false;
                    textBox2.Enabled = false;
                    comboBox6.Enabled = false;
                }

                if (frm_type == "Endorse" || frm_type == "Approved")
                {
                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                    this.BeginInvoke(new MethodInvoker(Close));
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
                        comboBox1.Items.Add(items);
                        comboBox4.Items.Add(items);
                        comboBox6.Items.Add(items);
                    }
                }
            }
        }

        public void getRR_Table()
        {
            try
            {
                if (frm_type.Trim() != "")
                {
                    DataSet ds = new DataSet();

                    if (frm_type.Trim() == "Preparation")
                    {
                        label16.Text = "Receiving Report Preparation";
                        button1.Text = "Submit";
                        button2.Enabled = true;
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        comboBox4.Enabled = true;
                        ds = rr.getRRData(int.Parse(Program.loginfrm.userid), "PENDING");
                    }
                    else if (frm_type == "Endorse")
                    {
                        lSignatoryCtrl = new SignatoryController();
                        lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.ReceivingReport);

                        label16.Text = "Receiving Report For Noted";
                        this.Text = "Receiving Report For Noted";
                        button1.Text = "Noted";
                        button2.Enabled = false;
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = false;
                        comboBox4.Enabled = false;
                        ds = rr.getRRData(int.Parse(Program.loginfrm.userid), "RR-NOTED");

                    }
                    else if (frm_type == "Approved")
                    {
                        lSignatoryCtrl = new SignatoryController();
                        lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Approver, TransactionType.ReceivingReport);

                        label16.Text = "Receiving Report for Approval";
                        button1.Text = "Approved";
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = false;
                        comboBox4.Enabled = false;
                        button2.Enabled = false;
                        ds = rr.getRRData(int.Parse(Program.loginfrm.userid), "RR-APPROVED");
                    }

                    InitializeForm(frm_type);
                    if (ds.Tables.Count > 0)
                    {
                        RR_Table = ds.Tables[0];
                        if (RR_Table.Rows.Count > 0)
                        {
                            label10.Text = "Request Summary: " + RR_Table.Rows.Count + " Request";
                            RR_Counter = RR_Table.Rows.Count;
                            //DataTable dt = rr.RRCount(RR_Table, RR_Table.Rows.Count - 1);
                            //retrieve_RR(RR_Table);

                            foreach (DataRow row in RR_Table.Rows)
                            {
                                ComboBoxItem items = new ComboBoxItem();
                                items.Text = row["RRNo"].ToString();
                                items.Value = row["ID"].ToString();
                                comboBox7.Items.Add(items);
                            }
                        }
                        else
                        {
                            comboBox1.Enabled = true;
                            comboBox2.Enabled = true;
                            comboBox4.Enabled = true;
                            button1.Enabled = true;
                            label10.Text = "Request Summary: " + 0 + " Request";

                        }
                    }
                    else
                    {
                        label10.Text = "Request Summary: " + 0 + " Request";

                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void getPOApproved()
        {
            DataTable dt = po.getPOApproved().Tables[0];
            PO_Table = dt;
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow row in dt.Rows)
                {
                    ComboBoxItem items = new ComboBoxItem();
                    items.Text = row["PONumber"].ToString();
                    items.Value = row["ID"].ToString();
                    comboBox3.Items.Add(items);
                }
            }
        }

        public void retrieve_RR(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();

                if (frm_type != "Preparation")
                {
                    comboBox1.Text = dt.Rows[0]["PONo"].ToString();
                }

                dtTR.Rows.Clear();
                dtTR.Columns.Clear();
                TableReceived();
                foreach (DataRow row in dt.Rows)
                {
                    comboBox6.Text = row["Received"].ToString();
                    comboBox1.Text = row["Noted"].ToString();
                    comboBox4.Text = row["Approver"].ToString();

                    comboBox3.Text = row["PONo"].ToString();
                    comboBox7.Text = row["RRNo"].ToString();

                    rrid = int.Parse(row["RRID"].ToString());

                    dtTR.Rows.Add(row["DateReceived"].ToString().Substring(0, 10).Trim()
                                    , row["Supplier"].ToString()
                                    , row["Description"].ToString()
                                    , int.Parse(row["Quantity"].ToString()));

                }

                dataGridView1.DataSource = dtTR;
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text.Trim() != "" && comboBox7.Text.Trim() == "")
            {
                int poID = int.Parse((comboBox3.SelectedItem as ComboBoxItem).Value.ToString());


                string leadTime = "";
                string Quantity = "";
                string UnitPrice = "";
                DataSet ds = po.getPODetails(poID);

                var query = RR_Table.AsEnumerable()
                            .Where(p => p.Field<string>("PONo") == comboBox3.Text)
                            ;

                if (query.Any())
                {

                    if (ds.Tables.Count > 0)
                    {
                        PO_Table = ds.Tables[0];

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            comboBox5.Items.Clear();
                            comboBox2.Items.Clear();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                foreach (DataRow row1 in query.CopyToDataTable().Rows)
                                {
                                    comboBox2.Text = row["ItemName"].ToString();
                                    comboBox5.Text = row["SupplierName"].ToString();
                                    textBox1.Text = (int.Parse(row["quantity"].ToString()) - int.Parse(row1["Quantity"].ToString())).ToString() + " " + row["unitMeasure"].ToString();
                                    qty = int.Parse(row1["Quantity"].ToString());
                                    supplier = int.Parse(row["supplierID"].ToString());
                                    itemName = int.Parse(row["itemID"].ToString());
                                    itemCode = row["itemCode"].ToString(); 
                                }
                            }
                            // getRR_Table();
                        }
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0)
                    {
                        PO_Table = ds.Tables[0];

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            comboBox5.Items.Clear();
                            comboBox2.Items.Clear();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                comboBox2.Text = row["ItemName"].ToString();
                                comboBox5.Text = row["SupplierName"].ToString();
                                textBox1.Text = row["quantity"].ToString() + " " + row["unitMeasure"].ToString();
                                qty = int.Parse(row["quantity"].ToString());
                                supplier = int.Parse(row["supplierID"].ToString());
                                itemName = int.Parse(row["itemID"].ToString());
                                itemCode = row["itemCode"].ToString();

                            }

                            // getRR_Table();
                        }
                    } 
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text.Trim() != "")
            {
                int itemID = int.Parse((comboBox2.SelectedItem as ComboBoxItem).Value.ToString());

                var query1 = PO_Table.AsEnumerable()
                             .Where(p => p.Field<int>("itemID") == itemID
                             && p.Field<int>("supplierID") == supID)
                             ;

                if (comboBox7.Text.Length == 0)
                {
                    if (query1.Any())
                    {
                        foreach (DataRow row in query1.CopyToDataTable().Rows)
                        {
                            textBox1.Text = row["quantity"].ToString() + " " + row["unitMeasure"].ToString();
                            qty = int.Parse(row["quantity"].ToString());
                        }
                    }
                }
                else
                {
                    int getRemQ = 0;
                    if (query1.Any())
                    {
                        foreach (DataRow row in query1.CopyToDataTable().Rows)
                        {
                            foreach (DataRow row1 in dtTR.Rows)
                            {
                                if (row1["Supplier"].ToString() == comboBox5.Text
                                    && row1["Description"].ToString() == comboBox2.Text)
                                {
                                    getRemQ = int.Parse(row["quantity"].ToString()) - int.Parse(row1["Quantity"].ToString());
                                    textBox1.Text = getRemQ.ToString() + " " + row["unitMeasure"].ToString();
                                    qty = getRemQ;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text.Trim() != "")
            {
                supID = int.Parse((comboBox5.SelectedItem as ComboBoxItem).Value.ToString());
             
                if (PO_Table.Rows.Count > 0)
                {
                    var query1 = PO_Table.AsEnumerable()
                                 .Where(p => p.Field<int>("supplierID") == supID)
                                 ;

                    if (query1.Any())
                    {
                        comboBox2.Items.Clear();
                        foreach (DataRow row in query1.CopyToDataTable().Rows)
                        {
                            ComboBoxItem items = new ComboBoxItem();
                            items.Text = row["Description"].ToString();
                            items.Value = row["itemID"].ToString();
                            comboBox2.Items.Add(items);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBox2.Text) <= qty)
                {
                    if (dataGridView1.Rows.Count < 1)
                    {
                        dtTR.Rows.Clear();
                        dtTR.Columns.Clear();
                        TableReceived();
                        dtTR.Rows.Add(dateTimePicker1.Value.ToString().Substring(0, 10)
                                             , comboBox5.Text
                                             , comboBox2.Text
                                             , textBox2.Text);

                        dataGridView1.DataSource = dtTR;
                    }
                    else
                    {
                        if (comboBox7.Text.Length == 0)
                        {
                            var qry = dtTR.AsEnumerable()
                                     .Where(p => p.Field<string>("Description") == comboBox2.Text
                                     && p.Field<string>("Supplier") == comboBox5.Text)
                                     .Sum(r => r.Field<int>("Quantity"))
                                     ;

                            qry = int.Parse(textBox2.Text) + qry;

                            if (qry <= qty)
                            {
                                dtTR.Rows.Add(dateTimePicker1.Value.ToString().Substring(0, 10)
                                                      , comboBox5.Text
                                                      , comboBox2.Text
                                                      , textBox2.Text);

                                dataGridView1.DataSource = dtTR;
                            }
                            else
                            {
                                MessageBox.Show("Item(s) to receive is not equal to the requested quantity.", "ERROR!");
                                return;
                            }
                        }
                        else
                        {
                            //var qry = dtTR.AsEnumerable()
                            //            .Where(p => p.Field<string>("Description") == comboBox2.Text
                            //            && p.Field<string>("Supplier") == comboBox5.Text)
                            //            .Sum(r => r.Field<int>("Quantity"))
                            //            ;

                            //qry = int.Parse(textBox2.Text) + qry;

                            if (int.Parse(textBox2.Text) <= qty)
                            {
                                dtTR.Rows.Add(dateTimePicker1.Value.ToString().Substring(0, 10)
                                                      , comboBox5.Text
                                                      , comboBox2.Text
                                                      , textBox2.Text);

                                dataGridView1.DataSource = dtTR;
                            }
                            else
                            {
                                MessageBox.Show("Item(s) to receive is not equal to the requested quantity.", "ERROR!");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Item(s) to receive is not equal to the request quantity.", "ERROR!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!.", "ERROR!");
            }
        }

        public void TableReceived()
        {
            dtTR.Columns.Add("DateReceived");
            dtTR.Columns.Add("Supplier");
            dtTR.Columns.Add("Description");
            dtTR.Columns.Add("Quantity", typeof(int));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (frm_type == "Preparation")
                {
                    submitRR_Preparation();
                }
                else
                {
                    int poID = int.Parse((comboBox7.SelectedItem as ComboBoxItem).Value.ToString());

                    DataTable container = new DataTable();
                    container.Columns.Add("POID");
                    //int POID = int.Parse(lblPOID.Text.Split('-')[1].ToString());
                    container.Rows.Add(poID);
                    string response = rr.ApproveRRDetails(container, frm_type);

                    if (response == "SUCCESS")
                    {
                        dtTR.Rows.Clear();
                        comboBox1.Text = "";
                        comboBox1.Items.Clear();
                        getRR_Table();
                        if (frm_type == "Endorse")
                        {
                            MessageBox.Show("RR is successully submitted for Approval!");
                        }
                        else
                        {
                            MessageBox.Show("RR has been successfully Approved!");

                        }

                        comboBox5.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(response);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR!");
            }
        }

        public void submitRR_Preparation()
        {
            try
            {
                DataTable container = new DataTable();
                container.Columns.Add("PONo");
                container.Columns.Add("Prepared");
                container.Columns.Add("Noted");
                container.Columns.Add("Confirmed");
                container.Columns.Add("User");
                container.Columns.Add("Position");
                container.Columns.Add("Branch");
                container.Columns.Add("Department");
                container.Columns.Add("Received");

                DataTable container2 = new DataTable();
                container2.Columns.Add("itemID");
                container2.Columns.Add("itemCode");
                container2.Columns.Add("Quantity");
                container2.Columns.Add("supplierID");

                if (string.IsNullOrEmpty(comboBox6.Text))
                    throw new Exception("Receive By is null or empty");

                int receivedID = int.Parse((comboBox6.SelectedItem as ComboBoxItem).Value.ToString());

                int notedID = 0;//int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString()); //noted by
                int approverID = 0;// int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString()); //approver

                container.Rows.Add(comboBox3.Text
                                    , Program.loginfrm.userid //"RequestorID"
                                    , notedID
                                    , approverID
                                    , Program.loginfrm.userid
                                    , Program.loginfrm.posID
                                    , Program.loginfrm.branchID
                                    , Program.loginfrm.deptID
                                    , receivedID);


                if (dtTR.Rows.Count > 0)
                {
                    foreach (DataRow row in dtTR.Rows)
                    {
                        var qry = PO_Table.AsEnumerable()
                                .Where(p => p.Field<string>("Description") == row["Description"].ToString()
                                && p.Field<string>("SupplierName") == row["Supplier"].ToString())
                                ;

                        if (qry.Any())
                        {

                            foreach (DataRow row1 in qry.CopyToDataTable().Rows)
                            {
                                container2.Rows.Add(row1["itemID"].ToString()
                                        , row1["itemCode"].ToString()
                                        , row["Quantity"].ToString()
                                        , row1["supplierID"].ToString());
                            }
                        }
                        else
                        {
                            container2.Rows.Add(itemName.ToString()
                                    , itemCode
                                    , row["Quantity"].ToString()
                                    , supplier.ToString());
                        }
                    }
                }

                string data_summary = ToXML.Toxml(container);
                string data_items = ToXML.Toxml(container2);

                bool stat = false;
                if (comboBox7.Text.Length != 0)
                {
                    stat = true;
                }

                string response = rr.SubmitRR(data_summary, data_items, stat, rrid, comboBox3.Text);

                if (stat == false)
                {
                    if (response == "SUCCESS")
                    {
                        dtTR.Rows.Clear();
                        dtTR.Rows.Clear();
                        //getPO_Table();
                        MessageBox.Show("RR has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
                        this.Close();

                        POPrep_frm prep = new POPrep_frm("Preparation");
                        prep.Show();
                    }
                    else
                    {
                        MessageBox.Show(response);
                    }
                }
                else
                {

                    if (response == "SUCCESS")
                    {
                        dtTR.Rows.Clear();
                        dtTR.Rows.Clear();
                        //getPO_Table();
                        MessageBox.Show("RR has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
                        this.Close();

                        POPrep_frm prep = new POPrep_frm("Preparation");
                        prep.Show();
                    }
                    else
                    {
                        MessageBox.Show(response);
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            #region Commented By Ren | 2019-03-30
            //if (comboBox6.Text == "")
            //{
            //    MessageBox.Show("Please select a signatory that will received the item(s). Thank you!", "ERROR!");
            //    return;
            //}
            //else if (comboBox1.Text == "")
            //{
            //    MessageBox.Show("Please select a signatory noted by. Thank you!", "ERROR!");
            //    return;
            //}
            //else if (comboBox4.Text == "")
            //{
            //    MessageBox.Show("Please select a signatory for approver. Thank you!", "ERROR!");
            //    return;
            //}
            //else
            //{
            //    if (comboBox1.Text.Length != 0)
            //    {
            //        int receivedID = int.Parse((comboBox6.SelectedItem as ComboBoxItem).Value.ToString());

            //        int notedID = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString()); //noted by
            //        int approverID = int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString()); //approver

            //        container.Rows.Add(comboBox3.Text
            //                            , Program.loginfrm.userid //"RequestorID"
            //                            , notedID
            //                            , approverID
            //                            , Program.loginfrm.userid
            //                            , Program.loginfrm.posID
            //                            , Program.loginfrm.branchID
            //                            , Program.loginfrm.deptID
            //                            , receivedID);


            //        if (dtTR.Rows.Count > 0)
            //        {
            //            foreach (DataRow row in dtTR.Rows)
            //            {
            //                var qry = PO_Table.AsEnumerable()
            //                        .Where(p => p.Field<string>("Description") == row["Description"].ToString()
            //                        && p.Field<string>("SupplierName") == row["Supplier"].ToString())
            //                        ;

            //                if (qry.Any())
            //                {

            //                    foreach (DataRow row1 in qry.CopyToDataTable().Rows)
            //                    {
            //                        container2.Rows.Add(row1["itemID"].ToString()
            //                                , row1["itemCode"].ToString()
            //                                , row["Quantity"].ToString()
            //                                , row1["supplierID"].ToString());
            //                    }
            //                }
            //                else
            //                {
            //                    container2.Rows.Add(itemName.ToString()
            //                            , itemCode
            //                            , row["Quantity"].ToString()
            //                            , supplier.ToString());
            //                }
            //            }
            //        }

            //        string data_summary = ToXML.Toxml(container);
            //        string data_items = ToXML.Toxml(container2);

            //        bool stat = false;
            //        if (comboBox7.Text.Length != 0)
            //        {
            //            stat = true;
            //        }

            //        string response = rr.SubmitRR(data_summary, data_items, stat, rrid, comboBox3.Text);

            //        if (stat == false)
            //        {
            //            if (response == "SUCCESS")
            //            {
            //                dtTR.Rows.Clear();
            //                dtTR.Rows.Clear();
            //                //getPO_Table();
            //                MessageBox.Show("RR has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
            //                this.Close();

            //                POPrep_frm prep = new POPrep_frm("Preparation");
            //                prep.Show();
            //            }
            //            else
            //            {
            //                MessageBox.Show(response);
            //            }
            //        }
            //        else
            //        {

            //            if (response == "SUCCESS")
            //            {
            //                dtTR.Rows.Clear();
            //                dtTR.Rows.Clear();
            //                //getPO_Table();
            //                MessageBox.Show("RR has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
            //                this.Close();

            //                POPrep_frm prep = new POPrep_frm("Preparation");
            //                prep.Show();
            //            }
            //            else
            //            {
            //                MessageBox.Show(response);
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = true;
                comboBox1.Enabled = false;
                //comboBox2.Enabled = true;
                comboBox4.Enabled = false;
                comboBox3.Enabled = true;
                //comboBox5.Enabled = true;
                button4.Enabled = true;
                textBox2.Enabled = true;
                comboBox6.Enabled = true;
                comboBox7.Enabled = false;
                comboBox7.Text = "";
                textBox2.Text = "";
                comboBox6.Text = "";
                comboBox1.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
                comboBox2.Text = "";
                comboBox5.Text = "";
                textBox1.Text = ""; 

                dataGridView1.DataSource = null;
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            if (RR_Table.Rows.Count > 0)
            {
                dtTR.Rows.Clear();

                if (frm_type != "Preparation")
                {
                    comboBox1.Text = RR_Table.Rows[0]["PONo"].ToString();
                }

                dtTR.Rows.Clear();
                dtTR.Columns.Clear();
                TableReceived();

                var query = RR_Table.AsEnumerable()
                            .Where(p => p.Field<string>("RRNo") == comboBox7.Text)
                            ;

                if (query.Any())
                {
                    foreach (DataRow row in query.CopyToDataTable().Rows)
                    {
                        comboBox6.Text = row["Receiver"].ToString();
                        comboBox1.Text = row["Noter"].ToString();
                        comboBox4.Text = row["Approver"].ToString();

                        comboBox3.Text = row["PONo"].ToString();
                        //comboBox7.Text = row["RRNo"].ToString();

                        rrid = int.Parse(row["ID"].ToString());

                        dtTR.Rows.Add(row["DateReceived"].ToString().Substring(0, 10).Trim()
                                        , row["SupplierName"].ToString()
                                        , row["Description"].ToString()
                                        , int.Parse(row["Quantity"].ToString()));

                    }

                    dsPrint.Tables.Add(query.CopyToDataTable());

                    dataGridView1.DataSource = dtTR;
                }
            }
        }

        private void InitializeForm(string FormType)
        {
            label5.Visible = FormType == "Preparation" ? true : false;
            comboBox6.Visible = FormType == "Preparation" ? true : false;
            label6.Visible = FormType == "Preparation" ? true : false;
            comboBox1.Visible = FormType == "Preparation" ? true : false;
            label8.Visible = FormType == "Preparation" ? true : false;
            comboBox4.Visible = FormType == "Preparation" ? true : false;
        }
    }
}
