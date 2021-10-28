using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
public Transform floor;
public GameObject[] objects;
float xOff = 0.0f;
// Start is called before the first frame update
void Start()
{
}

void OnWillRenderObject()
{
}

// Update is called once per frame
void Update()
{
        // int sPos = Random.Range(0, objects.Length);
        GameObject obj = objects[0];
        Instantiate(obj, floor);
}
}
