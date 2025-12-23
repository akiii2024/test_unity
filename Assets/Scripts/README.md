# 3D障害物よけランナーゲーム

## セットアップ手順

### 1. Prefabの作成
Unityエディタで以下のメニューからPrefabを作成してください：
- `Tools > Create All Prefabs` - 障害物とコインのPrefabを自動生成

### 2. シーンのセットアップ
Unityエディタで以下のメニューからシーンをセットアップしてください：
- `Tools > Setup Runner Game Scene` - ゲームシーンを自動生成

これにより以下が作成されます：
- Player（Capsule + PlayerController）
- Ground（Plane）
- GameManager
- ObstacleSpawner
- UIManager
- AudioManager
- Canvas（UI要素すべて）

### 3. Prefabの割り当て
1. Hierarchyで`ObstacleSpawner`を選択
2. Inspectorで`Obstacle Prefab`に`Assets/Prefabs/Obstacle.prefab`を割り当て

### 4. 音声ファイルの設定（オプション）
1. Hierarchyで`AudioManager`を選択
2. Inspectorで以下のAudioClipを割り当て：
   - Coin Sound
   - Collision Sound
   - Game Start Sound
   - Game Over Sound

音声ファイルがない場合でもゲームは動作しますが、SEは再生されません。

## 操作方法

- **←/→ または A/D**: レーン切り替え（左/中央/右）
- **H**: ヘルプパネル表示/非表示
- **R**: リスタート（ゲームオーバー時）
- **任意のキー**: ゲーム開始

## ゲームシステム

- **自動前進**: プレイヤーは常に前進します
- **3レーンシステム**: 左(-2)、中央(0)、右(+2)の3レーン
- **体力システム**: 初期体力3、障害物衝突で-1、0でゲームオーバー
- **スコアシステム**: 距離 + コイン数×10
- **無限生成**: 障害物は前方から無限に生成されます

## スクリプト説明

- **PlayerController.cs**: プレイヤーの移動とレーン切り替え
- **GameManager.cs**: ゲーム状態、スコア、体力管理
- **ObstacleSpawner.cs**: 障害物の無限生成
- **Obstacle.cs**: 障害物の衝突判定
- **Coin.cs**: コインの収集処理
- **UIManager.cs**: UI表示管理
- **AudioManager.cs**: 音響効果管理

## トラブルシューティング

- UIが表示されない場合: Canvasが正しく設定されているか確認
- 障害物が生成されない場合: ObstacleSpawnerにPrefabが割り当てられているか確認
- 衝突が検知されない場合: PlayerのTagが"Player"になっているか確認

