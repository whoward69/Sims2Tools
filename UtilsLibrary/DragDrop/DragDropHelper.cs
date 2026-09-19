/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

// For your own sanity, do not use either of the following
// using System.Windows;
// using System.Windows.Forms;

using System.IO;
using System.Xml.Serialization;

namespace Sims2Tools.DragDrop
{
    public class DragDropHelper
    {
        public static readonly string DragItemsLabel = "WH_DragItems";
        public static readonly string DragItemListLabel = "WH_DragItemList";

        public static bool ContainsDragItemList(System.Windows.Forms.IDataObject dataObject)
        {
            string dataType = (string)dataObject.GetData(DragItemsLabel);

            return (dataType != null && dataType.Equals(DragDropHelper.DragItemListLabel));
        }

        internal static System.Windows.Forms.IDataObject SetObjectData<T>(object value, string format) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T), "");

                    serializer.Serialize(sw, value);
                    sw.Flush();

                    System.Windows.Forms.DataObject data = new System.Windows.Forms.DataObject(format, ms.ToArray());
                    data.SetData(DragItemsLabel, format);

                    return data;
                }
            }
        }

        internal static T GetObjectData<T>(System.Windows.Forms.IDataObject data, string format) where T : class
        {
            using (MemoryStream ms = new MemoryStream(data.GetData(format) as byte[]))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
