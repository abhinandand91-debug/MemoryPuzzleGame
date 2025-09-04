using System.Collections.Generic;

namespace CyberSpeed.MemoryPuzzleGame
{
    public interface ICardGridFactory
    {
        List<CardView> CreateGrid(
            int rows,
            int cols,
            ObjectPool pool,
            FruitsSpritesScriptable sprites,
            MatchSystem matchSystem,
            DifficultyConfig config);
    }
}