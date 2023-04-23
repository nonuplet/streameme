using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Streameme.Editor
{
    /// <summary>
    /// ビルド前処理
    /// </summary>
    public class BuildPostProcessor
    {
        private const string LICENSE_PATH = "Assets/nonuplet/Streameme/LICENSE";

        [PostProcessBuild(1)]
        public static void OnBuild(BuildTarget target, string buildPath)
        {
            // buildPathから終端のファイル名を削除
            var path = Path.GetDirectoryName(buildPath);

#if UNITY_EDITOR_WIN
            path += "\\LICENSE";
#endif
            //TODO: Macへの対応

            Debug.Log(path);
            File.Copy(LICENSE_PATH, path, true);
        }
    }
}