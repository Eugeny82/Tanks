using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;
using System.Windows.Media;
using System.Windows.Forms;
using System.Timers;            

namespace Tanks
{
    public delegate void Strip();

    public delegate void soundBangPlayDeleg();
    public delegate void soundTankPlayDeleg();
    public delegate void newGameDeleg(object sender, EventArgs e);

    class Model
    {
        public System.Timers.Timer timerTankCreate;
        public System.Timers.Timer timerPackmanCreate;
                
        public event Strip changeStrip;
        public event soundBangPlayDeleg soundBangPlayEvent;        
        public event newGameDeleg newGameEvent;

        int sizeField;
        public int amountTanks;        
        public int speedGame;
        
        public int countLoserPackman;

        int countTanks;
        int countTanksRun;
        public int countTanksBang;
        int countPackmanCreation;
        int iEnum;        

        Random r;

        public bool flagPackmanCreate;   

        public bool flagStaffDown; 

        public bool isADown = false;
        public bool isWDown = false;
        public bool isDDown = false;
        public bool isSDown = false;
        public bool isLDown = false;

        Projectile proj;  
        internal Projectile Projectile
        {
            get { return proj; }
        }

        Packman packman;
        internal Packman Packman
        {
            get { return packman; }
        }

        public Staff staff;

        public GameStatus gameStatus;

        List<Projectile> projectiles;

        List<Tank> t;
        internal List<Tank> Tanks
        {
            get { return t; }
        }         

        List<FireTanks> fireTank;
        internal List<FireTanks> FireTank
        {
            get { return fireTank; }
        }

        public Wall wall;               

        public Model(int amountTanks, int speedGame)
        {            
            r = new Random();

            iEnum = -1;     

            timerTankCreate = new System.Timers.Timer();
            timerTankCreate.Interval = 2000;
            timerTankCreate.Enabled = false;
            timerTankCreate.Elapsed += timerTankCreate_Elapsed;
            
            timerPackmanCreate = new System.Timers.Timer();
            timerPackmanCreate.Interval = 250;      
            timerPackmanCreate.Enabled = false;
            timerPackmanCreate.Elapsed += timerPackmanCreate_Elapsed;
                        
            this.sizeField = 260;
            this.amountTanks = amountTanks;
            this.speedGame = speedGame;
                        
            NewGame();    
        }

        private void timerPackmanCreate_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (countPackmanCreation < 20)
            {
                flagPackmanCreate = !flagPackmanCreate; 
                countPackmanCreation++;
            }
            else
            {
                flagPackmanCreate = false;
                timerPackmanCreate.Enabled = false;
                countPackmanCreation = 0;
            }            
        }

        void timerTankCreate_Elapsed(object sender, ElapsedEventArgs e)
        {      
            CreateTanks();
        }
            
        void CreateTanks()
        {            
            int x, y;

            for (int i = 0; i < 3; i++)
            {
                if (countTanks < amountTanks)
                {

                    if (countTanksRun < 4)    
                    {
                        
                        if ((countTanks + 1) % 3 == 0)          
                            x = 240;
                        else
                            if ((countTanks + 1) % 2 == 0)      
                                x = 120;
                            else
                                x = 0;                         

                        y = 0;  
                        countTanksRun++;
                        countTanks++;
                        t.Add(new Tank(sizeField, x, y, countTanks));
                    }                    
                }
            }                        
        }

        public void PackmanRun()        
        {                       
            if (isADown) 
                packman.Run();
            else 
                if (isDDown)
                    packman.Run();
                else
                    if (isWDown)
                        packman.Run();
                    else
                        if (isSDown)
                            packman.Run();                
        }                

