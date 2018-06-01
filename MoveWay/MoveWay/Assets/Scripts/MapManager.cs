using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Item    = 5,
        TreasureBox = 6,
    }
    //マップに配置するオブジェクト群
    public GameObject[] mapObject;

    //マップのマスの最大幅と高さ
    int mapHeight = 9;
    int mapWidth = 9;

    //マップデータ
    int[,] baseMapData;

    int[,] ansMapData;


    //いじるマップのオブジェクトデータ
    GameObject[,] mapData;

    //配置されたマスの親
    GameObject[] gridParent;
    GameObject[] gridParentIsChange;

    //プレイヤー
    GameObject player;

    //親
    public GameObject parent;

    //アイテム管理
    List<GameObject> items;
    //宝箱の管理
    List<GameObject> treasures;

    //選べるタイルの数
    int tileSelectNum;
    //クリックした位置のタイルの番号
    int[] clickedTileNum;
    //選択時のエフェクト
    public GameObject selectEffect;

    //タイルを選んだ数
    int tileSelectCount;

    //クリアしたかどうか
    public bool IsClear;
    bool isClearCommentDisplayed = false;


    //コメント表示オブジェクト
    public GameObject commentObj;

    //お題の画像オブジェクト
    public GameObject odaiObject;

    //シーン切り替えボタン
    public GameObject button;

    void Start()
    {
        string[,] str = new string[9, 9];
        int stageNum = PlayerPrefs.GetInt("StageNum",1);
        Debug.Log(stageNum.ToString());
        baseMapData = GetComponent<MapReader>().readCSVData(Application.dataPath + "/Resources/Stage"+ stageNum +"/Stage" + stageNum + ".csv",ref str);
        ansMapData = GetComponent<MapReader>().readCSVData(Application.dataPath + "/Resources/Stage" + stageNum +"/StageAns" + stageNum +".csv", ref str);
        SpriteRenderer sprite = odaiObject.GetComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>("Stage" + stageNum + "/StageIcon" + stageNum);
        //アイテムの管理
        items = new List<GameObject>();
        treasures = new List<GameObject>();
        //マスの親の数を決める
        int parentMaxNum = (mapHeight / 3) * (mapWidth / 3);
        //親を生成する
        gridParent = new GameObject[parentMaxNum];
        gridParentIsChange = new GameObject[parentMaxNum];
        for(int i = 0;i < parentMaxNum;i++)
        {
            //親の生成
            gridParent[i] = Instantiate(parent,transform.position,transform.rotation);
            //"GridParent" + i.ToString()
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
                        {
                            obj = Instantiate(mapObject[0], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            break;
                        }
                    case (int)mapObjectNum.Road:
                        {
                            obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            break;
                        }
                    case (int)mapObjectNum.Key:
                        {
                            obj = Instantiate(mapObject[2], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            break;
                        }
                    case (int)mapObjectNum.Player:
                        {
                            obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            player = Instantiate(mapObject[3], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            player.GetComponent<PlayerController>().SetMapPosition(i, j);
                            baseMapData[i, j] = 1;
                            break;
                        }
                    case (int)mapObjectNum.Item:
                        {
                            obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            GameObject item = Instantiate(mapObject[4], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            item.GetComponent<ItemController>().SetMapPosition(j, i);
                            baseMapData[i, j] = 1;
                            items.Add(item);
                            break;
                        }
                    case (int)mapObjectNum.TreasureBox:
                        {
                            obj = Instantiate(mapObject[1], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            GameObject treasure = Instantiate(mapObject[5], new Vector3(pos.x, pos.y, 0.0f), transform.rotation);
                            treasure.GetComponent<TreasureBoxManager>().SetMapPosition(j, i);
                            treasures.Add(treasure);
                            baseMapData[i, j] = 1;
                            break;
                        }
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

        player.GetComponent<PlayerController>().SetMapManager(this);
        IsClear = false;
    }

    //更新処理
    void Update()
    {
        if (IsClear == false)
        {
            //タイルが選択されているかチェック
            CheckClickParent();
            //タイルが2枚選択されているときの処理
            if (tileSelectCount == 2)
            {
                ChangeMapData(clickedTileNum[0], clickedTileNum[1]);
                ChangeMapDataPosition(clickedTileNum[0], clickedTileNum[1]);
                selectEffect.SetActive(false);
                tileSelectCount = 0;
            }
            //プレイヤーがアイテムをゲットしているか確認
            IsGetItem();
            //宝箱が空くかどうか確認
            CheckTreasureBoxOpen();
            IsClear = CheckClear();
        }
        else
        {
            if (isClearCommentDisplayed == false)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].GetComponent<ItemController>().Destroy();
                }
                commentObj.GetComponent<CommentController>().InstantiateComment("クリア！", 500.0f);
                isClearCommentDisplayed = true;
                button.SetActive(true);
            }

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
                selectEffect.SetActive(true);
                selectEffect.transform.position = gridParent[clickedTileNum[tileSelectCount]].transform.position;
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
        int playerX = player.GetComponent<PlayerController>().GetMapPositionX();
        int playerY = player.GetComponent<PlayerController>().GetMapPositionY();

        for (int i = 0;i < 3;i++)
        {
            for(int j = 0;j < 3;j++)
            {
                int mapData1X = j + (parentNum1 % 3) * 3;
                int mapData1Y = i + (parentNum1 / 3) * 3;
                int mapData2X = j + (parentNum2 % 3) * 3;
                int mapData2Y = i + (parentNum2 / 3) * 3;
                GameObject tmp;
                tmp = mapData[mapData1Y,mapData1X];
                mapData[mapData1Y, mapData1X] = mapData[mapData2Y, mapData2X];
                mapData[mapData2Y, mapData2X] = tmp;

                int tmp2;
                tmp2 = baseMapData[mapData1Y, mapData1X];
                baseMapData[mapData1Y, mapData1X] = baseMapData[mapData2Y, mapData2X];
                baseMapData[mapData2Y, mapData2X] = tmp2;

                if (playerX == mapData1X && playerY == mapData1Y)
                {
                    player.GetComponent<PlayerController>().SetMapPosition(mapData2X, mapData2Y);
                    player.GetComponent<PlayerController>().TranslateMapPosition();
                }
                else if(playerX == mapData2X && playerY == mapData2Y)
                {
                    player.GetComponent<PlayerController>().SetMapPosition(mapData1X, mapData1Y);
                    player.GetComponent<PlayerController>().TranslateMapPosition();
                }

                for (int k = 0; k < items.Count; k++)
                {
                    int iX = items[k].GetComponent<ItemController>().GetMapPositionX();
                    int iY = items[k].GetComponent<ItemController>().GetMapPositionY();

                    if(iX == mapData1X && iY == mapData1Y)
                    {
                        items[k].GetComponent<ItemController>().SetMapPosition(mapData2X, mapData2Y);
                        items[k].GetComponent<ItemController>().TranslateMapPosition();
                    }
                    if (iX == mapData2X && iY == mapData2Y)
                    {
                        items[k].GetComponent<ItemController>().SetMapPosition(mapData1X, mapData1Y);
                        items[k].GetComponent<ItemController>().TranslateMapPosition();
                    }
                }
                for(int l = 0;l < treasures.Count;l++)
                {
                    int iX = treasures[l].GetComponent<TreasureBoxManager>().GetMapPositionX();
                    int iY = treasures[l].GetComponent<TreasureBoxManager>().GetMapPositionY();

                    if (iX == mapData1X && iY == mapData1Y)
                    {
                        treasures[l].GetComponent<TreasureBoxManager>().SetMapPosition(mapData2X, mapData2Y);
                        treasures[l].GetComponent<TreasureBoxManager>().TranslateMapPosition();
                    }
                    if (iX == mapData2X && iY == mapData2Y)
                    {
                        treasures[l].GetComponent<TreasureBoxManager>().SetMapPosition(mapData1X, mapData1Y);
                        treasures[l].GetComponent<TreasureBoxManager>().TranslateMapPosition();
                    }

                }
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

    //移動可能かどうか
    public bool IsMove(int dx,int dy)
    {
        if (dx >= 0 && dx < 9 && dy >= 0 && dy < 9)
        {
            if (mapData[dy, dx].tag == "Wall")
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    //アイテムの総計を数える
    int GetItemNum()
    {
        return items.Count;
    }

    //プレイヤーがアイテムを獲得しているか確認
    void IsGetItem()
    {
        if (items.Count == 0) return;
        int pX = player.GetComponent<PlayerController>().GetMapPositionX();
        int pY = player.GetComponent<PlayerController>().GetMapPositionY();

        for (int i = 0;i < items.Count;i++)
        {
            int iX = items[i].GetComponent<ItemController>().GetMapPositionX();
            int iY = items[i].GetComponent<ItemController>().GetMapPositionY();

            if(iX == pX && iY == pY)
            {
                items[i].GetComponent<ItemController>().Destroy();
                items.RemoveAt(i);
                player.GetComponent<PlayerController>().AddItemNum();
            }
        }
    }

    //宝箱が開かれるかチェック
    //アイテムが全部取られていたら開ける
    void CheckTreasureBoxOpen()
    {
        if(items.Count == 0)
        {
            for(int i = 0;i < treasures.Count;i++)
            {
                treasures[i].GetComponent<TreasureBoxManager>().OpenTreasureBox();
            }
        }
    }


    bool CheckClear()
    {
        for(int i = 0;i < 9;i++)
        {
            for(int j = 0;j < 9;j++)
            {
                if(ansMapData[i,j] != baseMapData[i,j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}   
