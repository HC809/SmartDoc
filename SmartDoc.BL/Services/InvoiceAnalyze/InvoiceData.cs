namespace SmartDoc.BL.Services.InvoiceAnalyze;
public sealed record InvoiceData(
    string VendorName,
    string VendorAddress,
    string CustomerName,
    string CustomerAddress,
    string InvoiceId,
    string InvoiceDate,
    string Total,
    List<Item> Items);

public sealed class Item
{
    public Item()
    {
        Name = "N/D";
        UnitPrice = "N/D";
        Quantity = "N/D";
        Total = "N/D";
    }

    public string Name { get; set; }
    public string UnitPrice { get; set; }
    public string Quantity { get; set; }
    public string Total { get; set; }
}
