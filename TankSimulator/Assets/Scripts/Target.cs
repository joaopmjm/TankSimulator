﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool hitted= false;
    void OnCollisionEnter(Collision col)
    {
        hitted = true;
    }
}
