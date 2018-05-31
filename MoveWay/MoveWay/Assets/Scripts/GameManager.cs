using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float timer;

    public GameObject countDownText;

    public GameObject mapManager;

    public GameObject comment;

	void Start ()
    {
        timer = 180;
	}
	
	void Update ()
    {
        //ゲーム開始時のカウント
        if (timer >= 0)
        {
            if (timer % 60 == 0)
            {
                float min = GetComponent<GetScreenData>().getScreenBottomRight().y + 50.0f;
                float max = GetComponent<GetScreenData>().getScreenTopLeft().y - 50.0f;
               comment.GetComponent<CommentController>().InstantiateComment((timer / 60).ToString(), 700.0f,true,min,max);
            }
        }
        else if(timer < -60)
        {
            mapManager.SetActive(true);
        }

        timer--;
    }

}
