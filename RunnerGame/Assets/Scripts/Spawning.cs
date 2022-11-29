using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject obstacle;
    private bool spawning = true;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
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

            spawning = !spawning;

            yield return new WaitForSeconds(10f / PlayerController.speed);
        }
    }
}
