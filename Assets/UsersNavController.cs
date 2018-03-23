using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersNavController : MonoBehaviour {

    System.DateTime startDate = new System.DateTime(2018, 1, 1, 10, 0, 0);
    float timer = 0;

    void Start () {
        float[][] ProbArrs = new float[][]
        {
            new float[] { 0.021f, 0.002f, 0.003f, 0.005f, 0.89f, 0.072f, 0.004f, 0.003f },
            new float[] { 0.06f, 0.025f, 0.05f, 0.85f, 0.015f }
        };

        UserController[] users = GetComponentsInChildren<UserController>();
        for(int i = 0; i <= users.Length; i++)
        {
            users[i].SetProbArr(ProbArrs[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
    }

    public System.DateTime GetDateTime(float coeff)
    {
        return startDate.AddSeconds(timer * coeff);
    }
}
