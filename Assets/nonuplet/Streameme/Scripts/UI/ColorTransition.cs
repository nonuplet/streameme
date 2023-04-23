using System;
using System.Collections;
using UnityEngine;

namespace Streameme.UI
{
    /// <summary>
    /// ButtonColorTransmitterの受け側
    /// </summary>
    [Serializable]
    public abstract class ColorTransition : MonoBehaviour
    {
        public enum Selection
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled
        }

        [SerializeField] protected Color normalColor = Color.white;
        [SerializeField] protected Color highlightedColor = Color.white;
        [SerializeField] protected Color pressedColor = Color.white;
        [SerializeField] protected Color selectedColor = Color.white;
        [SerializeField] protected Color disabledColor = Color.white;
        [SerializeField] protected float fadeDuration = 0.1f;
        private bool _started = false;

        protected abstract void ChangeColorLerp(Color toColor, float delta);
        protected abstract void ChangeColor(Color toColor);

        protected IEnumerator Transition(Color toColor)
        {
            for (var f = 0f; f <= fadeDuration; f += Time.deltaTime)
            {
                ChangeColorLerp(toColor, f);
                yield return null;
            }

            ChangeColor(toColor);
        }

        public void SetState(Selection state)
        {
            if (_started) StopAllCoroutines();
            else _started = true;

            switch (state)
            {
                case Selection.Normal:
                    StartCoroutine(Transition(normalColor));
                    break;
                case Selection.Highlighted:
                    StartCoroutine(Transition(highlightedColor));
                    break;
                case Selection.Pressed:
                    StartCoroutine(Transition(pressedColor));
                    break;
                case Selection.Selected:
                    StartCoroutine(Transition(selectedColor));
                    break;
                case Selection.Disabled:
                    StartCoroutine(Transition(disabledColor));
                    break;
            }
        }
    }
}