using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    /// <summary>
        /// Class Node is used for saving node status and tiles.
        /// <list type="Properties">
            /// <item>
                /// <description>Status is an int for describing the tile texture.</description>
            /// </item>
            /// <item>
                /// <description>StatusSebenarnya is used for describing the tile availability.</description>
            /// </item>
        /// </list>
    /// </summary>
    class Node
    {
        int status; //gambar
        int statussebenarnya; //0 kosong, 1 isi

        /// <summary>
            /// Getter and Setter for element status.
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
            /// Getter and Setter for element statussebenarnya.
        /// </summary>
        public int StatusSebenarnya
        {
            get { return statussebenarnya; }
            set { statussebenarnya = value; }
        }

        /// <summary>
            /// Constructor for Node Class.
        /// </summary>
        public Node()
        {
        }

        /// <summary>
            /// Constructor for Node Class.
        /// </summary>
        /// <param name="n">Node to be copied.</param>
        public Node(Node n)
        {
            status = n.status;
            statussebenarnya = n.statussebenarnya;
        }
    }
}