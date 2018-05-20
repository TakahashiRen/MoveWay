using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //プレイヤーのグリッド位置
    int pDx;
    int pDy;
    //プレイヤーの移動したい方向
    Vector2 velocity;
    //マップマネージャ
    public MapManager mapManager;
    void Start ()
    {
        velocity = Vector2.zero;
        pDx = 1;
        pDy = 1;
	}
	
	void Update ()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            velocity = Vector2.down;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            velocity = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity = Vector2.left;
        }
        else
        {
            velocity = Vector2.zero;
        }

        MapManager.mapObjectNum mapD = mapManager.GetMapData(pDx + (int)velocity.x, pDy + (int)velocity.y);

        if(mapD != MapManager.mapObjectNum.Wall)
        {
            pDx += (int)velocity.x;
            pDy += (int)velocity.y;
        }

        transform.position = new Vector3((-64 * 4) + pDx * 64,(64 * 4) - pDy * 64);
    }
}
