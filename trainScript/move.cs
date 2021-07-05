using UnityEngine;

public class Move : MonoBehavior {
    Vecotr3 target = new Vecotr3(8, 1.5f, 0);

    void Update() {
        //1. MoveTowards 일정한 이동
        // transform.position = Vector3.MoveTowards(transform.position, target, 2f);

        //2. SmoothDamp 부드러운 이동
        Vector3 velo = Vector3.zero;
        // Vector3 velo = Vector3.up * 50;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velo, 0.1f);
        
        //3. Lerp 선형 보간
        transform.position = Vector3.Lerp(transform.position, target, 0.1f);

        //4. SLerp 구면 선형 보간
        transform.position = Vector3.Slerp(transform.position, target, 0.005f);
        
    }
}