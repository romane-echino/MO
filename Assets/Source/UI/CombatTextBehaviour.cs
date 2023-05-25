using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MO.UI
{
    public class CombatTextBehaviour : MonoBehaviour
    {
        public float NormalLifeTime = 1f;
        public float DisapearLifeTime = 1f;
        public float NormalVerticalSpeed = 0.4f;
        public float DisapearAcceleration = 0.1f;

        [SerializeField]
        private TextMeshPro text;

        private bool isStarted = false;
        private float lifeTime;
        private float disapearVerticalSpeed = 0f;

        public void ShowText(string text)
        {
            disapearVerticalSpeed = 0f;
            lifeTime = 0f;
            this.text.text = text;
            isStarted = true;
        }

        private void Update()
        {
            if (!isStarted)
                return;
            
            lifeTime += Time.smoothDeltaTime;

            if (lifeTime < NormalLifeTime)
            {
                transform.position = transform.position + Vector3.up * Time.smoothDeltaTime * NormalVerticalSpeed;
                disapearVerticalSpeed = NormalVerticalSpeed;
            }
            else
            {
                disapearVerticalSpeed += DisapearAcceleration * Time.smoothDeltaTime;
                transform.position = transform.position + Vector3.up * Time.smoothDeltaTime * disapearVerticalSpeed;

                Color color = text.color;
                color.a = Mathf.MoveTowards(color.a, 0f, Time.smoothDeltaTime / DisapearLifeTime);

                if (color.a <= 0f)
                {
                    // end
                    Destroy(gameObject);
                    return;
                }

                text.color = color;
            }
        }
    }
}