using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 3f, 0);

        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position - Vector3.forward, 
            Mathf.Round(PlayerController.speed) * Time.deltaTime);

        if (transform.position.z <= -9f)
        {
            ObjectPools.Instance.ReturnToPool(this);
        }      
    }
}