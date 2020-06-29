using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] Transform playerPivot;
    [SerializeField] float speed = 2.0f;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, playerPivot.eulerAngles.y, 0f);
        Debug.Log(playerPivot.transform.localPosition);
        Debug.Log(playerPivot.transform.position);

        if(Vector3.Distance(transform.position, playerPivot.transform.position) > Mathf.Epsilon)
        {
            var direction = Vector3.MoveTowards(transform.position, playerPivot.transform.position, speed * Time.deltaTime);
            transform.position = direction;
        }
    }
}
