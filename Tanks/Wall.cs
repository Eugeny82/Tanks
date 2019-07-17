using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;   //для ArrayList

namespace Tanks
{
    class Wall 
    {        
        public PictureBox[,] pole;

        Image imgArmor = global::Tanks.Properties.Resources.BlockGrey;  
        Image imgEmpty = global::Tanks.Properties.Resources.BlockNull;  

        Random r = new Random();

        public Tiles[,] tiles;  

        public Wall()
        {            
            pole = new PictureBox[13, 13];
               
            tiles = new Tiles[26, 26];
            
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    Bitmap bmp = new Bitmap(20, 20);
                    Graphics g = Graphics.FromImage(bmp);

                    if (r.Next(100) < 30)
                    {
                        pole[x, y] = new PictureBox()              
                        {                       
                            BackColor = System.Drawing.Color.Red,
                            Image = imgEmpty,                                        
                        };                        

                        if (
                            ((x == 0 || x == 6 || x == 12) && (y == 0))
                            ||
                            (x == 4 && y == 12)
                            ||
                            (x == 6 && y == 12)
                            )
                        {                            
                            pole[x, y].BackColor = System.Drawing.Color.Blue;
                        }                        
                    }
                    else
                    {
                        pole[x, y] = new PictureBox()              
                        {
                            BackColor = System.Drawing.Color.Blue,
                            Image = imgEmpty,                       
                        }; 
                    }

                    if (
                            ((x == 5 || x == 6 || x == 7) && (y == 11))
                            ||
                            ((x == 5 || x == 7) && (y == 12))
                       )
                    {
                        pole[x, y].BackColor = System.Drawing.Color.Red;
                    }
                }
            }

            CreateTiles();   
        }

        void CreateTiles()        
        {                        
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    Bitmap bmp = new Bitmap(20, 20);
                    Graphics g = Graphics.FromImage(bmp);

                    if (pole[x, y].BackColor == System.Drawing.Color.Red)
                    {
                        for (int j = 0; j < 2; j++)
                            for (int i = 0; i < 2; i++)
                            {
                                tiles[(x * 2) + i, (y * 2) + j] = new Tiles(((x * 2) + i) * 10, ((y * 2) + j) * 10, true);

                                for (int l = 0; l < 2; l++)
                                    for (int k = 0; k < 2; k++)
                                    {
                                        g.DrawImage(tiles[(x * 2) + i, (y * 2) + j].tilesElm.picBxElm[k, l].Image, (i * 10 + k * 5), (j * 10 + l * 5));
                                    }
                            }

                        pole[x, y].Image = bmp;
                    }
                    else                        
                        for (int j = 0; j < 2; j++)
                            for (int i = 0; i < 2; i++)
                            {
                                tiles[(x * 2) + i, (y * 2) + j] = new Tiles(((x * 2) + i) * 10, ((y * 2) + j) * 10, false);
                            }
                }
            }
        }

        public void RewritePole()   
        {
            
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    
                    Bitmap bmp = new Bitmap(20, 20);
                    Graphics g = Graphics.FromImage(bmp);
                    
                    for (int j = 0; j < 2; j++)
                        for (int i = 0; i < 2; i++)
                        {
                            if (tiles[(x * 2) + i, (y * 2) + j].status)
                            {
                                for (int l = 0; l < 2; l++)
                                    for (int k = 0; k < 2; k++)
                                    {
                                        g.DrawImage(tiles[(x * 2) + i, (y * 2) + j].tilesElm.picBxElm[k, l].Image, (i * 10 + k * 5), (j * 10 + l * 5));
                                    }
                            }
                        }                        
                                        
                    pole[x, y].Image = bmp;                                                         
                }
            }  
        }
    }
}
