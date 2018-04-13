using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
    string id;
    private IEnumerator coroutine;
    Vector3 prevPosition;

    List<UserController> users;

    // Use this for initialization
    void Start()
    {
        id = System.Guid.NewGuid().ToString("N");
        prevPosition = Vector3.zero;
        users = new List<UserController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartTracking()
    {
        while (true)
        {
            foreach (UserController user in users)
            {
                if (user.GetPrevPosition() != user.gameObject.transform.position) user.trackUser(id);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<UserController>() != null)
        {
            users.Add(col.gameObject.GetComponent<UserController>());
            if (coroutine == null)
            {
                coroutine = StartTracking();
                StartCoroutine(coroutine);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<UserController>() != null)
        {
            users.Remove(col.gameObject.GetComponent<UserController>());
            if (users.Count == 0)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

        }
    }
}