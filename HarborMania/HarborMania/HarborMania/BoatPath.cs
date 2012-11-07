using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class BoatPath
    {
        int boat;
        Vector2 posisi_awal;
        Vector2 posisi_akhir;

        public int Boat
        {
            get { return boat; }
            set { boat = value; }
        }

        public Vector2 PosisiAwal 
        {
            get { return posisi_awal; }
            set { posisi_awal = value; }
        }

        public Vector2 PosisiAkhir 
        {
            get { return posisi_akhir; } 
            set { posisi_akhir = value; }
        }
        
        public BoatPath () 
        {
            posisi_awal = new Vector2();
            posisi_akhir = new Vector2();
        }

        public BoatPath(int posboat, Vector2 awal, Vector2 akhir)
        {
            boat = posboat;
            posisi_awal = awal;
            posisi_akhir = akhir;
        }
    }
}