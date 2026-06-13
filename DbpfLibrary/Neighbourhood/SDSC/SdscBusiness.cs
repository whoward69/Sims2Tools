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
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal class SdscBusiness : SdscData
    {
        internal SdscBusiness() : base() { }
        internal SdscBusiness(ushort[] data) : base(data) { }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("lotId", Helper.Hex8PrefixString(data[(int)SdscIndex.OwnableLotJobLotID]));
                parent.SetAttribute("salary", data[(int)SdscIndex.OwnableLotJobSalary].ToString());
                parent.SetAttribute("flags", data[(int)SdscIndex.OwnableLotJobFlags].ToString());
                parent.SetAttribute("assignment", ((JobAssignment)(data[(int)SdscIndex.OwnableLotJobAssignmentID])).ToString());
            }
        }
    }
}
