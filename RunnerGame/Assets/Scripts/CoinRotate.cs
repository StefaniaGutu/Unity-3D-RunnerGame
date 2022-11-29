using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 3f, 0);
        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position - Vector3.forward,
            Mathf.Round(PlayerController.speed) * Time.deltaTime);

        if (transform.position.z <= -9f)
        {
            ObjectPools.Instance.ReturnToPool(this);
        }
    }
}
