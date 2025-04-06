using UnityEngine;
using UnityEngine.UI;

namespace Microlight.MicroBar
{
    // ****************************************************************************************************
    // Stores default values for a graphics component
    // ****************************************************************************************************
    public readonly struct GraphicsDefaultValues
    {
        readonly Color color;
        readonly float fade;
        readonly float fill;
        readonly Vector3 position;
        readonly float rotation;
        readonly Vector3 scale;
        readonly Vector2 anchorPosition;

        public readonly Color Color => color;
        public readonly float Fade => fade;
        public readonly float Fill => fill;
        public readonly Vector3 Position => position;
        public readonly float Rotation => rotation;
        public readonly Vector3 Scale => scale;
        public readonly Vector2 AnchorPosition => anchorPosition;

        public GraphicsDefaultValues(Image image)
        {
            color = image.color;
            fade = image.color.a;
            fill = image.fillAmount;
            position = image.rectTransform.localPosition;
            rotation = image.rectTransform.localRotation.eulerAngles.z;
            scale = image.rectTransform.localScale;
            anchorPosition = image.rectTransform.anchoredPosition;
        }
        public GraphicsDefaultValues(SpriteRenderer sprite)
        {
            color = sprite.color;
            fade = sprite.color.a;
            fill = sprite.transform.localScale.x;
            position = sprite.transform.localPosition;
            rotation = sprite.transform.localRotation.eulerAngles.z;
            scale = sprite.transform.localScale;
            anchorPosition = Vector2.zero;
        }
    }
}
