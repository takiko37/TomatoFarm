using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startCanvas; // スタート画面の Canvas
    public AudioSource startButtonAudio;
    void Start()
    {
        // ゲームが始まったときに、時間を止める
        Time.timeScale = 0;
    }

    // スタートボタンを押したら呼び出す関数
    public void StartGame()
    {
        startCanvas.SetActive(false); // スタート画面を非表示
        startButtonAudio.Play();
        Time.timeScale = 1; // 時間を動かす
    }
}
