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
    public partial class POPrep_frm : Form
    {
        SignatoryController lSignatoryCtrl;
        PurchaseOrderController po = new PurchaseOrderController();
        CanvassController canvass = new CanvassController();
        UserController user = new UserController();
        DataTable PO_Table = new DataTable();

        DataTable Canvass_Table_S = new DataTable();
        DataTable Canvass_Table_D = new DataTable();

        DataSet dsData = new DataSet();
        DataSet dsDataItems = new DataSet();
        DataSet dsDataSupplier = new DataSet();

        DataSet dsPrint = new DataSet();
        public string RONumber = "";
        int PO_counter = 0;
        int Sup_counter = 0;
        int supplierID = 0;
        int canvassID = 0;
        public string frm_type = "";
        public POPrep_frm(string type)
        {
            frm_type = type;
            InitializeComponent();
        }

        //DataSet ds = new DataSet();
        private void button3_Click(object sender, EventArgs e)
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
            
            if (Sup_counter > 1)
            {
                if (comboBox3.Text == "")
                {
                    MessageBox.Show("Multiple supplier found. Please select a supplier to proceed in printing. Thank you!", "Ops!");
                    return;
                }
                else
                {
                    var query1 = dsPrint.Tables[0].AsEnumerable()
                                 .Where(p => p.Field<string>("SupplierName") == comboBox3.Text)
                                 ;

                    if (query1.Any())
                    {
                        dsP.Tables.Add(query1.CopyToDataTable());

                        foreach (DataRow row in dsP.Tables[0].Rows)
                        {
                            dt.Rows.Add(row["SupplierName"].ToString()
                           , row["quantity"].ToString() + " " + row["unitMeasure"].ToString()
                           , row["Description"].ToString()
                           , row["unitCost"].ToString()
                           , decimal.Parse(row["unitCost"].ToString()) * decimal.Parse(row["quantity"].ToString())
                           , comboBox5.Text
                           , textBox3.Text
                           , row["BusinessAddress"].ToString()
                           , row["Requestor"].ToString() //PreparedBy
                           , row["Aprrover"].ToString() //ApprovedBy
                           , row["TIN"].ToString());

                            dReq = DateTime.Parse(row["DateRequested"].ToString().Substring(0, 10)).ToString("F").Replace("00:00:00","").Trim();
                        }

                        dsToPrint.Tables.Add(dt);

                        UI_Report.Report_RO rro = new UI_Report.Report_RO("PURCHASE ORDER - " + dReq, dsToPrint, 5);
                        rro.ShowDialog(); 
                    }
                    else
                    {
                        MessageBox.Show("No supplier to print. Please try again. Thank you!", "Error!!");
                        return;
                    }
                }
            }
            else
            {
                foreach (DataRow row in dsPrint.Tables[0].Rows)
                {
                    dt.Rows.Add(row["SupplierName"].ToString()
                   , row["quantity"].ToString() + " " + row["unitMeasure"].ToString()
                   , row["Description"].ToString()
                   , row["unitCost"].ToString()
                   , decimal.Parse(row["unitCost"].ToString()) * decimal.Parse(row["quantity"].ToString())
                   , comboBox5.Text
                   , textBox3.Text
                   , row["BusinessAddress"].ToString()
                   , row["Requestor"].ToString() //PreparedBy
                   , row["Aprrover"].ToString() //ApprovedBy
                   , row["TIN"].ToString());

                    dReq = DateTime.Parse(row["DateRequested"].ToString().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim();
                }

                dsToPrint.Tables.Add(dt);

                UI_Report.Report_RO rro = new UI_Report.Report_RO("PURCHASE ORDER - " + dReq, dsToPrint, 5);
                rro.ShowDialog(); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frm_type.Trim() == "Preparation")
            {
                submitPO_preparation();
            }
            else
            {
                DataTable container = new DataTable();
                container.Columns.Add("POID");
                int POID = int.Parse(lblPOID.Text.Split('-')[1].ToString());
                container.Rows.Add(POID);
                string response = po.ApprovePODetails(container, frm_type);

                if (response == "SUCCESS")
                {
                    dataGridView1.Rows.Clear();
                    comboBox1.Text = "";
                    comboBox1.Items.Clear();
                    getPO_Table();
                    if (frm_type == "Endorse")
                    {
                        MessageBox.Show("PO is successully submitted for Approval!");
                    }
                    else
                    {
                        MessageBox.Show("PO has been successfully Approved!");

                    }

                    comboBox5.Enabled = true;
                }
                else
                {
                    MessageBox.Show(response);
                }
            }


        }

        public void submitPO_preparation()
        {
            DataTable container = new DataTable();
            container.Columns.Add("canvassNumber");
            container.Columns.Add("userID");
            container.Columns.Add("positionID");
            container.Columns.Add("branchID");
            container.Columns.Add("departmentID");
            container.Columns.Add("status");
            container.Columns.Add("urgent");
            container.Columns.Add("deliveryVia");
            container.Columns.Add("paymentCondition");
            container.Columns.Add("DeliveryTo");
            container.Columns.Add("requestorID");
            container.Columns.Add("DateRequested");
            container.Columns.Add("endorserID");
            container.Columns.Add("dateEndorsed");
            container.Columns.Add("recommendersID");
            container.Columns.Add("dateRecommended");
            container.Columns.Add("approversID");
            container.Columns.Add("dateApproved");
            container.Columns.Add("remarks");

            DataTable container2 = new DataTable();
            container2.Columns.Add("canvassNumber");
            container2.Columns.Add("supplierID");
            container2.Columns.Add("itemID");
            container2.Columns.Add("quantity");
            container2.Columns.Add("unitMeasure");
            container2.Columns.Add("terms");
            container2.Columns.Add("DeliveryDate");
            container2.Columns.Add("Warranty");
            container2.Columns.Add("unitCost");
            container2.Columns.Add("approved");

            if (comboBox1.Text.Length != 0)
            {
                int canvassID = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString());
                string canvassNo = "";
                //Commented By Ren | 2019-03-29
                //int endorserID = int.Parse((comboBox2.SelectedItem as ComboBoxItem).Value.ToString()); //noted by
                //int approverID = int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString()); //approver
                //End
                if (Canvass_Table_S.Rows.Count > 0)
                {
                    foreach (DataRow row in dsDataSupplier.Tables[0].Rows)
                    {
                        canvassNo = row["CanvassNumber"].ToString();
                        container.Rows.Add(row["CanvassNumber"].ToString(),
                                            Program.loginfrm.userid
                                            , Program.loginfrm.posID
                                            , Program.loginfrm.branchID
                                            , Program.loginfrm.deptID
                                            , ""
                                            , row["Urgent"].ToString()
                                            , "DeliveryVia"
                                            , "PaymentCondition"
                                            , "DeliveryTo"
                                            , Program.loginfrm.userid //"RequestorID"
                                            , DateTime.Now //"DateRequested"
                                            , 0
                                            , DateTime.Now.ToString()
                                            , ""
                                            , ""
                                            , 0
                                            , DateTime.Now
                                            , "");
                    }
                }

                //DataSet ds = canvass.getCanvassDetails(canvassID);

                if (dsDataItems.Tables.Count > 0)
                {
                    //Canvass_Table_D = ds.Tables[0];

                    if (dsDataItems.Tables[0].Rows.Count > 0)
                    {
                        decimal totalCost = 0m;
                        foreach (DataRow row in dsDataItems.Tables[0].Rows)
                        {
                            container2.Rows.Add(canvassNo
                            , row["SupplierID"].ToString()
                            , row["ItemID"].ToString()
                            , row["Qty"].ToString()
                            , row["Unit"].ToString()
                            , row["Terms"].ToString()
                            , row["DeliveryDate"].ToString()
                            , row["Warranty"].ToString()
                            , row["UnitCost"].ToString()
                            , row["Approved"].ToString());
                        }

                    }
                }

                string data_summary = ToXML.Toxml(container);
                string data_items = ToXML.Toxml(container2);

                string response = po.SubmitPO(data_summary, data_items);
                if (response == "SUCCESS")
                {
                    dataGridView1.Rows.Clear();
                    comboBox1.Text = "";
                    comboBox1.Items.Clear();
                    //getPO_Table();
                    MessageBox.Show("PO has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
                    this.Close();

                    POPrep_frm prep = new POPrep_frm("Preparation");
                    prep.Show();
                }
                else
                {
                    MessageBox.Show(response);
                }
            }

            #region Commented By Ren | 2019-03-29 Due to dynamic signatory
            //if (comboBox2.Text == "")
            //{
            //    MessageBox.Show("Please select a signatory for noted by. Thank you!", "ERROR!");
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
            //        int canvassID = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString());
            //        string canvassNo = "";

            //        int endorserID = int.Parse((comboBox2.SelectedItem as ComboBoxItem).Value.ToString()); //noted by
            //        int approverID = int.Parse((comboBox4.SelectedItem as ComboBoxItem).Value.ToString()); //approver
            //        if (Canvass_Table_S.Rows.Count > 0)
            //        {
            //            foreach (DataRow row in dsDataSupplier.Tables[0].Rows)
            //            {
            //                canvassNo = row["CanvassNumber"].ToString();
            //                container.Rows.Add(row["CanvassNumber"].ToString(),
            //                                    Program.loginfrm.userid
            //                                    , Program.loginfrm.posID
            //                                    , Program.loginfrm.branchID
            //                                    , Program.loginfrm.deptID
            //                                    , ""
            //                                    , row["Urgent"].ToString()
            //                                    , "DeliveryVia"
            //                                    , "PaymentCondition"
            //                                    , "DeliveryTo"
            //                                    , Program.loginfrm.userid //"RequestorID"
            //                                    , DateTime.Now //"DateRequested"
            //                                    , endorserID.ToString()
            //                                    , DateTime.Now.ToString()
            //                                    , ""
            //                                    , ""
            //                                    , approverID.ToString()
            //                                    , DateTime.Now
            //                                    , "");
            //            }
            //        }

            //        //DataSet ds = canvass.getCanvassDetails(canvassID);

            //        if (dsDataItems.Tables.Count > 0)
            //        {
            //            //Canvass_Table_D = ds.Tables[0];

            //            if (dsDataItems.Tables[0].Rows.Count > 0)
            //            {
            //                decimal totalCost = 0m;
            //                foreach (DataRow row in dsDataItems.Tables[0].Rows)
            //                {
            //                    container2.Rows.Add(canvassNo
            //                    , row["SupplierID"].ToString()
            //                    , row["ItemID"].ToString()
            //                    , row["Qty"].ToString()
            //                    , row["Unit"].ToString()
            //                    , row["Terms"].ToString()
            //                    , row["DeliveryDate"].ToString()
            //                    , row["Warranty"].ToString()
            //                    , row["UnitCost"].ToString()
            //                    , row["Approved"].ToString());
            //                }

            //            }
            //        }

            //        string data_summary = ToXML.Toxml(container);
            //        string data_items = ToXML.Toxml(container2);

            //        string response = po.SubmitPO(data_summary, data_items);
            //        if (response == "SUCCESS")
            //        {
            //            dataGridView1.Rows.Clear();
            //            comboBox1.Text = "";
            //            comboBox1.Items.Clear();
            //            //getPO_Table();
            //            MessageBox.Show("PO has been successfully submitted and subject for noted! Thank you!", "CONFIRM!");
            //            this.Close();

            //            POPrep_frm prep = new POPrep_frm("Preparation");
            //            prep.Show();
            //        }
            //        else
            //        {
            //            MessageBox.Show(response);
            //        }
            //    }               
            //}
            #endregion
        }

        private void POPrep_frm_Load(object sender, EventArgs e)
        {
            try
            {
                label7.Text = "Request Summary: " + 0 + " Request";
                fill_userID();
                getPO_Table();
                //textBox3.Text = Program.loginfrm.fullname;
                getCanvassApproved();

                if (comboBox5.Text.Length == 0)
                {
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    textBox2.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    button1.Enabled = false;
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
                        comboBox2.Items.Add(items);
                        comboBox4.Items.Add(items);

                    }
                }
            }
        }
        public void getCanvassApproved()
        {
            DataTable dt = canvass.getCanvasApproved();
            Canvass_Table_S = dt;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ComboBoxItem items = new ComboBoxItem();
                    items.Text = row["CanvassNumber"].ToString();
                    items.Value = row["ID"].ToString();
                    comboBox1.Items.Add(items);
                }
            }
        }

        public void getPO_Table()
        {
            if (frm_type.Trim() != "")
            {
                DataSet ds = new DataSet();
                
                if (frm_type.Trim() == "Preparation")
                {
                    label16.Text = "Purchase Order Preparation";
                    button1.Text = "Submit";
                    button2.Enabled = true;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox4.Enabled = true;
                    ds = po.getPOData(int.Parse(Program.loginfrm.userid), "PENDING");
                }
                else if (frm_type == "Endorse")
                {
                    lSignatoryCtrl = new SignatoryController();
                    lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Noted, TransactionType.PurchaseOrder);
                    label16.Text = "Purchase Order For Noted";
                    this.Text = "Purchase Order For Noted";
                    button1.Text = "Noted";
                    button2.Enabled = false;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = false;
                    comboBox4.Enabled = false;
                    ds = po.getPOData(int.Parse(Program.loginfrm.userid), "PO-NOTED");

                }
                else if (frm_type == "Approved")
                {
                    lSignatoryCtrl = new SignatoryController();
                    lSignatoryCtrl.GetSignatoryByUserId(int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid), SignatoryType.Approver, TransactionType.PurchaseOrder);
                    label16.Text = "Purchase Order Approval";
                    button1.Text = "Approved";
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox4.Enabled = false;
                    button2.Enabled = false;
                    ds = po.getPOData(int.Parse(Program.loginfrm.userid), "PO-APPROVED");
                }
                IntializeForm(frm_type);

                if (ds.Tables.Count > 0)
                {
                    PO_Table = ds.Tables[0];
                    if (PO_Table.Rows.Count > 0)
                    {
                        //comboBox1.Enabled = false;
                        //comboBox2.Enabled = false;
                        //comboBox4.Enabled = false;
                        //button1.Enabled = false;
                        label7.Text = "Request Summary: " + PO_Table.Rows.Count + " Request";
                        PO_counter = PO_Table.Rows.Count;
                        //DataTable dt = po.POCount(PO_Table, PO_Table.Rows.Count - 1);
                        //retrieve_PO(PO_Table);

                        comboBox5.Items.Clear();
                        comboBox5.Text = string.Empty;

                        foreach (DataRow row in PO_Table.Rows)
                        {
                            ComboBoxItem items = new ComboBoxItem();
                            items.Text = row["PONumber"].ToString();
                            items.Value = row["ID"].ToString();
                            comboBox5.Items.Add(items);
                            //textBox3.Text = row["RONumber"].ToString().Trim();
                        }

                    }
                    else
                    {
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        comboBox4.Enabled = true;
                        button1.Enabled = true;
                        label7.Text = "Request Summary: " + 0 + " Request";

                    }
                }
                else
                {
                    label7.Text = "Request Summary: " + 0 + " Request";

                }
            }
        }


        public void retrieve_PO(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataSet ds = po.getPODetails(int.Parse(dt.Rows[0]["POID"].ToString()));
                dsPrint = ds;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        textBox3.Text = dt.Rows[0]["RONumber"].ToString();
                        comboBox5.Text = dt.Rows[0]["PONumber"].ToString();

                        lblPOID.Text = "POID-" + dt.Rows[0]["POID"].ToString();
                        if (frm_type != "Preparation")
                        {
                            comboBox1.Text = dt.Rows[0]["CanvassNumber"].ToString();
                        }

                        decimal unitCost_Total = 0m;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dataGridView1.Rows.Add(false,
                                                   row["SupplierName"].ToString(),
                                                   row["ItemName"].ToString(),
                                                   row["DeliveryDate"].ToString(),
                                                   row["quantity"].ToString() + " " + row["unitMeasure"].ToString(),
                                                   row["UnitCost"].ToString());
                            unitCost_Total += decimal.Parse(row["UnitCost"].ToString()) * decimal.Parse(row["quantity"].ToString());

                            comboBox2.Text = row["Requestor"].ToString();
                            comboBox4.Text = row["Aprrover"].ToString();


                            //ComboBoxItem items = new ComboBoxItem();
                            //items.Text = row["SupplierName"].ToString();
                            //items.Value = row["supplierID"].ToString();
                            //comboBox3.Items.Add(items);
                        }

                        var query1 = ds.Tables[0].AsEnumerable()
                                    .GroupBy(r => new { BatchNo = r["supplierID"] }).Count()
                                    ;
                        
                        Sup_counter = query1;

                        textBox2.Text = unitCost_Total.ToString("N2");

                    }

                    if (Sup_counter > 1)
                    {
                        label2.Show();
                        comboBox3.Show();
                    }
                    else
                    {
                        label2.Hide();
                        comboBox3.Hide();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (PO_Table.Rows.Count > 0)
            {
                PO_counter--;

                if (PO_counter == 0)
                {
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter < 0)
                {
                    PO_counter++;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter == PO_Table.Rows.Count)
                {
                    PO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter > PO_Table.Rows.Count)
                {
                    PO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else
                {
                    //RO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                    //MessageBox.Show("No more data to show!");
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (PO_Table.Rows.Count > 0)
            {
                PO_counter++;

                if (PO_counter == 0)
                {
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter == PO_Table.Rows.Count)
                {
                    PO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter > PO_Table.Rows.Count - 1)
                {
                    PO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else if (PO_counter < 0)
                {
                    PO_counter++;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
                else
                {
                    // RO_counter--;
                    DataTable dt = po.POCount(PO_Table, PO_counter);
                    retrieve_PO(dt);
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() != "")
            {
                canvassID = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Value.ToString());

                string supplier = "";
                string itemName = "";
                string leadTime = "";
                string Quantity = "";
                string UnitPrice = "";
                dsData = canvass.getCanvassDetails(canvassID);

                if (Canvass_Table_S.Rows.Count > 0)
                {
                    var query = Canvass_Table_S.AsEnumerable()
                                .Where(p => p.Field<int>("ID") == canvassID)
                                ;

                    if (query.Any())
                    {
                        dsDataSupplier.Tables.Add(query.CopyToDataTable());

                        foreach (DataRow row in query.CopyToDataTable().Rows)
                        {
                            RONumber = row["RONumber"].ToString().Trim();
                            textBox3.Text = RONumber;
                        }
                    }
                }
                
                comboBox3.Items.Clear();
                comboBox3.Text = string.Empty;

                if (dsData.Tables.Count > 0)
                {
                    foreach (DataRow row in dsData.Tables[0].Rows)
                    {
                        ComboBoxItem items = new ComboBoxItem();
                        items.Text = row["SupplierName"].ToString();
                        items.Value = row["supplierID"].ToString();
                        comboBox3.Items.Add(items);
                    }
                }                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = false;
            comboBox3.Enabled = true;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            button1.Enabled = true;
            comboBox1.Focus();
            dataGridView1.Rows.Clear();
            comboBox5.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            textBox3.Text = "";
            textBox2.Enabled = true;
            lblPOID.Text = "";
            comboBox3.Show();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text.Length != 0)
            {
                if (dsData.Tables.Count > 0)
                {
                    supplierID = int.Parse((comboBox3.SelectedItem as ComboBoxItem).Value.ToString());

                    var query = dsData.Tables[0].AsEnumerable()
                                .Where(p => p.Field<int>("supplierID") == supplierID)
                                ;

                    if (query.Any())
                    {
                        if (query.CopyToDataTable().Rows.Count > 0)
                        {
                            dsDataItems.Clear();
                            dsDataItems.Tables.Add(query.CopyToDataTable());
                            decimal totalCost = 0m;
                            foreach (DataRow row in query.CopyToDataTable().Rows)
                            {
                                dataGridView1.Rows.Add(false
                                    , row["SupplierName"].ToString().Trim()
                                    , row["Itemname"].ToString()
                                    , row["DeliveryDate"].ToString()
                                    , row["Qty"].ToString(),
                                    row["UnitCost"].ToString());

                                totalCost += decimal.Parse(row["UnitCost"].ToString()) * decimal.Parse(row["Qty"].ToString());
                            }
                            textBox2.Text = totalCost.ToString("N2");
                        } 
                    }
                }
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            var query = PO_Table.AsEnumerable()
                        .Where(p => p.Field<string>("PONumber") == comboBox5.Text).Select(s => s)
                        ;

            if (query.Any())
            {
                int _Id = int.Parse(string.IsNullOrEmpty(query.CopyToDataTable().Rows[0]["ID"].ToString()) ? "0" : query.CopyToDataTable().Rows[0]["ID"].ToString());
                DataSet ds = po.getPODetails(_Id);
                dsPrint = ds;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        textBox3.Text = query.CopyToDataTable().Rows[0]["RONumber"].ToString();
                        comboBox5.Text = query.CopyToDataTable().Rows[0]["PONumber"].ToString();

                        lblPOID.Text = "POID-" + query.CopyToDataTable().Rows[0]["ID"].ToString();
                        //if (frm_type != "Preparation")
                       // {
                            comboBox1.Text = query.CopyToDataTable().Rows[0]["CanvassNumber"].ToString();
                       // }

                        decimal unitCost_Total = 0m;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dataGridView1.Rows.Add(false,
                                                   row["SupplierName"].ToString(),
                                                   row["ItemName"].ToString(),
                                                   row["DeliveryDate"].ToString(),
                                                   row["quantity"].ToString() + " " + row["unitMeasure"].ToString(),
                                                   row["UnitCost"].ToString());
                            unitCost_Total += decimal.Parse(row["UnitCost"].ToString()) * decimal.Parse(row["quantity"].ToString());

                            comboBox2.Text = row["Requestor"].ToString();
                            comboBox4.Text = row["Aprrover"].ToString();
                        }

                        //var query1 = ds.Tables[0].AsEnumerable()
                        //            .GroupBy(r => new { BatchNo = r["supplierID"] }).Count()
                        //            ;

                        //Sup_counter = query1;

                        textBox2.Text = unitCost_Total.ToString("N2");

                    }

                    //if (Sup_counter > 1)
                    //{
                    //    label2.Show();
                    //    comboBox3.Show();
                    //}
                    //else
                    //{
                    //    label2.Hide();
                    //    comboBox3.Hide();
                    //}
                }
            }
            
        }

        private void IntializeForm(string FormType)
        {
            label5.Visible = FormType == "Preparation" ? true : false;
            comboBox2.Visible = FormType == "Preparation" ? true : false;
            label8.Visible = FormType == "Preparation" ? true : false;
            comboBox4.Visible = FormType == "Preparation" ? true : false;
            comboBox2.Enabled = FormType == "Preparation" ? false : true;
            comboBox4.Enabled = FormType == "Preparation" ? false : true;
        }
    }
}
