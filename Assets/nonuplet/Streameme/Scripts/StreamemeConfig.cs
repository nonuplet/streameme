using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Streameme
{
    [Serializable]
    public class Config
    {
        /** memeのWebsocketポート */
        public int websocketPort;

        /** 背景モード */
        public CameraControl.BackgroundMode bgMode;

        /** マイクのデバイス */
        public string micDevice;

        /** ロードしたVRMのパス */
        public string vrmPath;

        public static Config GetDefault()
        {
            var config = new Config();
            config.websocketPort = 5000;
            config.bgMode = CameraControl.BackgroundMode.Skybox;
            config.micDevice = "";
            config.vrmPath = "";
            return config;
        }
    }

    [Serializable]
    public class StreamemeConfig : MonoBehaviour
    {
        [NonSerialized]
        public static Config config;

        private void Awake()
        {
            if (!Load())
            {
                config = Config.GetDefault();
            }
        }

        public static bool Load()
        {
            return Load(Application.dataPath + "/defaultSetting.json");
        }

        public static bool Load(string path)
        {
            if (!File.Exists(path)) return false;
            var reader = new StreamReader(path, Encoding.GetEncoding("utf-8"));
            var raw = reader.ReadToEnd();
            reader.Close();
            config = JsonUtility.FromJson<Config>(raw);
            return true;
        }

        public static void Save()
        {
            Save(Application.dataPath + "/defaultSetting.json");
        }

        public static void Save(string path)
        {
            var json = JsonUtility.ToJson(config);
            var writer = new StreamWriter(path, false, Encoding.GetEncoding("utf-8"));
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }
    }
}