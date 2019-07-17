using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class StaffImg
    {
        Image[] img = new Image[]
            {
                Properties.Resources.staff,
                Properties.Resources.staffDown
            };       

        public Image[] Img
        {
            get { return img; }
        }
    }
}
