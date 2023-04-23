using Streameme.UI.SubMenu;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Streameme.UI
{
    /// <summary>
    /// 画面下のメニューボタン
    /// </summary>
    public class BottomMenuButton : ButtonColorTransmitter
    {
        public BottomSubMenu subMenu;

        protected override void Awake()
        {
            base.Awake();
            gameObject.TryGetComponent(out RectTransform rect);
        }

        /// <summary>
        /// OnPointerClick実装のためにオーバーライドで無効化
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnSelect(BaseEventData eventData)
        {
            return;
        }

        public override void OnCustomDeselect()
        {
            base.OnCustomDeselect();
            subMenu.SetInactive();
        }

        /// <summary>
        /// 通常時は選択・既に選択されていた場合はメニューオフ
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (SelectedManager.IsSelected(this))
            {
                SelectedManager.AllDeselect();
            }
            else
            {
                base.OnSelect(eventData);
                subMenu.SetActive();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BottomMenuButton))]
    public class BottomMenuButtonEditor : ButtonColorTransmitterEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var component = (BottomMenuButton)target;
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(component.subMenu)),
                new GUIContent("Target Submenu"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}