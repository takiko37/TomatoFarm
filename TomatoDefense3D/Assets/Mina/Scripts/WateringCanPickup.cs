using UnityEngine;

public class WateringCanPickup : MonoBehaviour
{
    public WateringCanManager canManager; // 管理するスクリプト

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider")) // プレイヤーに当たったら
        {
            bool collected = canManager.TryCollectCan();

            if (collected)
            {
                Destroy(gameObject); // じょうろ消す
            }
        }
    }
}