using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Tanks
{
    public delegate void soundProjectileDeleg();
    
    partial class View : UserControl
    {
        Model model;


        public event soundProjectileDeleg soundProjectileEvent;
        
        public View(Model model)
        {
            InitializeComponent();            
            this.model = model;

            
        }

        void Draw(PaintEventArgs e)
        {
            
            //DrawApple(e);            
            DrawTank(e);
            DrawWall(e);
            DrawProjectile(e);
            DrawPackman(e);
            DrawStaff(e);
            DrawFireTank(e);
            DrawTanksProjectile(e);             

            if (model.gameStatus != GameStatus.playing) //если игра не выполняется (не начата или остановлена), исключаем запуск очередной перерисовки
                return;
            
            Thread.Sleep(model.speedGame);      

            Invalidate();       // Делает недействительной всю поверхность элемента управления и вызывает его перерисовку. (Вызывает событие Paint)
        }        

        void DrawStaff(PaintEventArgs e)
        {
            e.Graphics.DrawImage(model.staff.CurrentImg, model.staff.x, model.staff.y);
        }

        void DrawProjectile(PaintEventArgs e)
        {
            e.Graphics.DrawImage(model.Projectile.Img, model.Projectile.X, model.Projectile.Y);
        }

        void DrawPackman(PaintEventArgs e)
        {            
           if (!model.flagPackmanCreate)
           {
            e.Graphics.DrawImage(model.Packman.CurrentImg, new Point(model.Packman.X, model.Packman.Y));
            //или
            //e.Graphics.DrawImage(model.Packman.CurrentImg, model.Packman.X, model.Packman.Y);
           }
        }

        void DrawFireTank(PaintEventArgs e)
        {
            for (int i = 0; i < model.FireTank.Count; i++)
                e.Graphics.DrawImage(model.FireTank[i].CurrentImg, new Point(model.FireTank[i].X, model.FireTank[i].Y));
        }                
        
        void DrawTanksProjectile(PaintEventArgs e)
        {
            for (int i = 0; i < model.Tanks.Count; i++)
                e.Graphics.DrawImage(model.Tanks[i].tProj.Img, new Point(model.Tanks[i].tProj.X, model.Tanks[i].tProj.Y));
        }
        
        void DrawTank(PaintEventArgs e)
        {
             for (int i = 0; i < model.Tanks.Count; i++)
                e.Graphics.DrawImage(model.Tanks[i].CurrentImg, new Point(model.Tanks[i].X, model.Tanks[i].Y));
        }

        void DrawWall(PaintEventArgs e)
        {
            for (int y = 0; y < 13; y++)            
                for (int x = 0; x < 13; x++)
                    
                    e.Graphics.DrawImage(model.wall.pole[x, y].Image, new Point(x * 20, y * 20));      //вывод стенок   
        }

        protected override void OnPaint(PaintEventArgs e)       //переопределяем встроенный метод OnPaint (он уже подписан на событие Paint)
        {
            Draw(e);
            //this.Focus();       //фокус на контрол View
        }               

        private void View_KeyDown(object sender, KeyEventArgs e)    //клавиша нажата, но не отпущена
        {            
            if (model.gameStatus == GameStatus.playing)
            {
                switch (e.KeyCode)       //не зависит от раскладки и регистра нажимаемых клавиш (по умолчанию - Англ. заглавные)
                {
                    case Keys.A:
                        {
                            model.Packman.NextDirect_x = -1;
                            model.Packman.NextDirect_y = 0;                           
                            
                            model.isADown = true;   //флаг события движения влево
                            
                            model.isWDown = false;
                            model.isDDown = false;
                            model.isSDown = false;
                            
                        }
                        break;
                    case Keys.W:
                        {
                            model.Packman.NextDirect_x = 0;
                            model.Packman.NextDirect_y = -1;

                            model.isWDown = true;   //флаг события движения вниз

                            model.isADown = false;
                            model.isDDown = false;
                            model.isSDown = false;
                        }
                        break;
                    case Keys.D:
                        {
                            model.Packman.NextDirect_x = 1;
                            model.Packman.NextDirect_y = 0;
                            
                            model.isDDown = true;

                            model.isADown = false;
                            model.isWDown = false;                            
                            model.isSDown = false;
                        }
                        break;
                    case Keys.S:
                        {
                            model.Packman.NextDirect_x = 0;
                            model.Packman.NextDirect_y = 1;
                            
                            model.isSDown = true;

                            model.isADown = false;
                            model.isWDown = false;
                            model.isDDown = false;
                            
                        }
                        break;

                    case Keys.L:
                        {
                            if (!model.Projectile.flagRunProjectilePackman)
                            {
                                model.Projectile.D_x = model.Packman.Direct_x;  //направление движения снаряда
                                model.Projectile.D_y = model.Packman.Direct_y;

                                //расположение снаряда
                                model.Projectile.X = model.Packman.X + 5;
                                model.Projectile.Y = model.Packman.Y + 5;

                                model.isLDown = true;

                                if (soundProjectileEvent != null)
                                    soundProjectileEvent();                             //звук выстрела

                                model.Projectile.flagRunProjectilePackman = true;
                                model.Projectile.flagStopNextRunProjectileTanks = false;
                            }
                        }
                        break;
                }                                
            }
        }                

        private void View_KeyUp(object sender, KeyEventArgs e)      //клавиша отпущена
        {    
            
            switch (e.KeyCode)       //не зависит от раскладки и регистра нажимаемых клавиш (по умолчанию - Англ. заглавные)
            {
                case Keys.A:
                    {                        
                        model.isADown = false;
                    }
                    break;
                case Keys.W:
                    {
                        model.isWDown = false;
                    }
                    break;
                case Keys.D:
                    {                        
                        model.isDDown = false;
                    }
                    break;
                case Keys.S:
                    {
                        model.isSDown = false;
                    }
                    break;
            }
        }        
    }
}
