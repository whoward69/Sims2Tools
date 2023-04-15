using Saxon.Api;
using Sims2Tools;
using Sims2Tools.DBPF;
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

        private readonly Uri baseUri;

        // See https://www.saxonica.com/html/documentation10/dotnetdoc/Saxon/Api/Processor.html
        private readonly Processor processor;
        readonly Dictionary<QName, XdmValue> xsltParams = new Dictionary<QName, XdmValue>(2);
        readonly XsltCompiler compiler;

        public HoodExporterTransformer(String exportPath)
        {
            baseUri = new Uri($"file:////{exportPath}");

            processor = new Processor();
            // String saxonEd = processor.Edition;

            processor.RegisterExtensionFunction(new AsBinaryDefn());
            processor.RegisterExtensionFunction(new AsIntDefn());
            processor.RegisterExtensionFunction(new AsHexDefn());
            processor.RegisterExtensionFunction(new AsHexNoPrefixDefn());
            processor.RegisterExtensionFunction(new AsYesNoDefn());
            processor.RegisterExtensionFunction(new AsObjectNameDefn());

            compiler = processor.NewXsltCompiler();
            compiler.BaseUri = baseUri;
        }

        public XmlElement Transform(XmlElement rootElement, string xslPath)
        {
            DocumentBuilder builder = processor.NewDocumentBuilder();
            XdmNode input = builder.Build(new XmlNodeReader(rootElement));

            DomDestination serializer = new DomDestination();

            Xslt30Transformer transformer30 = compiler.Compile(new XmlTextReader(File.OpenRead(xslPath))).Load30();
            transformer30.SetStylesheetParameters(xsltParams);
            transformer30.BaseOutputURI = baseUri.ToString();

            transformer30.ResultDocumentHandler = this;
            transformer30.ApplyTemplates(input, serializer);

            return serializer.XmlDocument?.DocumentElement;
        }

        public XmlDestination HandleResultDocument(string href, Uri baseUri)
        {
            return processor.NewSerializer(new FileStream($"{baseUri.AbsolutePath}/{href}", FileMode.Create, FileAccess.Write));
        }
    }

    abstract class AsStringCastDefn : ExtensionFunctionDefinition
    {
        private readonly String name;
        private readonly QName resultType;

        public AsStringCastDefn(String name, QName resultType)
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
                String str = (String)arg.Value;

                try
                {
                    UInt32 result;

                    if (str.StartsWith("0x"))
                    {
                        result = UInt32.Parse(str.Substring(2), NumberStyles.HexNumber);
                    }
                    else
                    {
                        result = UInt32.Parse(str);
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
        private readonly String name;
        private readonly QName resultType;

        public AsIntCastDefn(String name, QName resultType)
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
            String hex = $"00000000{Helper.Hex8String((uint)value)}";

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
            String hex = $"00000000{Helper.Hex8String((uint)value)}";

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
            String bin = $"0000000000000000{Helper.Binary16String((uint)value)}";

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
                String str = "No";

                if (arguments[0].MoveNext())
                {
                    str = (String)((XdmAtomicValue)arguments[0].Current).Value;
                }

                long digits = 0;
                if (arguments.Length > 1 && arguments[1].MoveNext())
                {
                    digits = (long)((XdmAtomicValue)arguments[1].Current).Value;
                }

                String result;

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
                        uint num = UInt32.Parse(str, NumberStyles.HexNumber);

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

    class AsObjectNameDefn : ExtensionFunctionDefinition
    {
        public override QName FunctionName => new QName(HoodExporterTransformer.extnUri, "asObjectName");
        public override int MinimumNumberOfArguments => 1;
        public override int MaximumNumberOfArguments => 1;
        public override XdmSequenceType[] ArgumentTypes => new XdmSequenceType[] { new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ONE) };
        public override bool TrustResultType => true;
        public override XdmSequenceType ResultType(XdmSequenceType[] ArgumentTypes) => new XdmSequenceType(XdmAtomicType.BuiltInAtomicType(QName.XS_STRING), XdmSequenceType.ONE);
        public override ExtensionFunctionCall MakeFunctionCall() => new AsObjectNameCall();
    }

    class AsObjectNameCall : ExtensionFunctionCall
    {
        public override IEnumerator<XdmItem> Call(IEnumerator<XdmItem>[] arguments, DynamicContext context)
        {
            if (arguments.Length == 1)
            {
                uint guid = 0;

                if (arguments[0].MoveNext())
                {
                    string str = (String)((XdmAtomicValue)arguments[0].Current).Value;

                    try
                    {
                        if (str.StartsWith("0x"))
                        {
                            guid = UInt32.Parse(str.Substring(2), NumberStyles.HexNumber);
                        }
                        else
                        {
                            guid = UInt32.Parse(str);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                TypeGUID GUID = (TypeGUID)guid;

                if (GameData.globalObjectsByGUID.ContainsKey(GUID))
                {
                    return new XdmAtomicValue(GameData.globalObjectsByGUID[GUID]).GetEnumerator();
                }

                return new XdmAtomicValue(GUID.ToString()).GetEnumerator();
            }

            return EmptyEnumerator<XdmItem>.INSTANCE;
        }
    }
}
