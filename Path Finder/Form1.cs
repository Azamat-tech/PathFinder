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
    public partial class Form1 : Form
    {
        readonly Pen pen = new Pen(Brushes.Black, 2);
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawBoard(g);
            DrawGrid(g);
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
            for (int x = 5; x <= Grid.SQUARE * Grid.COLUMNSIZE; x += Grid.SQUARE)
            {
                g.DrawLine(pen, x, (Board.TOOLBOXHEIGHT + 3 * Board.MARGIN), x, Grid.SQUARE * Grid.ROWSIZE + Board.MARGIN);
            }
            for (int y = (Board.TOOLBOXHEIGHT + 3 * Board.MARGIN); y <= Grid.SQUARE * Grid.ROWSIZE; y += Grid.SQUARE)
            {
                g.DrawLine(pen, 5, y, Grid.SQUARE * Grid.COLUMNSIZE + Board.MARGIN, y);
            }
        }
    }
}
