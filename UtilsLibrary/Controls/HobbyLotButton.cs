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

using Sims2Tools.DBPF.Neighbourhood.SDSC;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    [DefaultEvent(nameof(Click))]
    public partial class HobbyLotButton : UserControl
    {
        private Mementos memento;

        public HobbyLotButton()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button InnerButton
        {
            get => button;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Mementos Memento
        {
            get => memento;
            set => memento = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image
        {
            get => button.BackgroundImage;
            set => button.BackgroundImage = value;
        }

        private SdscIndex sdscIndex = SdscIndex.NONE;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public SdscIndex SdscIndex
        {
            get => sdscIndex;
            set => sdscIndex = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Selected
        {
            get => button.BackColor == Color.CadetBlue;
            set
            {
                if (value)
                {
                    button.BackColor = Color.CadetBlue;
                    button.BackgroundImage = Properties.Resources.Hobby_MembershipCard_Enabled;
                }
                else
                {
                    button.BackColor = Color.WhiteSmoke;
                    button.BackgroundImage = Properties.Resources.Hobby_MembershipCard_Disabled;
                }
            }
        }

        [Browsable(true)]
        public new event EventHandler Click
        {
            add => button.Click += value;
            remove => button.Click -= value;
        }
    }
}
