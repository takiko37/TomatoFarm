using UnityEngine;
using UnityEngine.AI; // AI 機能（ナビメッシュ）を使えるようにする

public class SlimeController : MonoBehaviour
{
    public Transform target; // ターゲットになる場所（畑など）

    private NavMeshAgent agent; //このキャラの NavMesh Agent（エージェント）

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // このキャラが持っている NavMesh Agent（移動用の機能）を取得！

        agent.SetDestination(target.position);
        // 目的地を target（ターゲット）の位置に設定する
        //agent.SetDestination(target.position); → ゴールまで自動で歩かせる！
    }
}