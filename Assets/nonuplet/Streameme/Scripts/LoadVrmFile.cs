using System.Threading.Tasks;
using SFB;
using UniGLTF;
using UnityEngine;
using VRM;
using VRMShaders;

namespace Streameme
{
    /// <summary>
    /// VRMファイルのロード
    /// </summary>
    public class LoadVrmFile : MonoBehaviour
    {
        private RuntimeGltfInstance _instance;

        public async Task<GameObject> OpenFile()
        {
            string[] filePath;
            var ext = new[]
            {
                new ExtensionFilter("VRM Files", "vrm")
            };
            filePath = StandaloneFileBrowser.OpenFilePanel("読み込むVRMファイルを選択", "", ext, false);
            if (filePath.Length == 0) return null;

            var path = filePath[0];

            _instance = await VrmUtility.LoadAsync(path, new RuntimeOnlyAwaitCaller());
            _instance.ShowMeshes();
            return _instance.gameObject;
        }
    }
}