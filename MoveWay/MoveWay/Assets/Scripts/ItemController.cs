using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    int iDx;
    int iDy;
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetMapPosition(int dx, int dy)
    {
        this.iDx = dx;
        this.iDy = dy;
    }

    public void TranslateMapPosition()
    {
        transform.position = new Vector3((-64 * 4) + iDx * 64, (64 * 4) - iDy * 64);
    }

    public int GetMapPositionX()
    {
        return iDx;
    }

    public int GetMapPositionY()
    {
        return iDy;
    }

}
