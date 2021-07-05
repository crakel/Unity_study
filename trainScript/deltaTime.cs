using UnityEngine;

public class deltaTime : MonoBehaviour {
    void Start() {

    }

    void Update() {
        Vector3 vector3 = new Vector3(
            Input.GetAxisRaw("Horizontal") * deltaTime,
            Input.GetAxisRaw("Vertical") * deltaTime,
            0);
        transform.Translate(vec);
    }
}