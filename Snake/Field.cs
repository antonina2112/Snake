using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Field : Form
    {
        private int _width = 520;
        private int _height = 450;
        Point _headLocation = new Point(110, 10);
        private int dirX, dirY;
        private int score = 0;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[400];
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
            //Color[] colors = { Color.Red, Color.Yellow, Color.Orange, Color.Purple, Color.Blue };
            //int indexOfColor = r.Next(colors.Length);
            fruit.BackColor = Color.Yellow/*colors[indexOfColor]*/;
            fruit.Size = new Size(_sizeOfSide, _sizeOfSide);
            _updateFruit();
            timer.Tick += new EventHandler(_update);
            timer.Interval = 200;
            timer.Start();
            this.KeyDown += new KeyEventHandler(OKP);

        }

        private void _generateSnake()
        {
            snake[0] = new PictureBox();
            snake[0].Location = _headLocation;
            snake[0].Size = new Size(_sizeOfSide, _sizeOfSide);
            snake[0].BackColor = Color.DarkGreen;
            this.Controls.Add(snake[0]);
        }

        //private void _generateFruit()
        //{
        //    Random r = new Random();
        //    fruit = new PictureBox();
        //    Color[] colors = { Color.Red, Color.Yellow, Color.Orange, Color.Purple, Color.Blue };
        //    int indexOfColor = r.Next(colors.Length);
        //    fruit.BackColor = colors[indexOfColor];
        //    fruit.Size = new Size(_sizeOfSide, _sizeOfSide);
        //    _updateFruit();
        //}

        private void _updateFruit()
        {
            
            Random r = new Random();
            rI = r.Next(110, _width - _sizeOfSide-10);
            int tempI = rI % _sizeOfSide;
            rI -= tempI;
            rJ = r.Next(10, _height - _sizeOfSide - 10);
            int tempJ = rJ % _sizeOfSide;
            rJ -= tempJ;
            //rI++;
            //rJ++;
            fruit.Location = new Point(rI + 10, rJ+ 10);
            this.Controls.Add(fruit);
        }

        private void _generateMap()
        {
            PictureBox picTop = new PictureBox();
            picTop.BackColor = Color.Black;
            picTop.Location = new Point(_width - 420, 0);
            picTop.Size = new Size(_width - 100, 10);
            this.Controls.Add(picTop);

            PictureBox picButtom = new PictureBox();
            picButtom.BackColor = Color.Black;
            picButtom.Location = new Point(_width - 420, _height - 10);
            picButtom.Size = new Size(_width - 100, 10);
            this.Controls.Add(picButtom);

            PictureBox picLeft = new PictureBox();
            picLeft.BackColor = Color.Black;
            picLeft.Location = new Point(_width - 420, 0);
            picLeft.Size = new Size(10, _height);
            this.Controls.Add(picLeft);

            PictureBox picRight = new PictureBox();
            picRight.BackColor = Color.Black;
            picRight.Location = new Point(_width - 10, 0);
            picRight.Size = new Size(10, _height);
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
            //_eatItself();
        }

        private void _eatFruit()
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                lbScore.Text = "Score: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 20 * dirX, snake[score - 1].Location.Y - 20 * dirY);
                snake[score].Size = new Size(_sizeOfSide - 1, _sizeOfSide - 1);
                snake[score].BackColor = Color.Red;
                this.Controls.Add(snake[score]);
                
                _updateFruit();
            }
        }

        private void _checkBorders()
        {
            if (snake[0].Location.X <= 105)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                lbScore.Text = "Score: " + score;
                dirX = 1;
            }
            if (snake[0].Location.X > _width - 40)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                lbScore.Text = "Score: " + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 20)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                lbScore.Text = "Score: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > _height-30)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                lbScore.Text = "Score: " + score;
                dirY = -1;
            }
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
            }
        }
    }
}
