using System.Collections.Generic;
using Streameme;
using UniHumanoid;
using UnityEngine;
using VRM;

namespace Streameme.Avatar
{
    /// <summary>
    /// VRMアバターを操作するためのコントローラクラス
    /// VRMのPrefabに直接アタッチすることを想定しています
    /// </summary>
    public class MotionController : MonoBehaviour
    {
        /// <summary>
        /// NeckのBone
        /// </summary>
        private Transform _neck;

        /// <summary>
        /// SpineのBone
        /// </summary>
        private Transform _spine;

        /// <summary>
        /// 角度何度以上でSpineを傾け始めるか
        /// </summary>
        [SerializeField] private float _spineRotateLimit = 23.0f;

        /// <summary>
        /// VRMのBlendshape
        /// </summary>
        private VRMBlendShapeProxy _blendShape;

        /// <summary>
        /// JINS MEMEから送られてくるcurrentData(20Hz Data)の保持変数
        /// </summary>
        private MemeCurrentData _meme;

        /// <summary>
        /// JINS MEMEのPitch/Yaw/Rollデータ
        /// </summary>
        private Vector3 _memeRot = Vector3.zero;

        /// <summary>
        /// 初期状態かどうか（yawの初期値を設定するため）
        /// </summary>
        private bool _init = true;

        /// <summary>
        /// 正面を向いた時のYawの値、未接続状態の時はマイナスの値となる
        /// </summary>
        private float _frontYaw = -1f;

        /// <summary>
        /// Lerp用
        /// </summary>
        private float _delta = 0;

        /// <summary>
        /// 読み込んだVRMの初期設定
        /// （AddComponentを行うことにより、OnEnableやStartが機能しないため）
        /// </summary>
        public void SetupAvatar(RuntimeAnimatorController controller)
        {
            // 各BoneのTransform取得
            var humanoid = gameObject.AddComponent<Humanoid>();
            humanoid.AssignBonesFromAnimator();
            _neck = humanoid.Neck;
            _spine = humanoid.Spine;

            // Blendshape Proxyの取得
            gameObject.TryGetComponent(out _blendShape);

            // Animatorのセットアップ
            gameObject.TryGetComponent(out Animator animator);
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            animator.runtimeAnimatorController = controller;
        }

        private void Update()
        {
            if (_meme == null) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetFrontYaw();
                _init = false;
            }

            if (_init)
            {
                ResetFrontYaw();
                _init = false;
            }
            else
            {
                // 補完フレームの処理
                Vector3 rot;
                if (_delta > 0.05f)
                {
                    rot = Vector3.Lerp(_neck.localRotation.eulerAngles, _memeRot, _delta * 20f);
                    _delta += Time.deltaTime;
                }
                else
                {
                    rot = _memeRot;
                }


                // NeckのRotation設定
                _neck.localRotation = Quaternion.Euler(rot);

                // SpineのRotation設定
                if (Mathf.Abs(rot.z) > _spineRotateLimit)
                {
                    _spine.localRotation = Quaternion.Euler(0, 0,
                        rot.z > 0 ? rot.z - _spineRotateLimit : rot.z + _spineRotateLimit);
                }

                // まばたき
                if (!_meme.noiseStatus && _meme.fitError == 0 && !_meme.walking)
                {
                    if (_meme.blinkStrength > 0)
                    {
                        // ON
                        ChangeBlendShapeFromPreset(BlendShapePreset.Blink, 1.0f);
                    }
                    else
                    {
                        // OFF
                        ChangeBlendShapeFromPreset(BlendShapePreset.Blink, 0.0f);
                    }
                }
            }
        }

        /// <summary>
        /// 正面を向いた時のYaw値の設定
        /// </summary>
        public void ResetFrontYaw()
        {
            _frontYaw = _meme.yaw;
        }

        /// <summary>
        /// JINS MEMEから新しい情報が送られてきた際に処理用データのセット
        /// </summary>
        /// <param name="current">currentData(20Hz Data)</param>
        public void SetCurrentData(MemeCurrentData current)
        {
            _meme = current;
            _delta = 0;
            if (_frontYaw < 0) ResetFrontYaw();
            _memeRot = new Vector3(_meme.pitch, _meme.yaw - _frontYaw, _meme.roll);
        }

        /// <summary>
        /// BlendShapeの更新（標準にあるもの）
        /// </summary>
        /// <param name="preset">BlandShapePreset(Blinkなど)</param>
        /// <param name="value">変更値</param>
        public void ChangeBlendShapeFromPreset(BlendShapePreset preset, float value)
        {
            _blendShape.SetValues(new Dictionary<BlendShapeKey, float>
            {
                { BlendShapeKey.CreateFromPreset(preset), value }
            });
        }
    }
}