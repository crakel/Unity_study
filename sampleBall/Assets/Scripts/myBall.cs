using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBall : MonoBehaviour
{
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rigid.velocity = new Vector3(2, 4, -1);
        //#1. 속력 바꾸기
        //rigid.velocity = Vector3.forward;
        
        //#2. 힘을 가하기
        if(Input.GetButtonDown("Jump")) {
            rigid.AddForce(Vector3.up * 50, ForceMode.Impulse);
        }

        Vector3 vec = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            0,
            Input.GetAxisRaw("Vertical")
        );

        rigid.AddForce(vec, ForceMode.Impulse);
        
        //#3. 회전력
        // Vec 방향을 축으로 회전력이 생김.
        //rigid.AddTorque(Vector3.back);
    }
}
