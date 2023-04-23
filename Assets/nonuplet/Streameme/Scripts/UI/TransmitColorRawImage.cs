using UnityEngine;
using UnityEngine.UI;

namespace Streameme.UI
{
    /// <summary>
    /// ボタンのセレクト状態に合わせてRawImageの色変更
    /// </summary>
    public class TransmitColorRawImage : ColorTransition
    {
        [SerializeField] private RawImage target;

#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.TryGetComponent(out target);
        }
#endif

        private void Awake()
        {
            TryGetComponent(out target);
            target.color = normalColor;
        }

        protected override void ChangeColorLerp(Color toColor, float delta)
        {
            target.color = Color.Lerp(target.color, toColor, delta);
        }

        protected override void ChangeColor(Color toColor)
        {
            target.color = toColor;
        }
    }
}