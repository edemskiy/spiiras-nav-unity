using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectLocation
{

    public string time;
    public float x;
    public float z;
    public string userID;

    public ObjectLocation(string time, float x, float z, string userID)
    {
        this.x = (float)System.Math.Round((double)x, 2);
        this.z = (float)System.Math.Round((double)z, 2);
        this.time = time;
        this.userID = userID;
    }

    public string toJSON()
    {
        return JsonUtility.ToJson(this);
    }
}