        public void Play()
        {
            while (gameStatus == GameStatus.playing)
            {
                Thread.Sleep(speedGame);
                                
                wall.RewritePole();

                proj.Run();

                PackmanRun();

                for (int i = 0; i < t.Count; i++)       
                {                    
                    t[i].Run();

                    t[i].ProjectileRun();
                }                                 

                for (int i = 0; i < fireTank.Count; i++)    
                {                                       
                        fireTank[i].Fire();                                       
                }

                CollisionTanksProjectileToWall();

                CollisionPackmanProjectileToWall();

                CollisionsTanksToWall();
                
                CollisionPackmanToWall();      

                CollisionProjectiles();

                CollisionTankProjectileToPackman();
                
                CollisionPackmanProjectileToTanks();

                CollisionBetweenTanks();

                CollisionTanksAndPackman();

                CollisionTankProjectileToStaff();

                CollisionPackmanToStaff();

                CollisionPackmanProjectileToStaff();
            }
            
            if (gameStatus == GameStatus.winer)
            {
                MessageBox.Show("Победа!!!");
                if (newGameEvent != null)
                    newGameEvent(this.newGameEvent, null);                
            }

            if (gameStatus == GameStatus.loozer)
            {
                MessageBox.Show("Вы проиграли!!!\nПопробуйте еще раз.");
                if (newGameEvent != null)
                    newGameEvent(this.newGameEvent, null);
           }

        }              

