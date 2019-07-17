using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Tanks
{
    class Projectile
    {
        private ProjectileImg projectileImg = new ProjectileImg();

        private Image img;

        public bool flagRunProjectilePackman;

        public bool flagRunProjectileTanks;
        public bool flagStopNextRunProjectileTanks;

        Random r;

        private int sizeField;

        public Image Img
        {
            get { return img; }
        }
        
        int x, y, direct_x, direct_y;        

        public Projectile(int sizeField)
        {
            this.sizeField = sizeField;
            
            r = new Random();

            flagStopNextRunProjectileTanks = false;
                        
            img = projectileImg.Right;

            DefaultProjectile();
        }
        
        public void DefaultProjectile()
        {            
            x = y = -20;
            D_x = D_y = 0;

            flagStopNextRunProjectileTanks = false;

            flagRunProjectilePackman = false;
            flagRunProjectileTanks = false;
            
        }

        public int D_x
        {
            get { return direct_x; }
            set
            {
                if (value == 0 || value == -1 || value == 1)
                direct_x = value;
                else direct_x = 0;
            }
        }

        public int D_y
        {
            get { return direct_y; }
            set
            {
                if (value == 0 || value == -1 || value == 1)
                direct_y = value;
                else direct_y = 0;
            }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        
        public void SetRunProjectileTanks()
        {
            if (!flagStopNextRunProjectileTanks)
            {                
                Run();

                flagRunProjectileTanks = true;
            }
        }

        public void Run()
        {
            if (D_x == 0 && D_y == 0)     
                return;
            
            PutImg();

            if (D_y == 0 && D_x !=0)
                x += D_x * 8;
            else
                if (D_x == 0 && D_y != 0)
                y += D_y * 8;            
                              
            if (((x > sizeField) || (y > sizeField) || (x < 0) || (y < 0)) && flagStopNextRunProjectileTanks == false)
            {                                
                DefaultProjectile();                
                flagStopNextRunProjectileTanks = true;               
            }
        }

        private void PutImg()
        {
            if (direct_x == 1)
                img = projectileImg.Right;
            if (direct_x == -1)
                img = projectileImg.Left;
            if (direct_y == 1)
                img = projectileImg.Down;
            if (direct_y == -1)
                img = projectileImg.Up;
        }       
    }
}
