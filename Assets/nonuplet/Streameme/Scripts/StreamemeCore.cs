using Streameme.Avatar;
using Streameme.UI;
using Streameme.UI.SubMenu;
using UniHumanoid;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Streameme
{
    /// <summary>
    /// メインスクリプト
    /// </summary>
    public class StreamemeCore : MonoBehaviour
    {
        public MemeReceiver memeReceiver;
        public LoadVrmFile vrmLoader;
        public EventSystem eventSystem;
        public CameraControl cameraControl;
        public LipSyncController lipSync;

        public RuntimeAnimatorController animControllerAsset;
        public ButtonColorTransmitter vrmLoadButton;
        public MemeSubMenu memeSubMenu;
        public CanvasGroupTransition mainCanvas;

        private GameObject _avatar;
        private MotionController _motion;
        private Humanoid _humanoid;
        private bool _isCanvasActive = true;


#if UNITY_EDITOR
        private void Reset()
        {
            transform.Find("MemeReceiver").gameObject.TryGetComponent(out memeReceiver);
            transform.Find("VRMLoader").gameObject.TryGetComponent(out vrmLoader);
            transform.Find("LipSync").gameObject.TryGetComponent(out lipSync);
            GameObject.Find("EventSystem").gameObject.TryGetComponent(out eventSystem);
            GameObject.Find("CameraControl").gameObject.TryGetComponent(out cameraControl);
            GameObject.Find("Canvas").gameObject.TryGetComponent(out mainCanvas);
            GameObject.Find("SubMenu_Meme").gameObject.TryGetComponent(out memeSubMenu);
        }
#endif

        private void Awake()
        {
            vrmLoadButton.onClick.AddListener(LoadVrm);
        }

        private void Update()
        {
            // UIの表示・非表示
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (_isCanvasActive)
                {
                    mainCanvas.SetInactive();
                    _isCanvasActive = false;
                }
                else
                {
                    mainCanvas.SetActive();
                    _isCanvasActive = true;
                }
            }
        }

        /// <summary>
        /// VRMの読み込み
        /// </summary>
        private async void LoadVrm()
        {
            // ランタイムロード
            var oldAvatar = _avatar;
            var avatar = await vrmLoader.OpenFile();
            eventSystem.SetSelectedGameObject(null);
            if (avatar == null) return;

            // 元々読み込んでいたアバターがあればDestroy
            memeReceiver.ResetMotion();
            if (oldAvatar != null) GameObject.Destroy(oldAvatar);

            // アバターのセットアップ
            _avatar = avatar;
            _motion = _avatar.AddComponent<MotionController>();
            _motion.SetupAvatar(animControllerAsset);
            _avatar.TryGetComponent(out _humanoid);
            memeReceiver.SetMotion(_motion);

            // カメラのリセット
            cameraControl.ResetPosition(_humanoid.Head);

            // リップシンクのセットアップ
            lipSync.SetupLipSync(_avatar);

            // Yawをリセットするボタンの参照先を変更
            memeSubMenu.OnModelLoaded(_motion);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(StreamemeCore))]
    public class StreamemeCoreEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var core = (StreamemeCore)target;
            EditorGUILayout.LabelField("Scripts & Objects", EditorStyles.boldLabel);
            addField(nameof(core.memeReceiver), "MemeReceiver");
            addField(nameof(core.vrmLoader), "VRMLoader");
            addField(nameof(core.cameraControl), "Camera Control");
            addField(nameof(core.lipSync), "Lipsync Controller");
            addField(nameof(core.eventSystem), "Event System");
            addField(nameof(core.mainCanvas), "Main Canvas Group");
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            addField(nameof(core.vrmLoadButton), "VRMLoad Button");
            addField(nameof(core.memeSubMenu), "MEME Submenu");
            EditorGUILayout.LabelField("Assets", EditorStyles.boldLabel);
            addField(nameof(core.animControllerAsset), "Animator Controller");
            serializedObject.ApplyModifiedProperties();
        }

        private void addField(string property, string label)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label));
        }
    }
#endif
}