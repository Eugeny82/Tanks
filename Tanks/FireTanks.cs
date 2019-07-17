using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class FireTanks
    {
        FireTankImg ftImg = new FireTankImg();
        public Image currentImg;
        public bool fireTankEnd;

        public Image CurrentImg
        {
            get { return currentImg; }
        }

        Image[] img;

        int x, y;

        public int Y
        {
            get { return y; }
        }

        public int X
        {
            get { return x; }
        }

        public FireTanks(int x, int y)
        {
            this.x = x;
            this.y = y;
            img = ftImg.Img;
            PutCurrentImage();
            fireTankEnd = false;
        }

        public void Fire()
        {            
            PutCurrentImage();
        }

        int k;        
        
        protected void PutCurrentImage()
        {        
            
            if (k < 25)        
            {
                currentImg = img[k];
                k++;
            }

            else
                currentImg = img[25];
        }
    }
}
