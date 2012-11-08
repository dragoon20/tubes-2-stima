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
        /// Class Boat is used for saving boat movement.
        /// <list type="Properties">
            /// <item>
                /// <description>Boat is used for saving boat position on list.</description>
            /// </item>
            /// <item>
                /// <description>PosisiAwal is used for saving boat position before movement.</description>
            /// </item>
            /// <item>
                /// <description>PosisiAkhir is used for saving boat position after movement.</description>
            /// </item>
        /// </list>
    /// </summary>
    class BoatPath
    {
        int boat;
        Vector2 posisi_awal;
        Vector2 posisi_akhir;

        /// <summary>
            /// Getter and Setter for element boat yang dipilih.
        /// </summary>
        public int Boat
        {
            get { return boat; }
            set { boat = value; }
        }

        /// <summary>
            /// Getter and Setter for element Posisi Awal.
        /// </summary>
        public Vector2 PosisiAwal 
        {
            get { return posisi_awal; }
            set { posisi_awal = value; }
        }

        /// <summary>
            /// Getter and Setter for element Posisi Akhir.
        /// </summary>
        public Vector2 PosisiAkhir 
        {
            get { return posisi_akhir; } 
            set { posisi_akhir = value; }
        }

        /// <summary>
            /// Constructor untuk BoatPath.
        /// </summary>
        public BoatPath() 
        {
            posisi_awal = new Vector2();
            posisi_akhir = new Vector2();
        }

        /// <summary>
            /// Constructor untuk BoatPath.
        /// </summary>
        /// <param name="posboat">Posisi Boat di list.</param>
        /// <param name="awal">Posisi Awal Boat.</param>
        /// <param name="akhir">Posisi Akhir Boat.</param>
        public BoatPath(int posboat, Vector2 awal, Vector2 akhir)
        {
            boat = posboat;
            posisi_awal = awal;
            posisi_akhir = akhir;
        }
    }
}