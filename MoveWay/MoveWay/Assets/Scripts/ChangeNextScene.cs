using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeNextScene : MonoBehaviour
{

    void Start() { }
	
	void Update() { }	

    public void ChangeScene(int SceneNum)
    {
        SceneManager.LoadScene(SceneNum);
    }
}
