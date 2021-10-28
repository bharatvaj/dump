using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListener : MonoBehaviour
{
  public GameController gameController;
  public GameObject p1, p2;
    void Start()
    {
        //Test
        gameController.OnPlayerFall(Helper.GetPlayer(p1));
        gameController.OnBothFall();
    }

    void Update()
    {
    }
}
