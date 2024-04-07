using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem With Order")]
public class ProblemWithOrder : ProblemConfig
{
    public OrderConfig ProblemedOrder { get; private set; }

    private PCGenerator _pcGenerator;

    public void SetParameters(OrderConfig problemedOrder)
    {
        if (_pcGenerator == null) _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();
        ProblemedOrder = problemedOrder;
        _coinsRequirement = problemedOrder.Cost;
    }

    public override void Apply()
    {
        base.Apply();
        _pcGenerator.GenerateReusedPC(ProblemedOrder.NeededGoods);
    }
}