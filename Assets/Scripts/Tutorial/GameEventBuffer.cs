using System;

public class GameEventBuffer
{
    public event Action<string> OnNewEvent;

    public void CastNewEvent(string str)
    {
        OnNewEvent?.Invoke(str);
    }
}
