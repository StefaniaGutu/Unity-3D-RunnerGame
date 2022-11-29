using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    public static ObjectPools Instance { get;set; }

    public CoinRotate Coin;

    private Queue<CoinRotate> Items = new Queue<CoinRotate>();

    private void Awake(){
        Instance = this;
    }

    public CoinRotate Get(){
        if (Items.Count == 0){
            CoinRotate obstacle = Instantiate(Coin);
            obstacle.gameObject.SetActive(false);
            Items.Enqueue(obstacle);
        }
        return Items.Dequeue();
    }

    public void ReturnToPool(CoinRotate obj){
        obj.gameObject.SetActive(false);
        Items.Enqueue(obj);
    }
/*     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } */
}
