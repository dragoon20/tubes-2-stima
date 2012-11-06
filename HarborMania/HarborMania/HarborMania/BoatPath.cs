using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class BoatPath : Microsoft.Xna.Framework.GameComponent
    {
        Vector2 posisi_awal;
        Vector2 posisi_akhir;
        
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
        
        public BoatPath (Game game) : base(game) {
            posisi_awal = new Vector2();
            posisi_akhir = new Vector2();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

    }
}