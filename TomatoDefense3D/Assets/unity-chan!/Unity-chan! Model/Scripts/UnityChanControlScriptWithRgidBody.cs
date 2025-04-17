using UnityEngine; 
using System.Collections; // 「ちょっと待つ」処理を使いたいときに必要


namespace UnityChan
{


    [RequireComponent(typeof(Animator))] // 動き・表情をつける「アニメーション」
    [RequireComponent(typeof(CapsuleCollider))] // 当たり判定の「カプセルコライダー」
    [RequireComponent(typeof(Rigidbody))] // 重さや物理演算の「リジッドボディ」

    public class UnityChanControlScriptWithRgidBody : MonoBehaviour
    {

        public float animSpeed = 1.5f; // アニメーションの速さ（速いときは数字大きく）
        public float lookSmoother = 3.0f; // カメラが動くときに、ぬるぬる滑らかにする強さ
        public bool useCurves = true; // アニメーションで当たり判定をなめらかに調整するかどうか
        public float useCurvesHeight = 0.5f; // 当たり判定をどの高さで調整するか（たとえばジャンプのとき）

        public float forwardSpeed = 7.0f; // 前に進む速さ
        public float backwardSpeed = 2.0f; // 後ろに下がるときの速さ
        public float rotateSpeed = 2.0f; // 左右を向くスピード
        public float jumpPower = 3.0f; // ジャンプの高さ
        public GameObject attackCollider; // 攻撃の当たり判定
        public ParticleSystem attackEffect; // 攻撃時のエフェクト（手から出る光）
        public int enemyCount = 0;
        public AudioSource runAudio;

        private bool isRunningSoundPlaying = false;

        private CapsuleCollider col; // ユニティちゃんの当たり判定（カプセル型）
        private Rigidbody rb; // ユニティちゃんの重さ・物理演算の処理
        private Vector3 velocity; // キャラクターの移動スピードと方向

        private float orgColHight; // コライダー（当たり判定）のもともとの高さ
        private Vector3 orgVectColCenter; // コライダーのもともとの中心位置

        private Animator anim; // アニメーションを動かす装置！
        private AnimatorStateInfo currentBaseState; // 今どのアニメーション中か？をメモしておく

        private GameObject cameraObject; // カメラのオブジェクト（位置や角度を知るため）
        
        

        // アニメーションの「状態」を覚えておく
        static int idleState = Animator.StringToHash("Base Layer.Idle"); // 待機中
        static int locoState = Animator.StringToHash("Base Layer.Locomotion"); // 歩いたり走ってるとき
        static int jumpState = Animator.StringToHash("Base Layer.Jump"); // ジャンプ中
        static int restState = Animator.StringToHash("Base Layer.Rest"); // 休憩ポーズ中

        void Start()
        {
            anim = GetComponent<Animator>(); // アニメーションを動かす部品を持ってくる
            col = GetComponent<CapsuleCollider>(); // コライダー（当たり判定）を持ってくる
            rb = GetComponent<Rigidbody>(); // リジッドボディ（重力や衝突処理）を持ってくる
            cameraObject = GameObject.FindWithTag("MainCamera"); // メインカメラを探して持ってくる

            orgColHight = col.height; // 当たり判定のもともとの高さをメモ
            orgVectColCenter = col.center; // 当たり判定のもともとの中心をメモ
            attackCollider.SetActive(false); // 攻撃の当たり判定は最初は「オフ」
        }

        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                anim.SetTrigger("AttackTrigger"); // 攻撃アニメーションを発動
                StartCoroutine(EnableAttackCollider()); // 攻撃の当たり判定を一瞬だけオンにする
                attackEffect.Play(); //  エフェクト再生！
            }

            // キーボードの入力を取得
            float h = Input.GetAxis("Horizontal"); // 横移動（A,Dキー または ←→キー）
            float v = Input.GetAxis("Vertical"); // 縦移動（W,Sキー または ↑↓キー）

            anim.SetFloat("Speed", v); // 縦入力の値をアニメーションに送る
            anim.SetFloat("Direction", h); // 横入力の値をアニメーションに送る
            anim.speed = animSpeed; // アニメーションの速度を設定

            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            //anim = Animator コンポーネント（アニメーションを管理する部品）
            //.GetCurrentAnimatorStateInfo(0) = Animator の「0番目のレイヤー（Base Layer）」の、現在のアニメーション状態の情報を取得（※ 0 は Base Layer の番号）
            // これを currentBaseState に代入して、 → あとで「今どのアニメーションしてるか」を条件として使えるようにしてる！
            rb.useGravity = true; // 重力はつねにオン

