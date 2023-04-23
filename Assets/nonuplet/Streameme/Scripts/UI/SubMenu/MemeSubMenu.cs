using Streameme.UI;
using Streameme.Avatar;
using TMPro;
using UnityEngine;

namespace Streameme.UI.SubMenu
{
    /// <summary>
    /// MEMEのサブメニュー
    /// </summary>
    public class MemeSubMenu : BottomSubMenu
    {
        [SerializeField] private ButtonColorTransmitter yawResetButton;
        [SerializeField] private TMP_InputField portField;

        private bool _init = false;

#if UNITY_EDITOR
        protected override void Reset()
        {
            GameObject.Find("Strememe").TryGetComponent(out core);
            var panel = transform.Find("Panel");
            panel.transform.Find("Button_YawReset").TryGetComponent(out yawResetButton);
            panel.transform.Find("Textfield_Port").TryGetComponent(out portField);
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            portField.onValueChanged.AddListener(OnPortChanged);
        }

        public override void SetActive()
        {
            _init = true;
            base.SetActive();
            UpdatePortNumber();
            _init = false;
        }

        /// <summary>
        /// ポート番号の値を取得し表示を更新
        /// </summary>
        private void UpdatePortNumber()
        {
            portField.text = core.memeReceiver.websocketPort.ToString();
        }

        /// <summary>
        /// ポート番号を変更
        /// </summary>
        /// <param name="str"></param>
        private void OnPortChanged(string str)
        {
            if (_init) return;
            if (int.TryParse(str, out var port))
            {
                core.memeReceiver.ChangePort(port);
            }
            else
            {
                Debug.LogError("ポート番号でint型が指定されていません");
            }
        }

        /// <summary>
        /// モデルロード時にボタンが参照するメソッドを更新
        /// </summary>
        /// <param name="motion"></param>
        public void OnModelLoaded(MotionController motion)
        {
            yawResetButton.onClick.RemoveAllListeners();
            yawResetButton.onClick.AddListener(motion.ResetFrontYaw);
        }
    }
}