using UnityEngine; // Unityエンジン

public class CameraFollow : MonoBehaviour
{
    public Transform target; //ユニティちゃんの場所
    public float distance = 5.5f; // ターゲットからの距離
    public float height = 2.2f; // カメラの高さ
    public float smoothSpeed = 0.18f; // なめらかさの速さ

    void LateUpdate() 
    {
        if (target == null) return;
        // もしターゲットがなければ、これ以上処理しない

        // ターゲットの背中側の位置を計算する
        Vector3 desiredPosition = target.position 
                                  - target.forward * distance 
                                  + Vector3.up * height;
        // ターゲットの位置 - 向いてる方向 × 距離 + 高さ

        // なめらかにカメラを移動する
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // 現在のカメラ位置と理想の位置を、なめらかにまぜる

        transform.position = smoothedPosition;
        // カメラの位置を更新

        // カメラがターゲットを見る
        transform.LookAt(target.position + Vector3.up * 1.5f);
        // たーげっとの位置 + 上に1.5 の場所を向く（少し上向き）
    }
}