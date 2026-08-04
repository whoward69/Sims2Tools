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
    public partial class DoubleTextBox : UserControl
    {
        public static double NO_VALUE = double.MaxValue;

        private string format = "N1";
        private double minimum = 0.0f;
        private double maximum = 1000.0f;

        public DoubleTextBox()
        {
            this.Name = "DoubleTextBox";

            InitializeComponent();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Format
        {
            get => format;
            set => format = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double Minimum
        {
            get => minimum;
            set => minimum = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double Maximum
        {
            get => maximum;
            set => maximum = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double Value
        {
            get => double.TryParse(textBox.Text, out double value) ? Math.Max(minimum, Math.Min(maximum, value)) : 0.0f;
            set => textBox.Text = (value == NO_VALUE) ? "" : value.ToString(format);
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
            else if (e.KeyChar == '.')
            {
                e.Handled = textBox.Text.Contains(".");
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = !string.IsNullOrEmpty(textBox.Text);
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