            velocity = new Vector3(0, 0, v); // 移動する方向とスピードを作る（前後移動）
            velocity = transform.TransformDirection(velocity); // キャラの向きに合わせる！

            // 入力に応じて速度を変える
            if (v > 0.1f) velocity *= forwardSpeed; // 前に進むときの速さ
            else if (v < -0.1f) velocity *= backwardSpeed; // 後ろに進むときの速さ

            // ===== 足音再生の処理（走ってる間だけ） =====
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && currentBaseState.nameHash == locoState)
            {
                if (!isRunningSoundPlaying)
                {
                    runAudio.Play(); // 再生！
                    isRunningSoundPlaying = true;
                }
            }
            else
            {
                if (isRunningSoundPlaying)
                {
                    runAudio.Stop(); // 停止！
                    isRunningSoundPlaying = false;
                }
            }

            
            // ジャンプボタンが押されたかチェック
            if (Input.GetButtonDown("Jump"))
            {
                // 今が移動中ならジャンプ可能
                if (currentBaseState.nameHash == locoState)
                    //currentBaseState.nameHash = 現在のアニメーションの「名前」を数字で取得（高速で比較できる）
                {
                    // アニメーションの切り替え中でないならジャンプ
                    if (!anim.IsInTransition(0))
                    {
                        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange); // 上に力を加える！
                        anim.SetBool("Jump", true); // アニメーションにもジャンプフラグをセット！
                    }
                }
            }

            // 実際にキャラクターを移動させる処理
            transform.localPosition += velocity * Time.fixedDeltaTime;

            // 左右の入力に応じてキャラクターを回転！
            transform.Rotate(0, h * rotateSpeed, 0);


            if (currentBaseState.nameHash == locoState)
            {
                if (useCurves) resetCollider(); // 移動中はコライダーをリセット
            }
            else if (currentBaseState.nameHash == jumpState)
            {
                if (!anim.IsInTransition(0))
                {
                    if (useCurves)
                    {
                        float jumpHeight = anim.GetFloat("JumpHeight"); // アニメーションからジャンプ高さ取得
                        float gravityControl = anim.GetFloat("GravityControl"); // 重力をかけるかどうか

                        if (gravityControl > 0) rb.useGravity = false; // ジャンプ中は重力オフ！

                        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                        // 地面を調べるための「見えない光線」を飛ばす！
                        //new Ray(始点, 向き) =「Ray（レイ）」は、見えない直線のこと
                        // transform.position + Vector3.up = キャラクターの今の位置より「ちょっと上」から
                        //-Vector3.up = 「真下」に向けて光線を飛ばす！ = 地面までの距離を計測してる！
                        RaycastHit hitInfo;

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.distance > useCurvesHeight)
                            {
                                col.height = orgColHight - jumpHeight;
                                float adjCenterY = orgVectColCenter.y + jumpHeight;
                                col.center = new Vector3(0, adjCenterY, 0);
                            }
                            else
                            {
                                resetCollider(); // もどす！
                            }
                        }
                    }

                    anim.SetBool("Jump", false); // ジャンプ終了
                }
            }
            else if (currentBaseState.nameHash == idleState)
            {
                if (useCurves) resetCollider(); // 待機中もコライダーをリセット

                if (Input.GetButtonDown("Jump"))
                {
                    anim.SetBool("Rest", true); // スペースで休憩ポーズ！
                }
            }
            else if (currentBaseState.nameHash == restState)
            {
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Rest", false); // 休憩終わり
                }
            }
        }
        
        

        //  コライダーを元に戻す関数
        void resetCollider()
        {
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }

        //攻撃コライダーを一瞬だけ ON にする処理！
        IEnumerator EnableAttackCollider()
        {
            attackCollider.SetActive(true); // 攻撃の当たり判定を出す！
            yield return new WaitForSeconds(0.3f); // 0.3秒だけ待つ（攻撃のタイミングに合わせる）
            attackCollider.SetActive(false); // 攻撃の当たり判定を消す！
        }
    }
}
