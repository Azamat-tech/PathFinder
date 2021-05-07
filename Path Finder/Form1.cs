using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Path_Finder
{
    /// <summary>
    /// The Visual part of the program.
    /// </summary>
    public partial class Form1 : Form
    {
        private const int BUTTONMAZEWIDTH = 165;
        private const int BUTTONALGORITHMWIDTH = 154;
        private const int BUTTONHEIGHT = 60;

        private readonly Pen pen = new Pen(Brushes.Black, 2);
        private readonly StringFormat format = new StringFormat();

        private readonly Board board = new Board();

        private bool isMouseDown, isStartMoving, isEndMoving;
        private bool wallKeyDown, startKeyDown, endKeyDown, deleteKeyDown;

        private const string dfsName = "Depth-First Search";
        private const string bfsName = "Breadth-First Search";
        private const string dfsSmartName = "Depth-First Search (smart)";
        private const string aStarName = "A*";
        private const string dijkstraName = "Dijkstra";

        private const string randomMazeName = "Random Maze";
        private const string recursiveMazeNamze = "Recursive Maze";

        public Form1()
        {
            InitializeComponent();

            // Create the algorithm buttons once the form is initialized
            CreateButton(dfsName, 2 * Board.MARGIN, 2 * Board.MARGIN, BUTTONALGORITHMWIDTH, BUTTONHEIGHT);
            CreateButton(bfsName, 2 * Board.MARGIN + (BUTTONALGORITHMWIDTH + Board.MARGIN), 2 * Board.MARGIN, BUTTONALGORITHMWIDTH, BUTTONHEIGHT);
            CreateButton(dfsSmartName, 2 * Board.MARGIN + 2 * (BUTTONALGORITHMWIDTH + Board.MARGIN), 2 * Board.MARGIN, BUTTONALGORITHMWIDTH, BUTTONHEIGHT);
            CreateButton(aStarName, 2 * Board.MARGIN + 3 * (BUTTONALGORITHMWIDTH + Board.MARGIN), 2 * Board.MARGIN, BUTTONALGORITHMWIDTH, BUTTONHEIGHT);
            CreateButton(dijkstraName, 2 * Board.MARGIN + 4 * (BUTTONALGORITHMWIDTH + Board.MARGIN), 2 * Board.MARGIN, BUTTONALGORITHMWIDTH, BUTTONHEIGHT);

            // Create the maze buttons once the form is initialized
            CreateButton(randomMazeName, 800 + 2 * Board.MARGIN, 2 * Board.MARGIN, BUTTONMAZEWIDTH, BUTTONHEIGHT);
            CreateButton(recursiveMazeNamze, 800 + 2 * Board.MARGIN + BUTTONMAZEWIDTH + 2 * Board.MARGIN, 2 * Board.MARGIN, BUTTONMAZEWIDTH, BUTTONHEIGHT);
        }

        private void CreateButton(string name, int posX, int posY, int width, int height)
        {
            Button button = new Button();
            this.Controls.Add(button);
            button.Name = name;
            button.Text = name;
            button.Location = new Point(posX, posY);
            button.Height = height;
            button.Width = width;
            button.BackColor = Color.White;
            button.ForeColor = Color.Black;
            button.Font = new Font("Georgia", 12);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keys.S == e.KeyCode) 
            { 
                startKeyDown = true; 
            }
            else if (Keys.E == e.KeyCode) 
            { 
                endKeyDown = true; 
            }
            else if (Keys.Tab == e.KeyCode) 
            { 
                wallKeyDown = true; 
            }
            else if (Keys.Delete == e.KeyCode)
            {
                deleteKeyDown = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Keys.S == e.KeyCode)
            {
                startKeyDown = false;
            }
            else if (Keys.E == e.KeyCode)
            {
                endKeyDown = false;
            }
            else if (Keys.Tab == e.KeyCode)
            {
                wallKeyDown = false;
            }
            else if (Keys.Delete == e.KeyCode)
            {
                deleteKeyDown = false;
            }
        }

        private bool IsKeyPressed()
        {
            if (wallKeyDown || startKeyDown || endKeyDown || deleteKeyDown)
            {
                return true;
            }
            return false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Position position = new Position((e.X - Board.MARGIN) / Board.SQUARE, (e.Y - Board.MARGIN) / Board.SQUARE);
            if (board.InsideTheBoard(e.X, e.Y) && IsKeyPressed())
            {
                if (wallKeyDown)
                {
                    board.SetWall(position.x, position.y);
                }
                else if (startKeyDown)
                {
                    board.SetStartPosition(position.x, position.y);
                }
                else if (endKeyDown)
                {
                    board.SetEndPosition(position.x, position.y);
                }
                else if (deleteKeyDown)
                {
                    board.RemoveWall(position.x, position.y);
                }
                isMouseDown = true;
                Invalidate();
            }
            
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Position position = new Position((e.X - Board.MARGIN) / Board.SQUARE, (e.Y - Board.MARGIN) / Board.SQUARE);
            if (isMouseDown && board.InsideTheBoard(e.X, e.Y))
            {
                if(board.IsStartPosition(position.x, position.y) || isStartMoving)
                {
                    board.SetStartPosition(position.x, position.y);
                    isStartMoving = true;
                }
                else if(board.IsEndPosition(position.x, position.y) || isEndMoving)
                {
                    board.SetEndPosition(position.x, position.y);
                    isEndMoving = true;
                }
                else if(wallKeyDown)
                {
                    board.SetWall(position.x, position.y);
                }
                else if(deleteKeyDown)
                {
                    board.RemoveWall(position.x, position.y);
                }
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = isStartMoving = isEndMoving = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawBoard(g);
            DrawGrid(g);

            DrawToolBoxBorders(g);

            DrawStartAndEndPosition(g);
            DrawWalls(g);
        }

        public void DrawToolBoxBorders(Graphics g)
        {
            Rectangle algorithmBorder = new Rectangle(Board.MARGIN, Board.MARGIN, 800, Board.TOOLBOXHEIGHT);
            Rectangle mazeBorder = new Rectangle(800 + Board.MARGIN, Board.MARGIN, 350, Board.TOOLBOXHEIGHT);
            g.DrawRectangle(pen, algorithmBorder);
            g.DrawRectangle(pen, mazeBorder);
        }

        public void DrawWalls(Graphics g)
        {
            for(int i = 0; i < Board.ROWSIZE; i++)
            {
                for(int j = 0; j < Board.COLUMNSIZE; j++)
                {
                    if(board.grid[i,j] == Cell.WALL)
                    {
                        g.FillRectangle(Brushes.Black, j * Board.SQUARE + Board.MARGIN,
                                        i * Board.SQUARE + Board.MARGIN, 
                                        Board.SQUARE, Board.SQUARE);
                    }
                }
            }
        }

        public void DrawStartAndEndPosition(Graphics g)
        {
            Font f = new Font("Georgia", 12);
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            Rectangle startRectangle = new Rectangle(board.startPosition.x * Board.SQUARE + Board.MARGIN,
                                                     board.startPosition.y * Board.SQUARE + Board.MARGIN,
                                                     Board.SQUARE, Board.SQUARE);
            Rectangle endPosition = new Rectangle(board.endPosition.x * Board.SQUARE + Board.MARGIN,
                                                  board.endPosition.y * Board.SQUARE + Board.MARGIN,
                                                  Board.SQUARE, Board.SQUARE);
            g.FillRectangle(Brushes.Aqua, startRectangle);
            g.DrawString("S", f, Brushes.Black, startRectangle, format);

            g.FillRectangle(Brushes.Lime, endPosition);
            g.DrawString("E", f, Brushes.Black, endPosition, format);
            f.Dispose();
        }

        public void DrawBoard(Graphics g)
        {
            Rectangle boardOutLine = new Rectangle(
                                0, 0,
                                Board.WIDTH,
                                Board.HEIGHT
                                );

            Rectangle toolBoxOutLine = new Rectangle( 
                                Board.MARGIN, Board.MARGIN, 
                                Board.TOOLBOXWIDTH,
                                Board.TOOLBOXHEIGHT
                                );

            Rectangle gridOutLine = new Rectangle(
                                Board.MARGIN,
                                Board.TOOLBOXHEIGHT + 3 * Board.MARGIN,
                                Board.TOOLBOXWIDTH,
                                Board.HEIGHT - (Board.TOOLBOXHEIGHT + 4 * Board.MARGIN)
                                );

            g.DrawRectangle(pen, boardOutLine);
            g.DrawRectangle(pen, toolBoxOutLine);
            g.DrawRectangle(pen, gridOutLine);
       
        }

        public void DrawGrid(Graphics g)
        {
            for (int x = 5; x <= Board.SQUARE * Board.COLUMNSIZE; x += Board.SQUARE)
            {
                g.DrawLine(pen, x, (Board.TOOLBOXHEIGHT + 3 * Board.MARGIN), x, Board.SQUARE * Board.ROWSIZE + Board.MARGIN);
            }
            for (int y = (Board.TOOLBOXHEIGHT + 3 * Board.MARGIN); y <= Board.SQUARE * Board.ROWSIZE; y += Board.SQUARE)
            {
                g.DrawLine(pen, 5, y, Board.SQUARE * Board.COLUMNSIZE + Board.MARGIN, y);
            }
        }
    }
}
