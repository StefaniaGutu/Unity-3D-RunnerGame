using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMatGen : MonoBehaviour
{
    public GameObject[] other;

    void Start()
    {
        List<Color> color = new List<Color> { Color.magenta, Color.green, Color.blue, Color.yellow};

        if (transform.name.Equals("Player"))
        {
            var partOfBody = "Body";
            Color randomColor = color[Random.Range(0, color.Count)];
            GameObject.Find(partOfBody).GetComponent<Renderer>().material.color = randomColor;
        }

        if (!transform.childCount.Equals(0))
        {
            foreach (GameObject item in other)
            {
               Color randomColor = color[Random.Range(0, color.Count)];
               item.GetComponent<Renderer>().material.color = randomColor;
               color.Remove(randomColor);
            }
        }
    }
}
