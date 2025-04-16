using UnityEngine;
using TMPro; 

public class WateringCanManager : MonoBehaviour
{
    public FarmManager farmManager; // 畑のスクリプトをつなぐ
    public int currentCanCount = 0;
    public TextMeshProUGUI canCountText;
    public TextMeshProUGUI massageText;
    // 今取ったじょうろの数
    private int maxCanCount = 3; // 今のレベルで取れるじょうろの数
    
    void Start()
    {
        
        maxCanCount = 3; // 最初は 3 個
    }

    // じょうろを取るときに呼ぶ関数
    public bool TryCollectCan()
    {
        if (currentCanCount >= maxCanCount)
        {
            Debug.Log("もうじょうろは取れません！ 畑をレベルアップしよう！");
            
            return false; // ダメ
            
        }

        currentCanCount++;
        Debug.Log("じょうろ取ったよ！ 現在の数: " + currentCanCount);
        canCountText.text = $"じょうろの数:{currentCanCount}/{maxCanCount}";
        if (currentCanCount == maxCanCount)
        {
            massageText.gameObject.SetActive(true);
        }
        return true; // 取れたよ！
    }
    

    // 畑レベルアップしたときに呼ぶ関数
    public void ResetCanCount(int newLevel)
    {
        currentCanCount = 0; // カウントリセット！

        switch (newLevel)
        {
            case 2:
                maxCanCount = 4; // レベル 2→3 は 4 個
                break;
            case 3:
                maxCanCount = 5; // レベル 3→4 は 5 個
                break;
            default:
                maxCanCount = 0;
                break;
        }
        canCountText.text = $"じょうろの数:{currentCanCount}/{maxCanCount}";
        massageText.gameObject.SetActive(false);
        Debug.Log("じょうろリセット！ 新しい上限: " + maxCanCount);
    }
}