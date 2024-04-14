using System;

public class PCConstructHandler
{
    private ConfirmHandler _confirmHandler;

    public PCConstructHandler()
    {
        _confirmHandler = new ConfirmHandler();
    }

    public void ConstructPC(Goods goods)
    {
        Action action = () =>
        {
            goods.ConstructPC();
        };

        _confirmHandler.ConfirmAction(action, ConfirmType.SkipTime, goods.Time);
    }
}