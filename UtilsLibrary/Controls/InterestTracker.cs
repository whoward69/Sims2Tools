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

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    public enum InterestTrackerStyle : uint
    {
        BarAndBox,
        BarOnly,
        BoxOnly
    }

    [DefaultEvent(nameof(Changed))]
    public partial class InterestTracker : UserControl
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ushort NO_VALUE = ushort.MaxValue;

        public InterestTracker()
        {
            InitializeComponent();
        }

        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                if (Name.StartsWith("track"))
                {
                    string lblName = $"lbl{Name.Substring(5)}";

                    if (Parent.Controls.ContainsKey(lblName))
                    {
                        Parent.Controls[lblName].Enabled = value;
                    }
                }

                base.Enabled = value;
            }
        }

        private InterestTrackerStyle style = InterestTrackerStyle.BarAndBox;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InterestTrackerStyle Style
        {
            get => style;
            set
            {
                if (style != value)
                {
                    style = value;

                    switch (style)
                    {
                        case InterestTrackerStyle.BarAndBox:
                            trackBar.Visible = true;
                            trackBar.Size = new Size(100, trackBar.Size.Height);

                            textBox.Visible = true;
                            textBox.Location = new Point(103, 0);
                            break;
                        case InterestTrackerStyle.BarOnly:
                            trackBar.Visible = true;
                            trackBar.Size = new Size(138, trackBar.Size.Height);

                            textBox.Visible = false;
                            break;
                        case InterestTrackerStyle.BoxOnly:
                            trackBar.Visible = false;

                            textBox.Visible = true;
                            textBox.Location = new Point(0, 0);
                            break;
                    }
                }
            }
        }
        private SdscIndex sdscIndex = SdscIndex.NONE;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public SdscIndex SdscIndex
        {
            get => sdscIndex;
            set => sdscIndex = value;
        }

        private uint tokenGuid = DBPFData.GUID_NULL.AsUInt();

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Token
        {
            get => Helper.Hex8PrefixString(tokenGuid);
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public uint TokenGuid
        {
            get => tokenGuid;
            set => tokenGuid = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ushort Value
        {
            get => (ushort)trackBar.Value;
            set
            {
                if (value != NO_VALUE)
                {
                    trackBar.Value = value;
                    textBox.Value = (value / 100.0f);
                }
                else
                {
                    trackBar.Value = 0;
                    textBox.Value = DoubleTextBox.NO_VALUE;
                }
            }
        }

        [Browsable(true)]
        public event EventHandler Changed
        {
            add => trackBar.Changed += value;
            remove => trackBar.Changed -= value;
        }

        private bool internalChange = false;

        private void OnTrackBarChanged(object sender, System.EventArgs e)
        {
            if (internalChange) return;

            internalChange = true;
            textBox.Value = trackBar.Value / 100;
            internalChange = false;
        }

        private void OnTextBoxChanged(object sender, System.EventArgs e)
        {
            if (internalChange) return;

            internalChange = true;
            trackBar.Value = (int)(textBox.Value * 100);
            internalChange = false;
        }
    }
}
