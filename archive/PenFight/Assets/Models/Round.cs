using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Round
{
  const int CLIMATE_SPRING = 0;
  const int CLIMATE_SUMMER = 1;
  const int CLIMATE_AUTUMN = 2;
  const int CLIMATE_WINTER = 3;

  const int BORDER_TOP = 0;
  const int BORDER_BOTTOM = 01;
  const int BORDER_START = 001;
  const int BORDER_END = 0001;
  const int BORDER_ALL = BORDER_TOP | BORDER_BOTTOM | BORDER_START | BORDER_END;

  const int P1_NOT_SET = 0;
  const int P1_WON = 1;
  const int P1_LOSE = 2;

  public string bg;
  //should use values only from CLIMATE_*
  public int climate;
  //should user values only from BORDER_*
  public int border;
  //should user values only from P1_*
  public int isP1Winner = P1_NOT_SET;
}
