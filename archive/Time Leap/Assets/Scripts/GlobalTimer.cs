
public sealed class GlobalTimer
{
private static readonly GlobalTimer instance = new GlobalTimer();

public float time = 0.0f;
static GlobalTimer()
{
}
private GlobalTimer()
{
}
public static GlobalTimer Instance
{
        get
        {
                return instance;
        }
}
}
