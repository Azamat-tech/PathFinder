namespace Path_Finder.Grid
{
    public enum CellType
    {
        EMPTY, WALL, WEIGHT, START, END, BOMB
    }

    class Cell
    {
        public Position position { get; set; }
        public Position parent { get; set; }
        public CellType type { get; set; }
        public bool visited { get; set; }
    }
}
