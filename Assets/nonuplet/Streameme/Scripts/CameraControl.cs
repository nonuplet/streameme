using UnityEngine;

namespace Streameme
{
    /// <summary>
    /// カメラの操作
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        public enum CameraMode
        {
            Front,
            Back,
            Free,
            None
        }

        public enum BackgroundMode
        {
            Skybox,
            GreenBack,
            BlueBack
        }

        [SerializeField] private Transform anchor;
        [SerializeField] private Transform camTransform;
        [SerializeField] private Camera mainCam;

        private Vector3 _center, _front, _back;
        private Vector3 _camAngle;
        private Vector3 _freePos, _freeAngle;

        /// <summary>
        /// カメラがセットアップ中かどうか（リセット中にtransformがUpdate側で勝手に操作されることの防止）
        /// </summary>
        private bool _isSetup = false;

        /// <summary>
        /// カメラのモード(Front/Back/Free)
        /// </summary>
        public CameraMode camMode = CameraMode.None;

        public BackgroundMode bgMode = BackgroundMode.Skybox;

        /// <summary>
        /// カメラ移動時のマウス感度
        /// </summary>
        [SerializeField] public float sensitivity = 1.0f;

#if UNITY_EDITOR
        private void OnValidate()
        {
            transform.Find("Anchor Point").gameObject.TryGetComponent(out anchor);
            anchor.Find("Main Camera").gameObject.TryGetComponent(out camTransform);
            camTransform.gameObject.TryGetComponent(out mainCam);
        }
#endif

        private void Awake()
        {
            _center = Vector3.zero;
            _front = Vector3.zero;
            _back = Vector3.zero;
            _camAngle = Vector3.zero;
            _freePos = Vector3.zero;
            _freeAngle = Vector3.zero;
        }

        /// <summary>
        /// アバターがロードされた際に呼び出して正面等の位置を調整
        /// </summary>
        /// <param name="head">アバターの頭のボーン位置</param>
        public void ResetPosition(Transform head)
        {
            var beforeCamMode = camMode;
            camMode = CameraMode.None;

            _isSetup = true;
            var pos = head.position;
            _center = pos;
            _front = new Vector3(pos.x, pos.y, pos.z + 1f);
            _back = new Vector3(pos.x, pos.y, pos.z - 1f);

            _freePos = _front;
            _freeAngle = new Vector3(0f, 180f, 0f);

            _isSetup = false;

            switch (beforeCamMode)
            {
                case CameraMode.None:
                case CameraMode.Front:
                    SetCameraFront();
                    break;
                case CameraMode.Back:
                    SetCameraBack();
                    break;
                case CameraMode.Free:
                    SetCameraFree();
                    break;
            }
        }

        public void SetCameraFront()
        {
            if (_isSetup || camMode == CameraMode.Front) return;

            _isSetup = true;

            if (camMode == CameraMode.Free)
            {
                _freePos = camTransform.position;
                _freeAngle = _camAngle;
            }

            camMode = CameraMode.Front;
            _camAngle = new Vector3(0f, 0f, 0f);

            anchor.position = _center;
            anchor.rotation = Quaternion.Euler(_camAngle);
            camTransform.position = _front;
            camTransform.rotation = Quaternion.Euler(0f, 180f, 0f);

            _isSetup = false;
        }

        public void SetCameraBack()
        {
            if (_isSetup || camMode == CameraMode.Back) return;

            _isSetup = true;

            if (camMode == CameraMode.Free)
            {
                _freePos = camTransform.position;
                _freeAngle = _camAngle;
            }

            camMode = CameraMode.Back;
            _camAngle = new Vector3(0f, 0f, 0f);

            anchor.position = _center;
            anchor.rotation = Quaternion.Euler(_camAngle);
            camTransform.position = _back;
            camTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

            _isSetup = false;
        }

        public void SetCameraFree()
        {
            if (_isSetup || camMode == CameraMode.Free) return;

            _isSetup = true;

            camMode = CameraMode.Free;
            _camAngle = _freeAngle;

            anchor.position = _center;
            anchor.localRotation = Quaternion.Euler(0f, 0f, 0f);
            camTransform.position = _freePos;
            camTransform.rotation = Quaternion.Euler(_camAngle);

            _isSetup = false;
        }

        public void SetBackground(BackgroundMode mode)
        {
            switch (mode)
            {
                case BackgroundMode.Skybox:
                    mainCam.clearFlags = CameraClearFlags.Skybox;
                    break;
                case BackgroundMode.GreenBack:
                    mainCam.clearFlags = CameraClearFlags.SolidColor;
                    mainCam.backgroundColor = Color.green;
                    break;
                case BackgroundMode.BlueBack:
                    mainCam.clearFlags = CameraClearFlags.SolidColor;
                    mainCam.backgroundColor = Color.blue;
                    break;
            }

            bgMode = mode;
        }


        private void Update()
        {
            if (!_isSetup) // アバターロード中はカメラ操作を禁止
            {
                // カメラ操作
                float x_delta = Input.GetAxis("Mouse X") * sensitivity;
                float y_delta = Input.GetAxis("Mouse Y") * sensitivity;
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (Input.GetMouseButton(1) && !Input.GetMouseButtonDown(1))
                {
                    // 回転
                    var x = x_delta * 100f;
                    var y = y_delta * 100f;
                    switch (camMode)
                    {
                        case CameraMode.Front:
                            _camAngle.x = (_camAngle.x + y) % 360;
                            _camAngle.y = (_camAngle.y + x) % 360;
                            anchor.rotation = Quaternion.Euler(_camAngle);
                            break;
                        case CameraMode.Back:
                            _camAngle.x = (_camAngle.x - y) % 360;
                            _camAngle.y = (_camAngle.y + x) % 360;
                            anchor.rotation = Quaternion.Euler(_camAngle);
                            break;
                        case CameraMode.Free:
                            _camAngle.x = (_camAngle.x + y) % 360;
                            _camAngle.y = (_camAngle.y + x) % 360;
                            camTransform.rotation = Quaternion.Euler(_camAngle);
                            break;
                    }
                }

                if (Input.GetMouseButton(2))
                {
                    // 平面移動
                    camTransform.Translate(-x_delta, -y_delta, 0, Space.Self);
                }

                if (scroll != 0f)
                {
                    // ドリーイン・アウト
                    camTransform.Translate(0, 0, scroll, Space.Self);
                }

                // キー操作
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SetCameraFront();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SetCameraBack();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    SetCameraFree();
                }
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SetBackground(BackgroundMode.Skybox);
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    SetBackground(BackgroundMode.GreenBack);
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    SetBackground(BackgroundMode.BlueBack);
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                SetBackground(BackgroundMode.Skybox);
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                SetBackground(BackgroundMode.GreenBack);
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                SetBackground(BackgroundMode.BlueBack);
            }
        }
    }
}