using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody rb;
    public Vector3 Offset;
    public float speed;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 playerforward = (rb.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position, player.position + player.transform.TransformVector(Offset) + playerforward*(-5f), speed * Time.deltaTime);
        transform.LookAt(player);
    }
}
