﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject grenadeObj;
    public Camera followCamera;
    
    public int score;
    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool fDown;
    bool gDown;
    bool iDown;
    bool rDown;

    // 장비 단축키 1, 2, 3
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;
    bool isReload;
    bool isBorder; // 벽 충돌 플래그
    bool isDamage;
    bool isShop;

    Vector3 moveVec;
    Vector3 dodgeVec; // 회피 도중 방향전환이 되지 않도록 회피방향벡터추가
    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;
    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        
        PlayerPrefs.SetInt("MaxScore", 112500);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Grenade();
        Dodge();
        Swap();
        Interaction();
        Attack();
        Reload();
    }
    
    void Move() {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge) {
            moveVec = dodgeVec;
        }

        if (isSwap || isReload || !isFireReady) { // Swap, 장전, 망치휘두르는 중
            moveVec = Vector3.zero;
        }

        if(!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void GetInput() {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");   
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Turn() {
        // #1 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);

        // #2 마우스에 의한 회전
        if (fDown) {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, 100)) {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0; // RayCastHit의 높이는 무시하도록
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Jump() {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge) {
            rigid.AddForce(Vector3.up * 25, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Grenade() {
        if(hasGrenades == 0)
            return;
        
        if(gDown && !isReload && !isSwap) {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10; // RayCastHit의 높이는 무시하도록

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }

    void Attack() {
        if(equipWeapon == null)
            return;
        
        fireDelay += Time.deltaTime; // 공격딜레이 시간을 더해줌
        isFireReady = equipWeapon.rate < fireDelay;
        
        if (fDown && isFireReady && !isDodge && !isSwap && !isShop) {
            equipWeapon.Use(); // 공격 로직은 모두 Weapon.cs에
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload() {
        if(equipWeapon == null) {
            return;
        }

        if(equipWeapon.type == Weapon.Type.Melee)
            return;
        
        if(ammo == 0)
            return;
        
        if(rDown && !isJump && !isDodge && !isSwap && isFireReady && !isShop) {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut() {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap && !isShop)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }
    
    void DodgeOut() {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap() {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isShop) {
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeaponIndex = weaponIndex;
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");
            
            isSwap = true;
            
            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut() {
        isSwap = false;
    }
    void Interaction() {
        if (iDown && nearObject != null && !isJump && !isDodge) {
            if (nearObject.tag == "Weapon") {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                
                Destroy(nearObject);
            }

            else if (nearObject.tag == "Shop") {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;
            }
        }
    }

    void FreezeRotation() {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall() {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    void FixedUpdate() {
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor") {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            switch(item.type) {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }

        else if (other.tag == "EnemyBullet") {
            if (!isDamage) {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                
                if(other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                    
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage() {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs) {
            mesh.material.color = Color.yellow;
        }
        
        yield return new WaitForSeconds(1f);
         
        isDamage = false;
        foreach(MeshRenderer mesh in meshs) {
            mesh.material.color = Color.white;
        }
    }
    void OnTriggerStay(Collider other) {
        if (other.tag == "Weapon" || other.tag == "Shop")
            nearObject = other.gameObject;
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Weapon")
            nearObject = null;
        
        else if (other.tag == "Shop") {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isShop = false;
            nearObject = null;
        }
    }
}
