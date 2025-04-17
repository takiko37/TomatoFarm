using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public FarmManager farmManager; // 畑のスクリプトをつなぐ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ユニティちゃんが入ったら！
        {
            Debug.Log("畑に戻ってきた！");
            GameObject.Find("FarmManager").GetComponent<FarmManager>().CheckFarmLevelUp();
            
            // 畑のレベルアップをチェック！
            farmManager.CheckFarmLevelUp();
        }
    }
}