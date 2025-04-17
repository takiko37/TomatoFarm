using UnityEngine;
using TMPro;

public class FarmManager : MonoBehaviour
{
    public GameObject farmLevel1;
    public GameObject farmLevel2;
    public GameObject farmLevel3;
    public GameObject farmLevel4;
    public GameObject levelUpEffectPrefab;
    public GameObject clearPanel; 
    public WateringCanManager wateringCanManager;
    public TextMeshProUGUI massageText;
    public AudioSource levelUpSound;
    private int currentLevel = 1; // 現在の畑レベル

    

    // レベルアップのときにエフェクトを出す処理
    void PlayLevelUpEffect()
    {
        if (levelUpEffectPrefab != null)
        {
            // 今回は「FarmManager（このスクリプトが付いているオブジェクト）」の位置でエフェクトを出す！
            Instantiate(levelUpEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("レベルアップエフェクトがセットされていません！");
        }
    }

    // 畑に戻ってきたときにレベルアップする処理
    public void CheckFarmLevelUp()
    {
        switch (currentLevel)
        {
            case 1:
                if (wateringCanManager.currentCanCount >= 3)
                {
                    farmLevel1.SetActive(false);
                    farmLevel2.SetActive(true);
                    PlayLevelUpEffect();
                    currentLevel++;
                    wateringCanManager.ResetCanCount(2);
                    levelUpSound.Play();
                    Debug.Log("畑レベル 2 にアップ！");
                }
                break;

            case 2:
                if (wateringCanManager.currentCanCount >= 4)
                {
                    farmLevel2.SetActive(false);
                    farmLevel3.SetActive(true);
                    PlayLevelUpEffect();
                    currentLevel++;
                    wateringCanManager.ResetCanCount(3); 
                    levelUpSound.Play();
                    Debug.Log("畑レベル 3 にアップ！");
                }
                break;

            case 3:
                if (wateringCanManager.currentCanCount >= 5)
                {
                    farmLevel3.SetActive(false);
                    farmLevel4.SetActive(true);
                    PlayLevelUpEffect();
                    currentLevel++;
                    Debug.Log("畑レベル 4 にアップ！（ゲームクリア！）");
                    levelUpSound.Play();
                    // クリア演出追加できるよ！
                    clearPanel.SetActive(true);
                    massageText.gameObject.SetActive(false);
                }
                break;
        }
    }
}
