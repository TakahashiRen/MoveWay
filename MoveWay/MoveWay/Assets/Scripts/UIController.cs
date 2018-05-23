using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GameObject player;
	void Start ()
    {

    }
	
	void Update ()
    {
        player = GameObject.Find("Player(Clone)");

        GetComponent<Text>().text = player.GetComponent<PlayerController>().GetItemNum().ToString();
	}
}
