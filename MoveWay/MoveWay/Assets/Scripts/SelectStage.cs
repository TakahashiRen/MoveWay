using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour
{
    public int StageNum;
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void PushButton()
    {
        PlayerPrefs.SetInt("StageNum", StageNum);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
