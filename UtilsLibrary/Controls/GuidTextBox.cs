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

using Sims2Tools.DBPF.Utils;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    [DefaultEvent(nameof(TextChanged))]
    public partial class GuidTextBox : UserControl
    {
        public static uint NO_VALUE = 0xFFFFFFFE;

        public GuidTextBox()
        {
            this.Name = "GuidTextBox";

            InitializeComponent();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public uint Value
        {
            get
            {
                try
                {
                    return uint.Parse(textBox.Text.Substring(2), NumberStyles.HexNumber);
                }
                catch { }

                return 0;
            }

            set => textBox.Text = (value == NO_VALUE) ? "" : Helper.Hex8PrefixString(value);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort LoWord
        {
            get => (ushort)(Value & 0x0000FFFF);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort HiWord
        {
            get => (ushort)((Value & 0xFFFF0000) >> 16);
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
            else if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (e.KeyChar == '0')
                {
                    e.Handled = false;
                }
            }
            else if (textBox.Text.Equals("0"))
            {
                if (e.KeyChar == 'x' || e.KeyChar == 'X')
                {
                    e.Handled = false;
                }
            }
            else if (textBox.Text.Length < 10)
            {
                if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= 'a' && e.KeyChar <= 'f'))
                {
                    e.Handled = false;
                }
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
