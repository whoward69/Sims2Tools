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
using Sims2Tools.DBPF.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    [DefaultEvent(nameof(Click))]
    public partial class TourButton : UserControl
    {
        public TourButton()
        {
            InitializeComponent();

            iconTourGuide.Parent = button;
            iconVacation.Parent = button;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button InnerButton
        {
            get => button;
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
        public Image Image
        {
            get => button.BackgroundImage;
            set => button.BackgroundImage = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image VacationImage
        {
            get => iconVacation.BackgroundImage;
            set => iconVacation.BackgroundImage = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Selected
        {
            get => button.BackColor == Color.CadetBlue;
            set => button.BackColor = value ? Color.CadetBlue : Color.LightGray;
        }

        [Browsable(true)]
        public new event EventHandler Click
        {
            add => button.Click += value;
            remove => button.Click -= value;
        }

        private void OnTourGuideIconClicked(object sender, EventArgs e)
        {
            button.PerformClick();
        }

        private void OnVacationIconClicked(object sender, EventArgs e)
        {
            button.PerformClick();
        }
    }
}
