using Streameme;
using Streameme.UI;
using UnityEngine;

namespace Streameme.UI.SubMenu
{
    /// <summary>
    /// サブメニューの基底クラス
    /// </summary>
    public abstract class BottomSubMenu : MonoBehaviour
    {
        public StrememeCore core;
        private CanvasGroupTransition _canvasGroup;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            GameObject.Find("Strememe").TryGetComponent(out core);
        }
#endif

        /// <summary>
        /// サブメニュー開いた時
        /// </summary>
        public virtual void SetActive()
        {
            _canvasGroup.SetActive();
        }

        /// <summary>
        /// サブメニューを閉じた時
        /// </summary>
        public virtual void SetInactive()
        {
            _canvasGroup.SetInactive();
        }

        protected virtual void Awake()
        {
            gameObject.TryGetComponent(out _canvasGroup);
        }
    }
}