using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snake.Properties;

namespace Snake
{

    public partial class Field : Form
    {
        private int _width = 520;
        private int _height = 420;
        Point _headLocation = new Point(120, 20);
        private int dirX, dirY;
        private int score = 0;
        private PictureBox[] snake = new PictureBox[400];
        private PictureBox fruit;
        private int _sizeOfSide = 20;
        private int rI, rJ;
        
       public Field()
        {
            InitializeComponent();
            this.Width = _width;
            this.Height = _height;
            dirX = 1;
            dirY = 0;
            lbScore.Text = "Score: " + score;
            _generateMap();
            _generateSnake();
            fruit = new PictureBox();            
            fruit.Size = new Size(_sizeOfSide, _sizeOfSide);
            _updateFruit();
            timer.Tick += new EventHandler(_update);
            _updateSpeed();
            timer.Start();
            this.KeyDown += new KeyEventHandler(OKP);
        }

        private void _generateSnake()
        {
            snake[0] = new PictureBox();
            snake[0].Image = Resources.headOfSnake;/*Image.FromFile("headOfSnake.png");*/
            snake[0].Location = _headLocation;
            snake[0].Size = new Size(_sizeOfSide, _sizeOfSide);
            snake[0].BackColor = Color.DarkGreen;
            this.Controls.Add(snake[0]);
        }

        private void _updateFruit()
        {
            Random r = new Random();
            Color[] colors = { Color.Red, Color.Yellow, Color.Orange, Color.Purple, Color.Blue };
            int indexOfColor = r.Next(colors.Length);
            fruit.BackColor = colors[indexOfColor];

            rI = r.Next(120, _width - _sizeOfSide - 20);
            int tempI = rI % _sizeOfSide;
            rI -= tempI;
            rJ = r.Next(20, _height - _sizeOfSide - 20);
            int tempJ = rJ % _sizeOfSide;
            rJ -= tempJ;
            
            for (int i = 0; i < snake.Count(); i++)
            {
                if (snake[i] != null)
                {
                    if (snake[i].Location == new Point(rI, rJ))
                    {
                        rI = r.Next(120, _width - _sizeOfSide - 20);
                        tempI = rI % _sizeOfSide;
                        rI -= tempI;
                        rJ = r.Next(20, _height - _sizeOfSide - 20);
                        tempJ = rJ % _sizeOfSide;
                        rJ -= tempJ;
                        i = 0;
                    }
                }
            }
            //rI++;
            //rJ++;
            fruit.Location = new Point(rI, rJ);
            this.Controls.Add(fruit);
        }

        private void _generateMap()
        {
            PictureBox picTop = new PictureBox();
            picTop.BackColor = Color.Black;
            picTop.Location = new Point(_width - 420, 0);
            picTop.Size = new Size(_width - 100, 20);
            this.Controls.Add(picTop);

            PictureBox picButtom = new PictureBox();
            picButtom.BackColor = Color.Black;
            picButtom.Location = new Point(_width - 420, _height - 20);
            picButtom.Size = new Size(_width - 100, 20);
            this.Controls.Add(picButtom);

            PictureBox picLeft = new PictureBox();
            picLeft.BackColor = Color.Black;
            picLeft.Location = new Point(_width - 420, 0);
            picLeft.Size = new Size(20, _height);
            this.Controls.Add(picLeft);

            PictureBox picRight = new PictureBox();
            picRight.BackColor = Color.Black;
            picRight.Location = new Point(_width - 20, 0);
            picRight.Size = new Size(20, _height);
            this.Controls.Add(picRight);
        }

        private void _update(Object myObject, EventArgs eventsArgs)
        {
            _checkBorders();
            _eatFruit();
            _moveSnake();
            
            //cube.Location = new Point(cube.Location.X + dirX * _sizeOfSides, cube.Location.Y + dirY * _sizeOfSides);
        }

        private void _moveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point
                (snake[0].Location.X + dirX * (_sizeOfSide), 
                snake[0].Location.Y + dirY * (_sizeOfSide));
            _eatItself();
        }

        private void _eatItself()
        {
            for (int _i = 1; _i < score; _i++)
            {
                if (snake[0].Location == snake[_i].Location)
                {
                    for (int _j = _i; _j <= score; _j++)
                        this.Controls.Remove(snake[_j]);
                    timer.Stop();
                    MessageBox.Show("You have eaten yourself! Your score: " + score);
                    score = score - (score - _i + 1);
                    MessageBox.Show("You can continue with score: " + score + "\n Ready? Go!!");
                    timer.Start();
                    _updateSpeed();
                    lbScore.Text = "Score: " + score;
                }
            }
        }

        private void _eatFruit()
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                lbScore.Text = "Score: " + ++score;
                _updateSpeed();
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 20 * dirX, snake[score - 1].Location.Y - 20 * dirY);
                snake[score].Size = new Size(_sizeOfSide, _sizeOfSide);
                snake[score].BackColor = Color.DarkGreen;
                this.Controls.Add(snake[score]);
                
                _updateFruit();
            }
        }

        private void _checkBorders()
        {
            if (snake[0].Location.X < 110)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                timer.Stop();
                MessageBox.Show("Your score: " + score);
                timer.Start();
                score = 0;
                _updateSpeed();
                lbScore.Text = "Score: " + score;
                dirX = 1;
            }
            if (snake[0].Location.X > _width - 39)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                timer.Stop();
                MessageBox.Show("Your score: " + score);
                timer.Start();
                score = 0;
                _updateSpeed();
                lbScore.Text = "Score: " + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 10)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                timer.Stop();
                MessageBox.Show("Your score: " + score);
                timer.Start();
                score = 0;
                _updateSpeed();
                lbScore.Text = "Score: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > _height-30)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                timer.Stop();
                MessageBox.Show("Your score: " + score);
                timer.Start();
                score = 0;
                _updateSpeed();
                lbScore.Text = "Score: " + score;
                dirY = -1;
            }
        }

        private void _updateSpeed()
        {
            int speed = 300;
            if ((0 <= score) && (score < 5))
            {
                speed = speed - 50;
            }
            else
            {
                if ((5 <= score) && (score < 10))
                    speed = speed - 100;
                else
                {
                    if ((10 <= score) && (score < 15))
                        speed = speed - 125;
                    else
                    {
                        if ((15 <= score) && (score < 20))
                            speed = speed - 150;
                        else speed = speed - 200;
                    }
                }
            }
            timer.Interval = speed;
        }

        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down":
                    dirY = 1;
                    dirX = 0;
                    break;
                default:
                    timer.Stop();
                    MessageBox.Show("Pause! Your score for now: " + score+ "\n To continue press Ok!");
                    timer.Start();
                    break;
            }
            
        }
    }
}
