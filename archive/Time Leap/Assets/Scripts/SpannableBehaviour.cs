using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpannableBehaviour : MonoBehaviour
{
public float lifeSpan = 0.0f;

void PostAt(float t)
{
        if(t > lifeSpan)
        {
                Destroy(this.gameObject);
        }
}

void OnWillRenderObject()
{
        PostAt(GlobalTimer.Instance.time);
}

void Start()
{

}

void Update()
{

}
}
