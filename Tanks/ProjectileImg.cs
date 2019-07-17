using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class ProjectileImg
    {
        Image up = Properties.Resources.ProjectileUp20_20b;
        public Image Up
        {
            get { return up; }
        }

        Image down = Properties.Resources.ProjectileDown20_20b;
        public Image Down
        {
            get { return down; }
        }

        Image left = Properties.Resources.ProjectileLeft20_20b;
        public Image Left
        {
            get { return left; }
        }

        Image right = Properties.Resources.ProjectileRight20_20b;
        public Image Right
        {
            get { return right; }
        }           
    }
}
