using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usRecurringRow : UserControl
    {
        public event EventHandler RowDeleted;
        public usRecurringRow()
        {
            InitializeComponent();
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {

            // This removes it from the panel
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
            RowDeleted?.Invoke(this, EventArgs.Empty);
            this.Dispose();

        }

        private void lblCategory_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
