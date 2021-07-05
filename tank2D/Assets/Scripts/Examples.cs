using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Examples : MonoBehaviour
{
    Rigidbody2D rb;
    float xInput;
    int score = 0;

    public Text scoreText;
    public float speed;

    public SpriteRenderer sprite;
    // Start is called before the first frame update
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update() // 게임 로직 업데이트 60프레임
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Destroy(gameObject);
            rb.AddForce(Vector2.up * 500);
        }
        xInput = Input.GetAxis("Horizontal");

        if(xInput < 0) {
            sprite.flipX = true;
        } else if (xInput > 0) {
            sprite.flipX = false;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
    }
    private void FixedUpdate() { // 물리연산 업데이트 50프레임
        rb.velocity = Vector2.right * xInput * speed;
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            Destroy(collision.gameObject);
            score++;

            scoreText.text = score.ToString();

            if(score >= 5) {
                //Restart();
                Invoke("Restart", 2f); // wait 2second
            }
        }
    }

    void Restart() {
        SceneManager.LoadScene("Game");
    }
}
