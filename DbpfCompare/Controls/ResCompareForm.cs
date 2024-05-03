﻿/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
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
using System.Drawing;
using System.Windows.Forms;

namespace DbpfCompare.Controls
{
    public partial class ResCompareForm : Form
    {
        private static readonly Color colourDiffers = Color.FromName(Properties.Settings.Default.CompareDiffers);

        private static readonly Brush brushDiffers = new SolidBrush(Color.FromName(Properties.Settings.Default.ResDiffers));
        private static readonly Brush brushMissing = new SolidBrush(Color.FromName(Properties.Settings.Default.ResMissing));
        private static readonly Brush brushSame = new SolidBrush(Color.FromName(Properties.Settings.Default.ResSame));

        private readonly ResCompareData dataResCompare = new ResCompareData();

        private readonly DbpfCompareNodeResourceData nodeData;
        private readonly string leftPackagePath, rightPackagePath;

        private DBPFResource leftRes, rightRes;

        public ResCompareForm(DbpfCompareNodeResourceData nodeData, string leftPackagePath, string rightPackagePath)
        {
            InitializeComponent();

            this.nodeData = nodeData;
            this.leftPackagePath = leftPackagePath;
            this.rightPackagePath = rightPackagePath;

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
            else if (nodeData.TypeID == Trcn.TYPE)
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
            else if (nodeData.TypeID == Tprp.TYPE)
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
            else if (nodeData.TypeID == Objd.TYPE)
            {
                // OBJD - Table; Index, Left Value, Right Value
                gridResCompare.Columns["colKey"].HeaderText = "Name";
                gridResCompare.Columns["colLeftValue1"].HeaderText = "Left Value";
                gridResCompare.Columns["colLeftValue2"].Visible = false;
                gridResCompare.Columns["colRightValue1"].HeaderText = "Right Value";
                gridResCompare.Columns["colRightValue2"].Visible = false;

                Objd leftObjd = (Objd)leftPackage.GetResourceByKey(nodeData.Key);
                Objd rightObjd = (Objd)rightPackage.GetResourceByKey(nodeData.Key);

                DataRow row = dataResCompare.NewRow();
                row["Key"] = "GUID";
                row["LeftValue1"] = leftObjd.Guid;
                row["RightValue1"] = rightObjd.Guid;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Original GUID";
                row["LeftValue1"] = leftObjd.OriginalGuid;
                row["RightValue1"] = rightObjd.OriginalGuid;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Fallback GUID";
                row["LeftValue1"] = leftObjd.ProxyGuid;
                row["RightValue1"] = rightObjd.ProxyGuid;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Diagonal GUID";
                row["LeftValue1"] = leftObjd.DiagonalGuid;
                row["RightValue1"] = rightObjd.DiagonalGuid;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Grid Align GUID";
                row["LeftValue1"] = leftObjd.GridGuid;
                row["RightValue1"] = rightObjd.GridGuid;
                dataResCompare.Append(row);

                row = dataResCompare.NewRow();
                row["Key"] = "Object Type";
                row["LeftValue1"] = leftObjd.Type;
                row["RightValue1"] = rightObjd.Type;
                dataResCompare.Append(row);

                for (ObjdIndex index = ObjdIndex.Version1; index <= ObjdIndex.Requirements; ++index)
                {
                    row = dataResCompare.NewRow();
                    row["Key"] = index.ToString();

                    row["LeftValue1"] = leftObjd.GetRawData(index);
                    row["RightValue1"] = rightObjd.GetRawData(index);

                    dataResCompare.Append(row);
                }
            }
            else if (nodeData.TypeID == Objf.TYPE)
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
                    DataRow row = dataResCompare.NewRow();
                    row["Key"] = index.ToString();

                    row["LeftValue1"] = GetGuardian(leftObjf, index);
                    row["LeftValue2"] = GetAction(leftObjf, index);
                    row["RightValue1"] = GetGuardian(rightObjf, index);
                    row["RightValue2"] = GetAction(rightObjf, index);

                    dataResCompare.Append(row);
                }
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
            else if (nodeData.TypeID == Str.TYPE || nodeData.TypeID == Ctss.TYPE || nodeData.TypeID == Ttas.TYPE)
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

                List<StrLanguage> leftLanguages = leftStr.Languages;
                List<StrLanguage> rightLanguages = rightStr.Languages;

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
            else if (nodeData.TypeID == Txmt.TYPE)
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

            leftPackage?.Close();
            rightPackage?.Close();

            HighlightRows();
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
                List<string> leftNames = leftTxmt.MaterialDefinition.GetPropertyNames();
                List<string> rightNames = rightTxmt.MaterialDefinition.GetPropertyNames();

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
            foreach (DataGridViewRow row in gridResCompare.Rows)
            {
                string leftValue1 = row.Cells["colLeftValue1"].Value as string;
                string leftValue2 = row.Cells["colLeftValue2"].Value as string;
                string rightValue1 = row.Cells["colRightValue1"].Value as string;
                string rightValue2 = row.Cells["colRightValue2"].Value as string;

                if (!leftValue1.Equals(rightValue1) || (leftValue2 != null && !leftValue2.Equals(rightValue2)))
                {
                    row.DefaultCellStyle.BackColor = colourDiffers;
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
                    gridResCompare.DefaultCellStyle.SelectionBackColor = colourDiffers;
                }
                else
                {
                    if (row.Cells["colLeftValue2"].Value is string leftValue2 && !leftValue2.Equals(row.Cells["colRightValue2"].Value as string))
                    {
                        gridResCompare.DefaultCellStyle.SelectionBackColor = colourDiffers;
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