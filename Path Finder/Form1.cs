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
        readonly Pen pen = new Pen(Brushes.Black, 2);
        readonly StringFormat format = new StringFormat();
        readonly Board board = new Board();
        public bool isMouseDown = false;
        public bool isMouseMoving = false;
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawBoard(g);
            DrawGrid(g);
            DrawStartAndEndPosition(g);
            DrawWalls(g);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (board.InsideTheBoard(e.X, e.Y))
            {
                board.SetWalls((e.X - Board.MARGIN) / Board.SQUARE, (e.Y - Board.MARGIN) / Board.SQUARE);
            }
            isMouseDown = true;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(isMouseDown && board.InsideTheBoard(e.X, e.Y))
            {
                isMouseMoving = true;
                board.SetWalls((e.X - Board.MARGIN) / Board.SQUARE, (e.Y - Board.MARGIN) / Board.SQUARE);
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = isMouseMoving = false;
        }

        public void DrawWalls(Graphics g)
        {
            for(int i = 0; i < Board.ROWSIZE; i++)
            {
                for(int j = 0; j < Board.COLUMNSIZE; j++)
                {
                    if(board.grid[i,j] == Cell.wall)
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
