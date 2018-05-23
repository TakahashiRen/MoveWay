using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //マス目　＝　マス gird
    //タイル　＝　マスの塊、親、

    public enum mapObjectNum
    {
        Wall    = 0,
        Road    = 1,
        Key     = 2,
        Player  = 4,
    }
    //マップに配置するオブジェクト群
    public GameObject[] mapObject;

    //マップのマスの最大幅と高さ
    int mapHeight = 9;
    int mapWidth = 9;

    //マップデータ
    int[,] baseMapData =
    {
        { 0,0,0,0,0,0,0,0,0},
        { 0,4,1,0,0,0,0,0,0},
        { 0,1,0,0,0,0,0,0,0},
        { 0,0,0,0,1,0,0,0,0},
        { 0,0,0,1,2,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0},
    };

    //いじるマップのオブジェクトデータ
    GameObject[,] mapData;

    //配置されたマスの親
    GameObject[] gridParent;

    //プレイヤー
    GameObject player;

    //選べるタイルの数
    int tileSelectNum;
    //クリックした位置のタイルの番号
    int[] clickedTileNum;

    //タイルを選んだ数
    int tileSelectCount;

    void Start()
    {
        //マスの親の数を決める
        int parentMaxNum = (mapHeight / 3) * (mapWidth / 3);
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
        //マップデータの生成
        mapData = new GameObject[9, 9];
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
                GameObject obj = null;
                switch(baseMapData[i,j])
                {
                    case (int)mapObjectNum.Wall:
                        obj = Instantiate(mapObject[0], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                        break;
                    case (int)mapObjectNum.Road:
                        obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                        break;
                    case (int)mapObjectNum.Key:
                        obj = Instantiate(mapObject[2], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                        break;
                    case (int)mapObjectNum.Player:
                        obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                        player = Instantiate(mapObject[3], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                        break;
                }
                //親の設定
                obj.transform.parent = gridParent[parentNum].transform;
                mapData[i, j] = obj;
            }
        }
        //各種初期設定    
        tileSelectNum = 2;
        clickedTileNum = new int[tileSelectNum];
        tileSelectCount = 0;
    }

    //更新処理
    void Update()
    {
        //タイルが選択されているかチェック
        CheckClickParent();
        //タイルが2枚選択されているときの処理
        if(tileSelectCount == 2)
        {
            ChangeMapData(clickedTileNum[0], clickedTileNum[1]);
            ChangeMapDataPosition(clickedTileNum[0], clickedTileNum[1]);
            tileSelectCount = 0;
        }
    }

    private void CheckClickParent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int dx = (int)Mathf.Abs((mousePos.x + (64 * 4.5f)) / 192);
            int dy = (int)Mathf.Abs((mousePos.y - (64 * 4.5f)) / 192) * 3;
            clickedTileNum[tileSelectCount] = dx + dy;

            if (clickedTileNum[tileSelectCount] >= 0 && clickedTileNum[tileSelectCount] <= 8)
            {
                tileSelectCount++;
            }
            else
            {
                clickedTileNum[tileSelectCount] = 0;
            }
        }
    }

    void ChangeMapData(int parentNum1,int parentNum2)
    {
        for(int i = 0;i < 3;i++)
        {
            for(int j = 0;j < 3;j++)
            {
                GameObject tmp;
                tmp = mapData[i + (parentNum1 / 3) * 3, j + (parentNum1 % 3) * 3];
                mapData[i + (parentNum1 / 3) * 3, j + (parentNum1 % 3) * 3] = mapData[i + (parentNum2 / 3) * 3, j + (parentNum2 % 3) * 3];
                mapData[i + (parentNum2 / 3) * 3, j + (parentNum2 % 3) * 3] = tmp;
            }
        }
    }

    void ChangeMapDataPosition(int parentNum1,int parentNum2)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 tmp;
                tmp = mapData[i + (parentNum1 / 3) * 3, j + (parentNum1 % 3) * 3].transform.position;
                mapData[i + (parentNum1 / 3) * 3, j + (parentNum1 % 3) * 3].transform.position = mapData[i + (parentNum2 / 3) * 3, j + (parentNum2 % 3) * 3].transform.position;
                mapData[i + (parentNum2 / 3) * 3, j + (parentNum2 % 3) * 3].transform.position = tmp;
            }
        }

    }

    //マップデータの取得
    public mapObjectNum GetMapData(int dx, int dy)
    {

        if (dx <= 8 && dx >= 0 && dy <= 8 && dy >= 0)
        {
            return (mapObjectNum)baseMapData[dy, dx];
        }

        return 0;
    }
}   
