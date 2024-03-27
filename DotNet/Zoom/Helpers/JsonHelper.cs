using System.Text.Json;

namespace Zoom.Helpers
{
    public class JsonHelper
    {
        public static bool IsJsonObject(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static bool IsJsonArray(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            jsonString = jsonString.Trim();
            return jsonString.StartsWith("[") && jsonString.EndsWith("]");
        }
    }
}
