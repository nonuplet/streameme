using Streameme;
using Streameme.UI;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicator : MonoBehaviour
{
    [SerializeField] private StreamemeCore core;
    private MemeReceiver _receiver;

    private CanvasGroupTransition _batteryCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private Image gauge;
    [SerializeField] private RawImage frame;

#if UNITY_EDITOR
    private void Reset()
    {
        GameObject.Find("Streameme").TryGetComponent(out core);
        transform.Find("Slider").TryGetComponent(out slider);
        transform.Find("Slider").Find("Fill Area").Find("Fill").TryGetComponent(out gauge);
        transform.Find("Frame").TryGetComponent(out frame);
    }
#endif

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnChangeBatteryValue);
        _receiver = core.memeReceiver;
        transform.TryGetComponent(out _batteryCanvas);
        _receiver.onMemeConnected.AddListener(_batteryCanvas.SetActive);
        _receiver.onMemeDisconnected.AddListener(_batteryCanvas.SetInactive);
    }

    public void SetBatteryValue(float value)
    {
        slider.value = value;
    }

    private void OnChangeBatteryValue(float value)
    {
        // Color
        if (value > 2)
        {
            frame.color = new Color(255, 255, 255, 150);
            gauge.color = new Color(0, 255, 0, 150);
        }
        else if (value > 1)
        {
            frame.color = new Color(255, 255, 255, 150);
            gauge.color = new Color(255, 0, 0, 150);
        }
        else
        {
            frame.color = new Color(255, 0, 0, 150);
        }
    }
}
