using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] Transform playerPivot;
    [SerializeField] float speed = 2.0f;

    void Update()
    {
        if (Vector3.Distance(transform.position, playerPivot.transform.position) > Mathf.Epsilon)
        {
            var direction = Vector3.MoveTowards(transform.position, playerPivot.transform.position, speed * Vector3.Distance(transform.position, playerPivot.transform.position) * Time.deltaTime);
            transform.position = direction;

            transform.LookAt(playerPivot.TransformPoint(Vector3.forward * 0.1f));
        }
    }
}
