namespace InventorySystemDepEd.Api.Helpers
{
    public static class StringHelper
    {
        public static string ToProperCase(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Trim().ToLower();

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i][1..];
            }

            return string.Join(" ", words);
        }
    }
}
