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

namespace Sims2Tools.DBPF.Logger
{
    public interface IDBPFLogger
    {
        void Debug(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
    }

    public class DBPFLogger : IDBPFLogger
    {
        private readonly log4net.ILog logger;

        public DBPFLogger(log4net.ILog logger)
        {
            this.logger = logger;
        }

        public void Debug(string msg) => logger?.Debug(msg);
        public void Info(string msg) => logger?.Info(msg);
        public void Warn(string msg) => logger?.Warn(msg);
        public void Error(string msg) => logger?.Error(msg);
    }
}
