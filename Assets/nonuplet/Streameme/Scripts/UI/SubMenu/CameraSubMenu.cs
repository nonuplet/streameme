using Streameme;
using Streameme.UI;
using UnityEngine;

namespace Streameme.UI.SubMenu
{
    /// <summary>
    /// カメラ操作のサブメニュー
    /// </summary>
    public class CameraSubMenu : BottomSubMenu
    {
        [SerializeField] private ButtonColorTransmitter front, back, free;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.Find("Button_Front").TryGetComponent(out front);
            transform.Find("Button_Back").TryGetComponent(out back);
            transform.Find("Button_Free").TryGetComponent(out free);
        }

#endif

        private void Start()
        {
            front.onClick.AddListener(core.cameraControl.SetCameraFront);
            back.onClick.AddListener(core.cameraControl.SetCameraBack);
            free.onClick.AddListener(core.cameraControl.SetCameraFree);
        }

        public override void SetActive()
        {
            base.SetActive();
            switch (core.cameraControl.camMode)
            {
                case CameraControl.CameraMode.Front:
                    front.Select();
                    break;
                case CameraControl.CameraMode.Back:
                    back.Select();
                    break;
                case CameraControl.CameraMode.Free:
                    free.Select();
                    break;
            }
        }
    }
}