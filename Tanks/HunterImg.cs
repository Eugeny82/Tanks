using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class HunterImg
    {
        Image[] up = new Image[] 
        {
            Properties.Resources.HuntenUp_0px,
            Properties.Resources.HuntenUp_1px,
            Properties.Resources.HuntenUp_2px
        };

        Image[] down = new Image[]
        {
            Properties.Resources.HuntenDown_0px,
            Properties.Resources.HuntenDown_1px,
            Properties.Resources.HuntenDown_2px
        };

        Image[] right = new Image[]
        {
            Properties.Resources.HuntenRight_0px,
            Properties.Resources.HuntenRight_1px,
            Properties.Resources.HuntenRight_2px
        };

        Image[] left = new Image[]
        {
            Properties.Resources.HuntenLeft_0px,
            Properties.Resources.HuntenLeft_1px,
            Properties.Resources.HuntenLeft_2px
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
