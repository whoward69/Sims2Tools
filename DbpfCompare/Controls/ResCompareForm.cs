/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using DbpfCompare.Diff;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DBPF.XWNT;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DbpfCompare.Controls
{
    public partial class ResCompareForm : Form
    {
        private static readonly Color colourHighlightDiffers = Color.FromName(Properties.Settings.Default.CompareDiffers);

        private static readonly Color colourDiffers = Color.FromName(Properties.Settings.Default.ResDiffers);
        private static readonly Color colourMissing = Color.FromName(Properties.Settings.Default.ResMissing);
        private static readonly Color colourSame = Color.FromName(Properties.Settings.Default.ResSame);

        private static readonly Brush brushDiffers = new SolidBrush(colourDiffers);
        private static readonly Brush brushMissing = new SolidBrush(colourMissing);
        private static readonly Brush brushSame = new SolidBrush(colourSame);

        private readonly ResCompareData dataResCompare = new ResCompareData();

        private readonly DbpfCompareNodeResourceData leftNodeData, rightNodeData;
        private readonly string leftPackagePath, rightPackagePath;

        private DBPFResource leftRes, rightRes;

        public ResCompareForm(DbpfCompareNodeResourceData leftNodeData, string leftPackagePath, DbpfCompareNodeResourceData rightNodeData, string rightPackagePath)
        {
            InitializeComponent();

            this.leftNodeData = leftNodeData;
            this.leftPackagePath = leftPackagePath;
            this.rightNodeData = rightNodeData;
            this.rightPackagePath = rightPackagePath;

            gridResCompare.DataSource = dataResCompare;
            dataResCompare.Clear();

            this.Text = leftNodeData.Key.ToString();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadPopupSettings(DbpfCompareApp.RegistryKey, this);

            for (int i = 0; i < 5; ++i)
            {
                gridResCompare.Columns[i].Width = (int)RegistryTools.GetPopupSetting(DbpfCompareApp.RegistryKey, this, $"Width{i}", gridResCompare.Columns[i].Width);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SavePopupSettings(DbpfCompareApp.RegistryKey, this);

            for (int i = 0; i < 5; ++i)
            {
                RegistryTools.SavePopupSetting(DbpfCompareApp.RegistryKey, this, $"Width{i}", gridResCompare.Columns[i].Width);
            }
        }

        private void OnShow(object sender, EventArgs e)
        {
            comboVariations.Visible = false;

            // Should probably be on a worker thread, but hey, you're not doing anything until this loads anyway!
            using (DBPFFile leftPackage = new DBPFFile(leftPackagePath), rightPackage = new DBPFFile(rightPackagePath))
            {
                if (leftPackage != null && rightPackage != null)
                {
                    if (leftNodeData.TypeID == Bcon.TYPE)
                    {
                        ShowBcon(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Trcn.TYPE)
                    {
                        ShowTrcn(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Bhav.TYPE)
                    {
                        ShowBhav(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Tprp.TYPE)
                    {
                        ShowTprp(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Objd.TYPE)
                    {
                        ShowObjd(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Objf.TYPE)
                    {
                        ShowObjf(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Glob.TYPE)
                    {
                        ShowGlob(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Nref.TYPE)
                    {
                        ShowNref(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Slot.TYPE)
                    {
                        ShowSlot(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Idr.TYPE)
                    {
                        ShowIdr(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Aged.TYPE ||
                             leftNodeData.TypeID == Binx.TYPE ||
                             leftNodeData.TypeID == Coll.TYPE ||
                             leftNodeData.TypeID == Gzps.TYPE ||
                             leftNodeData.TypeID == Mmat.TYPE ||
                             leftNodeData.TypeID == Vers.TYPE ||
                             leftNodeData.TypeID == Sdna.TYPE ||
                             leftNodeData.TypeID == Xfch.TYPE || leftNodeData.TypeID == Xhtn.TYPE || leftNodeData.TypeID == Xmol.TYPE || leftNodeData.TypeID == Xstn.TYPE || leftNodeData.TypeID == Xtol.TYPE ||
                             leftNodeData.TypeID == Xflr.TYPE || leftNodeData.TypeID == Xfnc.TYPE || leftNodeData.TypeID == Xrof.TYPE ||
                             leftNodeData.TypeID == Xobj.TYPE ||
                             leftNodeData.TypeID == Xwnt.TYPE)
                    {
                        ShowCpf(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Str.TYPE || leftNodeData.TypeID == Ctss.TYPE || leftNodeData.TypeID == Ttas.TYPE)
                    {
                        ShowStr(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Shpe.TYPE)
                    {
                        ShowShpe(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Txmt.TYPE)
                    {
                        ShowTxmt(leftPackage, rightPackage);
                    }
                    else if (leftNodeData.TypeID == Lamb.TYPE || leftNodeData.TypeID == Ldir.TYPE || leftNodeData.TypeID == Lpnt.TYPE || leftNodeData.TypeID == Lspt.TYPE)
                    {
                        ShowLight(leftPackage, rightPackage);
                    }
                }

                leftPackage?.Close();
                rightPackage?.Close();
            }

            if (gridResCompare.Columns["colRightValue2"].Visible)
            {
                gridResCompare.Columns["colRightValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            else
            {
                gridResCompare.Columns["colRightValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            HighlightRows();
        }

        private void ShowBcon(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // BCON - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Bcon leftBcon = (Bcon)leftPackage.GetResourceByKey(leftNodeData.Key);
            Bcon rightBcon = (Bcon)rightPackage.GetResourceByKey(rightNodeData.Key);

            int entries = Math.Max(leftBcon.Count, rightBcon.Count);

            for (int index = 0; index < entries; ++index)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = index.ToString();

                row["LeftValue1"] = (index < leftBcon.Count) ? leftBcon.GetValue(index).ToString() : "";
                row["RightValue1"] = (index < rightBcon.Count) ? rightBcon.GetValue(index).ToString() : "";

                dataResCompare.Append(row);
            }
        }

        private void ShowTrcn(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // TRCN - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Name";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Name";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Trcn leftTrcn = (Trcn)leftPackage.GetResourceByKey(leftNodeData.Key);
            Trcn rightTrcn = (Trcn)rightPackage.GetResourceByKey(rightNodeData.Key);

            int entries = Math.Max(leftTrcn.Count, rightTrcn.Count);

            for (int index = 0; index < entries; ++index)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = index.ToString();

                row["LeftValue1"] = (index < leftTrcn.Count) ? leftTrcn.GetName(index) : "";
                row["RightValue1"] = (index < rightTrcn.Count) ? rightTrcn.GetName(index) : "";

                dataResCompare.Append(row);
            }
        }

        private void ShowBhav(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // BHAV - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Type";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Bhav leftBhav = (Bhav)leftPackage.GetResourceByKey(leftNodeData.Key);
            Bhav rightBhav = (Bhav)rightPackage.GetResourceByKey(rightNodeData.Key);

            List<string> leftText = new List<string>
                {
                    leftBhav.DiffString()
                };
            foreach (Instruction inst in leftBhav.Instructions)
            {
                leftText.Add(inst.DiffString(GameData.ShortPrimitivesByOpCode));
            }

            List<string> rightText = new List<string>
                {
                    rightBhav.DiffString()
                };
            foreach (Instruction inst in rightBhav.Instructions)
            {
                rightText.Add(inst.DiffString(GameData.ShortPrimitivesByOpCode));
            }

            ShowDiffs(leftText, rightText);
        }

        private void ShowSlot(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // SLOT - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Type";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Slot leftSlot = (Slot)leftPackage.GetResourceByKey(leftNodeData.Key);
            Slot rightSlot = (Slot)rightPackage.GetResourceByKey(rightNodeData.Key);

            List<string> leftText = new List<string>();
            foreach (SlotItem slotItem in leftSlot.Slots)
            {
                leftText.Add(slotItem.DiffString());
            }

            List<string> rightText = new List<string>();
            foreach (SlotItem slotItem in rightSlot.Slots)
            {
                rightText.Add(slotItem.DiffString());
            }

            ShowDiffs(leftText, rightText);
        }

        private void ShowIdr(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // IDR - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Type";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Idr leftIdr = (Idr)leftPackage.GetResourceByKey(leftNodeData.Key);
            Idr rightIdr = (Idr)rightPackage.GetResourceByKey(rightNodeData.Key);

            List<string> leftText = new List<string>();
            foreach (DBPFKey key in leftIdr.Items)
            {
                leftText.Add(key.ToString());
            }

            List<string> rightText = new List<string>();
            foreach (DBPFKey key in rightIdr.Items)
            {
                rightText.Add(key.ToString());
            }

            ShowDiffs(leftText, rightText);
        }

        private void ShowDiffs(List<string> leftText, List<string> rightText)
        {
            DiffItem[] diffItems = Diff.Diff.DiffText(leftText.ToArray(), rightText.ToArray());

            int leftIndex = 0;
            int rightIndex = 0;

            foreach (DiffItem diffItem in diffItems)
            {
                while (leftIndex < diffItem.startLeft)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = "Same";

                    row["LeftValue1"] = leftText[leftIndex++];
                    row["RightValue1"] = rightText[rightIndex++];

                    dataResCompare.Append(row);
                }

                Trace.Assert(rightIndex == diffItem.startRight);

                if (diffItem.deletedLeft > 0 && diffItem.insertedRight > 0)
                {
                    int rightCount = 0;

                    for (int leftCount = 0; leftCount < diffItem.deletedLeft; ++leftCount)
                    {
                        DataRow row = dataResCompare.NewRow();

                        if (rightCount < diffItem.insertedRight)
                        {
                            // this is a change
                            row["Key"] = "Change";

                            row["LeftValue1"] = leftText[leftIndex++];
                            row["RightValue1"] = rightText[rightIndex++];

                            ++rightCount;
                        }
                        else
                        {
                            // this is "only on the left"
                            row["Key"] = "Left Only";

                            row["LeftValue1"] = leftText[leftIndex++];
                            row["RightValue1"] = "";
                        }

                        dataResCompare.Append(row);
                    }

                    for (; rightCount < diffItem.insertedRight; ++rightCount)
                    {
                        // this is "only on the right"
                        DataRow row = dataResCompare.NewRow();

                        row["Key"] = "Right Only";

                        row["LeftValue1"] = "";
                        row["RightValue1"] = rightText[rightIndex++];

                        dataResCompare.Append(row);
                    }
                }
                else if (diffItem.deletedLeft > 0)
                {
                    for (int leftCount = 0; leftCount < diffItem.deletedLeft; ++leftCount)
                    {
                        // this is "only on the left"
                        DataRow row = dataResCompare.NewRow();
                        row["Key"] = "Left Only";

                        row["LeftValue1"] = leftText[leftIndex++];
                        row["RightValue1"] = "";

                        dataResCompare.Append(row);
                    }
                }
                else if (diffItem.insertedRight > 0)
                {
                    for (int rightCount = 0; rightCount < diffItem.insertedRight; ++rightCount)
                    {
                        // this is "only on the right"
                        DataRow row = dataResCompare.NewRow();
                        row["Key"] = "Right Only";

                        row["LeftValue1"] = "";
                        row["RightValue1"] = rightText[rightIndex++];

                        dataResCompare.Append(row);
                    }
                }
                else
                {
                    throw new Exception("Bad DiffItem");
                }
            }

            while (leftIndex < leftText.Count)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = (rightIndex < rightText.Count) ? "Same" : "Left Only";

                row["LeftValue1"] = leftText[leftIndex++];
                row["RightValue1"] = (rightIndex < rightText.Count) ? rightText[rightIndex++] : "";

                dataResCompare.Append(row);
            }

            while (rightIndex < rightText.Count)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = "Right Only";

                row["LeftValue1"] = "";
                row["RightValue1"] = rightText[rightIndex++];

                dataResCompare.Append(row);
            }
        }

        private void ShowTprp(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // TPRP - Drop-Down for Param/Local; Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Param/Local";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Name";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Name";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Tprp leftTprp = (Tprp)leftPackage.GetResourceByKey(leftNodeData.Key);
            Tprp rightTprp = (Tprp)rightPackage.GetResourceByKey(rightNodeData.Key);

            int paramEntries = Math.Max(leftTprp.ParamCount, rightTprp.ParamCount);

            for (int index = 0; index < paramEntries; ++index)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = $"Param:{index}";

                row["LeftValue1"] = (index < leftTprp.ParamCount) ? leftTprp.GetParamName(index) : "";
                row["RightValue1"] = (index < rightTprp.ParamCount) ? rightTprp.GetParamName(index) : "";

                dataResCompare.Append(row);
            }

            int localEntries = Math.Max(leftTprp.LocalCount, rightTprp.LocalCount);

            for (int index = 0; index < localEntries; ++index)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = $"Local:{index}";

                row["LeftValue1"] = (index < leftTprp.LocalCount) ? leftTprp.GetLocalName(index) : "";
                row["RightValue1"] = (index < rightTprp.LocalCount) ? rightTprp.GetLocalName(index) : "";

                dataResCompare.Append(row);
            }
        }

        private void ShowObjd(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // OBJD - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Name";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Objd leftObjd = (Objd)leftPackage.GetResourceByKey(leftNodeData.Key);
            Objd rightObjd = (Objd)rightPackage.GetResourceByKey(rightNodeData.Key);

            DataRow row;

            if (leftObjd.Guid != rightObjd.Guid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "GUID";
                row["LeftValue1"] = leftObjd.Guid;
                row["RightValue1"] = rightObjd.Guid;
                dataResCompare.Append(row);
            }

            if (leftObjd.OriginalGuid != rightObjd.OriginalGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Original GUID";
                row["LeftValue1"] = leftObjd.OriginalGuid;
                row["RightValue1"] = rightObjd.OriginalGuid;
                dataResCompare.Append(row);
            }

            if (leftObjd.ProxyGuid != rightObjd.ProxyGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Fallback GUID";
                row["LeftValue1"] = leftObjd.ProxyGuid;
                row["RightValue1"] = rightObjd.ProxyGuid;
                dataResCompare.Append(row);
            }

            if (leftObjd.DiagonalGuid != rightObjd.DiagonalGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Diagonal GUID";
                row["LeftValue1"] = leftObjd.DiagonalGuid;
                row["RightValue1"] = rightObjd.DiagonalGuid;
                dataResCompare.Append(row);
            }

            if (leftObjd.GridGuid != rightObjd.GridGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Grid Align GUID";
                row["LeftValue1"] = leftObjd.GridGuid;
                row["RightValue1"] = rightObjd.GridGuid;
                dataResCompare.Append(row);
            }

            if (leftObjd.Type != rightObjd.Type)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Object Type";
                row["LeftValue1"] = leftObjd.Type;
                row["RightValue1"] = rightObjd.Type;
                dataResCompare.Append(row);
            }

            for (ObjdIndex index = ObjdIndex.Version1; index <= ObjdIndex.Requirements; ++index)
            {
                if (leftObjd.GetRawData(index) != rightObjd.GetRawData(index))
                {
                    row = dataResCompare.NewRow();
                    row["Key"] = index.ToString();

                    row["LeftValue1"] = leftObjd.GetRawData(index);
                    row["RightValue1"] = rightObjd.GetRawData(index);

                    dataResCompare.Append(row);
                }
            }
        }

        private void ShowObjf(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // OBJF - Table; Cols - Function, Left Guardian, Left Action, Right Guardian, Right Action
            gridResCompare.Columns["colKey"].HeaderText = "Function";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Guardian";
            gridResCompare.Columns["colLeftValue2"].HeaderText = "Left Action";
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Guardian";
            gridResCompare.Columns["colRightValue2"].HeaderText = "Right Action";

            Objf leftObjf = (Objf)leftPackage.GetResourceByKey(leftNodeData.Key);
            Objf rightObjf = (Objf)rightPackage.GetResourceByKey(rightNodeData.Key);

            for (ObjfIndex index = ObjfIndex.init; index <= ObjfIndex.extractObjectInfoFromInvToken; ++index)
            {
                if (GetGuardian(leftObjf, index) != GetGuardian(rightObjf, index) || GetAction(leftObjf, index) != GetAction(rightObjf, index))
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = index.ToString();

                    row["LeftValue1"] = GetGuardian(leftObjf, index);
                    row["LeftValue2"] = GetAction(leftObjf, index);
                    row["RightValue1"] = GetGuardian(rightObjf, index);
                    row["RightValue2"] = GetAction(rightObjf, index);

                    dataResCompare.Append(row);
                }
            }
        }

        private void ShowGlob(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // GLOB - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Glob leftGlob = (Glob)leftPackage.GetResourceByKey(leftNodeData.Key);
            Glob rightGlob = (Glob)rightPackage.GetResourceByKey(rightNodeData.Key);

            DataRow row = dataResCompare.NewRow();
            row["Key"] = "Semi-Globals";

            row["LeftValue1"] = leftGlob.SemiGlobalName;
            row["RightValue1"] = rightGlob.SemiGlobalName;

            dataResCompare.Append(row);
        }

        private void ShowNref(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // NREF - Table; Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Nref leftNref = (Nref)leftPackage.GetResourceByKey(leftNodeData.Key);
            Nref rightNref = (Nref)rightPackage.GetResourceByKey(rightNodeData.Key);

            DataRow row = dataResCompare.NewRow();
            row["Key"] = "Name";

            row["LeftValue1"] = leftNref.KeyName;
            row["RightValue1"] = rightNref.KeyName;

            dataResCompare.Append(row);
        }

        private void ShowCpf(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // CPF - Table; Cols - Key, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Name";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            Cpf leftCpf = (Cpf)leftPackage.GetResourceByKey(leftNodeData.Key);
            Cpf rightCpf = (Cpf)rightPackage.GetResourceByKey(rightNodeData.Key);

            foreach (string leftName in leftCpf.GetItemNames())
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = leftName;

                row["LeftValue1"] = leftCpf.GetItem(leftName).StringValue;
                row["RightValue1"] = rightCpf.GetItem(leftName)?.StringValue ?? "";

                dataResCompare.Append(row);
            }

            foreach (string rightName in rightCpf.GetItemNames())
            {
                if (leftCpf.GetItem(rightName) == null)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = rightName;

                    row["LeftValue1"] = "";
                    row["RightValue1"] = rightCpf.GetItem(rightName).StringValue;

                    dataResCompare.Append(row);
                }
            }
        }

        private void ShowStr(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // STR/CTSS/TTAs - Drop-Down for language, Table; Cols - Index, Left Title, Left Desc, Right Title, Right Desc
            gridResCompare.Columns["colKey"].HeaderText = "Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Title";
            gridResCompare.Columns["colLeftValue2"].HeaderText = "Left Desc";
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Title";
            gridResCompare.Columns["colRightValue2"].HeaderText = "Right Desc";

            comboVariations.Visible = true;

            leftRes = leftPackage.GetResourceByKey(leftNodeData.Key);
            rightRes = rightPackage.GetResourceByKey(rightNodeData.Key);

            Str leftStr = (Str)leftRes;
            Str rightStr = (Str)rightRes;

            comboVariations.Items.Clear();

            SortedList<byte, StrLanguage> allLanguages = new SortedList<byte, StrLanguage>();

            ReadOnlyCollection<StrLanguage> leftLanguages = leftStr.Languages;
            ReadOnlyCollection<StrLanguage> rightLanguages = rightStr.Languages;

            foreach (StrLanguage lang in leftLanguages)
            {
                allLanguages.Add(lang.Id, lang);
            }

            foreach (StrLanguage lang in rightStr.Languages)
            {
                if (!leftLanguages.Contains(lang))
                {
                    allLanguages.Add(lang.Id, lang);
                }
            }

            foreach (StrLanguage lang in allLanguages.Values)
            {
                DbpfNodeState state = DbpfNodeState.Same;

                if (!leftLanguages.Contains(lang))
                {
                    state = DbpfNodeState.LeftMissing;
                }
                else if (!rightLanguages.Contains(lang))
                {
                    state = DbpfNodeState.RightMissing;
                }
                else
                {
                    List<StrItem> leftItems = leftStr.LanguageItems(lang);
                    List<StrItem> rightItems = rightStr.LanguageItems(lang);

                    if (leftItems.Count != rightItems.Count)
                    {
                        state = DbpfNodeState.Different;
                    }
                    else
                    {
                        for (int i = 0; i < leftItems.Count; ++i)
                        {
                            if (!leftItems[i].Equals(rightItems[i]))
                            {
                                state = DbpfNodeState.Different;
                                break;
                            }
                        }
                    }
                }

                comboVariations.Items.Add(new DropDownLanguage(state, lang));
            }

            comboVariations.SelectedIndex = 0;
        }

        private void ShowTxmt(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // TXMT - Drop-Down for MatDef/Props/Files, Table; Cols - Name/Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Name/Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            comboVariations.Visible = true;

            leftRes = leftPackage.GetResourceByKey(leftNodeData.Key);
            rightRes = rightPackage.GetResourceByKey(rightNodeData.Key);

            CMaterialDefinition leftMatDef = ((Txmt)leftRes).MaterialDefinition;
            CMaterialDefinition rightMatDef = ((Txmt)rightRes).MaterialDefinition;

            comboVariations.Items.Clear();
            int comboIndex = -1;

            DbpfNodeState propsState = DbpfNodeState.Same;
            if (leftMatDef.GetPropertyNames().Count != rightMatDef.GetPropertyNames().Count)
            {
                propsState = DbpfNodeState.Different;
            }
            else
            {
                foreach (string propName in leftMatDef.GetPropertyNames())
                {
                    if (!leftMatDef.GetProperty(propName).Equals(rightMatDef.GetProperty(propName)))
                    {
                        propsState = DbpfNodeState.Different;
                        break;
                    }
                }
            }
            comboVariations.Items.Add(new DropDownMaterial(propsState, "Properties"));
            if (comboIndex == -1 && propsState != DbpfNodeState.Same) comboIndex = 0;

            DbpfNodeState filesState = DbpfNodeState.Same;
            if (leftMatDef.FileList.Count != rightMatDef.FileList.Count)
            {
                filesState = DbpfNodeState.Different;
            }
            else
            {
                foreach (string file in leftMatDef.FileList)
                {
                    if (!rightMatDef.FileList.Contains(file))
                    {
                        filesState = DbpfNodeState.Different;
                        break;
                    }
                }
            }
            comboVariations.Items.Add(new DropDownMaterial(filesState, "Files"));
            if (comboIndex == -1 && filesState != DbpfNodeState.Same) comboIndex = 1;

            DbpfNodeState defState = (leftMatDef.Version != rightMatDef.Version ||
                                      !leftMatDef.MaterialType.Equals(rightMatDef.MaterialType) ||
                                      !leftMatDef.FileDescription.Equals(rightMatDef.FileDescription)) ? DbpfNodeState.Different : DbpfNodeState.Same;
            comboVariations.Items.Add(new DropDownMaterial(defState, "Definition"));
            if (comboIndex == -1 && defState != DbpfNodeState.Same) comboIndex = 2;

            comboVariations.SelectedIndex = (comboIndex == -1 ? 0 : comboIndex);
        }

        private void ShowShpe(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // SHPE - Drop-Down for Parts/Items/Details, Table; Cols - Name/Index, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Name/Index";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            comboVariations.Visible = true;

            leftRes = leftPackage.GetResourceByKey(leftNodeData.Key);
            rightRes = rightPackage.GetResourceByKey(rightNodeData.Key);

            CShape leftShape = ((Shpe)leftRes).Shape;
            CShape rightShape = ((Shpe)rightRes).Shape;

            comboVariations.Items.Clear();
            int comboIndex = -1;

            DbpfNodeState partsState = DbpfNodeState.Same;
            if (leftShape.Parts.Count != rightShape.Parts.Count)
            {
                partsState = DbpfNodeState.Different;
            }
            else
            {
                foreach (ShapePart part in leftShape.Parts)
                {
                    if (!part.Material.Equals(rightShape.GetSubsetMaterial(part.Subset)))
                    {
                        partsState = DbpfNodeState.Different;
                        break;
                    }
                }
            }
            comboVariations.Items.Add(new DropDownShape(partsState, "Parts"));
            if (comboIndex == -1 && partsState != DbpfNodeState.Same) comboIndex = 0;

            DbpfNodeState itemsState = DbpfNodeState.Same;
            if (leftShape.Items.Count != rightShape.Items.Count)
            {
                itemsState = DbpfNodeState.Different;
            }
            else
            {
                for (int index = 0; index < leftShape.Items.Count; ++index)
                {
                    if (!leftShape.Items[index].FileName.Equals(rightShape.Items[index].FileName))
                    {
                        itemsState = DbpfNodeState.Different;
                        break;
                    }
                }
            }
            comboVariations.Items.Add(new DropDownShape(itemsState, "Items"));
            if (comboIndex == -1 && itemsState != DbpfNodeState.Same) comboIndex = 1;

            DbpfNodeState defState = (leftShape.Version != rightShape.Version ||
                                      leftShape.Lod != rightShape.Lod ||
                                      !leftShape.ObjectGraphNode.Equals(rightShape.ObjectGraphNode)) ? DbpfNodeState.Different : DbpfNodeState.Same;
            comboVariations.Items.Add(new DropDownShape(defState, "Definition"));
            if (comboIndex == -1 && defState != DbpfNodeState.Same) comboIndex = 2;

            comboVariations.SelectedIndex = (comboIndex == -1 ? 0 : comboIndex);
        }

        private void ShowLight(DBPFFile leftPackage, DBPFFile rightPackage)
        {
            // Light - Table; Name, Left Value, Right Value
            gridResCompare.Columns["colKey"].HeaderText = "Name";
            gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
            gridResCompare.Columns["colLeftValue2"].Visible = false;
            gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
            gridResCompare.Columns["colRightValue2"].Visible = false;

            AbstractLightRcolBlock leftBaseLight = ((Lght)leftPackage.GetResourceByKey(leftNodeData.Key)).BaseLight;
            AbstractLightRcolBlock rightBaseLight = ((Lght)rightPackage.GetResourceByKey(rightNodeData.Key)).BaseLight;

            CDirectionalLight leftDirLight = leftBaseLight as CDirectionalLight;
            CDirectionalLight rightDirLight = rightBaseLight as CDirectionalLight;

            DataRow row;

            row = dataResCompare.NewRow();
            row["Key"] = "Version";
            row["LeftValue1"] = leftDirLight.Version;
            row["RightValue1"] = rightDirLight.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Name";
            row["LeftValue1"] = leftDirLight.Name;
            row["RightValue1"] = rightDirLight.Name;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Red";
            row["LeftValue1"] = leftDirLight.Red;
            row["RightValue1"] = rightDirLight.Red;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Green";
            row["LeftValue1"] = leftDirLight.Green;
            row["RightValue1"] = rightDirLight.Green;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Blue";
            row["LeftValue1"] = leftDirLight.Blue;
            row["RightValue1"] = rightDirLight.Blue;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Val1 (unknown3)";
            row["LeftValue1"] = leftDirLight.Val1;
            row["RightValue1"] = rightDirLight.Val1;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "Val2 (unknown4)";
            row["LeftValue1"] = leftDirLight.Val2;
            row["RightValue1"] = rightDirLight.Val2;
            dataResCompare.Append(row);

            if (leftBaseLight is CPointLight leftPointLight && rightBaseLight is CPointLight rightPointLight)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Val6 (unknown8)";
                row["LeftValue1"] = leftPointLight.Val6;
                row["RightValue1"] = rightPointLight.Val6;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Val7 (unknown9)";
                row["LeftValue1"] = leftPointLight.Val7;
                row["RightValue1"] = rightPointLight.Val7;
                dataResCompare.Append(row);

                if (leftBaseLight is CSpotLight leftSpotLight && rightBaseLight is CSpotLight rightSpotLight)
                {
                    row = dataResCompare.NewRow();
                    row["Key"] = "Val8 (unknown10)";
                    row["LeftValue1"] = leftSpotLight.Val8;
                    row["RightValue1"] = rightSpotLight.Val8;
                    dataResCompare.Append(row);

                    row = dataResCompare.NewRow();
                    row["Key"] = "Val9 (unknown11)";
                    row["LeftValue1"] = leftSpotLight.Val9;
                    row["RightValue1"] = rightSpotLight.Val9;
                    dataResCompare.Append(row);
                }
            }

            row = dataResCompare.NewRow();
            row["Key"] = "NameResource:Version";
            row["LeftValue1"] = leftDirLight.NameResource.Version;
            row["RightValue1"] = rightDirLight.NameResource.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "NameResource:FileName";
            row["LeftValue1"] = leftDirLight.NameResource.FileName;
            row["RightValue1"] = rightDirLight.NameResource.FileName;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "ObjectGraphNode:Version";
            row["LeftValue1"] = leftDirLight.ObjectGraphNode.Version;
            row["RightValue1"] = rightDirLight.ObjectGraphNode.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "ObjectGraphNode:FileName";
            row["LeftValue1"] = leftDirLight.ObjectGraphNode.FileName;
            row["RightValue1"] = rightDirLight.ObjectGraphNode.FileName;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "LightT:Version";
            row["LeftValue1"] = leftDirLight.LightT.Version;
            row["RightValue1"] = rightDirLight.LightT.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "LightT:NameResource:Version";
            row["LeftValue1"] = leftDirLight.LightT.NameResource.Version;
            row["RightValue1"] = rightDirLight.LightT.NameResource.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "LightT:NameResource:FileName";
            row["LeftValue1"] = leftDirLight.LightT.NameResource.FileName;
            row["RightValue1"] = rightDirLight.LightT.NameResource.FileName;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "StandardLightBase:Version";
            row["LeftValue1"] = leftDirLight.StandardLightBase.Version;
            row["RightValue1"] = rightDirLight.StandardLightBase.Version;
            dataResCompare.Append(row);

            row = dataResCompare.NewRow();
            row["Key"] = "ReferentNode:Version";
            row["LeftValue1"] = leftDirLight.ReferentNode.Version;
            row["RightValue1"] = rightDirLight.ReferentNode.Version;
            dataResCompare.Append(row);
        }

        private void PopulateLanguage(MetaData.Languages lang, Str leftStr, Str rightStr)
        {
            dataResCompare.Clear();

            List<StrItem> leftItems = leftStr.LanguageItems(lang);
            List<StrItem> rightItems = rightStr.LanguageItems(lang);

            int entries = Math.Max(leftItems.Count, rightItems.Count);

            for (int index = 0; index < entries; ++index)
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = index.ToString();

                if (index < leftItems.Count)
                {
                    row["LeftValue1"] = leftItems[index].Title;
                    row["LeftValue2"] = leftItems[index].Description;
                }
                else
                {
                    row["LeftValue1"] = "";
                    row["LeftValue2"] = "";
                }

                if (index < rightItems.Count)
                {
                    row["RightValue1"] = rightItems[index].Title;
                    row["RightValue2"] = rightItems[index].Description;
                }
                else
                {
                    row["RightValue1"] = "";
                    row["RightValue2"] = "";
                }

                dataResCompare.Append(row);
            }
        }

        private void PopulateMaterial(int key, Txmt leftTxmt, Txmt rightTxmt)
        {
            dataResCompare.Clear();

            if (key == 0)
            {
                ReadOnlyCollection<string> leftNames = leftTxmt.MaterialDefinition.GetPropertyNames();
                ReadOnlyCollection<string> rightNames = rightTxmt.MaterialDefinition.GetPropertyNames();

                foreach (string name in leftNames)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = name;

                    row["LeftValue1"] = leftTxmt.MaterialDefinition.GetProperty(name);

                    if (rightTxmt.MaterialDefinition.HasProperty(name))
                    {
                        row["RightValue1"] = rightTxmt.MaterialDefinition.GetProperty(name);
                    }
                    else
                    {
                        row["RightValue1"] = "";
                    }

                    dataResCompare.Append(row);
                }

                foreach (string name in rightNames)
                {
                    if (!leftNames.Contains(name))
                    {
                        DataRow row = dataResCompare.NewRow();
                        row["Key"] = name;

                        row["LeftValue1"] = "";
                        row["RightValue1"] = rightTxmt.MaterialDefinition.GetProperty(name);

                        dataResCompare.Append(row);
                    }
                }
            }
            else if (key == 1)
            {
                ReadOnlyCollection<string> leftFiles = leftTxmt.MaterialDefinition.FileList;
                ReadOnlyCollection<string> rightFiles = rightTxmt.MaterialDefinition.FileList;

                SortedList<string, string> allFiles = new SortedList<string, string>();

                foreach (string file in leftFiles)
                {
                    allFiles.Add(file, file);
                }

                foreach (string file in rightFiles)
                {
                    if (!allFiles.ContainsKey(file))
                    {
                        allFiles.Add(file, file);
                    }
                }

                foreach (string file in allFiles.Values)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = file;

                    row["LeftValue1"] = leftFiles.Contains(file) ? file : "";
                    row["RightValue1"] = rightFiles.Contains(file) ? file : "";

                    dataResCompare.Append(row);
                }
            }
            else
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = "Version";
                row["LeftValue1"] = Helper.Hex8PrefixString(leftTxmt.MaterialDefinition.Version);
                row["RightValue1"] = Helper.Hex8PrefixString(rightTxmt.MaterialDefinition.Version);
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Description";
                row["LeftValue1"] = leftTxmt.MaterialDefinition.FileDescription;
                row["RightValue1"] = rightTxmt.MaterialDefinition.FileDescription;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Type";
                row["LeftValue1"] = leftTxmt.MaterialDefinition.MaterialType;
                row["RightValue1"] = rightTxmt.MaterialDefinition.MaterialType;
                dataResCompare.Append(row);
            }
        }

        private void PopulateShape(int key, Shpe leftShpe, Shpe rightShpe)
        {
            dataResCompare.Clear();

            if (key == 0)
            {
                ReadOnlyCollection<ShapePart> leftParts = leftShpe.Shape.Parts;
                ReadOnlyCollection<ShapePart> rightParts = rightShpe.Shape.Parts;

                List<string> leftSubsets = new List<string>();
                List<string> rightSubsets = new List<string>();
                SortedList<string, string> allSubsets = new SortedList<string, string>();

                foreach (ShapePart part in leftParts)
                {
                    leftSubsets.Add(part.Subset);
                    allSubsets.Add(part.Subset, part.Subset);
                }

                foreach (ShapePart part in rightParts)
                {
                    rightSubsets.Add(part.Subset);

                    if (!allSubsets.ContainsKey(part.Subset))
                    {
                        allSubsets.Add(part.Subset, part.Subset);
                    }
                }

                foreach (string subset in allSubsets.Values)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = subset;

                    row["LeftValue1"] = leftSubsets.Contains(subset) ? leftShpe.Shape.GetSubsetMaterial(subset) : "";
                    row["RightValue1"] = rightSubsets.Contains(subset) ? rightShpe.Shape.GetSubsetMaterial(subset) : "";

                    dataResCompare.Append(row);
                }
            }
            else if (key == 1)
            {
                ReadOnlyCollection<ShapeItem> leftItems = leftShpe.Shape.Items;
                ReadOnlyCollection<ShapeItem> rightItems = rightShpe.Shape.Items;

                for (int index = 0; index < leftItems.Count; ++index)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = index;

                    row["LeftValue1"] = leftItems[index].FileName;
                    row["RightValue1"] = index < rightItems.Count ? rightItems[index].FileName : "";

                    dataResCompare.Append(row);
                }

                for (int index = leftItems.Count; index < rightItems.Count; ++index)
                {
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = index;

                    row["LeftValue1"] = "";
                    row["RightValue1"] = rightItems[index].FileName;

                    dataResCompare.Append(row);
                }
            }
            else
            {
                DataRow row = dataResCompare.NewRow();
                row["Key"] = "Version";
                row["LeftValue1"] = Helper.Hex8PrefixString(leftShpe.Shape.Version);
                row["RightValue1"] = Helper.Hex8PrefixString(rightShpe.Shape.Version);
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "OGN Filename";
                row["LeftValue1"] = leftShpe.Shape.ObjectGraphNode.FileName;
                row["RightValue1"] = rightShpe.Shape.ObjectGraphNode.FileName;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "LoD";
                row["LeftValue1"] = leftShpe.Shape.Lod;
                row["RightValue1"] = rightShpe.Shape.Lod;
                dataResCompare.Append(row);
            }
        }

        private string GetGuardian(Objf objf, ObjfIndex index)
        {
            string guardian = "";

            if (objf.HasEntry(index))
            {
                uint bhav = objf.GetGuardian(index);

                if (bhav != 0)
                {
                    return Helper.Hex4PrefixString(bhav);
                }
            }

            return guardian;
        }

        private string GetAction(Objf objf, ObjfIndex index)
        {
            string guardian = "";

            if (objf.HasEntry(index))
            {
                uint bhav = objf.GetAction(index);

                if (bhav != 0)
                {
                    return Helper.Hex4PrefixString(bhav);
                }
            }

            return guardian;
        }

        private void HighlightRows()
        {
            // if (nodeData.TypeID != Bhav.TYPE)
            {
                foreach (DataGridViewRow row in gridResCompare.Rows)
                {
                    string leftValue1 = row.Cells["colLeftValue1"].Value as string;
                    string leftValue2 = row.Cells["colLeftValue2"].Value as string;
                    string rightValue1 = row.Cells["colRightValue1"].Value as string;
                    string rightValue2 = row.Cells["colRightValue2"].Value as string;

                    if (!leftValue1.Equals(rightValue1) || (leftValue2 != null && !leftValue2.Equals(rightValue2)))
                    {
                        row.DefaultCellStyle.BackColor = colourHighlightDiffers;
                    }
                }
            }
        }

        private void OnKeepRight(object sender, EventArgs e)
        {
            leftNodeData.SetSame();
            rightNodeData.SetSame();
            this.Close();
        }

        private void OnUseLeft(object sender, EventArgs e)
        {
            leftNodeData.SetCopyLeftToRight();

            if (!leftNodeData.Equals(rightNodeData))
            {
                rightNodeData.SetToBeDeleted();
            }

            this.Close();
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (gridResCompare.SelectedRows.Count > 0)
            {
                // Zap the blue selection bar
                gridResCompare.DefaultCellStyle.SelectionForeColor = gridResCompare.DefaultCellStyle.ForeColor;

                DataGridViewRow row = gridResCompare.SelectedRows[0];

                if (!(row.Cells["colLeftValue1"].Value as string).Equals(row.Cells["colRightValue1"].Value as string))
                {
                    gridResCompare.DefaultCellStyle.SelectionBackColor = colourHighlightDiffers;
                }
                else
                {
                    if (row.Cells["colLeftValue2"].Value is string leftValue2 && !leftValue2.Equals(row.Cells["colRightValue2"].Value as string))
                    {
                        gridResCompare.DefaultCellStyle.SelectionBackColor = colourHighlightDiffers;
                    }
                    else
                    {
                        gridResCompare.DefaultCellStyle.SelectionBackColor = gridResCompare.DefaultCellStyle.BackColor;
                    }
                }
            }
        }

        private void OnDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }

        private void OnComboVariationsChanged(object sender, EventArgs e)
        {
            if (comboVariations.SelectedItem is DropDownLanguage data)
            {
                PopulateLanguage(data.Lang, (Str)leftRes, (Str)rightRes);
            }
            else if (comboVariations.SelectedItem is DropDownMaterial)
            {
                PopulateMaterial(comboVariations.SelectedIndex, (Txmt)leftRes, (Txmt)rightRes);
            }
            else if (comboVariations.SelectedItem is DropDownShape)
            {
                PopulateShape(comboVariations.SelectedIndex, (Shpe)leftRes, (Shpe)rightRes);
            }

            HighlightRows();
        }

        private void OnCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (leftNodeData.TypeID == Bhav.TYPE || leftNodeData.TypeID == Slot.TYPE || leftNodeData.TypeID == Idr.TYPE)
            {
                Graphics g = e.Graphics;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                using (Brush gridBrush = new SolidBrush(gridResCompare.GridColor), backColourBrush = new SolidBrush(e.CellStyle.BackColor), textColourBrush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        g.FillRectangle(backColourBrush, e.CellBounds);

                        // Draw the grid lines (only the right and bottom lines;
                        // DataGridView takes care of the others).
                        g.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        g.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                        // Draw the text content of the cell, ignoring alignment.
                        if (e.Value != null)
                        {
                            float xPos = e.CellBounds.X + 2;
                            float yPos = e.CellBounds.Y + 2;

                            Font font = e.CellStyle.Font;

                            if (e.ColumnIndex > 0 && e.RowIndex >= 0 && e.RowIndex < gridResCompare.Rows.Count)
                            {
                                string leftText = gridResCompare.Rows[e.RowIndex].Cells["colLeftValue1"].Value as string;
                                string rightText = gridResCompare.Rows[e.RowIndex].Cells["colRightValue1"].Value as string;

                                if (string.IsNullOrEmpty(leftText) || string.IsNullOrEmpty(rightText))
                                {
                                    // Left or Right Only
                                    StringOutput(g, e.Value as string, font, brushDiffers, xPos, yPos);
                                }
                                else if (leftText.Equals(rightText))
                                {
                                    // Same
                                    StringOutput(g, e.Value as string, font, textColourBrush, xPos, yPos);
                                }
                                else
                                {
                                    Regex re = new Regex("([^- ;:,]+)([- ;:,]+)?");

                                    List<string> leftWords = new List<string>();
                                    foreach (Match match in re.Matches(leftText))
                                    {
                                        leftWords.Add(match.Groups[1].Value);
                                        if (match.Groups.Count > 2)
                                        {
                                            leftWords.Add(match.Groups[2].Value);
                                        }
                                    }

                                    List<string> rightWords = new List<string>();
                                    foreach (Match match in re.Matches(rightText))
                                    {
                                        rightWords.Add(match.Groups[1].Value);
                                        if (match.Groups.Count > 2)
                                        {
                                            rightWords.Add(match.Groups[2].Value);
                                        }
                                    }

                                    DiffItem[] diffItems = Diff.Diff.DiffText(leftWords.ToArray(), rightWords.ToArray());

                                    string textRun;

                                    if (e.ColumnIndex < 2)
                                    {
                                        // Left words
                                        int leftIndex = 0;

                                        foreach (DiffItem diffItem in diffItems)
                                        {
                                            textRun = "";
                                            while (leftIndex < diffItem.startLeft)
                                            {
                                                textRun = $"{textRun}{leftWords[leftIndex++]}";
                                            }

                                            if (textRun.Length > 0)
                                            {
                                                xPos += StringOutput(g, textRun, font, textColourBrush, xPos, yPos);
                                            }

                                            textRun = "";
                                            for (int i = 0; i < diffItem.deletedLeft; ++i)
                                            {
                                                textRun = $"{textRun}{leftWords[leftIndex++]}";
                                            }

                                            xPos += StringOutput(g, textRun, font, brushDiffers, xPos, yPos);
                                        }

                                        textRun = "";
                                        while (leftIndex < leftWords.Count)
                                        {
                                            textRun = $"{textRun}{leftWords[leftIndex++]}";
                                        }

                                        if (textRun.Length > 0)
                                        {
                                            StringOutput(g, textRun, font, textColourBrush, xPos, yPos);
                                        }
                                    }
                                    else
                                    {
                                        // Right words
                                        int rightIndex = 0;

                                        foreach (DiffItem diffItem in diffItems)
                                        {
                                            textRun = "";
                                            while (rightIndex < diffItem.startRight)
                                            {
                                                textRun = $"{textRun}{rightWords[rightIndex++]}";
                                            }

                                            if (textRun.Length > 0)
                                            {
                                                xPos += StringOutput(g, textRun, font, textColourBrush, xPos, yPos);
                                            }

                                            textRun = "";
                                            for (int i = 0; i < diffItem.insertedRight; ++i)
                                            {
                                                textRun = $"{textRun}{rightWords[rightIndex++]}";
                                            }

                                            xPos += StringOutput(g, textRun, font, brushDiffers, xPos, yPos);
                                        }

                                        textRun = "";
                                        while (rightIndex < rightWords.Count)
                                        {
                                            textRun = $"{textRun}{rightWords[rightIndex++]}";
                                        }

                                        if (textRun.Length > 0)
                                        {
                                            StringOutput(g, textRun, font, textColourBrush, xPos, yPos);
                                        }
                                    }
                                }

                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        private float StringOutput(Graphics g, string textRun, Font font, Brush brush, float xPos, float yPos)
        {
            g.DrawString(textRun, font, brush, xPos, yPos, StringFormat.GenericTypographic);

            return g.MeasureString(textRun, font, 10000, StringFormat.GenericTypographic).Width;
        }

        private void OnCellToolTipNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (leftNodeData.TypeID == Bhav.TYPE)
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 3)
                {
                    int nodeIndex = -1;

                    for (int i = 0; i < e.RowIndex; ++i)
                    {
                        if (!string.IsNullOrEmpty(dataResCompare.Rows[i][e.ColumnIndex] as string))
                        {
                            ++nodeIndex;
                        }
                    }

                    if (nodeIndex >= 0)
                    {
                        e.ToolTipText = $"Node {Helper.Hex2PrefixString((uint)nodeIndex)} ({nodeIndex})";
                    }
                }
            }
        }

        private void OnDropDownDrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= comboVariations.Items.Count) return;

            Brush brush = brushSame;

            if (comboVariations.Items[e.Index] is DropDownItem data)
            {
                if (data.IsSame)
                {
                    brush = brushSame;
                }
                else if (data.IsDifferent)
                {
                    brush = brushDiffers;
                }
                else
                {
                    brush = brushMissing;
                }
            }

            e.Graphics.DrawString((sender as ComboBox).Items[e.Index].ToString(), e.Font, brush, e.Bounds);

            e.DrawFocusRectangle();
        }

        private void OnSorted(object sender, EventArgs e)
        {
            HighlightRows();
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    internal class ResCompareData : DataTable
    {
        public ResCompareData()
        {
            this.Columns.Add(new DataColumn("Key", typeof(string)));
            this.Columns.Add(new DataColumn("LeftValue1", typeof(string)));
            this.Columns.Add(new DataColumn("LeftValue2", typeof(string)));
            this.Columns.Add(new DataColumn("RightValue1", typeof(string)));
            this.Columns.Add(new DataColumn("RightValue2", typeof(string)));
        }

        public bool HasResults => (this.Rows.Count > 0);

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }

    internal abstract class DropDownItem
    {
        private readonly DbpfNodeState state;

        internal bool IsSame => (state == DbpfNodeState.Same);
        internal bool IsLeftMissing => (state == DbpfNodeState.LeftMissing);
        internal bool IsRightMissing => (state == DbpfNodeState.RightMissing);
        internal bool IsDifferent => (state == DbpfNodeState.Different);

        internal DropDownItem(DbpfNodeState state)
        {
            this.state = state;
        }
    }

    internal class DropDownLanguage : DropDownItem
    {
        private readonly StrLanguage lang;

        internal StrLanguage Lang => lang;

        internal DropDownLanguage(DbpfNodeState state, StrLanguage lang) : base(state)
        {
            this.lang = lang;
        }

        public override string ToString()
        {
            return lang.ToString();
        }
    }

    internal class DropDownMaterial : DropDownItem
    {
        private readonly string name;

        internal DropDownMaterial(DbpfNodeState state, string name) : base(state)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    internal class DropDownShape : DropDownItem
    {
        private readonly string name;

        internal DropDownShape(DbpfNodeState state, string name) : base(state)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
