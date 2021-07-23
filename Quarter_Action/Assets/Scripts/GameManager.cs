﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    // public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreTxt;
    public Text bestTxt;

    void Awake() {
        if(PlayerPrefs.HasKey("MaxScore")) {
             PlayerPrefs.SetInt("MaxScore", 0);
        }

        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));


    }

    public void GameStart() {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    public void GameOver() {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreTxt.text = scoreTxt.text;
        
        int maxScore = PlayerPrefs.GetInt("MaxScore");

        if(player.score > maxScore) {
            bestTxt.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void StageStart() {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(true);
        
        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd() {
        player.transform.position = Vector3.up * (-0.7f);
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(false);
        
        isBattle = false;
        stage++;
    }

    IEnumerator InBattle() {
        // BOSS LOGIC
        // if (stage % 5 == 0) {
            // enemyCntD++;
            // GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[ranZone].rotation);
            // Enemy enemy = instantEnemy.GetComponent<Enemy>();
            // enemy.target = player.transform;
            // enemy.manager = this;
            // boss = instantEnemy.GetComponent<Boss>();
        // }
        // else {
            
        // }


        for(int index=0; index < stage; index++) {
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);

            switch(ran) {
                case 0:
                    enemyCntA++;
                    break;
                case 1:
                    enemyCntB++;
                    break;
                case 2:
                    enemyCntC++;
                    break;
            }
        }

        while (enemyList.Count > 0) {
            int ranZone = Random.Range(0, 4);
            enemyZones[ranZone].position = Vector3.up * (-2f);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }
        // 남은 몬스터 수 검사
        while (enemyCntA + enemyCntB + enemyCntC > 0) {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        StageEnd();
    }

    void Update() {
        if (isBattle)
            playTime += Time.deltaTime;
    }
    void LateUpdate() {
        // 상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;
        
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int sec = (int)(playTime % 60); 

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);

        // 플레이어 UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null || player.equipWeapon.type == Weapon.Type.Melee) {
            playerAmmoTxt.text = "- / " + player.ammo;
        }

        else if (player.equipWeapon.type == Weapon.Type.Range) {
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        }

        // 무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 몬스터 숫자 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        // 보스 체력 UI
        // bossHealthBar.localscale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);

    }
}
