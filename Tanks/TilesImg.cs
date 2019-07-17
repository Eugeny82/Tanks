using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Tanks{   

    class TilesImg
    {
        public Image[,] img;

        public TilesImg()
        {                       
            img = new Image[,]  
            {
                {
                    global::Tanks.Properties.Resources._0_0b,
                    global::Tanks.Properties.Resources._1_0b                    
                },
                {
                    global::Tanks.Properties.Resources._0_1b,
                    global::Tanks.Properties.Resources._1_1b                   
                }                
            };
        }
    }
}
