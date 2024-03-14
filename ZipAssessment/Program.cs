using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZipAssessment;
using ZipAssessment.Model;

public class Program
{
    private static void Main(string[] args)
    {
        var startup = new Startup();

        if (startup.CreditCalculationDataOptions == null)
        {
            throw new NullReferenceException(nameof(startup.CreditCalculationDataOptions));
        }

        var creditCalculator = new CreditCalculator(startup.CreditCalculationDataOptions);
        
        Assert.AreEqual(200, creditCalculator.CalculateCredit(new Customer(451, 2, 3, 18)));

        Assert.AreEqual(0, creditCalculator.CalculateCredit(new Customer(451, 3, 2, 18)));

        Assert.ThrowsException<ArgumentException>(() => creditCalculator.CalculateCredit(new Customer(451, 2, 3, 17)));

        Assert.ThrowsException<ArgumentException>(() => creditCalculator.CalculateCredit(new Customer(450, 2, 3, 89)));

    }
}

internal class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("AppSettings.json", optional: false);

        IConfiguration config = builder.Build();

        if (!config.GetSection("CreditCalculationData").Exists())
        {
            throw new FileNotFoundException("AppSettings.json file doesn't exist");
        }

        CreditCalculationDataOptions = config.GetSection("CreditCalculationData").Get<CreditCalculationDataOptions>();
    }

    public CreditCalculationDataOptions? CreditCalculationDataOptions { get; private set; }
}