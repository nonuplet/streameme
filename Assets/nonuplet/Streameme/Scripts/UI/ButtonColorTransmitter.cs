using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Streameme.UI
{
    /// <summary>
    /// ボタン操作時に下のテキスト・アイコン色を操作
    /// </summary>
    public class ButtonColorTransmitter : Button
    {
        [SerializeField] public List<ColorTransition> targets;
        private BaseEventData _tempEventData;
        public float posZ;

        protected override void Awake()
        {
            base.Awake();
            gameObject.TryGetComponent(out RectTransform rect);
            posZ = rect.localPosition.z;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (targets == null) return;

            foreach (var target in targets)
            {
                switch (state)
                {
                    case SelectionState.Normal:
                        target.SetState(ColorTransition.Selection.Normal);
                        break;
                    case SelectionState.Highlighted:
                        target.SetState(ColorTransition.Selection.Highlighted);
                        break;
                    case SelectionState.Pressed:
                        target.SetState(ColorTransition.Selection.Pressed);
                        break;
                    case SelectionState.Selected:
                        target.SetState(ColorTransition.Selection.Selected);
                        break;
                    case SelectionState.Disabled:
                        target.SetState(ColorTransition.Selection.Disabled);
                        break;
                }
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            SelectedManager.SetNewSelected(this);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            _tempEventData = eventData;
            return;
            //print($"Deselected: {gameObject.name}");
        }

        public virtual void OnCustomDeselect()
        {
            base.OnDeselect(_tempEventData);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonColorTransmitter))]
    public class ButtonColorTransmitterEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var component = (ButtonColorTransmitter)target;
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(component.targets)),
                new GUIContent("Child Color Transitions"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}