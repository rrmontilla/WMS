using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uploading.UI;

namespace WMS
{
    public partial class WMS_Main : Form
    {
        public WMS_Main()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void accessRightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccessRight_frm access = new AccessRight_frm();
            this.IsMdiContainer = true;
            access.MdiParent = this;
            access.Show();

        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_frm user = new User_frm();
            this.IsMdiContainer = true;
            user.MdiParent = this;
            user.Show();
        }

        private void departmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Department_frm dept = new Department_frm();
            this.IsMdiContainer = true;
            dept.MdiParent = this;
            dept.Show();
        }

        private void positionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position_frm pos = new Position_frm();
            this.IsMdiContainer = true;
            pos.MdiParent = this;
            pos.Show();
        }
        private void branchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Branch_frm branch = new Branch_frm();
            this.IsMdiContainer = true;
            branch.MdiParent = this;
            branch.Show();
        }

        private void jonathanToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CanvassPrep_frm prep = new CanvassPrep_frm("Preparation");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CanvassPrep_frm prep = new CanvassPrep_frm("Endorse");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
            
            
            //CanvassEnd_frm endorse = new CanvassEnd_frm();
            //this.IsMdiContainer = true;
            //endorse.MdiParent = this;
            //endorse.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //CanvassRec_frm rec = new CanvassRec_frm();
            //this.IsMdiContainer = true;
            //rec.MdiParent = this;
            //rec.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CanvassPrep_frm prep = new CanvassPrep_frm("Approved");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
            //CanvassAppr_frm appr = new CanvassAppr_frm();
            //this.IsMdiContainer = true;
            //appr.MdiParent = this;
            //appr.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            POPrep_frm prep = new POPrep_frm("Preparation");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            POPrep_frm prep = new POPrep_frm("Endorse");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
            //POEndorse_frm endorse = new POEndorse_frm();
            //this.IsMdiContainer = true;
            //endorse.MdiParent = this;
            //endorse.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            PORecomend_frm rec = new PORecomend_frm();
            this.IsMdiContainer = true;
            rec.MdiParent = this;
            rec.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            POPrep_frm prep = new POPrep_frm("Approved");
            this.IsMdiContainer = true;
            prep.MdiParent = this;
            prep.Show();
        }

        private void preparationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_RO.ROPrep_frm ro = new UI_RO.ROPrep_frm("");
            this.IsMdiContainer = true;
            ro.MdiParent = this;
            ro.Show();
        }

        private void endorseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_RO.ROEndorse_frm sig = new UI_RO.ROEndorse_frm();
            this.IsMdiContainer = true;
            sig.MdiParent = this;
            sig.Show();
        }

        private void recommendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_RO.RORecommend_frm sig = new UI_RO.RORecommend_frm();
            this.IsMdiContainer = true;
            sig.MdiParent = this;
            sig.Show();
        }

        private void approvedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_RO.ROApproved_frm sig = new UI_RO.ROApproved_frm();
            this.IsMdiContainer = true;
            sig.MdiParent = this;
            sig.Show();
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            UI_Report.Report_frm rep = new UI_Report.Report_frm("Request");
            this.IsMdiContainer = true;
            rep.MdiParent = this;
            rep.Show();
        }

        private void toolStripMenuItem30_Click(object sender, EventArgs e)
        {
            UI_Report.Report_frm rep = new UI_Report.Report_frm("Canvass");
            this.IsMdiContainer = true;
            rep.MdiParent = this;
            rep.Show();
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            UI_Report.Report_frm rep = new UI_Report.Report_frm("Purchase");
            this.IsMdiContainer = true;
            rep.MdiParent = this;
            rep.Show();
        }

        private void WMS_Main_Load(object sender, EventArgs e)
        {
            displayname();
        }

        public void displayname()
        {
            this.Text = "Warehouse Management System" + " (" + Program.loginfrm.uname + ") ";
        }

        private void purchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            UI_RR.ReceivingReport_frm frm = new UI_RR.ReceivingReport_frm("Preparation");
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            UI_Accountability.Accountability_frm frm = new UI_Accountability.Accountability_frm();
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void jobOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addNewConstructionTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Construction frm = new Construction();
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void viewConstructionsTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewConstruction frm = new ViewConstruction();
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void addNewChartsOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            COA_frm coa = new COA_frm();
            this.IsMdiContainer = true;
            coa.MdiParent = this;
            coa.Show();
        }

        private void viewChartsOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.IsMdiContainer = true;
            UI_Tools.View_frm sup = new UI_Tools.View_frm("ACCOUNT");
            sup.MdiParent = this;
            sup.Show();
            Cursor.Current = Cursors.Default;
        }

        private void addNewSuppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Supplier_frm supplier = new Supplier_frm();
            this.IsMdiContainer = true;
            supplier.MdiParent = this;
            supplier.Show();
        }

        private void viewListOfSuppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.IsMdiContainer = true;
            UI_Tools.View_frm sup = new UI_Tools.View_frm("SUPPLIER");
            sup.MdiParent = this;
            sup.Show();
            Cursor.Current = Cursors.Default;
        }

        private void addNewItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            items_frm items = new items_frm();
            this.IsMdiContainer = true;
            items.MdiParent = this;
            items.Show();
        }

        private void viewItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.IsMdiContainer = true;
            UI_Tools.View_frm sup = new UI_Tools.View_frm("ITEM");
            sup.MdiParent = this;
            sup.Show();
            Cursor.Current = Cursors.Default;
        }

        private void preparationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //TO CREATE A PROJECT

            //Cursor.Current = Cursors.WaitCursor;
            //this.IsMdiContainer = true;
            //UI_Project.Project_frm frm = new UI_Project.Project_frm();
            //frm.MdiParent = this;
            //frm.Show();
            //Cursor.Current = Cursors.Default;
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewListOfUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.loginfrm.Show();
            Program.loginfrm.UserName_Focus();
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            UI_MRIS.MRIS_Prep mris = new UI_MRIS.MRIS_Prep("Preparation");
            mris.Show();
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            UI_MRIS.MRIS_Prep mris = new UI_MRIS.MRIS_Prep("Approved");
            mris.Show();
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            UI_MRIS.MRIS_Prep mris = new UI_MRIS.MRIS_Prep("Issued");
            mris.Show();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            UI_RI.RI_frm ri = new UI_RI.RI_frm("RI_Prep");
            ri.Show();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            UI_RI.RI_frm ri = new UI_RI.RI_frm("Noted");
            ri.Show();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            UI_RI.RI_frm ri = new UI_RI.RI_frm("Confirmed");
            ri.Show();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            UI_RR.ReceivingReport_frm frm = new UI_RR.ReceivingReport_frm("Endorse");
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            UI_RR.ReceivingReport_frm frm = new UI_RR.ReceivingReport_frm("Approved");
            this.IsMdiContainer = true;
            frm.MdiParent = this;
            frm.Show();
        }

        private void printROToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_RO.ROPrep_frm ro = new UI_RO.ROPrep_frm("Print");
            this.IsMdiContainer = true;
            ro.MdiParent = this;
            ro.Show();
        }

        private void printCanvassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanvassPrep_frm prep = new CanvassPrep_frm("Print");
           
        }

        private void btnOpen(object sender, EventArgs e)
        {
            var _form = new Signatory_frm();
            this.IsMdiContainer = true;
            _form.MdiParent = this;
            _form.Show();
        }
    }
}
