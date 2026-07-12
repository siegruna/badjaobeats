using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 scrollDirection = Vector3.right;

    [Range(0f, 50f)]
    public float scrollSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.position += scrollDirection.normalized * scrollSpeed * Time.deltaTime;
    }
}
