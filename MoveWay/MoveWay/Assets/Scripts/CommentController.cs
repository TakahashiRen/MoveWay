using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentController : MonoBehaviour
{
    public string text;
    public float speed;
    GetScreenData scrData;

	void Start ()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GetComponent<Text>().text = text;
        Destroy(gameObject, 5f);
	}
	
	void Update ()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
	}

    public void InstantiateComment(string text, float speed)
    {
        GameObject commentObj = Instantiate(this.gameObject, new Vector3(Screen.width / 2, 0, 0), transform.rotation);
        commentObj.GetComponent<CommentController>().text = text;
        commentObj.GetComponent<CommentController>().speed = speed;
    }

    public void InstantiateComment(string text, float speed, Vector3 pos)
    {
        GameObject commentObj = Instantiate(this.gameObject, pos, transform.rotation);
        commentObj.GetComponent<CommentController>().text = text;
        commentObj.GetComponent<CommentController>().speed = speed;
    }

    public void InstantiateComment(string text, float speed, bool isRandom,float min,float max)
    {
        scrData = GetComponent<GetScreenData>();

        Vector3 pos = new Vector3(Screen.width / 2, 0, 0);
        GameObject commentObj = Instantiate(this.gameObject, pos, transform.rotation);
        if (isRandom == true)
        {
            pos = new Vector3(Screen.width / 2, Random.Range(min, max), 0);
            commentObj.transform.position = pos;

        }
        commentObj.GetComponent<CommentController>().text = text;
        commentObj.GetComponent<CommentController>().speed = speed;
    }       
}
