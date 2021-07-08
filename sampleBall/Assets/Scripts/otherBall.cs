using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherBall : MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
    }

    // void OnCollisionStay(Collision collision) 
    // {
        
    // }
    // void OnCollisionExit(Collision collision) 
    // {

    // }
    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.name == "My Ball") {
            mat.color = new Color(0, 0, 0);
        }
    }
    void OnCollisionExit(Collision collision) 
    {
        if(collision.gameObject.name == "My Ball") {
            mat.color = new Color(1, 1, 1);
        }
    }
}
