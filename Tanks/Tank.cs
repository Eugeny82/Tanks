using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;            

namespace Tanks
{
    class Tank 
    {        
        private TankImg tankImg = new TankImg();
        HunterImg HunterImg = new HunterImg();
        bool flag;
        public bool flagTankStop;
        public bool flagTankToWallStop;     
        public bool flagHunterCreate;

        public string name;

        protected Image[] img;
        protected Image currentImg;

        public Projectile tProj;
  
        protected int sizeField;

        int x, y, direct_x, direct_y;
        int k;    
        int speedHunter;
        public int idTank;

        public Timer timerTankTurn;
        public Timer timerTankStop;

        protected static Random r;
        public Image CurrentImg
        {
            get { return currentImg; }
        } 

        public int Direct_x
        {
            get { return direct_x; }
            set
            {
                direct_x = value;
            }
        }

        public int Direct_y
        {
            get { return direct_y; }
            set
            {
                direct_y = value;
            }
        }

        public Tank(int sizeField, int x, int y, int idTank)
        {
            this.sizeField = sizeField;
            this.idTank = idTank;

            name = "Tank";      

            timerTankTurn = new System.Timers.Timer();
            timerTankTurn.Interval = 200;
            timerTankTurn.Enabled = true;
            timerTankTurn.Elapsed += timerTankTurn_Elapsed;

            timerTankStop = new System.Timers.Timer();
            timerTankStop.Interval = 2000;
            timerTankStop.Enabled = false;
            timerTankStop.Elapsed += timerTankStop_Elapsed;

            r = new Random();

            if (r.Next(100) < 25)      
            {
                flagHunterCreate = true;
                speedHunter = 2;
            }
            else
            {
                flagHunterCreate = false;
                speedHunter = 1;
            }

            tProj = new Projectile(sizeField);

            do
            {                
                Direct_x = 0;
                Direct_y = 1;

                if (Direct_x == 0)
                {
                    while (Direct_y == 0)
                        Direct_y = r.Next(-1, 2);
                    flag = true;
                }
                else
                    if (Direct_y == 0)                    
                        flag = true;                   
            }
            while (!flag);            
                   
            PutImg();

            PutCurrentImage();

            this.x = x;
            this.y = y;
        }        

        public void timerTankStop_Elapsed(object sender, ElapsedEventArgs e)
        {
            flagTankStop = false;
            timerTankStop.Enabled = false;
        }
        
        private void timerTankTurn_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Math.IEEERemainder(x, 10) == 0 && Math.IEEERemainder(y, 10) == 0)
            if (r.Next(100) < 80)
            Turn();
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
                
        public void Run()
        {                                 
            if (!flagTankStop && !flagTankToWallStop)
            {
                PutCurrentImage();

                if (((X >= sizeField - 20) && (Direct_x == 1)) || ((X <= 0) && (Direct_x == -1)))
                    return;
                else
                    x += direct_x * speedHunter;

                if (((Y >= sizeField - 20) && (Direct_y == 1)) || ((Y <= 0) && (Direct_y == -1)))
                    return;
                else
                    y += direct_y * speedHunter;
            }            
        }

        public void ProjectileRun()
        {
            if (
                (tProj.X > sizeField 
                || 
                tProj.X < 0 
                || 
                tProj.Y > sizeField 
                || 
                tProj.Y < 0) 
                && 
                tProj.flagStopNextRunProjectileTanks == false
                )
            {
                if (r.Next(100) < 10)
                {
                    tProj.D_x = Direct_x;
                    tProj.D_y = Direct_y;

                    tProj.X = X + 5;
                    tProj.Y = Y + 5;
                }
            }
            
            tProj.SetRunProjectileTanks();  
        }

        protected void PutCurrentImage()
        {         
            if (k == 3)
                k = 0;

            currentImg = img[k];
            k++;
        }

        public void Turn()          
        {
            if (r.Next(100) < 50) 
            {
                if (Direct_y == 0)
                {
                    Direct_x = 0;
                    while (Direct_y == 0)
                        Direct_y = r.Next(-1, 2);   
                }
            }
            else
            {
                if (Direct_x == 0)
                {
                    Direct_y = 0;
                    while (Direct_x == 0)
                        Direct_x = r.Next(-1, 2);   
                }
            }            

                if (r.Next(500) < 100)
                    TurnAround();

                if ((X % 10 < 5) & (Direct_y != 0))
                    X = X - (X % 10);
                if ((X % 10 >= 5) & (Direct_y != 0))
                    X = X - (X % 10) + 10;

                if ((Y % 10 < 5) & (Direct_x != 0))
                    Y = Y - (Y % 10);
                if ((Y % 10 >= 5) & (Direct_x != 0))
                    Y = Y - (Y % 10) + 10;

                PutImg();                           
        }

        public void Turn2()          
        {
            if (Direct_y == 0)
            {
                Direct_x = 0;
                while (Direct_y == 0)
                    Direct_y = r.Next(-1, 2); 
            }
            else
            {
                if (Direct_x == 0)
                {
                    Direct_y = 0;
                    while (Direct_x == 0)
                        Direct_x = r.Next(-1, 2);   
                }
            }
            
            if (X % 10 < 5)
                X = X - (X % 10);

            if (X % 10 >= 5)
                X = X - (X % 10) + 10;

            if (Y % 10 < 5)
                Y = Y - (Y % 10);

            if (Y % 10 >= 5)
                Y = Y - (Y % 10) + 10;

            PutImg();
                        
        }

        private void PutImg()
        {
            if (flagHunterCreate)
            {
                if (direct_x == 1)
                    img = HunterImg.Right;
                if (direct_x == -1)
                    img = HunterImg.Left;
                if (direct_y == 1)
                    img = HunterImg.Down;
                if (direct_y == -1)
                    img = HunterImg.Up;
            }
            else
            {
                if (direct_x == 1)
                    img = tankImg.Right;
                if (direct_x == -1)
                    img = tankImg.Left;
                if (direct_y == 1)
                    img = tankImg.Down;
                if (direct_y == -1)
                    img = tankImg.Up;
            }
        }                   
        public void TurnAround()
        {
            Direct_x = -1 * Direct_x;
            Direct_y = -1 * Direct_y;
                       
            PutImg();
        }
    }
}
