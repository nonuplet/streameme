# Streameme

**Strememe**（ストリーミーム） は [JINS MEME](https://jinsmeme.com/) を使った、PC向けVTuber配信用アプリケーションです。  
カメラを使わずにメガネ1台で配信ができることを目標にしています。

![title.png](docs/img/title.png)

現在テスト版のため、予期せぬ動作が発生する可能性があります。

# 免責事項 ⚠

- 2015年発売の旧JINS MEME(ESモデル)は非対応です。
- 本ソフトウェアは個人製作のソフトウェアであり、**JINS MEMEの発売元である株式会社ジンズホールディングスとは一切関係ありません**。
  - ソフトウェアに関する質問は作者（[@Nonuplet](https://twitter.com/nonuplet_)）までお願いします。
- 本ソフトウェアを使用すること、または使用できないこと、また使用に当たり実施したファームウェア更新やアプリケーションの購入などの関連した行動によって発生した損害・損失に対し作者は一切の責任を負いません。
- 各会社名・製品名は各社の商標または登録商標です。

# 動作に必要なもの

- Windows PC
  - 直近でMac OSも対応予定です
- JINS MEME本体
- iOS/Androidデバイス
- ローカルのWi-Fi環境
- JINS MEME Loggerアプリ **(有料, iOS 160円・Android 130円)**
- VRMファイル

# ダウンロード ⬇️

[Releases](https://github.com/nonuplet/streameme/releases)からどうぞ。
近日中にBoothも開設予定です。

# セットアップ 🔧

[JINS MEME](https://jinsmeme.com/) を準備して、購入時についてくる説明書を見ながら初回セットアップを行ってください。  
（※度あり・なし両方あります。JINSで購入したことがあればアプリから購入するのが簡単かも、自分はそうしました）

初回セットアップが完了したら、Loggerアプリの[iOS版](https://apps.apple.com/jp/app/jins-meme-logger/id1537937129)または[Android版](https://play.google.com/store/apps/details?id=com.jins_meme.logger4internal)をインストールし、お手持ちのMEMEと接続します。  
端末を接続したら、下メニューの「設定」をタップして設定を開き以下の手順で接続してください。
- ジャイロ取得をON
- Websocketクライアントの「追加する」をタップ、お使いのPCのローカルIPを設定し、ポート番号を指定
  - このときdata typeは `currentData` を指定します
- 本ソフトウェアを起動し、Loggerアプリの設定した接続先をON

[詳しいセットアップの方法はこちら](docs/setup.md)からご確認ください。

# 機能紹介

JINS MEMEのジャイロを使って首・腰を動かすことができます。  
まばたきも検知してアニメーションに反映されます。

![gyro.gif](docs/img/gyro.gif)

前カメラ・後カメラ・フリーカメラで自由に位置・角度を調整できます。

![camera.gif](docs/img/camera.gif)

背景をSkybox, グリーンバック、ブルーバックの三種類から選択できます。

![background.gif](docs/img/background.gif)

マイク入力を使ってリップシンクを行うこともできます。

![lipsync.gif](docs/img/lipsync.gif)

# 更新履歴 ⌚

- **v0.1.0 (2023/04/23)**
  - 初回リリース

[過去のパッチノートはこちら](docs/patchnote.md)

# ロードマップ 🗺

今後、以下の機能の追加を予定しています。

- 残りバッテリー量の表示・電池切れ前に通知する機能
- 接続状態の表示
- VMC Protocolによる他アプリケーションとの連携
- Mac OS対応
- 各種コンフィグの保存機能
- 画質の調整機能
- Live2D対応

# その他

説明用の3Dモデルは、[かめ山](https://twitter.com/mukumi) さんの [うささき](https://mukumi.booth.pm/items/3550881) ちゃんを使っています。オススメです。気になったらぜひ買ってみてください。  
[https://mukumi.booth.pm/items/3550881](https://mukumi.booth.pm/items/3550881)

# 開発者向け

- [ビルドガイド](docs/buildguide.md)