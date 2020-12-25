﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    GameObject anchor;
    Vector3 offset = new Vector3(4, 6, -20);
    void Start()
    {
        anchor = GameObject.Find("GameManager"); // gimmick!
        transform.position = new Vector3(0, 0, anchor.transform.position.z) + offset; 
    }
}
