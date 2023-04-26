using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchAI : MonoBehaviour
{
    public Image off;
    public Image on;
    public bool isAIControlled;
   
    void Start()
    {
        isAIControlled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isAIControlled = !isAIControlled;
        }

        if (isAIControlled)
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
        }
        else
        {
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
        }
    }
}
