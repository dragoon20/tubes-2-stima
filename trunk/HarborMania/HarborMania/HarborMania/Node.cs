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
        /// Class Node is used for saving node status, tiles, position and size.
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

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int StatusSebenarnya
        {
            get { return statussebenarnya; }
            set { statussebenarnya = value; }
        }

        public Node()
        {
        }

        public Node(Node n)
        {
            status = n.status;
            statussebenarnya = n.statussebenarnya;
        }
    }
}