using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameLifecycle : MonoBehaviour
{
  public abstract void MatchStart();
  public abstract void RoundStart(int roundNo);
  public abstract void RoundEnd(int roundNo);
  public abstract void MatchEnd();
}
