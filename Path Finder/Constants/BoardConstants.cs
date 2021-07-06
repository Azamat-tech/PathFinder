namespace Path_Finder.Constants
{
    public static class BoardConstants
    {
        public const int WIDTH = 1510;
        public const int HEIGHT = 790;

        public const int MARGIN = 5;

        public const int TOOLBOXWIDTH = 1500;
        public const int TOOLBOXHEIGHT = 70;

        public const int SQUARE = 20;
        public const int COLUMNSIZE = 75;
        public const int ROWSIZE = 35;

        public const int STARTXSQUARE = 18;
        public const int YSQUARE = 14;
        public const int ENDXSQUARE = 60;

        public const int BOMBXSQUARE = (STARTXSQUARE + ENDXSQUARE) / 2;
        public const int BOMBQSQUARE = YSQUARE / 2;
    }
}
