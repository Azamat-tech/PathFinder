using System.Windows.Forms;

using Path_Finder.Constants;

namespace Path_Finder.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Buttons of the Form
        private Button clearButton;
        private Button addRemoveButton;
        private Button visualizeButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(BoardConstants.WIDTH, BoardConstants.HEIGHT);
            this.Name = "Form1";
            this.Text = "Path Finding Application";
            this.ResumeLayout(false);

            // Allow Keyboard usage 
            KeyPreview = true;

            #region Adding menu

            ToolStripMenuItem[] aStarMethods =
            {
                new ToolStripMenuItem("A* Euclidean method", null, AStarEuclidean),
                new ToolStripMenuItem("A* Manhattan method", null, AStarManhattan)
            };

            ToolStripMenuItem[] algorithms =
            {
                new ToolStripMenuItem("Breadth-First Search", null, BFS),
                new ToolStripMenuItem("Depth-First Search", null, DFS),
                new ToolStripMenuItem("Dijkstra's Algorithm", null, Dijkstra),
                new ToolStripMenuItem("Depth-First Search Smart", null, SmartDFS),
                new ToolStripMenuItem("A* Search", null, aStarMethods)
            };

            ToolStripMenuItem[] mazeGenerators =
            {
                new ToolStripMenuItem("Random Maze", null, CallRandomMaze),
                new ToolStripMenuItem("Recursive Division", null, CallRecursiveMaze),
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

            #endregion

            #region Adding Buttons
            clearButton = CreateButton
            (
                ViewConstants.clearBoardName, 9 * ViewConstants.BUTTONWIDTH + 
                3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT
            );

            addRemoveButton = CreateButton
            (
                ViewConstants.addBombName, 10 * ViewConstants.BUTTONWIDTH + 
                3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT
            );

            visualizeButton = CreateButton
            (
                ViewConstants.visualizeName, 11 * ViewConstants.BUTTONWIDTH + 
                3 * BoardConstants.SQUARE, BoardConstants.MARGIN + BoardConstants.SQUARE + 2,
                ViewConstants.BUTTONWIDTH, ViewConstants.BUTTONHEIGHT
            );
            #endregion

            #region Adding Labels

            CreateTextLabel
                (
                    ViewConstants.startingNodeName, 2 * BoardConstants.MARGIN, 
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN,
                    ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT
                );

            CreateTextLabel
                (
                    ViewConstants.targetNodeName, 
                    3 * BoardConstants.MARGIN + BoardConstants.SQUARE + ViewConstants.LABELWIDTH, 
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN, 
                    ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT
                );
            
            CreateTextLabel
                (
                    ViewConstants.bombNodeName, 
                    3 * BoardConstants.MARGIN + 2 * BoardConstants.SQUARE + 2 * ViewConstants.LABELWIDTH, 
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, 
                    ViewConstants.LABELHEIGHT
                );
            
            CreateTextLabel
                (
                    ViewConstants.wallNodeName, 
                    3 * BoardConstants.MARGIN + 3 * BoardConstants.SQUARE + 3 * ViewConstants.LABELWIDTH,
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN, ViewConstants.LABELWIDTH, 
                    ViewConstants.LABELHEIGHT
                );
            
            CreateTextLabel
                (
                    ViewConstants.visitedNodeName, 
                    BoardConstants.MARGIN + 4 * BoardConstants.SQUARE + 4 * ViewConstants.LABELWIDTH,
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN, 
                    ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT
                );
            
            CreateTextLabel
                (
                    ViewConstants.shortestPathNodeName, 
                    8 * BoardConstants.MARGIN + 5 * BoardConstants.SQUARE + 5 * ViewConstants.LABELWIDTH,
                    2 * BoardConstants.SQUARE + BoardConstants.MARGIN, 
                    ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT
                );

            CreateTextLabel
            (
                ViewConstants.weightNodeName, 5 *
                BoardConstants.SQUARE + 7 * ViewConstants.LABELWIDTH, 2 * BoardConstants.SQUARE +
                BoardConstants.MARGIN, ViewConstants.LABELWIDTH, ViewConstants.LABELHEIGHT
            );
            #endregion
        }

        #endregion
    }
}

