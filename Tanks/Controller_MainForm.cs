using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Windows.Media;
using System.Timers;           


namespace Tanks
{
    public partial class ControllerMainForm : Form
    {
        View view;
        Model model;

        System.Timers.Timer timerTankSound;
                
        public delegate void Invoker();        

        bool isSound;

        bool flagNewGame; 

        public List<PictureBox> picEnemy;        

        MediaPlayer soundTanks;
        Uri soundTanksRes = new Uri("Media/Move.wav", UriKind.Relative);
    
        MediaPlayer soundBang;
        Uri soundBangRes = new Uri("Media/Bang.wav", UriKind.Relative);
                
        MediaPlayer soundProjectile;
        Uri soundProjectileRes = new Uri("Media/Shooting.wav", UriKind.Relative);

        Thread modelPlay;               

        public ControllerMainForm() : this(20)  {}
        public ControllerMainForm(int amountTanks) : this(amountTanks, 40) {}        
        public ControllerMainForm(int amountTanks, int speedGame)
        {
            InitializeComponent();

            isSound = true;

            model = new Model(amountTanks, speedGame);

            view = new View(model);
            this.Controls.Add(view);

            timerTankSound = new System.Timers.Timer();
            timerTankSound.Interval = 53000;
            timerTankSound.Enabled = false;
            timerTankSound.Elapsed += timer3_Elapsed;

            picEnemy = new List<PictureBox>()
            {
                picBx1, picBx2, picBx3, picBx4, picBx5, picBx6, picBx7, picBx8, picBx9, picBx10,
                picBx11, picBx12, picBx13, picBx14, picBx15, picBx16, picBx17, picBx18, picBx19, picBx20
            };
            

            textBoxScore.Name = "0";

            model.changeStrip += ChangerStatusStripLbl;     

            model.newGameEvent += NewGameSet;
            
            model.soundBangPlayEvent += new soundBangPlayDeleg(SoundBangPlay);            

            view.soundProjectileEvent += SoundProjectilePlay;

            soundTanks = new MediaPlayer();
        }

        void timer3_Elapsed(object sender, ElapsedEventArgs e)
        {           
            SoundTanksPlay();
        }

        void SoundProjectilePlay()
        {
            Invoke(new Invoker(StartProjectilePlay));
        }

        void StartProjectilePlay()
        {
            if (isSound)
            {
                soundProjectile = new MediaPlayer();    
                soundProjectile.Open(soundProjectileRes);                
                soundProjectile.Play();
            }
        }                

        void SoundBangPlay()
        {            
            Invoke(new Invoker(StartBangPlay));
        }

        void StartBangPlay()
        {
            if (isSound)
            {
                soundBang = new MediaPlayer();  
                soundBang.Open(soundBangRes);
                soundBang.Play();
            }

            textBoxScore.Text = model.countTanksBang.ToString();

            if (model.countTanksBang > 0)
                picEnemy[model.countTanksBang - 1].Visible = true;

            if (model.countTanksBang == model.amountTanks)
                StopGame(true, true);
            txBxLiveCount.Text = (3 - model.countLoserPackman).ToString();

            if (model.countLoserPackman == 3)
                StopGame(false, false);

            if (model.flagStaffDown)
                StopGame(false, true);
        }

        //
        private void StopGame(bool winner, bool loss)
        {
            model.gameStatus = GameStatus.playing;
            StartStopBtn_Click(null, null);

            soundTanks.Stop();
            StartStopBtn.Enabled = false;

            if (winner)
            {
                MessageBox.Show("       ПОБЕДА!");
            }
            else
            {
                if (loss)
                    MessageBox.Show("Штаб уничтожен.\nВы проиграли!");
                else
                    MessageBox.Show("Танк уничтожен.\nВы проиграли!");
            }

        }

        void SoundTanksPlay()
        {
            Invoke(new Invoker(StartSoundTankPlay));
        }

        void StartSoundTankPlay()
        {              
            soundTanks.Stop();

            if (isSound && model.gameStatus == GameStatus.playing)
            {
                soundTanks.Open(soundTanksRes);
                soundTanks.Play();
            }            
        }

        Invoker inv;               

        void ChangerStatusStripLbl()          
        {            
            inv += SetValueToStripLbl;

            Invoke(inv);        
        }

        void SetValueToStripLbl()
        {
            GameStatus_Lbl_strp.Text = model.gameStatus.ToString();
            if (isSound)
            {
                if (model.gameStatus == GameStatus.playing & !flagNewGame)   
                {
                    SoundTanksPlay();
                    timerTankSound.Enabled = true;
                }
                else
                {
                    soundTanks.Stop();
                    timerTankSound.Enabled = false;                    
                }                
            }            
        }

        private void StartStopBtn_Click(object sender, EventArgs e)
        {          
            if (model.gameStatus == GameStatus.playing)
            {
                if (modelPlay != null)
                    modelPlay.Abort();
                
                model.gameStatus = GameStatus.stoping; 
                StartStopBtn.Text = "Play";
                
                ChangerStatusStripLbl();
                flagNewGame = false;

                timerTankSound.Enabled = false;
                model.timerTankCreate.Enabled = false;

                soundTanks.Stop();                
            }
            else
            {
                view.Focus();

                if (!flagNewGame)
                {
                    model.gameStatus = GameStatus.playing;
                    modelPlay = new Thread(model.Play);
                    modelPlay.Start();

                    StartStopBtn.Text = "Pause";
                    ChangerStatusStripLbl();
                }
                model.timerTankCreate.Enabled = true;
                flagNewGame = false;           
                view.Invalidate();      
            }           
        }

        private void Controller_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modelPlay != null)
            {
                modelPlay.Abort();
                model.gameStatus = GameStatus.stoping;
            }

            timerTankSound.Enabled = false;
            soundTanks.Stop();
            StartStopBtn.Text = "Play";
                
            DialogResult dr =  MessageBox.Show("Вы действительно хотите выйти?", "Танки", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;           
        }    
       
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) //Выход из игры через меню
        {
            soundTanks.Stop();
            Application.Exit();
        }
                
        private void NewGameSet(object sender, EventArgs e)
        {            
            Invoke(new Invoker(NewGameToolStripMenuItem_Click));
        }        

        void NewGameToolStripMenuItem_Click()
        {
            NewGameToolStripMenuItem_Click(this, null);
        }
        void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flagNewGame = true;            //признак новой игры
            
            StartStopBtn_Click(sender, e);
            model.NewGame();
            txBxLiveCount.Text = "3";
            textBoxScore.Text = "0";            

            StartStopBtn.Text = "Play";
            StartStopBtn.Enabled = true;

            for (int i = 0; i < 20; i++)
            {
                picEnemy[i].Visible = false;
            }

            view.Refresh();         //тоже что и view.Invalidate()
        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            model.gameStatus = GameStatus.playing;
            StartStopBtn_Click(sender, e);

            MessageBox.Show(@"
            Игра 'Танки' версии 1.0
            Разработчик Гриненко Евгений Михайлович

            Для управления используйте клавиши 
            'A', 'S', 'D', 'W', 'L'", "Танки");
        }

        private void SoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSound = !isSound;

            SoundTanksPlay();

            if (isSound)
                soundToolStripMenuItem.Image = global::Tanks.Properties.Resources.SoundOn;
            else
                soundToolStripMenuItem.Image = global::Tanks.Properties.Resources.NoSound;
        }
    } 
}
