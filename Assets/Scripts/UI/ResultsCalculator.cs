public class ResultsCalculator
{
    public int CalculateSummary(ResultsOfTheMonth results)
    {
        return results.PurchaseCosts + results.EmergencyCosts + results.OrderIncome + results.BankIncome;
    }
}
