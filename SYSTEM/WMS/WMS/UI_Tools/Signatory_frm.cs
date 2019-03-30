using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMS.Controller;

namespace WMS
{
    public partial class Signatory_frm : Form
    {
        #region Local Variable
        UserController lUserCtrl;
        SignatoryController lSignatoryCtrl;
        int lSignatoryId;
        #endregion
        public Signatory_frm()
        {
            InitializeComponent();
        }
        private void LoadSignatory_frm(object sender, EventArgs e)
        {
            #region Users
            lUserCtrl = new UserController();
            var _user = lUserCtrl.getAllUser()
                .Tables[0]
                .AsEnumerable()
                .ToDictionary<DataRow, int, string>(
                    r => int.Parse(r["UserId"].ToString()),
                    r => r["firstName"].ToString() + " " + r["lastName"].ToString()
                );
            _user.Add(0, "Select Name");
            cbName.DataSource = new BindingSource(_user, null);
            cbName.DisplayMember = "Value";
            cbName.ValueMember = "Key";
            Reset();
            #endregion
        }
        private void btnSignatoryNew(object sender, EventArgs e)
        {
            Reset();
        }
        private void btnSignatorySave(object sender, EventArgs e)
        {
            try
            {
                lSignatoryCtrl = new SignatoryController();
                int _userId = int.Parse(cbName.SelectedValue.ToString());
                string _transType = cbTransType.SelectedItem.ToString();
                string _signaType = cbSignatoryType.SelectedItem.ToString();

                if (_userId.Equals(0))
                    throw new Exception("No selected name");
                else if (_transType.Equals("Select Type"))
                    throw new Exception("No selected transaction type");
                else if (_signaType.Equals("Select Type"))
                    throw new Exception("No selected signatory type");

                if (lSignatoryId.Equals(0))
                    lSignatoryCtrl.InsertSignatory(_userId, _transType, _signaType);
                else if (!lSignatoryId.Equals(0))
                    lSignatoryCtrl.UpdateSignatory(lSignatoryId, _userId, _transType, _signaType);

                Reset();
                MessageBox.Show("Successfully saved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnSignatoryDelete(object sender, EventArgs e)
        {
            if (lSignatoryId.Equals(0))
                throw new Exception("No selected item in grid");

            lSignatoryCtrl.DeleteSignatory(lSignatoryId);
            Reset();
        }
        private void Reset()
        {
            lSignatoryCtrl = new SignatoryController();

            lSignatoryId = 0;
            gvSignatory.DataSource = lSignatoryCtrl.GetSignatory().Tables[0];
            gvSignatory.Columns["SignatoryId"].Visible = false;
            gvSignatory.Columns["UserId"].Visible = false;
            gvSignatory.ClearSelection();
            cbName.SelectedIndex = cbName.FindStringExact("Select Name");
            cbTransType.SelectedIndex = cbTransType.FindStringExact("Select Type");
            cbSignatoryType.SelectedIndex = cbSignatoryType.FindStringExact("Select Type");
        }
        private void btnSignatoryClose(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void selRow(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            int _countR = gvSignatory.Rows.Count;
            for (int i = 0; i < _countR; i++)
            {
                var _row = gvSignatory.Rows[i];
                bool _isSel = _row.Selected;
                if (_isSel)
                {
                    int _userId = int.Parse(string.IsNullOrEmpty(_row.Cells["UserId"].Value.ToString()) ? "0" : _row.Cells["UserId"].Value.ToString());
                    string _transType = _row.Cells["TransactionType"].Value.ToString();
                    string _signatory = _row.Cells["SignatoryType"].Value.ToString();
                    lSignatoryId = int.Parse(string.IsNullOrEmpty(_row.Cells["SignatoryId"].Value.ToString()) ? "0" : _row.Cells["SignatoryId"].Value.ToString());
                    cbName.SelectedValue = _userId;
                    cbTransType.SelectedIndex = cbTransType.FindStringExact(_transType);
                    cbSignatoryType.SelectedIndex = cbSignatoryType.FindStringExact(_signatory);
                }
            }
        }
    }
}
