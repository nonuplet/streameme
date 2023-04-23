# Build Guide

初回起動時はパッケージが不足しているため起動時にコンパイルエラーの表示が出ますが、気にせずプロジェクトを開いてください

- TextMesh Pro
    - Window→TextMeshPro→Import TMP Essential Resources
  
- Shapes 2Dをインポート
    - [https://assetstore.unity.com/packages/tools/sprite-management/shapes2d-procedural-sprites-and-ui-62586](https://assetstore.unity.com/packages/tools/sprite-management/shapes2d-procedural-sprites-and-ui-62586?locale=ja-JP)
- UniVRMの VRM 0.x系のパッケージをインポート
    - 推奨はv0.110.0
    - [https://github.com/vrm-c/UniVRM](https://github.com/vrm-c/UniVRM)
- uOSCのインポート
    - 現在は使用していませんが、直近でVMC Protocolへの対応を予定しており導入しています
    - 推奨はv2.2.0
    - [https://github.com/hecomi/uOSC](https://github.com/hecomi/uOSC)
- uLipSyncをインポート
    - 推奨はv2.6.1
    - with samplesのパッケージを導入し、Samplesの中の `04. VRM/`
    - [https://github.com/hecomi/uLipSync](https://github.com/hecomi/uLipSync)
- StandaloneFileBrowserをインポート
    - [https://github.com/gkngkc/UnityStandaloneFileBrowser](https://github.com/gkngkc/UnityStandaloneFileBrowser)
- websocket-sharpのビルドと導入
    - [https://github.com/sta/websocket-sharp](https://github.com/sta/websocket-sharp)
    - Hikoさんという方の解説が分かりやすいのでオススメです。[https://note.com/hikohiro/n/n01007cb70c85](https://note.com/hikohiro/n/n01007cb70c85)
    - Assets/Plugins内にビルド後のdllを導入します
- 全て導入が終わったら、 `nonuplet/Strememe/Scenes/Strememe.scene` を開きます
    - パッケージ導入前に開くと、不足したパッケージのインスペクタの情報がロストするので注意してください