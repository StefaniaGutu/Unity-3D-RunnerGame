using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static float speed = 8f;
    private float prev_speed;
    private float addSpeed;
    public int coinGet; // collected coins
    private int goalCoin; // no. of coins that will increase our speed
    private static int max_coin; // no. of max coins that we managed to collect

    void Start()
    {
        addSpeed = 0;
        coinGet = 0;
        goalCoin = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && transform.position.x > -1.5f)
            transform.position += Vector3.left * 0.25f;

        if (Input.GetKeyDown(KeyCode.D) && transform.position.x < 1.5f)
            transform.position += Vector3.right * 0.25f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<Transform>().tag == "ColorObstacle")
        {
            Destroy(other.gameObject);
            if (GetComponent<Renderer>().material.color != other.GetComponent<Renderer>().material.color)
            {
                if (speed > 8) speed /= 1.5f;
                prev_speed = speed;
                StartCoroutine(gameEnd());
            }
        }

        if (other.gameObject.tag == "Cointag")
        {
            coinGet += 2;
            if (max_coin < coinGet)
            {
                max_coin = coinGet;
            }

            ObjectPools.Instance.ReturnToPool(other.GetComponent<CoinRotate>());

            if(coinGet >= goalCoin)
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
