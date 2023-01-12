using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public static float speed = 10f;

    public float prev_speed;
    private float addSpeed;

    Vector3 moveVector = Vector3.zero;
    CharacterController characterController;

    public float jumpSpeed;
    public float gravity;

    public int coinGet; // collected coins
    private int goalCoin; // no. of coins that will increase our speed
    private static int max_coin; // no. of max coins that we managed to collect

    private float screenWidth;

    [SerializeField] GameOverManager gameOverManager;

    AudioSource[] audioData;
    public AudioClip CoinSound;
    public AudioClip DiamondSound;

    void Start()
    {

        characterController = GetComponent<CharacterController>();

        transform.position = new Vector3(0,-0.3f,0);

        screenWidth = Screen.width;

        addSpeed = 0f;

        coinGet = 0;
        goalCoin = 10;

        audioData = GetComponents<AudioSource>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A) && transform.position.x > -1.5f)
        //{
        //    transform.position += Vector3.left * 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.D) && transform.position.x < 1.5f)
        //{
        //    transform.position += Vector3.right * 1f;
        //}

        moveVector.x = Input.GetAxis("Horizontal") * 4f;
        moveVector.z = 0;

        if (characterController.isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
        {
            moveVector.y = jumpSpeed;
        }

        moveVector.y -= gravity * Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "JumpObstacle")
        {
            Destroy(other.gameObject);
            prev_speed = speed;
            StartCoroutine(gameEnd());
            audioData[2].Play(0);
        }
        if (other.GetComponentInChildren<Transform>().tag == "Obstacle")
        {
            Destroy(other.gameObject);

            if (GameObject.Find("Body").GetComponent<Renderer>().material.color == other.GetComponent<Renderer>().material.color)
            {
                audioData[1].Play(0);
                ScoreManager.instance.AddPoints(2);
            }
                
            if (GameObject.Find("Body").GetComponent<Renderer>().material.color != other.GetComponent<Renderer>().material.color)
            {
                prev_speed = speed;
                StartCoroutine(gameEnd());
                audioData[2].Play(0);
            }
        }
        if (other.gameObject.tag == "cointag")
        {
            audioData[0].Play(0);

            ScoreManager.instance.AddPoints(1);

            coinGet += 2;
            if (max_coin < coinGet)
            {
                max_coin = coinGet;
            }

            ObjectPools.Instance.ReturnToPool(other.GetComponent<CoinRotate>());

            if (coinGet >= goalCoin)
            {
                goalCoin += (coinGet / 2);
                addSpeed += 2f;
                speed += (addSpeed / 2);
            }
        }
    }

    public void setSpeedRestart()
    {
        speed = prev_speed;
        SceneManager.LoadScene(1);
    }

    IEnumerator gameEnd()
    {
        gameOverManager.SetGameOver();
        speed = 0;
        yield return new WaitForSeconds(0.05f);
    }
}
