using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usRecurringRow : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecurringId { get; set; } = 0;
        public event EventHandler RowDeleted;

        public usRecurringRow()
        {
            InitializeComponent();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (this.Parent != null)
                this.Parent.Controls.Remove(this);

            RowDeleted?.Invoke(this, EventArgs.Empty);
            this.Dispose();
        }

        private void lblCategory_TextChanged(object sender, EventArgs e) { }
    }
}