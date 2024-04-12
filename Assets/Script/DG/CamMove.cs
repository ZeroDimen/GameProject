using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    [SerializeField] [Range(-1.0f, 1.0f)] private float _MoveSpeed = 1;

    private void Update()
    {
        transform.position += Vector3.right
        *_MoveSpeed * Time.deltaTime;
    }
}
