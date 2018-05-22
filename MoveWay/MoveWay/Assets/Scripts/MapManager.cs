using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //マス目　＝　マス gird
    //タイル　＝　マスの塊、親、

    public enum mapObjectNum
    {
        Wall = 0x0,
        Road = 0x1,
        Key = 0x2,
        Player = 0x04,
    }
    //マップに配置するオブジェクト群
    public GameObject[] mapObject;

    //マップのマスの最大幅と高さ
    uint mapHeight = 9;
    uint mapWidth = 9;

    //マップデータ
    int[,] mapData =
    {
        { 1,1,1,1,1,1,1,1,1},
        { 1,2,2,1,1,1,1,1,1},
        { 1,2,1,1,1,1,1,1,1},
        { 1,1,1,1,2,1,1,1,1},
        { 1,1,1,2,3,1,1,1,1},
        { 1,1,1,1,1,1,1,1,1},
        { 1,1,1,1,1,1,1,1,1},
        { 1,1,1,1,1,1,1,1,1},
        { 1,1,1,1,1,1,1,1,1},
    };

    //配置されたマスの親
    GameObject[] gridParent;

    //クリックされた親
    int[] clickedTileNum;
    //親を選択した数
    int parentSelectCount;

    //プレイヤー
    public GameObject player;

    void Start()
    {
        //マスの親の数を決める
        uint parentMaxNum = (mapHeight / 3) * (mapWidth / 3);
        //親を生成する
        gridParent = new GameObject[parentMaxNum];
        for(int i = 0;i < parentMaxNum;i++)
        {
            //親の生成
            gridParent[i] = new GameObject("GridParent" + i.ToString());

            //親の位置を設定
            Vector3 pos = new Vector3();
            pos.x = ((-64 * 4.5f) + (i % 3) * 192);
            pos.y = ((64 * 4.5f) - (i / 3) * 192);
            pos.z = 0.0f;

            gridParent[i].transform.position = pos;
        }

        for(int i = 0;i < mapHeight;i++)
        {
            for(int j = 0;j < mapWidth;j++)
            {
                //位置の設定
                Vector2 pos;
                pos.x = ((-64 * 4) + j * 64);
                pos.y = ((64 * 4) - i * 64);
                //所属する親の決定
                uint parentNum = (uint)(j / 3 + i / 3 * 3);
                //マップデータを読み対応したマップオブジェクトを生成する
                GameObject obj = Instantiate(mapObject[mapData[i, j]],new Vector3(pos.x,pos.y,0.0f),transform.rotation);
                //親の設定
                obj.transform.parent = gridParent[parentNum].transform;
            }
        }


        //各種初期化
        parentSelectCount = 0;
        clickedTileNum = new int[2];
    }

    void Update()
    {
        CheckClickParent();

        if(parentSelectCount == 2)
        {
            ChangeParentPosition(clickedTileNum[0], clickedTileNum[1]);
            clickedTileNum[0] = 0;
            clickedTileNum[1] = 0;
            parentSelectCount = 0;
        }
    }

    public mapObjectNum GetMapData(int dx,int dy)
    {

        if(dx <= 8  && dx >= 0 && dy <= 8 && dy >= 0)
        {
            return (mapObjectNum)mapData[dy, dx];
        }

        return 0;
    }

    private void CheckClickParent()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int dx = (int)Mathf.Abs((mousePos.x + (64 * 4.5f)) / 192);
            int dy = (int)Mathf.Abs((mousePos.y - (64 * 4.5f)) / 192) * 3;
            clickedTileNum[parentSelectCount] = dx + dy;

            if(clickedTileNum[parentSelectCount] >= 0 && clickedTileNum[parentSelectCount] <= 8)
            {
                parentSelectCount++;
            }
            else
            {
                clickedTileNum[parentSelectCount] = 0;
            }
        }
    }

    private void ChangeParentPosition(int parentNum1,int parentNum2)
    {
        Vector3 tmp;
        tmp = gridParent[parentNum1].transform.position;
        gridParent[parentNum1].transform.position = gridParent[parentNum2].transform.position;
        gridParent[parentNum2].transform.position = tmp;
        GameObject tmp2;
        tmp2 = gridParent[parentNum1];
        gridParent[parentNum1] = gridParent[parentNum2];
        gridParent[parentNum2] = tmp2;
    }

    private void ChangeMapData(int parentNum1,int parentNum2)
    {

    }

    private void ChangePlayerPos(int tileNum1,int tileNum2)
    {

    }
    
}   
