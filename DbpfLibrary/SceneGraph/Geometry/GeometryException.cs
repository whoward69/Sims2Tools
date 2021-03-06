﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    public class GeometryException : Exception
    {
        /// <summary>
        /// Create a new Instance
        /// </summary>
        /// <param name="message">The Message that should be displayed</param>
        public GeometryException(string message) : base(message)
        {

        }
    }
}
