namespace Path_Finder.Grid
{
    public enum CellType
    {
        EMPTY, WALL, START, END, BOMB
    }

    class Cell
    {
        public Position position { get; set; }
        public CellType type { get; set; }
    }
}
