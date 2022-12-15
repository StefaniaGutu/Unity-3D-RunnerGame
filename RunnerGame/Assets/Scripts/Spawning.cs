using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject jumpObstacle;
    private bool spawning = true;

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (spawning)
                {
                    var obs = ObjectPools.Instance.Get();
                    obs.transform.position = obstacle.GetComponentInChildren<Transform>().GetChild(Random.Range(0, 4)).position;
                    obs.transform.rotation = transform.rotation;
                    obs.gameObject.SetActive(true);
                }
                else
                {
                    Instantiate(obstacle);
                }
            }
            else
            {
                if (spawning)
                {
                    var obs = ObjectPools.Instance.Get();
                    obs.transform.position = jumpObstacle.transform.position;
                    obs.transform.rotation = transform.rotation;
                    obs.gameObject.SetActive(true);
                }
                else
                {
                    Instantiate(jumpObstacle);
                }
            }

            spawning = !spawning;

            yield return new WaitForSeconds(10f / PlayerController.speed);
        }
    }
}
