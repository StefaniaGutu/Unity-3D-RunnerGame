using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    Rigidbody m_Rigidbody;
    public static float speed = 10f;

    public float prev_speed;
    private float addSpeed;

    public ParticleSystem explosionParticleSystem;
    public ParticleSystem diamondParticleSystem;

    Vector3 moveVector = Vector3.zero;
    Vector3 holdPosition = Vector3.zero;
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

    public GameObject player; 

    void Start()
    {
        explosionParticleSystem = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
        diamondParticleSystem = GameObject.Find("Diamond Particle System").GetComponent<ParticleSystem>();
        m_Rigidbody = GetComponent<Rigidbody>();

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
        if (canMove)
        {
            moveVector.x = Input.GetAxis("Horizontal") * 4f;
            moveVector.z = 0;

            if (characterController.isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
            {
                moveVector.y = jumpSpeed;
            }

            moveVector.y -= gravity * Time.deltaTime;

            characterController.Move(moveVector * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "JumpObstacle")
        {
            explosionParticleSystem.Play();
            ParticleSystem.EmissionModule em = explosionParticleSystem.emission;
            em.enabled = true;
            
            Destroy(other.gameObject);
            speed -= 1f;

            LivesManager.instance.RemoveLife();
            if (LivesManager.instance.isDead){
                prev_speed = speed;
                StartCoroutine(gameEnd());
                audioData[2].Play(0);
            }
            else
            {
                StartCoroutine(BlinkGameObject());
            }
        }
        if (other.GetComponentInChildren<Transform>().tag == "Obstacle")
        {
            
            diamondParticleSystem.Play();
            diamondParticleSystem.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;
            ParticleSystem.EmissionModule em = diamondParticleSystem.emission;
            em.enabled = true;

            Destroy(other.gameObject);

            if (GameObject.Find("Body").GetComponent<Renderer>().material.color == other.GetComponent<Renderer>().material.color)
            {
                audioData[1].Play(0);
                ScoreManager.instance.AddPoints(2);
            }
                
            if (GameObject.Find("Body").GetComponent<Renderer>().material.color != other.GetComponent<Renderer>().material.color)
            {
                LivesManager.instance.RemoveLife();
                if (LivesManager.instance.isDead)
                {
                    prev_speed = speed;
                    holdPosition = characterController.transform.position;
                    StartCoroutine(gameEnd());
                    audioData[2].Play(0);
                }
                else
                {
                    StartCoroutine(BlinkGameObject());
                }
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
        canMove = true;
        speed = 10f;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        SceneManager.LoadScene(1);
    }

    IEnumerator gameEnd()
    {
        gameOverManager.SetGameOver();
        speed = 0;
        canMove = false;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator BlinkGameObject()
    {
        var mesh = GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>();
        var mesh2 = GameObject.Find("Crown").GetComponent<MeshRenderer>();

        for (int i = 0; i < 2; i++)
        {
            mesh.enabled = false;
            mesh2.enabled = false;
            yield return new WaitForSeconds(0.2f);

            mesh.enabled = true;
            mesh2.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
