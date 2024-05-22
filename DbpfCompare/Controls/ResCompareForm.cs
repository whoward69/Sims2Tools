/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2024
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
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
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

        private readonly DbpfCompareNodeResourceData nodeData;
        private readonly string leftPackagePath, rightPackagePath;

        private readonly bool excludeSame = false;

        private DBPFResource leftRes, rightRes;

        public ResCompareForm(DbpfCompareNodeResourceData nodeData, string leftPackagePath, string rightPackagePath, bool excludeSame)
        {
            InitializeComponent();

            this.nodeData = nodeData;
            this.leftPackagePath = leftPackagePath;
            this.rightPackagePath = rightPackagePath;

            // this.excludeSame = excludeSame;

            gridResCompare.DataSource = dataResCompare;
            dataResCompare.Clear();

            this.Text = nodeData.Key.ToString();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadPopupSettings(DbpfCompareApp.RegistryKey, this);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SavePopupSettings(DbpfCompareApp.RegistryKey, this);
        }

        // TODO - DBPF Compare - 5 - should probably be on a worker thread
        private void OnShow(object sender, EventArgs e)
        {
            comboVariations.Visible = false;

            DBPFFile leftPackage = new DBPFFile(leftPackagePath);
            DBPFFile rightPackage = new DBPFFile(rightPackagePath);

            if (nodeData.TypeID == Bcon.TYPE)
            {
                ShowBcon(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Trcn.TYPE)
            {
                ShowTrcn(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Bhav.TYPE)
            {
                ShowBhav(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Tprp.TYPE)
            {
                ShowTprp(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Objd.TYPE)
            {
                ShowObjd(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Objf.TYPE)
            {
                ShowObjf(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Glob.TYPE)
            {
                ShowGlob(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Nref.TYPE)
            {
                ShowNref(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Slot.TYPE)
            {
                ShowSlot(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Idr.TYPE)
            {
                ShowIdr(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Binx.TYPE ||
                     nodeData.TypeID == Coll.TYPE ||
                     nodeData.TypeID == Gzps.TYPE ||
                     nodeData.TypeID == Mmat.TYPE ||
                     nodeData.TypeID == Vers.TYPE ||
                     nodeData.TypeID == Sdna.TYPE ||
                     nodeData.TypeID == Xfch.TYPE || nodeData.TypeID == Xhtn.TYPE || nodeData.TypeID == Xmol.TYPE || nodeData.TypeID == Xstn.TYPE || nodeData.TypeID == Xtol.TYPE ||
                     nodeData.TypeID == Xflr.TYPE || nodeData.TypeID == Xfnc.TYPE || nodeData.TypeID == Xrof.TYPE ||
                     nodeData.TypeID == Xobj.TYPE ||
                     nodeData.TypeID == Xwnt.TYPE)
            {
                ShowCpf(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Str.TYPE || nodeData.TypeID == Ctss.TYPE || nodeData.TypeID == Ttas.TYPE)
            {
                ShowStr(leftPackage, rightPackage);
            }
            else if (nodeData.TypeID == Txmt.TYPE)
            {
                ShowTxmt(leftPackage, rightPackage);
            }

            leftPackage?.Close();
            rightPackage?.Close();

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

            Bcon leftBcon = (Bcon)leftPackage.GetResourceByKey(nodeData.Key);
            Bcon rightBcon = (Bcon)rightPackage.GetResourceByKey(nodeData.Key);

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

            Trcn leftTrcn = (Trcn)leftPackage.GetResourceByKey(nodeData.Key);
            Trcn rightTrcn = (Trcn)rightPackage.GetResourceByKey(nodeData.Key);

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

            Bhav leftBhav = (Bhav)leftPackage.GetResourceByKey(nodeData.Key);
            Bhav rightBhav = (Bhav)rightPackage.GetResourceByKey(nodeData.Key);

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

            Slot leftSlot = (Slot)leftPackage.GetResourceByKey(nodeData.Key);
            Slot rightSlot = (Slot)rightPackage.GetResourceByKey(nodeData.Key);

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

            Idr leftIdr = (Idr)leftPackage.GetResourceByKey(nodeData.Key);
            Idr rightIdr = (Idr)rightPackage.GetResourceByKey(nodeData.Key);

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

            Tprp leftTprp = (Tprp)leftPackage.GetResourceByKey(nodeData.Key);
            Tprp rightTprp = (Tprp)rightPackage.GetResourceByKey(nodeData.Key);

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

            Objd leftObjd = (Objd)leftPackage.GetResourceByKey(nodeData.Key);
            Objd rightObjd = (Objd)rightPackage.GetResourceByKey(nodeData.Key);

            DataRow row;

            if (!excludeSame || leftObjd.Guid != rightObjd.Guid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "GUID";
                row["LeftValue1"] = leftObjd.Guid;
                row["RightValue1"] = rightObjd.Guid;
                dataResCompare.Append(row);
            }

            if (!excludeSame || leftObjd.OriginalGuid != rightObjd.OriginalGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Original GUID";
                row["LeftValue1"] = leftObjd.OriginalGuid;
                row["RightValue1"] = rightObjd.OriginalGuid;
                dataResCompare.Append(row);
            }

            if (!excludeSame || leftObjd.ProxyGuid != rightObjd.ProxyGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Fallback GUID";
                row["LeftValue1"] = leftObjd.ProxyGuid;
                row["RightValue1"] = rightObjd.ProxyGuid;
                dataResCompare.Append(row);
            }

            if (!excludeSame || leftObjd.DiagonalGuid != rightObjd.DiagonalGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Diagonal GUID";
                row["LeftValue1"] = leftObjd.DiagonalGuid;
                row["RightValue1"] = rightObjd.DiagonalGuid;
                dataResCompare.Append(row);
            }

            if (!excludeSame || leftObjd.GridGuid != rightObjd.GridGuid)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Grid Align GUID";
                row["LeftValue1"] = leftObjd.GridGuid;
                row["RightValue1"] = rightObjd.GridGuid;
                dataResCompare.Append(row);
            }

            if (!excludeSame || leftObjd.Type != rightObjd.Type)
            {
                row = dataResCompare.NewRow();
                row["Key"] = "Object Type";
                row["LeftValue1"] = leftObjd.Type;
                row["RightValue1"] = rightObjd.Type;
                dataResCompare.Append(row);
            }

            for (ObjdIndex index = ObjdIndex.Version1; index <= ObjdIndex.Requirements; ++index)
            {
                if (!excludeSame || leftObjd.GetRawData(index) != rightObjd.GetRawData(index))
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

            Objf leftObjf = (Objf)leftPackage.GetResourceByKey(nodeData.Key);
            Objf rightObjf = (Objf)rightPackage.GetResourceByKey(nodeData.Key);

            for (ObjfIndex index = ObjfIndex.init; index <= ObjfIndex.extractObjectInfoFromInvToken; ++index)
            {
                if (!excludeSame || (GetGuardian(leftObjf, index) != GetGuardian(rightObjf, index) || GetAction(leftObjf, index) != GetAction(rightObjf, index)))
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

            Glob leftGlob = (Glob)leftPackage.GetResourceByKey(nodeData.Key);
            Glob rightGlob = (Glob)rightPackage.GetResourceByKey(nodeData.Key);

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

            Nref leftNref = (Nref)leftPackage.GetResourceByKey(nodeData.Key);
            Nref rightNref = (Nref)rightPackage.GetResourceByKey(nodeData.Key);

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

            Cpf leftCpf = (Cpf)leftPackage.GetResourceByKey(nodeData.Key);
            Cpf rightCpf = (Cpf)rightPackage.GetResourceByKey(nodeData.Key);

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

            leftRes = leftPackage.GetResourceByKey(nodeData.Key);
            rightRes = rightPackage.GetResourceByKey(nodeData.Key);

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

            leftRes = leftPackage.GetResourceByKey(nodeData.Key);
            rightRes = rightPackage.GetResourceByKey(nodeData.Key);

            CMaterialDefinition leftMatDef = ((Txmt)leftRes).MaterialDefinition;
            CMaterialDefinition rightMatDef = ((Txmt)rightRes).MaterialDefinition;

            comboVariations.Items.Clear();

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

            DbpfNodeState defState = (leftMatDef.Version != rightMatDef.Version ||
                                      !leftMatDef.MaterialType.Equals(rightMatDef.MaterialType) ||
                                      !leftMatDef.FileDescription.Equals(rightMatDef.FileDescription)) ? DbpfNodeState.Different : DbpfNodeState.Same;
            comboVariations.Items.Add(new DropDownMaterial(defState, "Definition"));

            comboVariations.SelectedIndex = 0;
        }

        private void PopulateLanguage(MetaData.Languages lang, Str leftStr, Str rightStr)
        {
            gridResCompare.Columns["colKey"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colLeftValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colLeftValue2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colRightValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colRightValue2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            gridResCompare.Columns["colKey"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colLeftValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridResCompare.Columns["colRightValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            nodeData.SetSame();
            this.Close();
        }

        private void OnUseLeft(object sender, EventArgs e)
        {
            nodeData.SetCopyLeftToRight();
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
            for (int i = 0; i < gridResCompare.Columns.Count - 1; i++)
            {
                int colw = gridResCompare.Columns[i].Width;
                gridResCompare.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                gridResCompare.Columns[i].Width = colw;
            }

            if (!gridResCompare.Columns["colRightValue2"].Visible)
            {
                gridResCompare.Columns["colRightValue1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
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

            HighlightRows();
        }

        private void OnCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (nodeData.TypeID == Bhav.TYPE || nodeData.TypeID == Slot.TYPE || nodeData.TypeID == Idr.TYPE)
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
}
