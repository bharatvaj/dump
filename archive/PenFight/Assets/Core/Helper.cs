using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
  //FIXME
  public static Player GetPlayer(GameObject go){
    Player p = go.name == "p1" ? GameEngine.GetInstance().p1 : GameEngine.GetInstance().p2;
    return p;
  }

  //TODO
  public static float CalculatePivot(Configuration c){
    return 0.0f;
  }

  //TODO
  public static int GetWinner(Match m){
    return 0;
  }

  //TODO
  public static void Randomize(Player p){
    p.id = "stupidBot97";
  }
}
