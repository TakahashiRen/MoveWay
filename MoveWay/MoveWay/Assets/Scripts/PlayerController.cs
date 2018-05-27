using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //プレイヤーのグリッド位置
    int pDx;
    int pDy;
    //移動後のグリッド位置
    int afPDx;
    int afPDy;
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
        afPDx = pDx;
        afPDy = pDy;
        time = 30;
        itemNum = 0;
    }

    void Update ()
    {
        time++;

        if (speed < time)
        {
            CheckMove();
            TranslateMapPosition();
            time = 0;
        }
        else
        {
            Move();
        }
    }

    public void SetMapManager(MapManager mapManager)
    {
        this.mapManager = mapManager;
    }

    public void SetMapPosition(int dx,int dy)
    {
        this.pDx = dx;
        this.pDy = dy;

        this.afPDx = pDx + (int)velocity.x;
        this.afPDy = pDy + (int)velocity.y;
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

    void CheckMove()
    {
        pDx = afPDx;
        pDy = afPDy;

        bool IsMove = mapManager.IsMove(pDx + (int)velocity.x, pDy + (int)velocity.y);
        
        if (IsMove == true)
        {
            velocity.x = velocity.x;
            velocity.y = velocity.y; 
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
        afPDx = (int)velocity.x + pDx;
        afPDy = (int)velocity.y + pDy;

    }

    void Move()
    {
        float x = transform.position.x + velocity.x * (64 / speed);
        float y = transform.position.y - velocity.y * (64 / speed);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}