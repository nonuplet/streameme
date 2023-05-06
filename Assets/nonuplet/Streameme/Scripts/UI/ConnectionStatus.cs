using System;
using Streameme;
using Streameme.UI;
using UnityEngine;

public class ConnectionStatus : MonoBehaviour
{
    [SerializeField] private StreamemeCore core;
    private MemeReceiver _receiver;

    [SerializeField] public CanvasGroupTransition connected;
    [SerializeField] public CanvasGroupTransition disconnected;

#if UNITY_EDITOR
    private void Reset()
    {
        GameObject.Find("Streameme").TryGetComponent(out core);
        transform.Find("Connected").TryGetComponent(out connected);
        transform.Find("Disconnected").TryGetComponent(out disconnected);
    }
#endif

    private void Awake()
    {
        _receiver = core.memeReceiver;
        _receiver.onMemeConnected.AddListener(OnMemeConnected);
        _receiver.onMemeDisconnected.AddListener(OnMemeDisconnected);
    }

    private void Start()
    {
        disconnected.SetActive();
    }

    private void OnMemeConnected()
    {
        connected.SetActive();
        disconnected.SetInactive();
    }

    private void OnMemeDisconnected()
    {
        print("ondis");
        connected.SetInactive();
        disconnected.SetActive();
    }
}