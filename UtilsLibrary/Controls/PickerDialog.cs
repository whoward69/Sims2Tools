/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * See https://github.com/ukushu/TextProgressBar
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    public partial class PickerDialog : Form
    {
        public virtual object SelectedItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void AddItem(object item)
        {
            throw new NotImplementedException();
        }

        public PickerDialog()
        {
            InitializeComponent();
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
