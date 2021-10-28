using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
  public const int P_LOCAL = 0;
  public const int P_AI = 1;
  public const int P_ONLINE = 2;

  public string id;
  public int type;
  public List<Part> partsOwned;
  public Configuration configuration;
  public List<Match> matches;
}
