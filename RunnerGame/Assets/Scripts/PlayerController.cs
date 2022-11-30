using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static float speed = 8f;

    private float prev_speed;
    private float addSpeed;

    Vector3 moveVector = Vector3.zero;
    CharacterController characterController;

    public float jumpSpeed;
    public float gravity;

    public int coinGet; // collected coins
    private int goalCoin; // no. of coins that will increase our speed
    private static int max_coin; // no. of max coins that we managed to collect

    private float screenWidth;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        screenWidth = Screen.width;

        addSpeed = 0f;

        coinGet = 0;
        goalCoin = 10;
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

        moveVector.x = Input.GetAxis("Horizontal") * 1f;
        moveVector.z = 0;

        if (characterController.isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            moveVector.y = jumpSpeed;
        }

        moveVector.y -= gravity * Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<Transform>().tag == "Obstacle")
        {
            Destroy(other.gameObject);

            if (GetComponent<Renderer>().material.color != other.GetComponent<Renderer>().material.color)
            {
                prev_speed = speed;
                StartCoroutine(gameEnd());
            }
        }
        if (other.gameObject.tag == "cointag")
        {
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

    IEnumerator gameEnd()
    {
        speed = 0;
        yield return new WaitForSeconds(3);
        speed = prev_speed;
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene("SampleScene");
    }
}
