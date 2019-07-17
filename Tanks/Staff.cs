using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class Staff
    {
        public int x;
        public int y;

        Image currentImg;

        public Image CurrentImg
        {
            get { return currentImg; }
        }

        StaffImg staffImg;

        public Staff()
        {
            x = 120;
            y = 240;

            staffImg = new StaffImg();

            currentImg = staffImg.Img[0];            
        }
        internal void StaffDown()
        {
            currentImg = staffImg.Img[1];
        }
    }
}
