using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Match
{
  public string p1;
  public string p2;
  public int roundMultiplier;
  public List<Round> rounds;
}
