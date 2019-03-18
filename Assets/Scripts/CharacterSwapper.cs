using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwapper : MonoBehaviour
{
    bool isCandleActive = true;
    public GameObject candle;
    public GameObject human;

    // Start is called before the first frame update
    void Start()
    {
        candle.SetActive(true);
        human.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCandleActive == true)
        {
            if (Input.GetButton("yButton"))
            {
                human.SetActive(true);
                candle.SetActive(false);
                isCandleActive = false;
            }
        }
        else
        {
            if (Input.GetButton("yButton"))
            {
                human.SetActive(false);
                candle.SetActive(true);
                isCandleActive = true;
            }
        }
    }
}
