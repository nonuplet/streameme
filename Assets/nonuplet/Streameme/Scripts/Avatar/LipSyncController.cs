using System;
using uLipSync;
using UnityEngine;

namespace Streameme.Avatar
{
    /// <summary>
    /// リップシンクの操作
    /// </summary>
    public class LipSyncController : MonoBehaviour
    {
        private GameObject _model;
        private uLipSyncBlendShapeVRM _lipBlend;
        private uLipSyncAudioSource audo;
        private uLipSync.uLipSync _uLipSync;
        [NonSerialized] public uLipSyncMicrophone _uLipMic;

        private void Awake()
        {
            gameObject.TryGetComponent(out _uLipSync);
            gameObject.TryGetComponent(out _uLipMic);
        }

        /// <summary>
        /// リップシンクのセットアップ（モデル読み込み時に実施）
        /// </summary>
        /// <param name="model"></param>
        public void SetupLipSync(GameObject model)
        {
            _model = model;

            _lipBlend = _model.AddComponent<uLipSyncBlendShapeVRM>();
            string[] phonemes = { "A", "I", "U", "E", "O" };
            foreach (var phoneme in phonemes)
            {
                _lipBlend.AddBlendShape(phoneme, phoneme);
            }

            _uLipSync.onLipSyncUpdate.AddListener(_lipBlend.OnLipSyncUpdate);
            _uLipMic.StartRecord();
        }

        /// <summary>
        /// リップシンクを起動する
        /// </summary>
        public void StartLipSync()
        {
            if (_uLipMic.isRecording) return;
            _uLipMic.StartRecord();
        }

        /// <summary>
        /// リップシンクを停止する
        /// </summary>
        public void StopLipSync()
        {
            if (!_uLipMic.isRecording) return;
            _uLipMic.StopRecord();
        }

        /// <summary>
        /// マイクの変更
        /// </summary>
        /// <param name="index">MicDeviceのindex</param>
        public void ChangeMic(int index)
        {
            _uLipMic.StopRecord();
            _uLipMic.index = index;
            _uLipMic.StartRecord();
        }
    }
}