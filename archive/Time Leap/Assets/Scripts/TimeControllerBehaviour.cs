using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerBehaviour : MonoBehaviour
{

public float time = 0.0f;

void Start()
{

}

void Update()
{
        time = GlobalTimer.Instance.time += 0.02f;
}
}
