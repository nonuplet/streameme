using Streameme;
using Streameme.UI;
using UnityEngine;

namespace Streameme.UI.SubMenu
{
    /// <summary>
    /// 背景色変更のサブメニュー
    /// </summary>
    public class BackgroundSubMenu : BottomSubMenu
    {
        [SerializeField] private ButtonColorTransmitter skybox, gb, bb;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.Find("Button_Skybox").TryGetComponent(out skybox);
            transform.Find("Button_GB").TryGetComponent(out gb);
            transform.Find("Button_BB").TryGetComponent(out bb);
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            skybox.onClick.AddListener(() => core.cameraControl.SetBackground(CameraControl.BackgroundMode.Skybox));
            gb.onClick.AddListener(() => core.cameraControl.SetBackground(CameraControl.BackgroundMode.GreenBack));
            bb.onClick.AddListener(() => core.cameraControl.SetBackground(CameraControl.BackgroundMode.BlueBack));
        }

        public override void SetActive()
        {
            base.SetActive();
            switch (core.cameraControl.bgMode)
            {
                case CameraControl.BackgroundMode.Skybox:
                    skybox.Select();
                    break;
                case CameraControl.BackgroundMode.GreenBack:
                    gb.Select();
                    break;
                case CameraControl.BackgroundMode.BlueBack:
                    bb.Select();
                    break;
            }
        }
    }
}