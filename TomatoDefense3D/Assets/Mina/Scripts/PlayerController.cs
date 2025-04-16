using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _groundObj;

    // プレイヤーの移動速度
    public float moveSpeed = 5f;

    // プレイヤーの回転速度
    public float rotationSpeed = 300f;

    // ジャンプの力
    public float jumpForce = 5f;

    // 重力の強さ
    public float gravity = -9.81f;

    // CharacterController を格納する変数
    //private CharacterController controller;

    // プレイヤーの上下の速度
    private Vector3 velocity;

    // アニメーターを格納する変数（追加！）
    private Animator animator;

    public bool _isGrounded;

    void Start()
    {
        // シーン開始時に CharacterController を取得する
        //controller = GetComponent<CharacterController>();

        // シーン開始時に Animator を取得する（追加！）
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // 横（A,D）の入力を取得
        float v = Input.GetAxis("Vertical"); // 縦（W,S）の入力を取得

        // 入力から移動方向のベクトルを作成
        Vector3 move = new Vector3(h, 0, v);

        // カメラの向きを基準にして移動方向を調整
        move = Camera.main.transform.TransformDirection(move);

        // 上下方向の移動は velocity に任せるから一旦 0
        move.y = 0f;

        // 実際にキャラクターを移動させる（横移動だけ）
        //controller.Move(move * moveSpeed * Time.deltaTime);

        // 入力があるときだけキャラクター（かお）の向きを変更する
        if (move != Vector3.zero)
        {
            // 進行方向を向く回転を計算　//Quaternion=回転
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);

            /* transform.rotation	キャラクターの今の「向き」
            Quaternion.RotateTowards() = 今の向きから目的の向きへ回転させる命令
            transform.rotation（第1引数) = 今の向き（スタート地点）
            toRotation（第2引数）=向きたい方向（ゴール地点）
            rotationSpeed * Time.deltaTime（第3引数）	どれくらいの速さで回すか（毎秒の回転速度）*/
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // 🔥 攻撃の入力を追加！🔥
        if (Input.GetMouseButtonDown(1)) // 右クリックで攻撃！（0 が左クリック、1 が右クリック）
        {
            animator.SetTrigger("AtackTrigger"); // AttackTrigger を実行！
        }

        // 地面にいるかチェック
        if (_isGrounded)
        {
            velocity.y = -2f; // 接地中は下向きに少し押す

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                velocity.y = jumpForce; // ジャンプボタンで上向きの力を加える
            }
        }
        else
        {
            // 空中にいる間は重力をかける
            velocity.y += gravity * Time.deltaTime;
        }

        GetComponent<Rigidbody>().AddForce(velocity * moveSpeed);
        
        // 上下の移動（ジャンプ・重力）
        //controller.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == _groundObj)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == _groundObj)
        {
            _isGrounded = false;
        }
    }
}