public abstract class ResetableObjForInit : ObjectForInitialization, IResetable
{
    private ResetContoroller _resetController;

    public override void Init()
    {
        base.Init();
        _resetController = ServiceLocator.Instance.Get<ResetContoroller>();
        _resetController.AddResetable(this);
    }

    public abstract void Reset();
}

public abstract class ResetableClassForInit : ClassForInitialization, IResetable
{
    private ResetContoroller _resetController;

    public ResetableClassForInit() : base() { }

    public override void Init()
    {
        _resetController = ServiceLocator.Instance.Get<ResetContoroller>();
        _resetController.AddResetable(this);
    }

    public abstract void Reset();
}
