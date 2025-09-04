namespace CyberSpeed.MemoryPuzzleGame
{
    public static class GridFactoryProvider
    {
        public static ICardGridFactory GetFactory()
        {
            return new RectangularCardGridFactory(); // only one now, can extend later
        }
    }
}