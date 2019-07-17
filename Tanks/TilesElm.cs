using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class TilesElm
    {        
        public PictureBox[,] picBxElm;

        TilesImg tilesImg;

        public TilesElm()
        {        
            tilesImg = new TilesImg();
            
            picBxElm = new PictureBox[2, 2];

            for (int j = 0; j < 2; j++)
                for (int i = 0; i < 2; i++)
                {
                    picBxElm[i, j] = new PictureBox();
                    picBxElm[i, j].Location = new System.Drawing.Point(0, 0);
                    picBxElm[i, j].Image = tilesImg.img[i, j];
                }
        }
    }
}
