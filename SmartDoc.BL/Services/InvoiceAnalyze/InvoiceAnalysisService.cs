using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;

namespace SmartDoc.BL.Services.InvoiceAnalyze;

internal sealed class InvoiceAnalysisService : IInvoiceAnalysisService
{
    private readonly DocumentAnalysisSettings _settings;

    public InvoiceAnalysisService(IOptions<DocumentAnalysisSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// The function `GetInvoiceAnalysisData` asynchronously retrieves and analyzes invoice data from a
    /// document using Azure Document Analysis service and returns an `InvoiceData` object.
    /// </summary>
    /// <param name="Stream">The `Stream fileStream` parameter in the `GetInvoiceAnalysisData` method
    /// represents the file stream of the invoice document that you want to analyze. This method uses
    /// Azure Cognitive Services Document Analysis to extract information from the invoice document such
    /// as vendor name, vendor address, customer name, customer address, invoice</param>
    /// <returns>
    /// The method `GetInvoiceAnalysisData` returns an `InvoiceData` object containing information
    /// extracted from the provided fileStream using Azure Document Analysis service. The extracted data
    /// includes vendor name, vendor address, customer name, customer address, invoice ID, invoice date,
    /// invoice total, and a list of items on the invoice.
    /// </returns>
    public async Task<InvoiceData> GetInvoiceAnalysisData(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(_settings.DocumentAnalysisApiKey);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(_settings.DocumentAnalysisEndpoint), credential);

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

   /// <summary>
   /// This C# function attempts to retrieve the content of a specific field from a dictionary based on
   /// the field name and type.
   /// </summary>
   /// <param name="fields">An IReadOnlyDictionary<string, DocumentField> containing document fields
   /// where the key is a string representing the field name and the value is a DocumentField
   /// object.</param>
   /// <param name="fieldName">The `fieldName` parameter is a string that represents the name of the
   /// field you are trying to retrieve the content for from the `fields` dictionary.</param>
   /// <param name="DocumentFieldType">DocumentFieldType is an enum that represents the type of a
   /// document field.</param>
   /// <returns>
   /// A `string` value is being returned from the `TryGetFieldContent` method. If the conditions are
   /// met (the field with the specified name exists in the dictionary, has a non-null content, and
   /// matches the specified field type), then the content of that field is returned as a string.
   /// Otherwise, `null` is returned.
   /// </returns>
    private string? TryGetFieldContent(IReadOnlyDictionary<string, DocumentField> fields, string fieldName, DocumentFieldType fieldType)
    {
        if (fields.TryGetValue(fieldName, out DocumentField? field) && field is not null)
        {
            if (field.FieldType == fieldType && field.Content is not null)
                return field.Content;
        }

        return null;
    }

    /// <summary>
    /// The function `GetItems` parses a dictionary of document fields to extract and populate a list of
    /// `Item` objects.
    /// </summary>
    /// <param name="fields">The `GetItems` method you provided takes an `IReadOnlyDictionary<string,
    /// DocumentField>` named `fields` as a parameter. This dictionary is used to extract information
    /// about items from a document.</param>
    /// <returns>
    /// The method `GetItems` returns a list of `Item` objects parsed from the input `fields`
    /// dictionary.
    /// </returns>
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
