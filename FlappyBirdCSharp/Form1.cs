using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using Windows.Gaming.Input;


namespace FlappyBirdCSharp
{
    public partial class Form1 : Form
    {
        private SoundPlayer sp = new SoundPlayer(FlappyBirdCSharp.Properties.Resources.arcade_music_loop);
        Gamepad Controller;

        // initialize variables
        int cloudSpeed = 2;
        int pipeSpeed = 6;
        int gravity = 1;
        int score = 0;
        int count = 0;
        int cloudCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        // what happens when timmer is running 
        private void gameTimerEvent(object sender, EventArgs e)
        {
            if(Gamepad.Gamepads.Count > 0)
            {
                Controller = Gamepad.Gamepads.First();

            }

            var randomNum = new Random();
            bird.Top += gravity;
            pipeBottom.Left -= pipeSpeed;
            pipeTop.Left -= pipeSpeed;
            clouds1.Left -= cloudSpeed;
            scoreText.Text = "Score: " + score;

            // respawn clouds 

            if(Controller != null)
            {
                ExecuteControllerActions();
            }

            if (clouds1.Left < -200)
            {
                cloudCount++;
                clouds1.Left = 950;
                // set random vertical position for clouds
                if (cloudCount % 2 == 0)
                {
                    clouds1.Top = randomNum.Next(158, 386);
                }
                else if (cloudCount % 2 == 1)
                {
                    clouds1.Top = randomNum.Next(237, 368);
                }
            }

            // move pipes to the left during timmer operation
            if (pipeBottom.Left < -150)
            {
                pipeBottom.Left = randomNum.Next(650, 950);
                score++;
            }

            if (pipeTop.Left < -150)
            {
                // set random distance for pipe spawn
                pipeTop.Left = randomNum.Next(750, 1050);
                score++;
                count++;
                // Set random vertical spawn of Top pipe
                if (count % 2 == 0)
                {
                    pipeTop.Top = randomNum.Next(-94, -31);
                }
                else if (count % 2 == 1)
                {
                    pipeTop.Top = randomNum.Next(-31, -10);
                }

            }
            // increase difficulty 
            if (score > 5 && score <= 10)
            {
                pipeSpeed = 8;
            }

            if (score > 10 && score <= 15)
            {
                pipeSpeed = 10;
            }

            // Collision detection 
            if (bird.Bounds.IntersectsWith(pipeBottom.Bounds) ||
                bird.Bounds.IntersectsWith(pipeTop.Bounds) ||
                bird.Bounds.IntersectsWith(ground.Bounds) ||
                bird.Top < -25)

            {
                //bird.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                endGame();
            }

        }

        private void gameKeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //When key is down gravity greater
                GetUp();
                //Suppress Key 'Ding' 
                e.Handled = true;
                e.SuppressKeyPress = true; 
            }
        }

        private void gameKeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //When space key is up Gravity lessons
                GetDown();
            }
        }

        private void endGame()
        {
            gameTimer.Stop();
            scoreText.Text += " Game Over!!!";
            //Enable restart button
            restartButton.Visible = true;
            restartButton.Enabled = true;
            stopPlayingMusic();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set ground as parent for score text 
            scoreText.Parent = ground;
            scoreText.BackColor = Color.Transparent;
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            //Reset the game when clicked 
            pipeSpeed = 6;
            gravity = 5;
            score = 0;
            scoreText.Text = "Score: " + score;
            count = 0;
            bird.Location = new Point(53, 175);
            pipeTop.Location = new Point(423, -94);
            pipeBottom.Location = new Point(320, 401);
            clouds1.Location = new Point(198, 115);
            restartButton.Visible = false;
            restartButton.Enabled = false;
            //playBackgroundMusic();
            gameTimer.Start();
            
        }

        private void GetUp()
        {
            //When key is down gravity greater
            gravity = -20;
            //Suppress Key 'Ding' 
        }

        private void GetDown()
        {
            //When space key is up Gravity lessons
            gravity = 5;
        }

        private void playBackgroundMusic()
        {
            try
            {
               
                sp.PlayLooping();
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Can not locate sound file");
            }
            
        }

        private void stopPlayingMusic()
        {
            sp.Stop();
        }

        private void ExecuteControllerActions()
        {
            if (Controller.GetCurrentReading().Buttons == GamepadButtons.A)
            {
                GetUp();
            }
            else
            {
                GetDown();
            }
        }

        private void scoreBackground_Click(object sender, EventArgs e)
        {

        }
    }
}
