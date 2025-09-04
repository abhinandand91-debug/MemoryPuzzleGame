using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class RectangularCardGridFactory : ICardGridFactory
    {
        public List<CardView> CreateGrid(
            int rows,
            int cols,
            ObjectPool pool,
            FruitsSpritesScriptable sprites,
            MatchSystem matchSystem,
            DifficultyConfig config)
        {
            var cards = new List<CardView>();
            int totalPairs = (rows * cols) / 2;

            if (!config.useFullGrid)
                totalPairs = Mathf.Min(totalPairs, config.maxPairs);

            // build values
            List<int> cardValues = new();
            for (int i = 0; i < totalPairs; i++)
            {
                int spriteIndex = i % sprites.Count;
                cardValues.Add(spriteIndex);
                cardValues.Add(spriteIndex);
            }

            // shuffle
            for (int i = 0; i < cardValues.Count; i++)
            {
                int rand = Random.Range(i, cardValues.Count);
                (cardValues[i], cardValues[rand]) = (cardValues[rand], cardValues[i]);
            }

            // spawn
            foreach (var spriteIndex in cardValues)
            {
                GameObject cardObj = pool.GetObjectFromPool();
                if (cardObj == null) continue;

                cardObj.SetActive(true);
                var card = cardObj.GetComponent<CardView>();
                card.Initialize(spriteIndex, sprites.GetSprite(spriteIndex));

                matchSystem?.RegisterCard(card);
                cards.Add(card);
            }

            return cards;
        }
    }
}