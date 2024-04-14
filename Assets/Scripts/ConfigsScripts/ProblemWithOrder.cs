using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem With Order")]
public class ProblemWithOrder : ProblemConfig
{
    public OrderInstanceConfig ProblemedOrder { get; private set; }

    private PCGenerator _pcGenerator;

    public override void InitProblemValues()
    {
        
    }

    public void SetParameters(OrderInstanceConfig problemedOrder)
    {
        if (_pcGenerator == null) _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();
        ProblemedOrder = problemedOrder;
        _coins = problemedOrder.Cost;
    }

    public override void Apply()
    {
        base.Apply();
        _pcGenerator.GenerateReusedPC(ProblemedOrder.NeededGoods);
    }
}