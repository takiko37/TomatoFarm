using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // シーンをまたいでも消えない！
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
