using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace GraWDopasowanie
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        List<string> icons = new List<string>()
         {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };
        List<string> goodCheckSound = new List<string>()
        {
            "woohoo.wav","wow-1.wav","wow-2.wav","yes-1.wav"
        };
        List<string> badCheckSound = new List<string>()
        {
            "no-1.wav","no-2.wav","no-3.wav","no-4.wav","no-5.wav","no-6.wav"
        };
        private SoundPlayer soundPlayer;


        Label firstClicked = null;
        Label secondClicked = null;
        bool gameStarted = false;
        Stopwatch watch = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }



        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }

        }

        private void label_Click(object sender, EventArgs e)
        {
            if (gameStarted == false)
            {
                gameStarted = true;
                watch.Start();
            }

            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;


                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    var rand = random.Next(0, 4); //25% to play sound if correct checked
                    if (rand == 0)
                    {
                        var rnd = random.Next(0, 4);
                        soundPlayer = new SoundPlayer(goodCheckSound.ElementAt(rnd));
                        soundPlayer.Play();
                    }
                    CheckForWinner();
                    return;
                }


                timer1.Start();
            }
        }

        private void CheckForWinner()
        {
           
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }
            watch.Stop();
            double time = watch.ElapsedMilliseconds;
            MessageBox.Show("You matched all the icons!" + "  Your time is:  " + time/1000 + " seconds", "Congratulations");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;
            var rand = random.Next(0, 4); //25% to play sound if incorrect checked
            if (rand == 0)
            {
                var rnd = random.Next(0, 6);
                soundPlayer = new SoundPlayer(badCheckSound.ElementAt(rnd));
                soundPlayer.Play();
            }

            firstClicked = null;
            secondClicked = null;
        }
    }
}
