using Streameme.Avatar;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Streameme
{
    /// <summary>
    /// MEMEのデータ取得
    /// </summary>
    public class MemeReceiver : MonoBehaviour
    {
        private WebSocketServer _ws;
        public int websocketPort = 5000;

        public MotionController _motion;

        private float _frontYaw;
        private bool _loaded = false;

        private void OnEnable()
        {
            StartListen();
        }

        private void OnDisable()
        {
            StopListen();
        }

        /// <summary>
        /// Websocketの受付開始
        /// </summary>
        private void StartListen()
        {
            _ws = new WebSocketServer(websocketPort);
            _ws.AddWebSocketService<OnReceived>("/",
                serverBehavior => { serverBehavior.SetOnMessageEvent(OnMessage); });
            _ws.Start();
        }

        /// <summary>
        /// Websocketの受付停止
        /// </summary>
        private void StopListen()
        {
            _ws.Stop();
            _ws.RemoveWebSocketService("/");
            _ws = null;
        }

        /// <summary>
        /// Websocketのポート変更
        /// </summary>
        /// <param name="port">ポート番号</param>
        public void ChangePort(int port)
        {
            StopListen();
            websocketPort = port;
            StartListen();
        }

        public void SetMotion(MotionController motion)
        {
            _motion = motion;
            _loaded = true;
        }

        public void ResetMotion()
        {
            _motion = null;
            _loaded = false;
        }

        private void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data;
            if (msg.Contains("heartbeat") || !_loaded) return;
            _motion.SetCurrentData(JsonUtility.FromJson<MemeCurrentData>(msg));
        }

        // private void SendOscTest()
        // { TODO: 別のクラスへ移行
        //     var pos = _neck.localPosition;
        //     var rot = _neck.localRotation;
        //     _osc.Send("/VMC/Ext/Bone/Pos", "Neck", 
        //         pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);
        // }
    }

    public class OnReceived : WebSocketBehavior
    {
        private OnMessageDelegate _onMessage;

        public delegate void OnMessageDelegate(MessageEventArgs e);

        public void SetOnMessageEvent(OnMessageDelegate dlg)
        {
            _onMessage = dlg;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _onMessage(e);
        }
    }
}