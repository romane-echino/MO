using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MO.UI
{
    public class EntityWorldUI : MonoBehaviour
    {
        public float FadeSpeed = 10f;
        public float LifeBarSpeed = 8f;

        public Sprite AllySprite;
        public Sprite EnemySprite;
        public Sprite NeutralSprite;

        public bool IsVisible { get; private set; } = false;

        [Header("References"), SerializeField]
        private GameObject container;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private TextMeshProUGUI entityName;
        [SerializeField]
        private TextMeshProUGUI level;
        [SerializeField]
        private Image levelShape;
        [SerializeField]
        private Image lifebarFill;
        [SerializeField]
        private Image lifebarSlowFill;


        private int maxLife;
        private int currentLife;
        private float currentPercentLife;

        private EntityType type;

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void InitializeLifebar(EntityType type, string name, int level, int maxLife, int currentLife)
        {
            this.type = type;
            entityName.text = name;
            this.level.text = level.ToString();
            this.maxLife = maxLife;
            this.currentLife = currentLife;
            currentPercentLife = Mathf.Clamp01(this.currentLife / this.maxLife);

            UpdateLevelSpriteShape(this.type);
        }

        public void UpdateLife(int currentLife)
        {
            this.currentLife = currentLife;
        }

        private void Awake()
        {
            if (IsVisible)
                Show();
            else
                Hide();
        }

        private void Update()
        {
            float truePercent = Mathf.Clamp01((float)currentLife / (float)maxLife);
            currentPercentLife = Mathf.Lerp(currentPercentLife, truePercent, Time.smoothDeltaTime * LifeBarSpeed);
            if (currentPercentLife - truePercent < 0.02f)
                currentPercentLife = truePercent;
            lifebarSlowFill.fillAmount = currentPercentLife;
            lifebarFill.fillAmount = truePercent;
        }

        private void LateUpdate()
        {
            // Show or hide
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, IsVisible ? 1f : 0f, Time.smoothDeltaTime * FadeSpeed);
            if (canvasGroup.alpha > 0.05f)
                container.SetActive(true);
            else if (canvasGroup.alpha < 0.05f)
                container.SetActive(false);
        }

        private void UpdateLevelSpriteShape(EntityType type)
        {
            switch (type)
            {
                case EntityType.Ally:
                    levelShape.sprite = AllySprite;
                    break;
                case EntityType.Enemy:
                    levelShape.sprite = NeutralSprite;
                    break;
                case EntityType.Neutral:
                default:
                    levelShape.sprite = NeutralSprite;
                    break;
            }
        }
    }
}