using System;

namespace Poc.ConsoleApp.Commands.Grouping;

public class CatalogueTitle
{
    public CatalogueTitle(
        int currencyId,
        string isbn13,
        int licenseModelId,
        DateTime dateModified)
    {
        Id = Guid.NewGuid();
        CurrencyId = currencyId;
        Isbn13 = isbn13;
        LicenseModelId = licenseModelId;
        DateModified = dateModified;
    }

    public Guid Id { get; set; }
    public int CurrencyId { get; set; }
    public string Isbn13 { get; set; }
    public int LicenseModelId { get; set; }
    public DateTime DateModified { get; set; }
}