        void CollisionTanksProjectileToWall()
        {
            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].tProj.X >= 0 && t[i].tProj.Y >= 0)             
                {

                    if (t[i].tProj.D_x != 0)
                    {
                        for (int x = (t[i].tProj.X / 10); x < 26; x++)
                        {
                            if (t[i].tProj.flagRunProjectileTanks)    
                            {                                
                                if (                              
                                    (Math.Abs(t[i].tProj.X - wall.tiles[x, (t[i].tProj.Y - 5) / 10].x) < 10 && Math.Abs(t[i].tProj.X - wall.tiles[x, (t[i].tProj.Y - 5) / 10].x) > 0 && wall.tiles[x, (t[i].tProj.Y - 5) / 10].status)
                                    ||
                                    (Math.Abs(t[i].tProj.X - wall.tiles[x, (t[i].tProj.Y + 5) / 10].x) < 10 && Math.Abs(t[i].tProj.X - wall.tiles[x, (t[i].tProj.Y + 5) / 10].x) > 0 && wall.tiles[x, (t[i].tProj.Y + 5) / 10].status)
                                    )
                                {
                                    
                                    wall.tiles[x, (t[i].tProj.Y - 5) / 10].RewriteTiles(t[i].tProj.X, t[i].tProj.Y, t[i].tProj.D_x, t[i].tProj.D_y, packman.name, "upRow");
                                    wall.tiles[x, (t[i].tProj.Y + 5) / 10].RewriteTiles(t[i].tProj.X, t[i].tProj.Y, t[i].tProj.D_x, t[i].tProj.D_y, packman.name, "downRow");

                                    t[i].tProj.DefaultProjectile();
                                }
                            }
                        }
                    }
                    
                    
                    if (t[i].tProj.D_y != 0)
                    {
                        for (int y = (t[i].tProj.Y / 10); y < 26; y++)
                        {
                            if (t[i].tProj.flagRunProjectileTanks)    
                            {
                                
                                if (                              
                                    (Math.Abs(t[i].tProj.Y - wall.tiles[(t[i].tProj.X - 5) / 10, y].y) < 10 && Math.Abs(t[i].tProj.Y - wall.tiles[(t[i].tProj.X - 5) / 10, y].y) > 0 && wall.tiles[(t[i].tProj.X - 5) / 10, y].status)
                                    ||
                                    (Math.Abs(t[i].tProj.Y - wall.tiles[(t[i].tProj.X + 5) / 10, y].y) < 10 && Math.Abs(t[i].tProj.Y - wall.tiles[(t[i].tProj.X + 5) / 10, y].y) > 0 && wall.tiles[(t[i].tProj.X + 5) / 10, y].status)
                                    )
                                {                                    
                                    wall.tiles[(t[i].tProj.X - 5) / 10, y].RewriteTiles(t[i].tProj.X, t[i].tProj.Y, t[i].tProj.D_x, t[i].tProj.D_y, packman.name, "upRow");
                                    wall.tiles[(t[i].tProj.X + 5) / 10, y].RewriteTiles(t[i].tProj.X, t[i].tProj.Y, t[i].tProj.D_x, t[i].tProj.D_y, packman.name, "downRow");

                                    t[i].tProj.DefaultProjectile();
                                }
                            }
                        }
                    }                    
                }
            }
        }
        
        void CollisionTankProjectileToPackman()
        {
            if (countPackmanCreation == 0)       
            {
                for (int i = 0; i < t.Count; i++)
                {
                    if ((Math.Abs(t[i].tProj.X - (packman.X + 5)) < 12) && (Math.Abs(t[i].tProj.Y - (packman.Y + 5)) < 12))
                    {
                        fireTank.Add(new FireTanks(packman.X - 20, packman.Y - 40));

                        countLoserPackman++;

                        packman.timerPackmanStop.Enabled = true;
                        packman.flagPackmanStop = true;

                        packman.X = 80;
                        packman.Y = 240;

                        flagPackmanCreate = true;
                        timerPackmanCreate.Enabled = true;

                        t[i].tProj.flagStopNextRunProjectileTanks = true;
                        t[i].tProj.DefaultProjectile();

                        if (soundBangPlayEvent != null)
                        {
                            soundBangPlayEvent();         
                        }
                    }
                }
            }
        }

        void CollisionTankProjectileToStaff()
        {
            for (int i = 0; i < t.Count; i++)
            {
                if ((Math.Abs(t[i].tProj.X - (staff.x + 5)) < 12) && (Math.Abs(t[i].tProj.Y - (staff.y + 5)) < 12))
                {
                    fireTank.Add(new FireTanks(staff.x - 20, staff.y - 40));

                    t[i].tProj.flagStopNextRunProjectileTanks = true;
                    t[i].tProj.DefaultProjectile();

                    staff.StaffDown();

                    flagStaffDown = true;

                    if (soundBangPlayEvent != null)
                    {
                        soundBangPlayEvent();         
                    }
                }
            }
        }
        
        void CollisionPackmanProjectileToWall()
        {
            if (proj.X >= 0 && proj.Y >= 0)             
            {
                 
                if (proj.D_x != 0)
                {
                    for (int x = (proj.X / 10); x < 26; x++)
                    {
                        if (proj.flagRunProjectilePackman)    
                        {                            
                            
                            if (                                
                                (Math.Abs(proj.X - wall.tiles[x, (proj.Y - 5) / 10].x) < 10 && Math.Abs(proj.X - wall.tiles[x, (proj.Y - 5) / 10].x) > 0 && wall.tiles[x, (proj.Y - 5) / 10].status)
                                ||
                                (Math.Abs(proj.X - wall.tiles[x, (proj.Y + 5) / 10].x) < 10 && Math.Abs(proj.X - wall.tiles[x, (proj.Y + 5) / 10].x) > 0 && wall.tiles[x, (proj.Y + 5) / 10].status)
                                )
                            {                                
                                wall.tiles[x, (proj.Y - 5) / 10].RewriteTiles(proj.X, proj.Y, proj.D_x, proj.D_y, packman.name, "upRow");                                
                                wall.tiles[x, (proj.Y + 5) / 10].RewriteTiles(proj.X, proj.Y, proj.D_x, proj.D_y, packman.name, "downRow");

                                proj.DefaultProjectile();
                            }                            
                        }                        
                    }
                }                

                
                if (proj.D_y != 0)
                {
                    for (int y = (proj.Y / 10); y < 26; y++)
                    {
                        if (proj.flagRunProjectilePackman)    
                        {                            
                            if (                                
                                (Math.Abs(proj.Y - wall.tiles[(proj.X - 5) / 10, y].y) < 10 && Math.Abs(proj.Y - wall.tiles[(proj.X - 5) / 10, y].y) > 0 && wall.tiles[(proj.X - 5) / 10, y].status)
                                ||
                                (Math.Abs(proj.Y - wall.tiles[(proj.X + 5) / 10, y].y) < 10 && Math.Abs(proj.Y - wall.tiles[(proj.X + 5) / 10, y].y) > 0 && wall.tiles[(proj.X + 5) / 10, y].status)
                                )
                            {
                                
                                wall.tiles[(proj.X - 5) / 10, y].RewriteTiles(proj.X, proj.Y, proj.D_x, proj.D_y, packman.name, "upRow");
                                wall.tiles[(proj.X + 5) / 10, y].RewriteTiles(proj.X, proj.Y, proj.D_x, proj.D_y, packman.name, "downRow");
                                proj.DefaultProjectile();
                            }
                        }
                    }
                }    
            }
        }
        
        void CollisionPackmanProjectileToTanks()
        {
            for (int i = 0; i < t.Count; i++)

                if (Math.Abs((t[i].X + 5) - proj.X) < 12 && Math.Abs((t[i].Y + 5) - proj.Y) < 12)
                {
                    fireTank.Add(new FireTanks(t[i].X - 20, t[i].Y - 40));
                    t.RemoveAt(i);
                    countTanksBang++;
                    countTanksRun--;

                    proj.DefaultProjectile();
                    packman.flagPackmanStop = false;

                    if (soundBangPlayEvent != null)
                    {
                        soundBangPlayEvent();         
                    }
                }
        }

        void CollisionPackmanProjectileToStaff()
        {
            if ((Math.Abs(proj.X - (staff.x + 5)) < 12) && (Math.Abs(proj.Y - (staff.y + 5)) < 12))
            {
                fireTank.Add(new FireTanks(staff.x - 20, staff.y - 40));

                proj.flagStopNextRunProjectileTanks = true;
                proj.DefaultProjectile();

                staff.StaffDown();

                flagStaffDown = true;

                if (soundBangPlayEvent != null)
                {
                    soundBangPlayEvent();        
                }
            }
        }
        
        void CollisionProjectiles()
        {
            for (int i = 0; i < t.Count; i++)
            {
                if ((Math.Abs(t[i].tProj.X - proj.X) < 10) && (Math.Abs(t[i].tProj.Y - proj.Y) < 10))
                {                    
                    proj.DefaultProjectile();
                    t[i].tProj.flagStopNextRunProjectileTanks = true;
                    t[i].tProj.DefaultProjectile();
                    proj.flagRunProjectilePackman = false;
                }
            }
        }
                
        void CollisionsTanksToWall()
        {
            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].Direct_x == 1)
                {

                    for (int x = 0; x < 26; x++)
                    {
                        if (
                            (wall.tiles[x, t[i].Y / 10].x - t[i].X) < 20
                            &&
                            (wall.tiles[x, t[i].Y / 10].x - t[i].X) > 0
                            &&
                            wall.tiles[x, t[i].Y / 10].status
                            )
                        {
                            t[i].X = t[i].X - t[i].X % 10;
                            t[i].flagTankToWallStop = false;
                        }

                        if (
                            (wall.tiles[x, (t[i].Y + 10) / 10].x - t[i].X) < 20
                            &&
                            (wall.tiles[x, (t[i].Y + 10) / 10].x - t[i].X) > 0
                            &&
                            wall.tiles[x, (t[i].Y + 10) / 10].status)
                        {
                            t[i].X = t[i].X - t[i].X % 10;
                            t[i].flagTankToWallStop = false;

                        }
                    }
                }

                if (t[i].Direct_x == -1)
                {

                    for (int x = 0; x < 26; x++)
                    {
                        if (
                            (t[i].X - wall.tiles[x, t[i].Y / 10].x) < 10
                            &&
                            (t[i].X - wall.tiles[x, t[i].Y / 10].x) > 0
                            &&
                            wall.tiles[x, t[i].Y / 10].status
                            )
                        {
                            t[i].X = t[i].X - t[i].X % 10 + 10;
                            t[i].flagTankToWallStop = false;
                        }

                        if (
                            (t[i].X - wall.tiles[x, (t[i].Y + 10) / 10].x) < 10
                            &&
                            (t[i].X - wall.tiles[x, (t[i].Y + 10) / 10].x) > 0
                            &&
                            wall.tiles[x, (t[i].Y + 10) / 10].status)
                        {
                            t[i].X = t[i].X - t[i].X % 10 + 10;
                            t[i].flagTankToWallStop = false;
                        }
                    }
                }

                if (t[i].Direct_y == 1)
                {

                    for (int y = 0; y < 26; y++)
                    {
                        if (
                            (wall.tiles[t[i].X / 10, y].y - t[i].Y) < 20
                            &&
                            (wall.tiles[t[i].X / 10, y].y - t[i].Y) > 0
                            &&
                            wall.tiles[t[i].X / 10, y].status
                            )
                        {
                            t[i].Y = t[i].Y - t[i].Y % 10;
                            t[i].flagTankToWallStop = false;
                        }

                        if (
                            (wall.tiles[(t[i].X + 10) / 10, y].y - t[i].Y) < 20
                            &&
                            (wall.tiles[(t[i].X + 10) / 10, y].y - t[i].Y) > 0
                            &&
                            wall.tiles[(t[i].X + 10) / 10, y].status
                            )
                        {
                            t[i].Y = t[i].Y - t[i].Y % 10;
                            t[i].flagTankToWallStop = false;
                        }
                    }
                }

                if (t[i].Direct_y == -1)
                {

                    for (int y = 0; y < 26; y++)
                    {
                        if (
                            (t[i].Y - wall.tiles[t[i].X / 10, y].y) < 10
                            &&
                            (t[i].Y - wall.tiles[t[i].X / 10, y].y) > 0
                            &&
                            wall.tiles[t[i].X / 10, y].status
                            )
                        {
                            t[i].Y = t[i].Y - t[i].Y % 10 + 10;
                            t[i].flagTankToWallStop = false;
                        }

                        if (
                            (t[i].Y - wall.tiles[(t[i].X + 10) / 10, y].y) < 10
                            &&
                            (t[i].Y - wall.tiles[(t[i].X + 10) / 10, y].y) > 0
                            &&
                            wall.tiles[(t[i].X + 10) / 10, y].status
                            )
                        {
                            t[i].Y = t[i].Y - t[i].Y % 10 + 10;
                            t[i].flagTankToWallStop = false;
                        }
                    }
                }
            }
        }

        void CollisionPackmanToWall()
        {
            if (packman.Direct_x == 1)
            {

                for (int x = 0; x < 26; x++)
                {
                    if (
                        (wall.tiles[x, packman.Y / 10].x - packman.X) < 20
                        &&
                        (wall.tiles[x, packman.Y / 10].x - packman.X) > 0
                        &&
                        wall.tiles[x, packman.Y / 10].status
                        )
                    {
                        packman.X = packman.X - packman.X % 10;
                        packman.flagPackmanToWallStop = false;
                    }

                    if (
                        (wall.tiles[x, (packman.Y + 10) / 10].x - packman.X) < 20
                        &&
                        (wall.tiles[x, (packman.Y + 10) / 10].x - packman.X) > 0
                        &&
                        wall.tiles[x, (packman.Y + 10) / 10].status)
                    {
                        packman.X = packman.X - packman.X % 10;
                        packman.flagPackmanToWallStop = false;
                    }
                }
            }

            if (packman.Direct_x == -1)
            {

                for (int x = 0; x < 26; x++)
                {
                    if (
                        (packman.X - wall.tiles[x, packman.Y / 10].x) < 10
                        &&
                        (packman.X - wall.tiles[x, packman.Y / 10].x) > 0
                        &&
                        wall.tiles[x, packman.Y / 10].status
                        )
                    {
                        packman.X = packman.X - packman.X % 10 + 10;
                        packman.flagPackmanToWallStop = false;
                    }

                    if (
                        (packman.X - wall.tiles[x, (packman.Y + 10) / 10].x) < 10
                        &&
                        (packman.X - wall.tiles[x, (packman.Y + 10) / 10].x) > 0
                        &&
                        wall.tiles[x, (packman.Y + 10) / 10].status)
                    {
                        packman.X = packman.X - packman.X % 10 + 10;
                        packman.flagPackmanToWallStop = false;
                    }
                }
            }

            
            if (packman.Direct_y == 1)
            {
                for (int y = 0; y < 26; y++)
                {
                    if (
                        (wall.tiles[packman.X / 10, y].y - packman.Y) < 20
                        &&
                        (wall.tiles[packman.X / 10, y].y - packman.Y) > 0
                        &&
                        wall.tiles[packman.X / 10, y].status
                        )
                    {
                        packman.Y = packman.Y - packman.Y % 10;
                        packman.flagPackmanToWallStop = false;
                    }

                    if (
                        (wall.tiles[(packman.X + 10) / 10, y].y - packman.Y) < 20
                        &&
                        (wall.tiles[(packman.X + 10) / 10, y].y - packman.Y) > 0
                        &&
                        wall.tiles[(packman.X + 10) / 10, y].status
                        )
                    {
                        packman.Y = packman.Y - packman.Y % 10;
                        packman.flagPackmanToWallStop = false;
                    }
                }
            }

            if (packman.Direct_y == -1)
            {

                for (int y = 0; y < 26; y++)
                {
                    if (
                        (packman.Y - wall.tiles[packman.X / 10, y].y) < 10
                        &&
                        (packman.Y - wall.tiles[packman.X / 10, y].y) > 0
                        &&
                        wall.tiles[packman.X / 10, y].status
                        )
                    {
                        packman.Y = packman.Y - packman.Y % 10 + 10;
                        packman.flagPackmanToWallStop = false;
                    }

                    if (
                        (packman.Y - wall.tiles[(packman.X + 10) / 10, y].y) < 10
                        &&
                        (packman.Y - wall.tiles[(packman.X + 10) / 10, y].y) > 0
                        &&
                        wall.tiles[(packman.X + 10) / 10, y].status
                        )
                    {
                        packman.Y = packman.Y - packman.Y % 10 + 10;
                        packman.flagPackmanToWallStop = false;
                    }
                }
            }
        }

        void CollisionPackmanToStaff()
        {
            if (packman.X > 100 && packman.X < 140 && packman.Y > 220)
            {

                if (packman.Direct_x == 1)
                {
                    packman.X = packman.X - packman.X % 10;
                    packman.flagPackmanToWallStop = false;
                }
                if (packman.Direct_x == -1)
                {
                    packman.X = packman.X - packman.X % 10 + 10;
                    packman.flagPackmanToWallStop = false;
                }

                if (packman.Direct_y == 1)
                {
                    packman.Y = packman.Y - packman.Y % 10;
                    packman.flagPackmanToWallStop = false;
                }
                if (packman.Direct_y == -1)
                {
                    packman.Y = packman.Y - packman.Y % 10 + 10;
                    packman.flagPackmanToWallStop = false;
                }

            }
        }
        
        void CollisionBetweenTanks()
        {  
            for (int i = 0; i < t.Count - 1; i++)
            {
                for (int j = i + 1; j < t.Count; j++)
                {
                    if (Math.Abs(t[j].X - t[i].X) < 20 && Math.Abs(t[j].Y - t[i].Y) < 20)
                    {
                        //навстречу танку
                        if (
                            (t[j].X < t[i].X && t[i].Direct_x == -1 && t[j].Direct_x == 1)
                            ||
                            (t[j].X > t[i].X && t[i].Direct_x == 1 && t[j].Direct_x == -1)
                            ||
                            (t[j].Y < t[i].Y && t[i].Direct_y == -1 && t[j].Direct_y == 1)
                            ||
                            (t[j].Y > t[i].Y && t[i].Direct_y == 1 && t[j].Direct_y == -1)
                            )
                        {                            
                            t[i].Turn2();
                            t[j].Turn2();
                        }
                        else
                            if (       
                                (t[j].X < t[i].X && t[i].Direct_x == 1 && t[j].Direct_x == 1)
                                ||
                                (t[j].X > t[i].X && t[i].Direct_x == -1 && t[j].Direct_x == -1)
                                ||
                                (t[j].Y < t[i].Y && t[i].Direct_y == 1 && t[j].Direct_y == 1)
                                ||
                                (t[j].Y > t[i].Y && t[i].Direct_y == -1 && t[j].Direct_y == -1)
                                )
                            {
                                t[j].flagTankStop = true;
                                t[j].timerTankStop.Enabled = true;
                            }
                            else
                                if (                                                                       
                                    (t[i].X < t[j].X && t[i].Direct_x == 1 && t[j].Direct_x == 1)
                                    ||
                                    (t[i].X > t[j].X && t[i].Direct_x == -1 && t[j].Direct_x == -1)
                                    ||
                                    (t[i].Y < t[j].Y && t[i].Direct_y == 1 && t[j].Direct_y == 1)
                                    ||
                                    (t[i].Y > t[j].Y && t[i].Direct_y == -1 && t[j].Direct_y == -1)
                                    )
                                {
                                    t[i].flagTankStop = true;
                                    t[i].timerTankStop.Enabled = true;
                                }
                                else                                
                                    if (    
                                        (t[j].X < t[i].X && t[i].Direct_y != 0 && t[j].Direct_x == 1)
                                        ||
                                        (t[j].X > t[i].X && t[i].Direct_y != 0 && t[j].Direct_x == -1)
                                        ||
                                        (t[j].Y < t[i].Y && t[i].Direct_x != 0 && t[j].Direct_y == 1)
                                        ||
                                        (t[j].Y > t[i].Y && t[i].Direct_x != 0 && t[j].Direct_y == -1)
                                        )
                                    {
                                        t[j].flagTankStop = true;
                                        t[j].Turn2();
                                        t[j].timerTankStop.Enabled = true;
                                    }
                                    else
                                        if (    
                                            (t[i].X < t[j].X && t[j].Direct_y != 0 && t[i].Direct_x == 1)
                                            ||
                                            (t[i].X > t[j].X && t[j].Direct_y != 0 && t[i].Direct_x == -1)
                                            ||
                                            (t[i].Y < t[j].Y && t[j].Direct_x != 0 && t[i].Direct_y == 1)
                                            ||
                                            (t[i].Y > t[j].Y && t[j].Direct_x != 0 && t[i].Direct_y == -1)
                                            )
                                        {
                                            t[i].flagTankStop = true;
                                            t[i].Turn2();
                                            t[i].timerTankStop.Enabled = true;
                                        }   
                    }                                         
                }
            }
        }

        void CollisionTanksAndPackman()
        {
            for (int i = 0; i < t.Count; i++)
            {
                if (Math.Abs(Packman.X - t[i].X) < 20 && Math.Abs(Packman.Y - t[i].Y) < 20)
                {
                    iEnum = i;

                    if (
                        (Packman.X < t[i].X && t[i].Direct_x == -1 && Packman.Direct_x == 1)
                        ||
                        (Packman.X > t[i].X && t[i].Direct_x == 1 && Packman.Direct_x == -1)
                        ||
                        (Packman.Y < t[i].Y && t[i].Direct_y == -1 && Packman.Direct_y == 1)
                        ||
                        (Packman.Y > t[i].Y && t[i].Direct_y == 1 && Packman.Direct_y == -1)
                        )
                    {
                        packman.flagPackmanStop = true;
                        t[i].TurnAround();
                    }
                    else
                        if (      
                            (Packman.X < t[i].X && t[i].Direct_x == 1 && Packman.Direct_x == 1)
                            ||
                            (Packman.X > t[i].X && t[i].Direct_x == -1 && Packman.Direct_x == -1)
                            ||
                            (Packman.Y < t[i].Y && t[i].Direct_y == 1 && Packman.Direct_y == 1)
                            ||
                            (Packman.Y > t[i].Y && t[i].Direct_y == -1 && Packman.Direct_y == -1)
                            )
                        {                            
                            packman.flagPackmanStop = true;
                        }
                        else
                            if (
                                (Packman.X < t[i].X && t[i].Direct_y != 0 && Packman.Direct_x == 1)
                                ||
                                (Packman.X > t[i].X && t[i].Direct_y != 0 && Packman.Direct_x == -1)
                                ||
                                (Packman.Y < t[i].Y && t[i].Direct_x != 0 && Packman.Direct_y == 1)
                                ||
                                (Packman.Y > t[i].Y && t[i].Direct_x != 0 && Packman.Direct_y == -1)
                                )
                            {
                                packman.flagPackmanStop = true;
                                t[i].Turn();
                            }
                            else
                            {
                                t[i].TurnAround();
                                packman.flagPackmanStop = false;
                            }
                        
                }
                else
                {
                    if (iEnum == i )
                    {
                        packman.flagPackmanStop = false;
                        iEnum = -1;
                    }
                }
            }
        }
        
        internal void NewGame()
        {           

            projectiles = new List<Projectile>();
            t = new List<Tank>();
            fireTank = new List<FireTanks>();
                
            packman = new Packman(sizeField);
            proj = new Projectile(sizeField);
            staff = new Staff();
            
            countTanks = 0;
            countTanksRun = 0;
            countTanksBang = 0;
            countLoserPackman = 0;
            flagStaffDown = false;  
            
            wall = new Wall();
            
            gameStatus = GameStatus.stoping;  
        }
    }
}



