using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrickToChangeScene : MonoBehaviour
{

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            GetComponent<ChangeNextScene>().ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
