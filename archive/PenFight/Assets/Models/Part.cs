using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
  public const int PT_SNIB = 0;
  public const int PT_BARREL = 1;
  public const int PT_CAP = 2;

  public string id;
  public int type; //accepts PT_*
  public float mass; //accepts value 0.0-1.0
}
