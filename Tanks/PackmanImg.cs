using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class PackmanImg
    {
        Image[] up = new Image[] 
        {
            Properties.Resources.PackmanUp_0px,
            Properties.Resources.PackmanUp_1px,
            Properties.Resources.PackmanUp_2px
        };

        Image[] down = new Image[]
        {
            Properties.Resources.PackmanDown_0px,
            Properties.Resources.PackmanDown_1px,
            Properties.Resources.PackmanDown_2px
        };

        Image[] right = new Image[]
        {
            Properties.Resources.PackmanRight_0px,
            Properties.Resources.PackmanRight_1px,
            Properties.Resources.PackmanRight_2px
        };

        Image[] left = new Image[]
        {
            Properties.Resources.PackmanLeft_0px,
            Properties.Resources.PackmanLeft_1px,
            Properties.Resources.PackmanLeft_2px
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
