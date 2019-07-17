using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class TankImg
    {
        Image[] up = new Image[] 
        {
            Properties.Resources.TankUp_0px,
            Properties.Resources.TankUp_1px,
            Properties.Resources.TankUp_2px
        };        

        Image[] down = new Image[]
        {
            Properties.Resources.TankDown_0px,
            Properties.Resources.TankDown_1px,
            Properties.Resources.TankDown_2px
        };
                
        Image[] right = new Image[]
        {
            Properties.Resources.TankRight_0px,
            Properties.Resources.TankRight_1px,
            Properties.Resources.TankRight_2px
        };
                
        Image[] left = new Image[]
        {
            Properties.Resources.TankLeft_0px,
            Properties.Resources.TankLeft_1px,
            Properties.Resources.TankLeft_2px
        };

        public Image[] Up
        {
            get { return up; }
        }

        public Image[] Down
        {
            get { return down; }
        }

        public Image[] Right
        {
            get { return right; }
        }

        public Image[] Left
        {
            get { return left; }
        }
    }
}
