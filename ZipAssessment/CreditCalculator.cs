using ZipAssessment.Model;

namespace ZipAssessment
{
    public class CreditCalculator : ICreditCalculator
    {
        private readonly CreditCalculationDataOptions _creditCalculationData;

        public CreditCalculator(CreditCalculationDataOptions creditCalculationData)
        {
            _creditCalculationData = creditCalculationData;
        }

        public decimal CalculateCredit(Customer customer)
        {
            var ageBracket = GetAgeBracketPoints(customer.AgeInYears);
            var bureauScore = GetBureauScorePoints(customer.BureauScore);

            if (ageBracket < 0 || bureauScore < 0)
            {
                throw new ArgumentException("Not eligible for credit");
            }

            var missedPayment = GetMissedPaymentsPoints(customer.MissedPaymentCount);

            var completedPayment = GetCompletedPaymentsPoints(customer.CompletedPaymentCount);

            var subTotal = bureauScore + missedPayment + completedPayment;

            var creditTotal = subTotal > ageBracket ? ageBracket : subTotal;

            return GetAvailableCredit(creditTotal);

        }

        private decimal GetAvailableCredit(int creditTotal)
        {
            var creditObject = _creditCalculationData.AvailableCredits.First(i => i.MinDefault);
            if (creditTotal > 0)
            {
                creditObject = _creditCalculationData.AvailableCredits.FirstOrDefault(i => i.Points == creditTotal) ?? _creditCalculationData.AvailableCredits.First(i => i.MaxDefault);
            }
            return creditObject.Credit;
        }

        private int GetBureauScorePoints(uint customerScore)
        {
            return _creditCalculationData.BureauScores.FirstOrDefault(i => customerScore >= i.Min && customerScore <= i.Max)?.Points ?? -1;
        }

        private int GetMissedPaymentsPoints(uint customerPaymentCount)
        {
            var paymentObject = _creditCalculationData.MissedPayments.FirstOrDefault(i => i.Number == customerPaymentCount) ?? _creditCalculationData.MissedPayments.First(i => i.MaxDefault);
            return paymentObject.Points;
        }

        private int GetCompletedPaymentsPoints(uint customerPaymentCount)
        {
            var paymentObject = _creditCalculationData.CompletedPayments.FirstOrDefault(i => i.Number == customerPaymentCount) ?? _creditCalculationData.CompletedPayments.First(i => i.MaxDefault);
            return paymentObject.Points;
        }

        private int GetAgeBracketPoints(uint customerAgeInyears)
        {
            return _creditCalculationData.AgeBrackets.FirstOrDefault(i => customerAgeInyears >= i.Min && customerAgeInyears <= i.Max)?.Points ?? -1;
        }
    }
}
