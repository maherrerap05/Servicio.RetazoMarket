using System.Text.Json;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    internal static class JsonDataMapper
    {
        public static JsonElement Clone(JsonDocument document) => document.RootElement.Clone();

        public static JsonDocument ToDocument(JsonElement element) =>
            JsonDocument.Parse(element.ValueKind == JsonValueKind.Undefined ? "[]" : element.GetRawText());
    }
}
