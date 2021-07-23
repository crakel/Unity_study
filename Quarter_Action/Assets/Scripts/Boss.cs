using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    Vector3 lookVec;
    Vector3 tauntVec; // 어디로 점프공격 할지
    bool isLook; // 플레이어를 바라보는 플래그

    void Awake() {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(Think());
    }
    void Update() {
        if(isLook) {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);

        }
    }

    IEnumerator Think() {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5); // random 0,1,2,3,4

        switch (ranAction) {
            case 0:
            case 1:
                // 미사일 발사 패턴 (확률 2)
                StartCoroutine(MissileShot());
                break;

            case 2:
            case 3:
                // 바위 투척 패턴 (확률 2)
                StartCoroutine(RockShot());
                break;

            case 4:
                // 점프 공격 패턴 (확률 1)
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot() {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }

    IEnumerator RockShot() {
        anim.SetTrigger("doBigShot");
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt() {
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

}
