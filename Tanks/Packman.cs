using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Tanks
{
    class Packman 
    {
        PackmanImg packmanImg = new PackmanImg();
        Image[] img;
        Image currentImg;

        public string name;

        public Timer timerPackmanStop;

        public bool flagPackmanStop;
        public bool flagPackmanToWallStop;

        public Image CurrentImg
        {
            get { return currentImg; }
        }

        int sizeField;

        int x, y, direct_x, direct_y, nextDirect_x, nextDirect_y;

        public int NextDirect_y
        {
            get { return nextDirect_y; }
            set { nextDirect_y = value; }
        }

        public int NextDirect_x
        {
            get { return nextDirect_x; }
            set { nextDirect_x = value; }
        }
        int k;

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

        public Packman(int sizeField)
        {
            this.sizeField = sizeField;
            this.NextDirect_x = 0;
            this.NextDirect_y = -1;

            name = "Packman";      

            this.x = 80;
            this.y = 240;
            this.Direct_x = 0;
            this.Direct_y = -1;

            timerPackmanStop = new Timer();
            timerPackmanStop.Interval = 2000;
            timerPackmanStop.Enabled = false;
            timerPackmanStop.Elapsed += timerPackmanStop_Elapsed;
                   
            PutImg();

            PutCurrentImage();

        }

        private void timerPackmanStop_Elapsed(object sender, ElapsedEventArgs e)
        {
            flagPackmanStop = false;
            timerPackmanStop.Enabled = false;
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
            Turn();

            PutCurrentImage();

            if (!flagPackmanStop && !flagPackmanToWallStop)
            {                
                if (((X == sizeField - 20) && (NextDirect_x == 1)) || ((X == 0) && (NextDirect_x == -1)))
                    return;
                else
                    x += direct_x * 2;

                if (((Y == sizeField - 20) && (NextDirect_y == 1)) || ((Y == 0) && (NextDirect_y == -1)))
                    return;
                else
                    y += direct_y * 2;
            }                       
        }

        private void PutCurrentImage()
        {
            if (k == 3)
                k = 0;

            currentImg = img[k];
            k++;
        }

        public void Turn()
        {
            Direct_x = NextDirect_x;
            Direct_y = NextDirect_y; 

            PutImg();

            if ((X % 10 < 5) && (Direct_y != 0))
                X = X - (X % 10);
            if ((X % 10 >= 5) && (Direct_y != 0))
                X = X - (X % 10) + 10;

            if ((Y % 10 < 5) && (Direct_x != 0))            
                Y = Y - (Y % 10);   
            if ((Y % 10 >= 5) && (Direct_x != 0))
                Y = Y - (Y % 10) + 10;            
        }                     

        void PutImg()
        {
            if (direct_x == 1)
                img = packmanImg.Right;
            if (direct_x == -1)
                img = packmanImg.Left;
            if (direct_y == 1)
                img = packmanImg.Down;
            if (direct_y == -1)
                img = packmanImg.Up;
        }        
    }
}
