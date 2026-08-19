/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    [DefaultEvent(nameof(TextChanged))]
    public partial class UIntTextBox : UserControl
    {
        public static uint NO_VALUE = uint.MaxValue;

        private uint minimum = 0;
        private uint maximum = 1000;

        public UIntTextBox()
        {
            this.Name = "UIntTextBox";

            InitializeComponent();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public uint Minimum
        {
            get => minimum;
            set => minimum = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public uint Maximum
        {
            get => maximum;
            set
            {
                if (value != maximum)
                {
                    maximum = value;
                    Value = Math.Min(maximum, Value);
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public uint Value
        {
            get => UInt32.TryParse(textBox.Text, out uint value) ? Math.Max(minimum, Math.Min(maximum, value)) : 0;
            set => textBox.Text = (value == NO_VALUE) ? "" : value.ToString();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Editable
        {
            get => textBox.Enabled;
            set => textBox.Enabled = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get => textBox.Text;
            set => throw new NotImplementedException($"Cannot set Text on {this.Name}");
        }

        [Browsable(true)]
        public new event EventHandler TextChanged
        {
            add => textBox.TextChanged += value;
            remove => textBox.TextChanged -= value;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            }
        }
    }
}
