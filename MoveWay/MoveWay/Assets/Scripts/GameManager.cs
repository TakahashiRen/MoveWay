using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float timer;

    public GameObject countDownText;

    public GameObject mapManager;

	void Start ()
    {
        timer = 3;	
	}
	
	void Update ()
    {

        if(timer > 0)
        {
            timer -= Time.deltaTime;

            countDownText.GetComponent<Text>().text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            countDownText.SetActive(false);
            mapManager.SetActive(true);
        }
    }
}
