using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos; // 총알 생성 위치
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;


    public void Use() {
        if(type == Type.Melee) {
            StopCoroutine("Swing"); // 코루틴 정지 함수 (지금 동작중인)
            StartCoroutine("Swing"); // 코루틴 실행 함수
        }

        else if (type == Type.Range && curAmmo > 0) {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() {
        // yield break; // yield break로 코루틴 탈출 가능
        // 1번 구역
        yield return new WaitForSeconds(0.1f); // 0.1 초 대기 (return null -> 1 프레임 대기)
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        // 2번 구역
        yield return new WaitForSeconds(0.6f); // 0.3 초 대기
        meleeArea.enabled = false;
        
        // 3번 구역
        yield return new WaitForSeconds(0.6f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot() {
        // #1 총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;
        // #2 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        
    }
}
