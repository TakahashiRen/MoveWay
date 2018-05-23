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
    //移動速度
    public int speed;
    //時間カウント
    int time;
    //アイテム獲得数
    int itemNum;
    //マップマネージャ
    MapManager mapManager;
    void Start ()
    {
        velocity = Vector2.right;
        speed = 30;
        time = 0;
        itemNum = 0;
        transform.position = new Vector3((-64 * 4) + pDx * 64, (64 * 4) - pDy * 64);
    }

    void Update ()
    {
        if(speed <= time)
        {
            Move();
            time = 0;
        }

        time++;
    }

    public void SetMapManager(MapManager mapManager)
    {
        this.mapManager = mapManager;
    }

    public void SetMapPosition(int dx,int dy)
    {
        this.pDx = dx;
        this.pDy = dy;
    }

    public void TranslateMapPosition()
    {
        transform.position = new Vector3((-64 * 4) + pDx * 64, (64 * 4) - pDy * 64);
    }

    public int GetMapPositionX()
    {
        return pDx;
    }

    public int GetMapPositionY()
    {
        return pDy;
    }

    public void AddItemNum()
    {
        itemNum++;
    }

    public int GetItemNum()
    {
        return itemNum;
    }

    void Move()
    {
        bool IsMove = mapManager.IsMove(pDx + (int)velocity.x, pDy + (int)velocity.y);

        if (IsMove == true)
        {
            pDx += (int)velocity.x;
            pDy += (int)velocity.y;
        }
        else
        {
            if (mapManager.IsMove(pDx - (int)velocity.y, pDy - (int)velocity.x))
            {
                float tmp;
                tmp = -velocity.x;
                velocity.x = -velocity.y;
                velocity.y = tmp;
            }
            else if (mapManager.IsMove(pDx + (int)velocity.y, pDy + (int)velocity.x))
            {
                float tmp;
                tmp = velocity.x;
                velocity.x = velocity.y;
                velocity.y = tmp;
            }
            else
            {
                velocity.x = -velocity.x;
                velocity.y = -velocity.y;
            }
        }

        TranslateMapPosition();
    }
}
