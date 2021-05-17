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

            clearButton = CreateButton(ViewConstants.clearBoardName, 9 * ViewConstants.BUTTONWIDTH + 3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                                                                                        ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            addRemoveButton = CreateButton(ViewConstants.addBombName, 10 * ViewConstants.BUTTONWIDTH + 3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                                                                              ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            visualizeButton = CreateButton(ViewConstants.visualizeName, 11 * ViewConstants.BUTTONWIDTH + 3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                                                                              ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT);

            CreateTextLabel(ViewConstants.startingNodeName, 2 * BoardConstants.MARGIN, 2 * BoardConstants.SQUARE + BoardConstants.MARGIN,
                                            ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);

            CreateTextLabel(ViewConstants.targetNodeName, 3 * BoardConstants.MARGIN + BoardConstants.SQUARE + ViewConstants.LABELWIDTH, 
                                            2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.bombNodeName, 3 * BoardConstants.MARGIN + 2 * BoardConstants.SQUARE + 2 * ViewConstants.LABELWIDTH, 
                                            2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.wallNodeName, 3 * BoardConstants.MARGIN + 3 * BoardConstants.SQUARE + 3 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.visitedNodeName, BoardConstants.MARGIN + 4 * BoardConstants.SQUARE + 4 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);
            
            CreateTextLabel(ViewConstants.shortestPathNodeName, 8 * BoardConstants.MARGIN + 5 * BoardConstants.SQUARE + 5 * ViewConstants.LABELWIDTH,
                                            2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT);


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
            Position position = new Position((e.X - BoardConstants.MARGIN) / BoardConstants.SQUARE, (e.Y - BoardConstants.MARGIN) / BoardConstants.SQUARE);
            
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
            Position position = new Position((e.X - BoardConstants.MARGIN) / BoardConstants.SQUARE, (e.Y - BoardConstants.MARGIN) / BoardConstants.SQUARE);
            
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

            DrawToolBoxBorders(g, new Position(7 * ViewConstants.BUTTONWIDTH - 2 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE), 
                    new Position(7 * ViewConstants.BUTTONWIDTH - 2 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + BoardConstants.TOOLBOXHEIGHT - BoardConstants.SQUARE + BoardConstants.MARGIN));

            DrawToolBoxItems(g);

            DrawStartPosition(g, board.GetStartingPosition().x * BoardConstants.SQUARE + BoardConstants.MARGIN,
                                       board.GetStartingPosition().y * BoardConstants.SQUARE + BoardConstants.MARGIN);

            DrawEndPosition(g, board.GetEndPosition().x * BoardConstants.SQUARE + BoardConstants.MARGIN,
                              board.GetEndPosition().y * BoardConstants.SQUARE + BoardConstants.MARGIN);

            if(board.IsBombSet())
            {
                DrawEllipseRectange(g, board.GetBombPosition().x * BoardConstants.SQUARE + BoardConstants.MARGIN,
                                       board.GetBombPosition().y * BoardConstants.SQUARE + BoardConstants.MARGIN);
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
            for(int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    if(grid[i,j].type == CellType.WALL)
                    {
                        g.FillRectangle(Brushes.Black, j * BoardConstants.SQUARE + BoardConstants.MARGIN,
                                        i * BoardConstants.SQUARE + BoardConstants.MARGIN, 
                                        BoardConstants.SQUARE, BoardConstants.SQUARE);
                    }
                }
            }
        }

        private void DrawToolBoxItems(Graphics g)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            DrawStartPosition(g, BoardConstants.MARGIN + 1 + ViewConstants.LABELWIDTH, 2 * BoardConstants.SQUARE + 4);
            DrawEndPosition(g, BoardConstants.MARGIN + 2 * ViewConstants.LABELWIDTH + BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4);

            DrawEllipseRectange(g, BoardConstants.MARGIN + 3 * ViewConstants.LABELWIDTH + 2 * BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4);
            DrawWallNode(g, 3 * BoardConstants.MARGIN + 4 * ViewConstants.LABELWIDTH + 2 * BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4);

            DrawVisitedNodes(g, 5 * ViewConstants.LABELWIDTH + 4 * BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4);
            DrawVisitedNodes(g, BoardConstants.MARGIN + 5 * ViewConstants.LABELWIDTH + 5 * BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4, isFirstDestination:false);

            DrawShortestPathNode(g, 2 * BoardConstants.MARGIN + 6 * ViewConstants.LABELWIDTH + 8 * BoardConstants.SQUARE, 2 * BoardConstants.SQUARE + 4);
        }

        private void DrawShortestPathNode(Graphics g, int posX, int posY)
        {
            Rectangle path = new Rectangle(posX, posY, BoardConstants.SQUARE, BoardConstants.SQUARE);
            g.FillRectangle(Brushes.Yellow, path);
        }

        private void DrawVisitedNodes(Graphics g, int posX, int PosY, bool isFirstDestination = true)
        {
            Rectangle visited = new Rectangle(posX, PosY, BoardConstants.SQUARE, BoardConstants.SQUARE);
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
            Rectangle unvisitedNode = new Rectangle(posX, posY, BoardConstants.SQUARE, BoardConstants.SQUARE);

            g.FillRectangle(Brushes.Black, unvisitedNode);
        }

        private void DrawEllipseRectange(Graphics g, int posX, int posY)
        {
            Pen extraPen = new Pen(Brushes.Purple, 2);
            Pen extraInnerPen = new Pen(Brushes.Purple, 4);

            Rectangle circle = new Rectangle(posX - 1, posY - 1, BoardConstants.SQUARE - 1, BoardConstants.SQUARE - 1);
            Rectangle innerCircle = new Rectangle(posX + BoardConstants.MARGIN + 1, posY + BoardConstants.MARGIN + 1, BoardConstants.MARGIN, BoardConstants.MARGIN);
            
            g.DrawEllipse(extraPen, circle);
            g.DrawEllipse(extraInnerPen, innerCircle);
        }

        private void DrawStartPosition(Graphics g, int posX, int posY)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Rectangle startRectangle = new Rectangle(posX, posY, BoardConstants.SQUARE, BoardConstants.SQUARE);

            g.FillRectangle(Brushes.Aqua, startRectangle);
            g.DrawString("S", font, Brushes.Black, startRectangle, format);
        }

        private void DrawEndPosition(Graphics g, int posX, int posY)
        {
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Rectangle endRectangle = new Rectangle(posX, posY, BoardConstants.SQUARE, BoardConstants.SQUARE);

            g.FillRectangle(Brushes.Lime, endRectangle);
            g.DrawString("E", font, Brushes.Black, endRectangle, format);
        }

        private void DrawBoard(Graphics g)
        {
            Rectangle boardOutLine = new Rectangle(
                                0, 0,
                                BoardConstants.WIDTH,
                                BoardConstants.HEIGHT
                                );

            Rectangle toolBoxOutLine = new Rectangle(
                                BoardConstants.MARGIN, BoardConstants.MARGIN + BoardConstants.SQUARE, 
                                BoardConstants.TOOLBOXWIDTH,
                                BoardConstants.TOOLBOXHEIGHT - BoardConstants.SQUARE + BoardConstants.MARGIN
                                );

            Rectangle gridOutLine = new Rectangle(
                                BoardConstants.MARGIN,
                                BoardConstants.TOOLBOXHEIGHT + 3 * BoardConstants.MARGIN,
                                BoardConstants.TOOLBOXWIDTH,
                                BoardConstants.HEIGHT - (BoardConstants.TOOLBOXHEIGHT + 4 * BoardConstants.MARGIN)
                                );

            g.DrawRectangle(pen, boardOutLine);
            g.DrawRectangle(pen, toolBoxOutLine);
            g.DrawRectangle(pen, gridOutLine);
       
        }

        private void DrawGrid(Graphics g)
        {
            for (int x = 5; x <= BoardConstants.SQUARE * BoardConstants.COLUMNSIZE; x += BoardConstants.SQUARE)
            {
                g.DrawLine(pen, x, (BoardConstants.TOOLBOXHEIGHT + 3 * BoardConstants.MARGIN), x, BoardConstants.SQUARE * BoardConstants.ROWSIZE + BoardConstants.MARGIN);
            }
            for (int y = (BoardConstants.TOOLBOXHEIGHT + 3 * BoardConstants.MARGIN); y <= BoardConstants.SQUARE * BoardConstants.ROWSIZE; y += BoardConstants.SQUARE)
            {
                g.DrawLine(pen, 5, y, BoardConstants.SQUARE * BoardConstants.COLUMNSIZE + BoardConstants.MARGIN, y);
            }
        }
    }
}
