using System;

[Serializable]
public class ActionLog
{
    public ActionType actionType;
    public DateTime timestamp;

    public ActionLog(ActionType actionType, DateTime timestamp)
    {
        this.actionType = actionType;
        this.timestamp = timestamp;
    }
}
