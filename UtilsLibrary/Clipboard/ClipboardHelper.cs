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

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Sims2Tools.Clipboard
{
    public class ClipboardHelper
    {
        public static bool ContainsFileList => System.Windows.Clipboard.ContainsFileDropList();
        public static StringCollection FileList => System.Windows.Clipboard.GetFileDropList();


        internal static readonly string CollItemsListLabel = "WH_CollItemList";
        public static bool ContainsCollItems(DBPFKey targetCollKey)
        {
            if (System.Windows.Clipboard.ContainsData(CollItemsListLabel))
            {
                TransferData clipTransferData = (TransferData)System.Windows.Clipboard.GetData(ClipboardHelper.CollItemsListLabel);

                return !targetCollKey.Equals(clipTransferData.CollectionKey);
            }

            return false;
        }
        public static List<DBPFKey> CollItems
        {
            get
            {
                ClipboardCollListItems clipCollListItems = new ClipboardCollListItems();
                clipCollListItems.ReadFromClipboard();

                return clipCollListItems.CollItems;
            }
        }
    }

    public abstract class AbstractCollListItems
    {
        public DBPFKey CollKey { get; protected set; }
        protected readonly SortedDictionary<int, DBPFKey> sortedItems = new SortedDictionary<int, DBPFKey>();

        public List<DBPFKey> CollItems
        {
            get
            {
                List<DBPFKey> items = new List<DBPFKey>();

                foreach (DBPFKey item in sortedItems.Values)
                {
                    items.Add(item);
                }

                return items;
            }
        }

        public void AddItem(int index, TypeTypeID type, uint subType, uint value)
        {
            sortedItems.Add(index, new DBPFKey(type, (TypeGroupID)subType, (TypeInstanceID)value, DBPFData.RESOURCE_NULL));
        }

        public void AddItem(int index, IDBPFKey item)
        {
            sortedItems.Add(index, new DBPFKey(item));
        }

    }

    public class ClipboardCollListItems : AbstractCollListItems
    {
        public ClipboardCollListItems() : this(new DBPFKey(Coll.TYPE, DBPFData.GROUP_NULL, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL))
        {
        }

        public ClipboardCollListItems(DBPFKey collKey)
        {
            CollKey = collKey;
        }

        public void PlaceOnClipboard(bool availableAfterClose)
        {
            TransferData clipTransferData = new TransferData(CollKey);

            foreach (DBPFKey item in sortedItems.Values)
            {
                clipTransferData.Add(item);
            }

            System.Windows.Clipboard.SetData(ClipboardHelper.CollItemsListLabel, clipTransferData);

            if (availableAfterClose) System.Windows.Clipboard.Flush();
        }

        public void ReadFromClipboard()
        {
            TransferData clipTransferData = (TransferData)System.Windows.Clipboard.GetData(ClipboardHelper.CollItemsListLabel);

            CollKey = clipTransferData.CollectionKey;

            int index = 0;
            foreach (DBPFKey item in clipTransferData.CollectionItems())
            {
                AddItem(index++, item);
            }
        }
    }

    public class DropCollListItems : AbstractCollListItems
    {
        public DropCollListItems() : this(new DBPFKey(Coll.TYPE, DBPFData.GROUP_NULL, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL))
        { 
        }

        public DropCollListItems(DBPFKey collKey)
        {
            CollKey = collKey;
        }

        public DropCollListItems(System.Windows.Forms.IDataObject dataObject)
        {
            TransferData dropTransferData = DragDropHelper.GetObjectData<TransferData> (dataObject, DragDropHelper.DragItemListLabel);

            CollKey = dropTransferData.CollectionKey;

            int index = 0;
            foreach (DBPFKey item in dropTransferData.CollectionItems())
            {
                sortedItems.Add(index++, item);
            }
        }

        public System.Windows.Forms.IDataObject GetDragData()
        {
            TransferData dropTransferData = new TransferData(CollKey);

            foreach (DBPFKey item in sortedItems.Values)
            {
                dropTransferData.Add(item);
            }

            return DragDropHelper.SetObjectData<TransferData>(dropTransferData, DragDropHelper.DragItemListLabel);
        }
    }

    [Serializable]
    public class TransferData
    {
        public uint CollKeyT { get; set; }
        public uint CollKeyG { get; set; }
        public uint CollKeyI { get; set; }
        public uint CollKeyR { get; set; }

        private List<uint[]> collItemsRaw = new List<uint[]>();
        public List<uint[]> CollItemsRaw { get => collItemsRaw; set => collItemsRaw = value; }

        [NonSerialized()] private DBPFKey collKey = null;
        [NonSerialized()] private List<DBPFKey> collItems = null;

        public TransferData()
        {
        }

        public TransferData(DBPFKey collKey)
        {
            CollKeyT = collKey.TypeID.AsUInt();
            CollKeyG = collKey.GroupID.AsUInt();
            CollKeyI = collKey.InstanceID.AsUInt();
            CollKeyR = collKey.ResourceID.AsUInt();
        }

        public DBPFKey CollectionKey
        {
            get
            {
                if (collKey == null)
                {
                    collKey = new DBPFKey((TypeTypeID)CollKeyT, (TypeGroupID)CollKeyG, (TypeInstanceID)CollKeyI, (TypeResourceID)CollKeyR);
                }

                return collKey;
            }
        }

        public List<DBPFKey> CollectionItems()
        {
            if (collItems == null)
            {
                collItems = new List<DBPFKey>();

                foreach (uint[] rawItem in CollItemsRaw)
                {
                    collItems.Add(new DBPFKey((TypeTypeID)rawItem[0], (TypeGroupID)rawItem[1], (TypeInstanceID)rawItem[2], (TypeResourceID)rawItem[3]));
                }
            }

            return collItems;
        }

        public void Add(DBPFKey item)
        {
            collItemsRaw.Add(new uint[] { item.TypeID.AsUInt(), item.GroupID.AsUInt(), item.InstanceID.AsUInt(), item.ResourceID.AsUInt() });
            collItems = null;
        }
    }
}
