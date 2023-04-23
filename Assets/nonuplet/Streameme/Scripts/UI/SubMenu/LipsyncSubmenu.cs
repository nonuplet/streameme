using Streameme.Avatar;
using TMPro;
using uLipSync;
using UnityEngine;
using UnityEngine.UI;

namespace Streameme.UI.SubMenu
{
    /// <summary>
    /// リップシンクのサブメニュー
    /// </summary>
    public class LipsyncSubmenu : BottomSubMenu
    {
        [SerializeField] private Toggle isLipsyncActive;
        [SerializeField] private TMP_Dropdown dropdown;

        private LipSyncController _lipSync;

        /// <summary>
        /// Dropdownリスト更新中のフラグ
        /// </summary>
        private bool _updating = false;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            var panel = transform.Find("Panel");
            panel.transform.Find("LipSync Toggle").TryGetComponent(out isLipsyncActive);
            panel.transform.Find("Devices").TryGetComponent(out dropdown);
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            _lipSync = core.lipSync;

            // LipSyncのON/OFFボタン
            isLipsyncActive.onValueChanged.AddListener(SetLipSyncActive);

            dropdown.onValueChanged.AddListener(ChangeMic);
        }

        /// <summary>
        /// リップシンクの起動・停止
        /// </summary>
        /// <param name="active"></param>
        public void SetLipSyncActive(bool active)
        {
            if (active)
            {
                _lipSync.StartLipSync();
                // TODO: オプションをActivateする
            }
            else
            {
                _lipSync.StopLipSync();
                // TODO: オプションをDeactivateする
            }
        }

        /// <summary>
        /// デバイス一覧のドロップダウン更新
        /// </summary>
        public void UpdateDevicesDropdown()
        {
            _updating = true;

            var options = dropdown.options;
            options.Clear();
            var devices = MicUtil.GetDeviceList();
            foreach (var device in devices)
            {
                options.Add(new TMP_Dropdown.OptionData(device.name));
            }

            dropdown.value = _lipSync._uLipMic.device.index;
            _updating = false;
        }

        /// <summary>
        /// マイクの切り替え
        /// </summary>
        /// <param name="index">MicDeviceのindex</param>
        public void ChangeMic(int index)
        {
            // Dropdownの更新中もOnValueChangedが走ってしまうため除外
            if (_updating) return;
            _lipSync.ChangeMic(index);
        }

        public override void SetActive()
        {
            base.SetActive();
            UpdateDevicesDropdown();
        }
    }
}