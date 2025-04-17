using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour
{
    public GameObject howToPanel; // 操作説明パネル
    public Button howToButton;    // 「操作方法」ボタン
    public Button backButton;     // 「戻る」ボタン
    public AudioSource howToSound;
    public AudioSource backSound;

    void Start()
    {
        // 最初はパネル非表示
        howToPanel.SetActive(false);

        // ボタンにイベントを登録！
        howToButton.onClick.AddListener(ShowPanel);
        backButton.onClick.AddListener(HidePanel);
    }

    void ShowPanel()
    {
        howToSound.Play();
        howToPanel.SetActive(true);
    }

    void HidePanel()
    {
        backSound.Play();
        howToPanel.SetActive(false);
    }
}