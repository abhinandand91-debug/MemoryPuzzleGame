using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class CardView : MonoBehaviour
    {
        public int Value { get; private set; }
        public bool IsFaceDown { get; private set; }
        public bool IsMatched { get; private set; }
        public bool IsAnimating { get; private set; }

        [Header("Card Images")]
        [SerializeField] private Image frontImage;
        [SerializeField] private Image backImage;

        [Header("UI Button")]
        [SerializeField] private Button cardButton;

        [Header("Flip Animation")]
        [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private Ease flipEase = Ease.InOutSine; // DOTween ease

        public Action<CardView> OnClicked;

        private void Awake()
        {
            if (cardButton == null)
                cardButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            cardButton.onClick.AddListener(HandleCardClicked);
        }

        private void OnDisable()
        {
            cardButton.onClick.RemoveListener(HandleCardClicked);
        }

        public void Initialize(int value, Sprite frontSprite)
        {
            Value = value;
            frontImage.sprite = frontSprite;

            IsMatched = false;
            IsFaceDown = true;
            frontImage.enabled = false;
            backImage.enabled = true;

            Color color = frontImage.color;
            color.a = 1f;
            frontImage.color = color;

            ResetTransformScale();
        }

        public void ResetCard()
        {
            Value = 0;
            IsFaceDown = true;
            IsMatched = false;
            frontImage.enabled = false;
            backImage.enabled = true;
        }

        public void ShowBack()
        {
            if (IsFaceDown) return;
            StartFlip(false);
        }

        public void ShowFront()
        {
            if (!IsFaceDown) return;
            StartFlip(true);
        }

        public void MarkAsMatched()
        {
            IsMatched = true;
            Color color = frontImage.color;
            color.a = 0.7f;
            frontImage.color = color;
        }

        private void HandleCardClicked()
        {
            if (IsMatched || !IsFaceDown || IsAnimating)
                return;

            OnClicked?.Invoke(this);
            SoundManager.Instance.PlayCardFlip();
        }

        private void StartFlip(bool toFront)
        {
            if (IsAnimating) return;

            RectTransform rect = transform as RectTransform;
            if (rect == null) return;

            IsAnimating = true;

            // Phase 1: shrink X to 0
            rect.DOScaleX(0f, flipDuration * 0.5f)
                .SetEase(flipEase)
                .OnComplete(() =>
                {
                    // Swap face at mid flip
                    if (toFront)
                    {
                        backImage.enabled = false;
                        frontImage.enabled = true;
                        IsFaceDown = false;
                    }
                    else
                    {
                        backImage.enabled = true;
                        frontImage.enabled = false;
                        IsFaceDown = true;
                    }

                    // Phase 2: expand X back to 1
                    rect.DOScaleX(1f, flipDuration * 0.5f)
                        .SetEase(flipEase)
                        .OnComplete(() => IsAnimating = false);
                });
        }

        private void ResetTransformScale()
        {
            RectTransform rect = transform as RectTransform;
            if (rect == null) return;

            Vector3 scale = rect.localScale;
            scale.x = 1f;
            rect.localScale = scale;
        }
    }
}
