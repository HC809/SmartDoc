using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace SmartDoc.BL.Services.InvoiceAnalyze;

internal sealed class InvoiceAnalysisService : IInvoiceAnalysisService
{
    private readonly string key = "b7265dd6172247ecb7b7af43a1fc2726";
    private readonly string endpoint = "https://file-analyze-service.cognitiveservices.azure.com/";

    public async Task<InvoiceData> Analyze(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", fileStream);
        AnalyzeResult result = operation.Value;

        AnalyzedDocument document = result.Documents[0];
        var docFields = document.Fields;

        InvoiceData data = new(
            TryGetFieldContent(docFields, "VendorName", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "VendorAddress", DocumentFieldType.Address) ?? "N/D",
            TryGetFieldContent(docFields, "CustomerName", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "CustomerAddress", DocumentFieldType.Address) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceId", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceDate", DocumentFieldType.Date) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceTotal", DocumentFieldType.Currency) ?? "0.00",
            GetItems(docFields));

        return data;
    }

    private string? TryGetFieldContent(IReadOnlyDictionary<string, DocumentField> fields, string fieldName, DocumentFieldType fieldType)
    {
        if (fields.TryGetValue(fieldName, out DocumentField? field) && field is not null)
        {
            if (field.FieldType == fieldType && field.Content is not null)
                return field.Content;
        }

        return null;
    }

    private List<Item> GetItems(IReadOnlyDictionary<string, DocumentField> fields)
    {
        List<Item> items = new();

        if (fields.TryGetValue("Items", out DocumentField? itemsField) && itemsField is not null)
        {
            if (itemsField.FieldType == DocumentFieldType.List)
            {
                foreach (DocumentField itemField in itemsField.Value.AsList())
                {
                    if (itemField.FieldType == DocumentFieldType.Dictionary)
                    {
                        IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                        var item = new Item();

                        if (itemFields.TryGetValue("Description", out DocumentField? itemDescriptionField) && itemDescriptionField is not null)
                        {
                            if (itemDescriptionField.FieldType == DocumentFieldType.String && itemDescriptionField is not null)
                            {
                                item.Name = itemDescriptionField.Content;
                            }
                        }

                        if (itemFields.TryGetValue("UnitPrice", out DocumentField? itemUnitPriceField) && itemUnitPriceField is not null)
                        {
                            if (itemUnitPriceField.FieldType == DocumentFieldType.Currency && itemUnitPriceField is not null)
                            {
                                item.UnitPrice = itemUnitPriceField.Content;
                            }
                        }

                        if (itemFields.TryGetValue("Quantity", out DocumentField? itemQuantityField) && itemQuantityField is not null)
                        {
                            if (itemQuantityField.FieldType == DocumentFieldType.Double && itemQuantityField is not null)
                            {
                                item.Quantity = itemQuantityField.Content;
                            }
                        }

                        if (itemFields.TryGetValue("Amount", out DocumentField? itemAmountField) && itemDescriptionField is not null)
                        {
                            if (itemAmountField.FieldType == DocumentFieldType.Currency && itemAmountField is not null)
                            {
                                item.Total = itemAmountField.Content;
                            }
                        }

                        items.Add(item);
                    }
                }
            }
        }

        return items;
    }
}
