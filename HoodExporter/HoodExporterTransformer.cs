/*
 * Hood Exporter - a utility for exporting a Sims 2 'hood as XML
 *               - see http://www.picknmixmods.com/Sims2/Notes/HoodExporter/HoodExporter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Saxon.Api;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace HoodExporter
{
    public class HoodExporterTransformer : IResultDocumentHandler
    {
        public static string extnUri = "http://picknmixmods.com/Sims2Tools/SaxonExtns";

        // See https://www.saxonica.com/html/documentation10/dotnetdoc/Saxon/Api/Processor.html
        private readonly Processor processor;
        private readonly Dictionary<QName, XdmValue> xsltParams = new Dictionary<QName, XdmValue>(2);
        private readonly XsltCompiler compiler;

        private List<Serializer> resultDocs;

        private static DBPFFile objectsPackage = null;

        public static void Shutdown()
        {
            objectsPackage?.Close();
        }

        public HoodExporterTransformer(string exportPath)
        {
            processor = new Processor();
            // string saxonEd = processor.Edition;

            processor.RegisterExtensionFunction(new AsBinaryDefn());
            processor.RegisterExtensionFunction(new AsIntDefn());
            processor.RegisterExtensionFunction(new AsHexDefn());
            processor.RegisterExtensionFunction(new AsHexNoPrefixDefn());
            processor.RegisterExtensionFunction(new AsYesNoDefn());
            processor.RegisterExtensionFunction(new AsObjectNameDefn());
            processor.RegisterExtensionFunction(new AsObjectTitleDefn());
            processor.RegisterExtensionFunction(new AsObjectDescDefn());

            compiler = processor.NewXsltCompiler();
            compiler.BaseUri = new Uri($"file:////{exportPath}");
        }

        public XmlElement Transform(XmlElement rootElement, string xslPath)
        {
            DocumentBuilder builder = processor.NewDocumentBuilder();
            XdmNode input = builder.Build(new XmlNodeReader(rootElement));

            DomDestination output = new DomDestination();

            resultDocs = new List<Serializer>();

            Xslt30Transformer transformer30 = compiler.Compile(new XmlTextReader(File.OpenRead(xslPath))).Load30();
            transformer30.SetStylesheetParameters(xsltParams);
            transformer30.BaseOutputURI = compiler.BaseUri.ToString().Replace(" ", "%20");

            transformer30.ResultDocumentHandler = this;
            transformer30.ApplyTemplates(input, output);

            XmlElement ele = output.XmlDocument?.DocumentElement;

            foreach (Serializer resultDoc in resultDocs)
            {
                if (resultDoc.GetOutputDestination() is java.io.Closeable c)
                {
                    c.close();
                }

                resultDoc.Close();
            }

            output.Close();

            return ele;
        }

        public XmlDestination HandleResultDocument(string href, Uri baseUri)
        {
            string fullpath = $"{baseUri.LocalPath}\\{href}";

            Serializer s = processor.NewSerializer(new FileStream(fullpath, FileMode.Create, FileAccess.Write));

            resultDocs.Add(s);

            return s;
        }

        private static StrItemList GetObjectCatalog(TypeGUID guid)
        {
            if (GameData.globalObjectsTgirHashByGUID.ContainsKey(guid))
            {
                if (objectsPackage == null)
                {
                    objectsPackage = new DBPFFile(Sims2ToolsLib.Sims2Path + GameData.objectsSubPath);
                }

                Objd objd = (Objd)objectsPackage.GetResourceByTGIR(GameData.globalObjectsTgirHashByGUID[guid]);
                Ctss ctss = (Ctss)objectsPackage.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                return ctss?.LanguageItems(MetaData.Languages.Default);
            }

            return null;
        }

        public static string GetObjectTitle(TypeGUID guid)
        {
            return GetObjectCatalog(guid)?[0]?.Title;
        }

        public static string GetObjectDesc(TypeGUID guid)
        {
            return GetObjectCatalog(guid)?[1]?.Title;
        }
    }

    abstract class AsStringCastDefn : ExtensionFunctionDefinition
    {
        private readonly string name;
        private readonly QName resultType;

        public AsStringCastDefn(string name, QName resultType)
        {
            this.name = name;
            this.resultType = resultType;
        }

        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, name);
        public override int MinimumNumberOfArguments => 0;
        public override int MaximumNumberOfArguments => 1;
        public override XdmSequenceType[] ArgumentTypes => new XdmSequenceType[] { new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ZERO_OR_ONE) };
        public override bool TrustResultType => true;
        public override XdmSequenceType ResultType(XdmSequenceType[] ArgumentTypes) => new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(resultType), XdmSequenceType.ONE);
    }

    abstract class AsStringCastCall : ExtensionFunctionCall
    {
        public override IEnumerator<XdmItem> Call(IEnumerator<XdmItem>[] arguments, DynamicContext context)
        {
            if (arguments.Length > 0 && arguments[0].MoveNext())
            {
                XdmAtomicValue arg = (XdmAtomicValue)arguments[0].Current;
                string str = (string)arg.Value;

                try
                {
                    uint result;

                    if (str.StartsWith("0x"))
                    {
                        result = uint.Parse(str.Substring(2), NumberStyles.HexNumber);
                    }
                    else
                    {
                        result = uint.Parse(str);
                    }

                    return GetResult(result).GetEnumerator();
                }
                catch (Exception)
                {
                    return (new XdmAtomicValue(0)).GetEnumerator();
                }
            }
            else
            {
                return EmptyEnumerator<XdmItem>.INSTANCE;
            }
        }

        protected abstract XdmAtomicValue GetResult(uint value);
    }

    abstract class AsIntCastDefn : ExtensionFunctionDefinition
    {
        private readonly string name;
        private readonly QName resultType;

        public AsIntCastDefn(string name, QName resultType)
        {
            this.name = name;
            this.resultType = resultType;
        }

        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, name);
        public override int MinimumNumberOfArguments => 1;
        public override int MaximumNumberOfArguments => 2;
        public override XdmSequenceType[] ArgumentTypes => new XdmSequenceType[] { new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_INTEGER), XdmSequenceType.ONE_OR_MORE) };
        public override bool TrustResultType => true;
        public override XdmSequenceType ResultType(XdmSequenceType[] ArgumentTypes) => new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(resultType), XdmSequenceType.ONE);
    }

    abstract class AsIntCastCall : ExtensionFunctionCall
    {
        public override IEnumerator<XdmItem> Call(IEnumerator<XdmItem>[] arguments, DynamicContext context)
        {
            if (arguments.Length > 0 && arguments[0].MoveNext())
            {
                long val = (long)((XdmAtomicValue)arguments[0].Current).Value;

                long digits = 0;
                if (arguments.Length > 1 && arguments[1].MoveNext())
                {
                    digits = (long)((XdmAtomicValue)arguments[1].Current).Value;
                }

                return GetResult(val, (int)digits).GetEnumerator();
            }
            else
            {
                return EmptyEnumerator<XdmItem>.INSTANCE;
            }
        }

        protected abstract XdmAtomicValue GetResult(long value, int digits);
    }

    class AsIntDefn : AsStringCastDefn
    {
        public AsIntDefn() : base("asInt", QName.XS_INTEGER) { }

        public override ExtensionFunctionCall MakeFunctionCall() => new AsIntCall();
    }

    class AsIntCall : AsStringCastCall
    {
        protected override XdmAtomicValue GetResult(uint value) => new XdmAtomicValue(value);
    }

    class AsHexDefn : AsIntCastDefn
    {
        public AsHexDefn() : base("asHex", QName.XS_STRING) { }

        public override ExtensionFunctionCall MakeFunctionCall() => new AsHexCall();
    }

    class AsHexCall : AsIntCastCall
    {
        protected override XdmAtomicValue GetResult(long value, int digits)
        {
            string hex = $"00000000{Helper.Hex8String((uint)value)}";

            if (digits <= 0) digits = 8;
            if (digits > hex.Length) digits = hex.Length;

            return new XdmAtomicValue($"0x{hex.Substring(hex.Length - digits, digits)}");
        }
    }

    class AsHexNoPrefixDefn : AsIntCastDefn
    {
        public AsHexNoPrefixDefn() : base("asHexNoPrefix", QName.XS_STRING) { }

        public override ExtensionFunctionCall MakeFunctionCall() => new AsHexNoPrefixCall();
    }

    class AsHexNoPrefixCall : AsIntCastCall
    {
        protected override XdmAtomicValue GetResult(long value, int digits)
        {
            string hex = $"00000000{Helper.Hex8String((uint)value)}";

            if (digits <= 0) digits = 8;
            if (digits > hex.Length) digits = hex.Length;

            return new XdmAtomicValue(hex.Substring(hex.Length - digits, digits));
        }
    }

    class AsBinaryDefn : AsIntCastDefn
    {
        public AsBinaryDefn() : base("asBinary", QName.XS_STRING) { }

        public override ExtensionFunctionCall MakeFunctionCall() => new AsBinaryCall();
    }

    class AsBinaryCall : AsIntCastCall
    {
        protected override XdmAtomicValue GetResult(long value, int digits)
        {
            string bin = $"0000000000000000{Helper.Binary16String((uint)value)}";

            if (digits <= 0) digits = 8;
            if (digits > bin.Length) digits = bin.Length;

            return new XdmAtomicValue(bin.Substring(bin.Length - digits, digits));
        }
    }

    class AsYesNoDefn : ExtensionFunctionDefinition
    {
        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, "asYesNo");
        public override int MinimumNumberOfArguments => 0;
        public override int MaximumNumberOfArguments => 2;
        public override XdmSequenceType[] ArgumentTypes => new XdmSequenceType[] { new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ZERO_OR_ONE), new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_INTEGER), XdmSequenceType.ZERO_OR_ONE) };
        public override bool TrustResultType => true;
        public override XdmSequenceType ResultType(XdmSequenceType[] ArgumentTypes) => new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ONE);
        public override ExtensionFunctionCall MakeFunctionCall() => new AsYesNoCall();
    }

    class AsYesNoCall : ExtensionFunctionCall
    {
        public override IEnumerator<XdmItem> Call(IEnumerator<XdmItem>[] arguments, DynamicContext context)
        {
            if (arguments.Length > 0)
            {
                string str = "No";

                if (arguments[0].MoveNext())
                {
                    str = (string)((XdmAtomicValue)arguments[0].Current).Value;
                }

                long digits = 0;
                if (arguments.Length > 1 && arguments[1].MoveNext())
                {
                    digits = (long)((XdmAtomicValue)arguments[1].Current).Value;
                }

                string result;

                if (str.Length == 0 || str.Equals("No", StringComparison.CurrentCultureIgnoreCase) || str.Equals("N", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = "No";
                }
                else if (str.Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || str.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = "Yes";
                }
                else
                {
                    if (str.StartsWith("0x")) str = str.Substring(2);

                    try
                    {
                        uint num = uint.Parse(str, NumberStyles.HexNumber);

                        result = (num == 0) ? "No" : "Yes";
                    }
                    catch (Exception)
                    {
                        result = "Yes";
                    }
                }

                if (digits <= 0) digits = result.Length;
                if (digits > result.Length) digits = result.Length;

                return new XdmAtomicValue(result.Substring(0, (int)digits)).GetEnumerator();
            }
            else
            {
                return EmptyEnumerator<XdmItem>.INSTANCE;
            }
        }
    }

    abstract class AsObjectDefn : ExtensionFunctionDefinition
    {
        public override int MinimumNumberOfArguments => 1;
        public override int MaximumNumberOfArguments => 1;
        public override XdmSequenceType[] ArgumentTypes => new XdmSequenceType[] { new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ONE) };
        public override bool TrustResultType => true;
        public override XdmSequenceType ResultType(XdmSequenceType[] ArgumentTypes) => new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ONE);
    }

    abstract class AsObjectCall : ExtensionFunctionCall
    {
        public abstract string GetValueFromGuid(TypeGUID guid);

        public override IEnumerator<XdmItem> Call(IEnumerator<XdmItem>[] arguments, DynamicContext context)
        {
            if (arguments.Length == 1)
            {
                uint guid = 0;

                if (arguments[0].MoveNext())
                {
                    string str = (string)((XdmAtomicValue)arguments[0].Current).Value;

                    try
                    {
                        if (str.StartsWith("0x"))
                        {
                            guid = uint.Parse(str.Substring(2), NumberStyles.HexNumber);
                        }
                        else
                        {
                            guid = uint.Parse(str);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                TypeGUID GUID = (TypeGUID)guid;

                string value = GetValueFromGuid(GUID);

                if (value != null)
                {
                    return new XdmAtomicValue(value).GetEnumerator();
                }

                return new XdmAtomicValue(GUID.ToString()).GetEnumerator();
            }

            return EmptyEnumerator<XdmItem>.INSTANCE;
        }
    }

    class AsObjectNameDefn : AsObjectDefn
    {
        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, "asObjectName");
        public override ExtensionFunctionCall MakeFunctionCall() => new AsObjectNameCall();
    }

    class AsObjectNameCall : AsObjectCall
    {
        public override string GetValueFromGuid(TypeGUID guid)
        {
            if (GameData.globalObjectsByGUID.ContainsKey(guid))
            {
                return GameData.globalObjectsByGUID[guid];
            }

            return null;
        }
    }

    class AsObjectTitleDefn : AsObjectDefn
    {
        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, "asObjectTitle");
        public override ExtensionFunctionCall MakeFunctionCall() => new AsObjectTitleCall();
    }

    class AsObjectTitleCall : AsObjectCall
    {
        public override string GetValueFromGuid(TypeGUID guid) => HoodExporterTransformer.GetObjectTitle(guid);
    }

    class AsObjectDescDefn : AsObjectDefn
    {
        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, "asObjectDesc");
        public override ExtensionFunctionCall MakeFunctionCall() => new AsObjectDescCall();
    }

    class AsObjectDescCall : AsObjectCall
    {
        public override string GetValueFromGuid(TypeGUID guid) => HoodExporterTransformer.GetObjectDesc(guid);
    }
}
