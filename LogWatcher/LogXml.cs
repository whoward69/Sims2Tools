/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace LogWatcher
{
    public class LogXml
    {
        private static readonly Dictionary<TypeGUID, string> knownTokens = new Dictionary<TypeGUID, string>();
        private static readonly Dictionary<TypeGUID, string> customTokens = new Dictionary<TypeGUID, string>();

        static LogXml()
        {
            ParseXml("Resources/XML/knowntokens.xml", "token", knownTokens);
            ParseXml("Resources/XML/customtokens.xml", "token", customTokens);
        }

        private static void ParseXml(string xml, string element, Dictionary<TypeGUID, string> byValue)
        {
            if (!File.Exists(xml)) return;

            XmlReader reader = XmlReader.Create(xml);

            TypeGUID guid = DBPFData.GUID_NULL;
            string name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        guid = (TypeGUID)uint.Parse(reader.Value.Substring(2), NumberStyles.HexNumber);
                    }
                    else if (reader.Name.Equals("name"))
                    {
                        reader.Read();
                        name = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(element))
                {
                    if (!byValue.ContainsKey(guid))
                    {
                        byValue.Add(guid, name);
                    }
                }
            }
        }

        private readonly XmlDocument logDoc;

        private readonly Regex reAttributes = new Regex("^([0-9]+), (.+) = (-?[0-9]+)$");
        private readonly Regex reArrayEntry = new Regex("^([0-9]+) = (-?[0-9]+)$");

        private readonly Regex reTokenGuid = new Regex("^Token GUID (-?[0-9]+)$");
        private readonly Regex reTokenCounted = new Regex("^Token ((Flags|(Count|Category) :) [0-9]+)$");
        private readonly Regex reTokenSingular = new Regex("^Token (Flags -?[0-9]+)$");
        private readonly Regex reTokenProperty = new Regex("^Token (Property [0-9]+: -?[0-9]+)$");

        private readonly Regex reLotObject = new Regex("^Object Name : (.+) id : ([0-9]+)(	Contained within object id	([0-9]+)	in slot	([0-9]+))?	Room: (-?[0-9]+)$");
        private readonly Regex reCheats = new Regex("^([^ ]+) += (.*)$");

        public XmlElement Root => logDoc.DocumentElement;
        public XmlElement Header => (XmlElement)logDoc.DocumentElement.FirstChild;

        public LogXml(string logFile)
        {
            bool inHeader = false;
            bool inTopFrame = false;
            bool inData = false;
            bool inAttrs = false;
            bool inArray = false;
            bool inLotObjects = false;
            bool inCheats = false;

            string myOid = null;
            string myName = null;
            string soOid = null;
            string soName = null;

            logDoc = new XmlDocument();
            logDoc.LoadXml("<?xml version='1.0' ?><log/>");
            XmlElement logRoot = logDoc.DocumentElement;

            XmlElement parent = MakeElement(logRoot, "header", "Header");
            XmlElement owner = parent;

            // Some magic to allow us to read the ObjectError file before the player clicks Reset
            using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Dual using may be overkill here
                using (StreamReader sr = new StreamReader(fs))
                {
                    inHeader = true;

                    try
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Length != 0)
                            {
                                if (line.StartsWith("Global Simulator Variables"))
                                {
                                    parent = MakeElement(logRoot, "globals", "Globals");
                                    owner = parent;

                                    inData = true;

                                    continue;
                                }
                                else if (line.StartsWith("Other Simulator Details:"))
                                {
                                    parent = MakeElement(logRoot, "details", "Details");
                                    owner = parent;

                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("My Object:"))
                                {
                                    parent = MakeElement(logRoot, "me", "MY", myName, myOid);
                                    owner = parent;

                                    inAttrs = false;

                                    continue;
                                }
                                else if (line.StartsWith("Current Stack Object:"))
                                {
                                    parent = MakeElement(logRoot, "so", "SO", soName, soOid);
                                    owner = parent;

                                    inAttrs = false;

                                    continue;
                                }
                                else if (line.StartsWith("Inventory contents:"))
                                {
                                    parent = MakeElement(logRoot, "inventory", "Inventory");
                                    owner = parent;

                                    inAttrs = false;

                                    continue;
                                }
                                else if (line.StartsWith("Lot Object Dump"))
                                {
                                    parent = MakeElement(logRoot, "lotObjects", "Lot Objects");
                                    owner = parent;

                                    inLotObjects = true;

                                    continue;
                                }
                                else if (line.StartsWith("Dumping all current cheats"))
                                {
                                    parent = MakeElement(logRoot, "cheats", "Cheats");
                                    owner = parent;

                                    inLotObjects = false;
                                    inCheats = true;

                                    continue;
                                }
                                else if (line.StartsWith("Person Data for"))
                                {
                                    owner = MakeElement(parent, "person", "Person Data");
                                    inData = true;

                                    continue;
                                }
                                else if (line.StartsWith("Motives:"))
                                {
                                    owner = MakeElement(parent, "motives", "Motive Data");
                                    inData = true;

                                    continue;
                                }
                                else if (line.StartsWith("Contained Objects:"))
                                {
                                    owner = MakeElement(parent, "slots", "Slots");

                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Data:"))
                                {
                                    owner = MakeElement(parent, "general", "Object Data");
                                    inData = true;

                                    continue;
                                }
                                else if (line.StartsWith("Attributes:"))
                                {
                                    owner = MakeElement(parent, "attrs", "Attributes");
                                    inAttrs = true;

                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Semi Global Attributes:"))
                                {
                                    owner = MakeElement(parent, "semis", "Semi-Attributes");
                                    inAttrs = true;

                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Arrays:"))
                                {
                                    owner = MakeElement(parent, "arrays", "Arrays");
                                    inArray = true;

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Temps:"))
                                {
                                    owner = MakeElement(parent, "temps", "Temps");
                                    inArray = false;

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Object Data Tables:"))
                                {
                                    owner = MakeElement(parent, "objectTables", "Object Tables");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Neighbor Data Tables:"))
                                {
                                    owner = MakeElement(parent, "neighbourTables", "Neighbour Tables");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Global Inventory"))
                                {
                                    owner = MakeElement(parent, "invGlobal", "Global");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Lot Inventory"))
                                {
                                    owner = MakeElement(parent, "invLot", "Lot");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Family Inventory"))
                                {
                                    owner = MakeElement(parent, "invFamily", "Family");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }
                                else if (line.StartsWith("Personal Inventory"))
                                {
                                    owner = MakeElement(parent, "invPerson", "Personal");

                                    inAttrs = false;
                                    inData = false;

                                    continue;
                                }

                                if (inHeader && line.StartsWith("Object id: "))
                                {
                                    myOid = line.Substring(11);
                                }
                                else if (inHeader && line.StartsWith("name: "))
                                {
                                    myName = line.Substring(6);
                                }
                                else if (inTopFrame && line.StartsWith("    Stack Object id: "))
                                {
                                    soOid = line.Substring(21);
                                }
                                else if (inTopFrame && line.StartsWith("    Stack Object name: "))
                                {
                                    soName = line.Substring(23);
                                }

                                if (inData)
                                {
                                    int pos = line.IndexOf("=");

                                    if (pos != -1)
                                    {
                                        XmlElement attr = MakeElement(owner, "data", "Data");
                                        attr.SetAttribute("value", line.Substring(pos + 2));
                                    }
                                }
                                else if (inAttrs)
                                {
                                    Match m = reAttributes.Match(line);

                                    if (m.Success)
                                    {
                                        XmlElement attr = MakeElement(owner, "attr", "Attribute");
                                        attr.SetAttribute("index", m.Groups[1].Value);
                                        attr.SetAttribute("key", m.Groups[2].Value);
                                        attr.SetAttribute("value", m.Groups[3].Value);
                                    }
                                }
                                else if (inLotObjects)
                                {
                                    Match m = reLotObject.Match(line);

                                    if (m.Success)
                                    {
                                        XmlElement lotObj = MakeElement(owner, "lotobj", "Lot Object");
                                        lotObj.SetAttribute("object", m.Groups[1].Value);
                                        lotObj.SetAttribute("oid", m.Groups[2].Value);

                                        lotObj.SetAttribute("container", m.Groups[4].Value);
                                        lotObj.SetAttribute("slot", m.Groups[5].Value);

                                        lotObj.SetAttribute("room", m.Groups[6].Value);
                                    }
                                }
                                else if (inCheats)
                                {
                                    Match m = reCheats.Match(line);

                                    if (m.Success)
                                    {
                                        XmlElement lotObj = MakeElement(owner, "cheat", "Cheat");
                                        lotObj.SetAttribute("key", m.Groups[1].Value);
                                        lotObj.SetAttribute("value", m.Groups[2].Value);
                                    }
                                }
                                else
                                {
                                    string eleType = "line";
                                    string colour = null;

                                    if (inArray && reArrayEntry.IsMatch(line))
                                    {
                                        line = "\t" + line;
                                    }
                                    else if (reTokenGuid.IsMatch(line))
                                    {
                                        eleType = "tokenGuid";

                                        TypeGUID guid = (TypeGUID)(uint)int.Parse(line.Substring(11));
                                        line = guid.ToString();

                                        if (customTokens.ContainsKey(guid))
                                        {
                                            line += " : " + customTokens[guid];
                                            colour = Properties.Settings.Default.CustomTokensColour;
                                        }
                                        else if (knownTokens.ContainsKey(guid))
                                        {
                                            line += " : " + knownTokens[guid];
                                            colour = Properties.Settings.Default.KnownTokensColour;
                                        }
                                        else if (GameData.globalObjectsByGUID.ContainsKey(guid))
                                        {
                                            line += " : " + GameData.globalObjectsByGUID[guid];
                                            colour = Properties.Settings.Default.GameTokensColour;
                                        }
                                    }
                                    else if (reTokenProperty.IsMatch(line))
                                    {
                                        eleType = "tokenProp";

                                        line = line.Substring(6);
                                    }
                                    else if (reTokenCounted.IsMatch(line) || reTokenSingular.IsMatch(line))
                                    {
                                        line = "\t" + line.Substring(6);
                                    }

                                    XmlElement logLine = logDoc.CreateElement(eleType);
                                    logLine.AppendChild(logDoc.CreateTextNode(line));
                                    owner.AppendChild(logLine);

                                    if (colour != null) logLine.SetAttribute("colour", colour);
                                }

                                if (line.StartsWith("Iterations:"))
                                {
                                    parent = MakeElement(logRoot, "frame", "Frame");
                                    owner = parent;
                                }

                                if (line.StartsWith("  Frame "))
                                {
                                    if (inHeader)
                                    {
                                        inHeader = false;
                                        inTopFrame = true;
                                    }
                                    else
                                    {
                                        inTopFrame = false;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private XmlElement MakeElement(XmlElement parent, string eleName, string name)
        {
            return MakeElement(parent, eleName, name, null, null);
        }

        private XmlElement MakeElement(XmlElement parent, string eleName, string name, string objName, string objId)
        {
            XmlElement ele = logDoc.CreateElement(eleName);

            if (objName != null)
            {
                ele.SetAttribute("name", $"{name}: {objName}");
            }
            else if (objId != null)
            {
                ele.SetAttribute("name", $"{name}: ID={objId}");
            }
            else
            {
                ele.SetAttribute("name", name);
            }

            parent.AppendChild(ele);

            return ele;
        }
    }
}
