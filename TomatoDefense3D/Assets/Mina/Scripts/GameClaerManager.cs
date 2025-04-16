using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public GameObject gameClearPanel; // ゲームクリアパネル
    public Button restartButton;      // 「最初から遊ぶ」ボタン

    void Start()
    {
        gameClearPanel.SetActive(false); // 最初は非表示にしておく
        restartButton.onClick.AddListener(RestartGame); // ボタンにイベントを登録
    }
   

    // 最初から遊ぶボタンを押したときの処理
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}