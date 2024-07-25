/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace Sims2Tools.DBPF.Logger
{
    public interface IDBPFLogger
    {
        void Debug(string msg);
        void Debug(Exception e);
        void Debug(string msg, Exception e);
        void Info(string msg);
        void Info(Exception e);
        void Info(string msg, Exception e);
        void Warn(string msg);
        void Warn(Exception e);
        void Warn(string msg, Exception e);
        void Error(string msg);
        void Error(Exception e);
        void Error(string msg, Exception e);
    }

    public class DBPFLoggerFactory
    {
        public static IDBPFLogger GetLogger(Type type)
        {
            return new DBPFLogger(type);
        }
    }

    public class DBPFLogger : IDBPFLogger
    {
        private readonly log4net.ILog netLogger;

        public DBPFLogger(Type type)
        {
            netLogger = log4net.LogManager.GetLogger(type);
        }

        public void Debug(string msg) => netLogger?.Debug(msg);
        public void Debug(Exception e) => netLogger?.Debug(e);
        public void Debug(string msg, Exception e) => netLogger?.Debug(msg, e);
        public void Info(string msg) => netLogger?.Info(msg);
        public void Info(Exception e) => netLogger?.Info(e);
        public void Info(string msg, Exception e) => netLogger?.Info(msg, e);
        public void Warn(string msg) => netLogger?.Warn(msg);
        public void Warn(Exception e) => netLogger?.Warn(e);
        public void Warn(string msg, Exception e) => netLogger?.Warn(msg, e);
        public void Error(string msg) => netLogger?.Error(msg);
        public void Error(Exception e) => netLogger?.Error(e);
        public void Error(string msg, Exception e) => netLogger?.Error(msg, e);
    }
}
