using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class Tiles
    {        
        public int x;
        public int y;
        public bool status;    

        string name;

        public int xPlus;        
        public int xMinus;
        public int yPlus;
        public int yMinus;
                
        public Image imgNull;
        public TilesElm tilesElm;                

        public Tiles(int x, int y, bool status)
        {
            this.x = x;
            this.y = y;
            this.status = status;            
            
            imgNull = global::Tanks.Properties.Resources.TilesNull5px;
            tilesElm = new TilesElm();                        
        }

        public void RewriteTiles(int xProj, int yProj, int d_xProj, int d_yProj, string name, string popad)
        {            
            this.name = name;

            if (d_xProj == 1)
            {
                xPlus++;

                if (xPlus == 1)
                {
                    tilesElm.picBxElm[0, 0].Image = imgNull;
                    tilesElm.picBxElm[0, 1].Image = imgNull;
                }

                if (xPlus == 2)
                {
                    tilesElm.picBxElm[1, 0].Image = imgNull;
                    tilesElm.picBxElm[1, 1].Image = imgNull;
                }
            }

            if (d_xProj == -1)
            {
                xMinus++;

                if (xMinus == 1)
                {
                    tilesElm.picBxElm[1, 0].Image = imgNull;
                    tilesElm.picBxElm[1, 1].Image = imgNull;
                }

                if (xMinus == 2)
                {
                    tilesElm.picBxElm[0, 0].Image = imgNull;
                    tilesElm.picBxElm[0, 1].Image = imgNull;
                }

            }

            if (d_yProj == 1)
            {
                yPlus++;

                if (yPlus == 1)
                {
                    tilesElm.picBxElm[0, 0].Image = imgNull;
                    tilesElm.picBxElm[1, 0].Image = imgNull;
                }

                if (yPlus == 2)
                {
                    tilesElm.picBxElm[0, 1].Image = imgNull;
                    tilesElm.picBxElm[1, 1].Image = imgNull;
                }
            }

            if (d_yProj == -1)
            {
                yMinus++;

                if (yMinus == 1)
                {
                    tilesElm.picBxElm[0, 1].Image = imgNull;
                    tilesElm.picBxElm[1, 1].Image = imgNull;
                }

                if (yMinus == 2)
                {
                    tilesElm.picBxElm[0, 0].Image = imgNull;
                    tilesElm.picBxElm[1, 0].Image = imgNull;
                }
            }            
 
            if (xPlus + xMinus == 2) { status = false; }
            if (yPlus + yMinus == 2) { status = false; }                     
        }        
    }
}
