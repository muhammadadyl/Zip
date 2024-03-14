namespace ZipAssessment.Model
{

    public class CreditCalculationDataOptions
    {
        public required IEnumerable<BureauScore> BureauScores { get; set; }
        public required IEnumerable<MissedPayment> MissedPayments { get; set; }
        public required IEnumerable<CompletedPayment> CompletedPayments { get; set; }
        public required IEnumerable<AgeBracket> AgeBrackets { get; set; }
        public required IEnumerable<AvailableCredit> AvailableCredits { get; set; }

    }

    public abstract class BaseEntity
    {
        public int Points { get; set; }
    }

    public abstract class BaseLimitEntity : BaseEntity
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class AgeBracket : BaseLimitEntity
    {

    }

    public class BureauScore : BaseLimitEntity
    {

    }

    public abstract class BaseDefault : BaseEntity
    {
        public bool MaxDefault { get; set; } = false;
    }

    public class AvailableCredit : BaseDefault
    {
        public bool MinDefault { get; set; } = false;
        public int Credit { get; set; }
    }

    public abstract class BasePayment : BaseDefault
    {
        public int Number { get; set; }
    }

    public class CompletedPayment : BasePayment
    {
    }

    public class MissedPayment : BasePayment
    {
    }


}
