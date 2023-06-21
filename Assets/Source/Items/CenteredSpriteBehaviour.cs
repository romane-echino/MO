using UnityEngine;

namespace MO.Item
{
    public class CenteredSpriteBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer renderer;

        public void SetSprite(Sprite sprite, float degAngle = 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, degAngle);

            // Need to compute the offset to center the sprite
            // It's the distance from the center to the pivot
            Vector2 center = sprite.rect.size / 2f;
            Vector2 offset = sprite.pivot - center;
            offset /= sprite.pixelsPerUnit;

            renderer.sprite = sprite;
            renderer.transform.localPosition = offset;
        }

        public void Reset()
        {
            transform.rotation = Quaternion.identity;
            renderer.transform.localPosition = Vector3.zero;
            renderer.sprite = null;
        }
    }
}
