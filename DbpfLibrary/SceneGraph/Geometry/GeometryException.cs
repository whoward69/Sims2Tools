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
