using System.Collections;
using UnityEngine;

namespace Streameme.UI
{
    /// <summary>
    /// Canvas Groupのフェードイン・フェードアウト
    /// </summary>
    public class CanvasGroupTransition : MonoBehaviour
    {
        public CanvasGroup canvas;
        public bool isActive;
        public float duration = 0.1f;
        public bool isAutoActive = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.TryGetComponent(out canvas);
        }
#endif

        private void Awake()
        {
            gameObject.TryGetComponent(out canvas);

            if (isAutoActive)
            {
                isActive = true;
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
                canvas.alpha = 1f;
            }
            else
            {
                isActive = false;
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
                canvas.alpha = 0;
            }
        }

        public void SetActive()
        {
            if (isActive) return;
            isActive = true;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            StartCoroutine(Transition(1f));
        }

        public void SetInactive()
        {
            if (!isActive) return;
            isActive = false;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
            StartCoroutine(Transition(0f));
        }

        private IEnumerator Transition(float alpha)
        {
            for (var f = 0f; f <= duration; f += Time.deltaTime)
            {
                canvas.alpha = Mathf.Lerp(canvas.alpha, alpha, f);
                yield return null;
            }

            canvas.alpha = alpha;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                SetActive();
            }

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                SetInactive();
            }
        }
    }
}