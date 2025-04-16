using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 敵のプレハブ
    public Vector3 spawnAreaMin = new Vector3(-10f, 0f, -10f); // スポーン範囲の最小座標
    public Vector3 spawnAreaMax = new Vector3(10f, 0f, 10f);  // スポーン範囲の最大座標

    public void RespawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnAreaMin.y,
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        // Quaternion.Euler(X回転, Y回転, Z回転)
        // Y回転だけランダムにすることで「左右の向き」をバラバラにできる
        
        Instantiate(enemyPrefab, spawnPosition, randomRotation); // 敵をランダム位置に生成
        Debug.Log("敵をスポーンしました！");
    }
}