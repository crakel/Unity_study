using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBall : MonoBehaviour
{
    public float jumpPower = 5;
    Rigidbody rg;
    void Awake()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetButtonDown("Jump")) {
            rg.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        rg.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnTriggerStay(Collider other) {
        if(other.name == "Cube") {
            rg.AddForce(Vector3.up * 2, ForceMode.Impulse);
        }
    }
}
