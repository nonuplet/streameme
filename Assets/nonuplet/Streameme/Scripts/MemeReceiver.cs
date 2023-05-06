using System;
using System.Windows.Forms;
using Streameme.Avatar;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Streameme
{
    /// <summary>
    /// 接続に関するイベント
    /// </summary>
    public enum ConnectionEvent
    {
        None,
        OnOpen,
        OnClose
    }
    
    /// <summary>
    /// MEMEのデータ取得
    /// </summary>
    public class MemeReceiver : MonoBehaviour
    {
        private WebSocketServer _ws;
        public int websocketPort = 5000;

        public MotionController _motion;

        public UnityEvent onMemeConnected = new UnityEvent();
        public UnityEvent onMemeDisconnected = new UnityEvent();

        private ConnectionEvent _connectionEvent = ConnectionEvent.None;

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

        private void Update()
        {
            // UnityEventのMain Thread問題対策
            if (_connectionEvent == ConnectionEvent.OnOpen)
            {
                onMemeConnected.Invoke();
                _connectionEvent = ConnectionEvent.None;
            }
            else if (_connectionEvent == ConnectionEvent.OnClose)
            {
                onMemeDisconnected.Invoke();
                _connectionEvent = ConnectionEvent.None;
            }
        }

        /// <summary>
        /// Websocketの受付開始
        /// </summary>
        private void StartListen()
        {
            _ws = new WebSocketServer(websocketPort);
            _ws.AddWebSocketService<OnReceived>("/",
                serverBehavior =>
                {
                    serverBehavior.SetOnMessageEvent(OnMessage);
                    serverBehavior.SetOnConnectionEvent(OnConnectionEvent);
                });
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
        
        
        private void OnConnectionEvent(ConnectionEvent connection)
        {
            // ここでEventをここでInvokeしようとすると、以下のエラーとなるためUpdate内で処理するように対策
            // "UnityEngine.UnityException: set_interactable can only be called from the main thread."
            _connectionEvent = connection;
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
        private OnConnectionEventDelegate _onConnectionEvent;

        public delegate void OnMessageDelegate(MessageEventArgs e);

        public delegate void OnConnectionEventDelegate(ConnectionEvent connection);

        public void SetOnMessageEvent(OnMessageDelegate dlg)
        {
            _onMessage = dlg;
        }

        public void SetOnConnectionEvent(OnConnectionEventDelegate dlg)
        {
            _onConnectionEvent = dlg;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _onMessage(e);
        }

        protected override void OnOpen()
        {
            _onConnectionEvent(ConnectionEvent.OnOpen);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _onConnectionEvent(ConnectionEvent.OnClose);
        }
    }
}