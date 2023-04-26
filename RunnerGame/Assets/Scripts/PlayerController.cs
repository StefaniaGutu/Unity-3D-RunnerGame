using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public Animator animator;
    Rigidbody m_Rigidbody;
    public static float speed = 10f;
    public Material face;
    public GameObject textObject;
    private Animation textAnimation;
    private TextMeshPro textMeshProComponent;

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
    public SwitchAI switchAI;

    private GameObject closestJumpObstacle;
    private GameObject closestCoin;
    private GameObject closestDiamond;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            if (switchAI.isAIControlled)
            {
                UpdateDecisionTree();
            }
            else
            {
                moveVector.x = Input.GetAxis("Horizontal") * 4f;
                moveVector.z = 0;

                if (characterController.isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
                {
                    moveVector.y = jumpSpeed;
                    animator.Play("Jump");
                }

                moveVector.y -= gravity * Time.deltaTime;

                characterController.Move(moveVector * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "JumpObstacle")
        {
            explosionParticleSystem.Play();
            ParticleSystem.EmissionModule em = explosionParticleSystem.emission;
            em.enabled = true;
            animator.Play("");
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
                PlayTextAnimation(2);
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

            PlayTextAnimation(1);
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
        var initialFace = GameObject.Find("Body").GetComponent<Renderer>().materials[1];
        var mats = GameObject.Find("Body").GetComponent<Renderer>().materials;
        mats[1] = face;
        GameObject.Find("Body").GetComponent<Renderer>().materials = mats;
        transform.rotation = new Quaternion(0, 180f, 0, 0);

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

        transform.rotation = new Quaternion(0, 0, 0, 0);
        mats = GameObject.Find("Body").GetComponent<Renderer>().materials;
        mats[1] = initialFace;
        GameObject.Find("Body").GetComponent<Renderer>().materials = mats;
    }

    void PlayTextAnimation(int points)
    {
        textMeshProComponent = textObject.GetComponent<TextMeshPro>();
        textMeshProComponent.text = "+" + points.ToString();
        textAnimation = textObject.GetComponent<Animation>();
        textAnimation.Play();
    }

    void UpdateDecisionTree()
    {
        // all the obstacles and coins
        List<GameObject> obstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("JumpObstacle"));
        List<GameObject> coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("cointag"));
        List<GameObject> diamonds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Obstacle"));

        closestCoin = null;
        closestDiamond = null;
        closestJumpObstacle= null;

        float minDistance = float.MaxValue;
        float secondMinDistance = float.MaxValue;
        float thirdMinDistance = float.MaxValue;

        // find the closest object
        foreach (GameObject obstacle in obstacles)
        {
            float distance = obstacle.transform.position.z - transform.position.z;
            if (distance > 0 && distance < minDistance)
            {
                minDistance = distance;
                closestJumpObstacle = obstacle;
            }
        }

        foreach (GameObject coin in coins)
        {
            float distance = coin.transform.position.z - transform.position.z;
            if (distance > 0 && distance < secondMinDistance)
            {
                secondMinDistance = distance;
                closestCoin = coin;
            }
        }

        foreach (GameObject diamond in diamonds)
        {
            float distance = diamond.transform.position.z - transform.position.z;
            if (distance > 0 && distance < thirdMinDistance)
            {
                thirdMinDistance = distance;
                closestDiamond = diamond;
            }
        }

        moveVector.z = 0;

        // move player based on type of obstacle and distance
        if (closestJumpObstacle != null && closestJumpObstacle.transform.position.z - transform.position.z < 3 && characterController.isGrounded)
        {
            moveVector.y = jumpSpeed;
            animator.Play("Jump");
        }
        else if (closestDiamond != null)
        {
            Transform parentTransform = closestDiamond.transform;
            var position = transform.position.x;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform childTransform = parentTransform.GetChild(i);
                GameObject child = childTransform.gameObject;
                if (GameObject.Find("Body").GetComponent<Renderer>().material.color == child.GetComponent<Renderer>().material.color)
                {
                    moveVector.x = child.transform.position.x * 3f;
                    position = child.transform.position.x;
                }
            }
            if (closestDiamond.transform.position.z - transform.position.z < 4 && characterController.isGrounded && Mathf.Abs(transform.position.x - position) > 0.3)
            {
                moveVector.y = jumpSpeed;
                animator.Play("Jump");
            }
        }
        else if (closestCoin != null && closestCoin.transform.position.z - transform.position.z < 10)
        {
            moveVector.x = closestCoin.transform.position.x;
        }

        moveVector.y -= gravity * Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);
    }
}
