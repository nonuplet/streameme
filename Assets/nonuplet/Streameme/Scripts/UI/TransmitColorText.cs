using TMPro;
using UnityEngine;

namespace Streameme.UI
{
    /// <summary>
    /// ボタンのセレクト状態に合わせてTextの色変更
    /// </summary>
    public class TransmitColorText : ColorTransition
    {
        [SerializeField] private TextMeshProUGUI target;

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