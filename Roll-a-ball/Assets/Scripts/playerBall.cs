using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerBall : MonoBehaviour
{
    public float jumpPower = 30;
    public int itemCount;
    public gameManagerLogic manager;
    bool isJump;
    Rigidbody rg;
    AudioSource audio;
    void Awake()
    {
        isJump = false;
        rg = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        itemCount = 0;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetButtonDown("Jump") && !isJump) {
            isJump = true;
            rg.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        rg.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor") {
            isJump = false;
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemCount++;
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }
        
        else if (other.tag == "Finish") {
            if(itemCount == manager.TotalItemCount) {
                // Game Clear
                if (manager.stage == 2)
                    SceneManager.LoadScene("Example1_0");

                else
                    SceneManager.LoadScene(manager.stage + 1);
            }
            
            else {
                // Restart
                SceneManager.LoadScene(manager.stage);
            }
        }
    }
}