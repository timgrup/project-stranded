using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVFX;
    [SerializeField] float speed;
    [SerializeField] float heightDifference;
    Rigidbody rb;
    float startTime;
    float startY;
    bool isExploded = false;

    private void Awake()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (transform.position.y - startY > heightDifference && !isExploded)
        {
            isExploded = true;
            Destroy(transform.Find("Body").gameObject);
            var vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, vfx.main.duration);
            Destroy(gameObject);
        }
        else
        {
            rb.AddForce(Vector3.up * speed * (1 + (Time.time - startTime)) * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
