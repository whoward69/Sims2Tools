﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using System;

namespace Sims2Tools.DBPF.CPF
{
    public abstract class Cpf : DBPFResource, IDisposable
    {
        private CpfItem[] items;

        public CpfItem[] Items
        {
            get => items;
        }

        public Cpf(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            items = new CpfItem[0];

            Unserialize(reader);
        }

        protected void Unserialize(IoBuffer reader)
        {
            reader.ReadBytes(0x06);
            items = new CpfItem[reader.ReadUInt32()];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new CpfItem();
                items[i].Unserialize(reader);
            }
        }

        public void Dispose()
        {
            if (items != null)
            {
                for (int i = items.Length - 1; i >= 0; i--)
                    if (items[i] != null)
                        items[i].Dispose();
            }

            items = new CpfItem[0];
            items = null;
        }
    }
}