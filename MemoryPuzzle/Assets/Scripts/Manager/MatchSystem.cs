using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    /// <summary>
    /// Controls the logic for matching pairs of cards in the game.
    /// </summary>
    public class MatchSystem : MonoBehaviour
    {
        private readonly List<CardView> registeredCards = new();

        private CardView firstSelectedCard;
        private CardView secondSelectedCard;

        private int matchedPairsCount;
        private int totalPairsCount;
        private bool isInputLocked;

        private Coroutine initialRevealRoutine;

        // Configurable via DifficultyConfig at runtime
        public float MismatchHideDelay { get; set; } = 0.6f;
        public float InitialRevealDuration { get; set; } = 2f;

        // ----------------- Initialization -----------------

        public void Initialize(int pairsCount)
        {
            totalPairsCount = pairsCount;
            matchedPairsCount = 0;
            firstSelectedCard = null;
            secondSelectedCard = null;
            isInputLocked = false;
        }

        public void RegisterCard(CardView card)
        {
            if (card == null) return;
            registeredCards.Add(card);
            card.OnClicked += HandleCardClicked;
        }

        private void OnDestroy()
        {
            foreach (var card in registeredCards)
            {
                if (card != null)
                    card.OnClicked -= HandleCardClicked;
            }
            registeredCards.Clear();
        }

        // ----------------- Card Handling -----------------

        private void HandleCardClicked(CardView card)
        {
            if (isInputLocked || card == null || card.IsMatched)
                return;

            card.ShowFront();

            if (firstSelectedCard == null)
            {
                firstSelectedCard = card;
                return;
            }

            if (secondSelectedCard == null && card != firstSelectedCard)
            {
                secondSelectedCard = card;
                EvaluateSelection();
            }
        }

        private void EvaluateSelection()
        {
            if (firstSelectedCard == null || secondSelectedCard == null)
                return;

            ScoreSystem.Instance?.IncrementTurn();

            if (firstSelectedCard.Value == secondSelectedCard.Value)
                HandleMatch();
            else
                HandleMismatch();
        }

        private void HandleMatch()
        {
            firstSelectedCard.MarkAsMatched();
            secondSelectedCard.MarkAsMatched();

            matchedPairsCount++;
            ScoreSystem.Instance?.OnMatch();

            ClearSelection();
            CheckForGameOver();
        }

        private void HandleMismatch()
        {
            ScoreSystem.Instance?.OnMismatch();
            isInputLocked = true;
            StartCoroutine(HideMismatchedCardsAfterDelay());
        }

        private IEnumerator HideMismatchedCardsAfterDelay()
        {
            yield return new WaitForSeconds(MismatchHideDelay);

            if (firstSelectedCard != null && !firstSelectedCard.IsMatched)
                firstSelectedCard.ShowBack();

            if (secondSelectedCard != null && !secondSelectedCard.IsMatched)
                secondSelectedCard.ShowBack();

            ClearSelection();
            isInputLocked = false;
        }

        private void ClearSelection()
        {
            firstSelectedCard = null;
            secondSelectedCard = null;
        }

        // ----------------- Game State -----------------

        private void CheckForGameOver()
        {
            if (matchedPairsCount >= totalPairsCount)
                Invoke(nameof(TriggerGameOver), 1f);
        }

        private void TriggerGameOver()
        {
            GameFlowController.Instance.GameOver();
        }

        public void SetMatchedPairs(int count)
        {
            matchedPairsCount = count;
            CheckForGameOver();
        }

        // ----------------- Initial Reveal -----------------

        public void RunInitialReveal()
        {
            if (!gameObject.activeInHierarchy)
                return;

            StopCoroutineSafe(initialRevealRoutine);
            initialRevealRoutine = StartCoroutine(InitialRevealSequence());
        }

        private IEnumerator InitialRevealSequence()
        {
            isInputLocked = true;

            foreach (var card in registeredCards)
            {
                if (card != null && !card.IsMatched)
                    card.ShowFront();
            }

            yield return new WaitForSeconds(InitialRevealDuration);

            foreach (var card in registeredCards)
            {
                if (card != null && !card.IsMatched)
                    card.ShowBack();
            }

            isInputLocked = false;
        }

        private void StopCoroutineSafe(Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);
        }
    }
}
