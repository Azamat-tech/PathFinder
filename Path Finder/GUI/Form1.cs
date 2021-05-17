using System;
using System.Drawing;
using System.Windows.Forms;
using Path_Finder.Grid;
using static Path_Finder.Grid.Enums;
using Path_Finder.Constants;

namespace Path_Finder.GUI
{
    /// <summary>
    /// The Visual part of the program.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Pen pen = new Pen(Brushes.Gray, 2);
        private readonly StringFormat format = new StringFormat();
        private readonly Font font = new Font("Times New Roman", 11);
        private readonly Board board = new Board();

        private bool isMouseDown, isStartMoving, isEndMoving, isWallMoving, isBombMoving;

        private readonly Button clearButton;
        private readonly Button addRemoveButton;
        private readonly Button visualizeButton;

        public Form1()
        {
            InitializeComponent();

            ToolStripMenuItem[] algorithms =
            {
                new ToolStripMenuItem("Breadth-First Search", null),
                new ToolStripMenuItem("Depth-First Search", null),
                new ToolStripMenuItem("Depth-First Search Smart", null),
                new ToolStripMenuItem("A* Search", null),
                new ToolStripMenuItem("Dijhstra's Algorithm", null),
            };

            ToolStripMenuItem[] mazeGenerators =
            {
                new ToolStripMenuItem("Random Maze", null),
                new ToolStripMenuItem("Recursive Division", null),
            };

            ToolStripMenuItem[] mainItems =
            {
                new ToolStripMenuItem("Algorithms", null, algorithms),
                new ToolStripMenuItem("Maze & Pattern", null, mazeGenerators),
            };

            MenuStrip menu = new MenuStrip();

            foreach (ToolStripMenuItem item in mainItems)
            {
                menu.Items.Add(item);
            }
            
            Controls.Add(menu);

            clearButton = CreateButton(ViewConstants.clearBoardName, 9 * ViewConstants.BUTTONWIDTH + 3 * BoardConstant.SQUARE, BoardConstant.MARGIN + BoardConstant.SQUARE + 2,
                                                                                        ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            addRemoveButton = CreateButton(ViewConstants.addBombName, 10 * ViewConstants.BUTTONWIDTH + 3 * BoardConstant.SQUARE, BoardConstant.MARGIN + BoardConstant.SQUARE + 2,
                                                                              ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            visualizeButton = CreateButton(ViewConstants.visualizeName, 11 * ViewConstants.BUTTONWIDTH + 3 * BoardConstant.SQUARE, BoardConstant.MARGIN + BoardConstant.SQUARE + 2,
                                                                              ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            CreateTextLabel(ViewConstants.startingNodeName, 2 * BoardConstant.MARGIN, 2 * BoardConstant.SQUARE + BoardConstant.MARGIN,
                                            ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);

            CreateTextLabel(ViewConstants.targetNodeName, 3 * BoardConstant.MARGIN + BoardConstant.SQUARE + ViewConstants.LABELWIDTH, 
                                            2 * BoardConstant.SQUARE + BoardConstant.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.bombNodeName, 3 * BoardConstant.MARGIN + 2 * BoardConstant.SQUARE + 2 * ViewConstants.LABELWIDTH, 
                                            2 * BoardConstant.SQUARE + BoardConstant.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.wallNodeName, 3 * BoardConstant.MARGIN + 3 * BoardConstant.SQUARE + 3 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstant.SQUARE + BoardConstant.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.visitedNodeName, BoardConstant.MARGIN + 4 * BoardConstant.SQUARE + 4 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstant.SQUARE + BoardConstant.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.shortestPathNodeName, 8 * BoardConstant.MARGIN + 5 * BoardConstant.SQUARE + 5 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstant.SQUARE + BoardConstant.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);


        }

        private void CreateTextLabel(string name, int posX, int posY, int width, int height)
        {
            Label label = new Label();
            this.Controls.Add(label);
            label.Name = name;
            label.Text = name;
            label.Location = new Point(posX, posY);
            label.Height = height;
            label.Width = width;
            label.Font = font;
            label.AutoSize = true;
        }
        private Button CreateButton(string name, int posX, int posY, int width, int height)
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

            if (name == ViewConstants.clearBoardName)
            {
                button.Click += new EventHandler(ClearBoard);
            }
            else if (name == ViewConstants.addBombName)
            {
                button.Click += new EventHandler(AddBomb);
            }
            else if (name == ViewConstants.visualizeName)
            {

            }
            return button;
        }

        private void ClearBoard(Object sender, EventArgs args)
        {
            RemoveBomb(sender, args);
            board.ClearBoard();
            Invalidate();
        }

        private void AddBomb(Object sender, EventArgs args)
        {
            if(!board.IsTaken(board.GetBombPosition().x, board.GetBombPosition().y))
            {
                board.AddBomb();
                addRemoveButton.Name = ViewConstants.removeBombName;
                addRemoveButton.Text = ViewConstants.removeBombName;
                addRemoveButton.Click -= AddBomb;
                addRemoveButton.Click += RemoveBomb;
                Invalidate();
            }
        }

        private void RemoveBomb(Object sender, EventArgs args)
        {
            if(board.IsBombSet())
            {
                board.RemoveBomb();
                addRemoveButton.Name = ViewConstants.addBombName;
                addRemoveButton.Text = ViewConstants.addBombName;
                addRemoveButton.Click -= RemoveBomb;
                addRemoveButton.Click += AddBomb;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Position position = new Position((e.X - BoardConstant.MARGIN) / BoardConstant.SQUARE, (e.Y - BoardConstant.MARGIN) / BoardConstant.SQUARE);
            
            if (board.InsideTheBoard(e.X, e.Y))
            {
                if(board.IsEmpty(position.x, position.y))
                {
                    board.SetWall(position.x, position.y);
                    Invalidate();
                }
                else if(board.IsWall(position.x, position.y))
                {
                    board.RemoveWall(position.x, position.y);
                    Invalidate();
                }
                isMouseDown = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Position position = new Position((e.X - BoardConstant.MARGIN) / BoardConstant.SQUARE, (e.Y - BoardConstant.MARGIN) / BoardConstant.SQUARE);
            
            if (isMouseDown && board.InsideTheBoard(e.X, e.Y))
            {
                if (board.IsEmpty(position.x, position.y) && !isStartMoving && !isEndMoving && !isBombMoving)
                {
                    board.SetWall(position.x, position.y, false);
                    isWallMoving = true;
                }
                else if (board.IsWall(position.x, position.y) && !isStartMoving && !isEndMoving && !isBombMoving)
                {
                    board.RemoveWall(position.x, position.y, false);
                }
                else if ((board.IsStartPosition(position.x, position.y) || isStartMoving) && !isWallMoving && !isBombMoving)
                {
                    board.SetStartPosition(position.x, position.y);
                    isStartMoving = true;
                }
                else if ((board.IsEndPosition(position.x, position.y) || isEndMoving) && !isWallMoving && !isBombMoving)
                {
                    board.SetEndPosition(position.x, position.y);
                    isEndMoving = true;
                }
                else if(board.IsBombSet())
                {
                    if((board.IsBombPosition(position.x, position.y) || isBombMoving) && !isWallMoving)
                    {
                        board.SetBombPosition(position.x, position.y);
                        isBombMoving = true;
                    }
                }
                Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = isStartMoving = isEndMoving = isWallMoving = isBombMoving = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawBoard(g);

            DrawGrid(g);

            DrawToolBoxBorders(g, new Position(7 * ViewConstants.BUTTONWIDTH - 2 * BoardConstant.SQUARE, BoardConstant.MARGIN + BoardConstant.SQUARE), 
                    new Position(7 * ViewConstants.BUTTONWIDTH - 2 * BoardConstant.SQUARE, BoardConstant.MARGIN + BoardConstant.SQUARE + BoardConstant.TOOLBOXHEIGHT - BoardConstant.SQUARE + BoardConstant.MARGIN));

            DrawToolBoxItems(g);

            DrawStartPosition(g, board.GetStartingPosition().x * BoardConstant.SQUARE + BoardConstant.MARGIN,
                                       board.GetStartingPosition().y * BoardConstant.SQUARE + BoardConstant.MARGIN);

            DrawEndPosition(g, board.GetEndPosition().x * BoardConstant.SQUARE + BoardConstant.MARGIN,
                              board.GetEndPosition().y * BoardConstant.SQUARE + BoardConstant.MARGIN);

            if(board.IsBombSet())
            {
                DrawEllipseRectange(g, board.GetBombPosition().x * BoardConstant.SQUARE + BoardConstant.MARGIN,
                                       board.GetBombPosition().y * BoardConstant.SQUARE + BoardConstant.MARGIN);
            }

            DrawWalls(g);
        }

        private void DrawToolBoxBorders(Graphics g, Position from, Position to)
        {
            g.DrawLine(pen, from.x, from.y, to.x, to.y);
        }

        private void DrawWalls(Graphics g)
        {
            Cell[,] grid = board.GetGrid();
            for(int i = 0; i < BoardConstant.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstant.COLUMNSIZE; j++)
                {
                    if(grid[i,j].type == CellType.WALL)
                    {
                        g.FillRectangle(Brushes.Black, j * BoardConstant.SQUARE + BoardConstant.MARGIN,
                                        i * BoardConstant.SQUARE + BoardConstant.MARGIN, 
                                        BoardConstant.SQUARE, BoardConstant.SQUARE);
                    }
                }
            }
        }

        private void DrawToolBoxItems(Graphics g)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            DrawStartPosition(g, BoardConstant.MARGIN + 1 + ViewConstants.LABELWIDTH, 2 * BoardConstant.SQUARE + 4);
            DrawEndPosition(g, BoardConstant.MARGIN + 2 * ViewConstants.LABELWIDTH + BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4);

            DrawEllipseRectange(g, BoardConstant.MARGIN + 3 * ViewConstants.LABELWIDTH + 2 * BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4);
            DrawWallNode(g, 3 * BoardConstant.MARGIN + 4 * ViewConstants.LABELWIDTH + 2 * BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4);

            DrawVisitedNodes(g, 5 * ViewConstants.LABELWIDTH + 4 * BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4);
            DrawVisitedNodes(g, BoardConstant.MARGIN + 5 * ViewConstants.LABELWIDTH + 5 * BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4, isFirstDestination:false);

            DrawShortestPathNode(g, 2 * BoardConstant.MARGIN + 6 * ViewConstants.LABELWIDTH + 8 * BoardConstant.SQUARE, 2 * BoardConstant.SQUARE + 4);
        }

        private void DrawShortestPathNode(Graphics g, int posX, int posY)
        {
            Rectangle path = new Rectangle(posX, posY, BoardConstant.SQUARE, BoardConstant.SQUARE);
            g.FillRectangle(Brushes.Yellow, path);
        }

        private void DrawVisitedNodes(Graphics g, int posX, int PosY, bool isFirstDestination = true)
        {
            Rectangle visited = new Rectangle(posX, PosY, BoardConstant.SQUARE, BoardConstant.SQUARE);
            if (isFirstDestination)
            {
                g.FillRectangle(Brushes.Aqua, visited);
            }
            else
            {
                g.FillRectangle(Brushes.MediumOrchid, visited);
            }
        }

        private void DrawWallNode(Graphics g, int posX, int posY)
        {
            Rectangle unvisitedNode = new Rectangle(posX, posY, BoardConstant.SQUARE, BoardConstant.SQUARE);

            g.FillRectangle(Brushes.Black, unvisitedNode);
        }

        private void DrawEllipseRectange(Graphics g, int posX, int posY)
        {
            Pen extraPen = new Pen(Brushes.Purple, 2);
            Pen extraInnerPen = new Pen(Brushes.Purple, 4);

            Rectangle circle = new Rectangle(posX - 1, posY - 1, BoardConstant.SQUARE - 1, BoardConstant.SQUARE - 1);
            Rectangle innerCircle = new Rectangle(posX + BoardConstant.MARGIN + 1, posY + BoardConstant.MARGIN + 1, BoardConstant.MARGIN, BoardConstant.MARGIN);
            
            g.DrawEllipse(extraPen, circle);
            g.DrawEllipse(extraInnerPen, innerCircle);
        }

        private void DrawStartPosition(Graphics g, int posX, int posY)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Rectangle startRectangle = new Rectangle(posX, posY, BoardConstant.SQUARE, BoardConstant.SQUARE);

            g.FillRectangle(Brushes.Aqua, startRectangle);
            g.DrawString("S", font, Brushes.Black, startRectangle, format);
        }

        private void DrawEndPosition(Graphics g, int posX, int posY)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Rectangle endRectangle = new Rectangle(posX, posY, BoardConstant.SQUARE, BoardConstant.SQUARE);

            g.FillRectangle(Brushes.Lime, endRectangle);
            g.DrawString("E", font, Brushes.Black, endRectangle, format);
        }

        private void DrawBoard(Graphics g)
        {
            Rectangle boardOutLine = new Rectangle(
                                0, 0,
                                BoardConstant.WIDTH,
                                BoardConstant.HEIGHT
                                );

            Rectangle toolBoxOutLine = new Rectangle(
                                BoardConstant.MARGIN, BoardConstant.MARGIN + BoardConstant.SQUARE, 
                                BoardConstant.TOOLBOXWIDTH,
                                BoardConstant.TOOLBOXHEIGHT - BoardConstant.SQUARE + BoardConstant.MARGIN
                                );

            Rectangle gridOutLine = new Rectangle(
                                BoardConstant.MARGIN,
                                BoardConstant.TOOLBOXHEIGHT + 3 * BoardConstant.MARGIN,
                                BoardConstant.TOOLBOXWIDTH,
                                BoardConstant.HEIGHT - (BoardConstant.TOOLBOXHEIGHT + 4 * BoardConstant.MARGIN)
                                );

            g.DrawRectangle(pen, boardOutLine);
            g.DrawRectangle(pen, toolBoxOutLine);
            g.DrawRectangle(pen, gridOutLine);
       
        }

        private void DrawGrid(Graphics g)
        {
            for (int x = 5; x <= BoardConstant.SQUARE * BoardConstant.COLUMNSIZE; x += BoardConstant.SQUARE)
            {
                g.DrawLine(pen, x, (BoardConstant.TOOLBOXHEIGHT + 3 * BoardConstant.MARGIN), x, BoardConstant.SQUARE * BoardConstant.ROWSIZE + BoardConstant.MARGIN);
            }
            for (int y = (BoardConstant.TOOLBOXHEIGHT + 3 * BoardConstant.MARGIN); y <= BoardConstant.SQUARE * BoardConstant.ROWSIZE; y += BoardConstant.SQUARE)
            {
                g.DrawLine(pen, 5, y, BoardConstant.SQUARE * BoardConstant.COLUMNSIZE + BoardConstant.MARGIN, y);
            }
        }
    }
}
