using System;
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

        public async Task<GameObject> OpenFile(string path)
        {
            _instance = await VrmUtility.LoadAsync(path, new RuntimeOnlyAwaitCaller());
            _instance.ShowMeshes();
            return _instance.gameObject;
        }
    }
}