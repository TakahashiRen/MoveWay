using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapReader : MonoBehaviour
{
    private int[,] stageMapData;

    private string[,] sDataArray;

    public int[,] readCSVData(string path,ref string [,] sData)
    {
        StreamReader sr = new StreamReader(path);

        string strstream = sr.ReadToEnd();
        System.StringSplitOptions option = System.StringSplitOptions.RemoveEmptyEntries;

        string[] lines = strstream.Split(new char[] { '\r', '\n' }, option);

        char[] spliter = new char[1] { ',' };

        int h = lines.Length;
        int w = lines[0].Split(spliter, option).Length;

        sData = new string[h, w];

        for(int i = 0;i < h;i++)
        {
            string[] splitedData = lines[i].Split(spliter, option);
                
            for(int j = 0; j < w;j++)
            {
                sData[i, j] = splitedData[j];
            }
        }
        convert2DArrayType(ref sData,ref stageMapData, h, w);

        return stageMapData;
    }

    private void convert2DArrayType(ref string[,] sarrays, ref int[,] iarrays, int h, int w)
    {
        iarrays = new int[h, w];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                iarrays[i, j] = int.Parse(sarrays[i, j]);
            }
        }
    }
}
