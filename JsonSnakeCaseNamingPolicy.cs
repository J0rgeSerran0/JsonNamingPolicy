namespace System.Text.Json
{
    public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (name == null) return String.Empty;
            
            name = name.Trim();
            if (String.IsNullOrEmpty(name)) return String.Empty;

            // Upper Case Characters
            var nameArray = name.ToArray();
            var uppers = (from character in Enumerable.Range(0, name.Length) where Char.IsUpper(nameArray[character]) select character);
            name = TranformName(ref uppers, name);

            // White Space Characters
            nameArray = name.ToArray();
            var whiteSpaces = (from character in Enumerable.Range(0, name.Length) where Char.IsWhiteSpace(nameArray[character]) select character);
            name = TranformName(ref whiteSpaces, name);

            return name.ToLower();
        }

        private string TranformName(ref IEnumerable<int> positions, string name)
        {
            var nameLength = name.Length;

            if (positions.Count() != nameLength)
            {
                var position = 0;

                foreach (var item in positions.Reverse())
                {
                    position = Convert.ToInt32(item);

                    if (position != 0)
                    {
                        if (name.Substring(position - 1, 1) == "_" &&
                            name.Substring(position, 1) == " ")
                            name = $"{name.Substring(0, position).Trim()}{name.Substring(position + 1).Trim()}";
                        else if (position + 1 != nameLength &&
                            name.Substring(position - 1, 1).Any(char.IsUpper) &&
                            name.Substring(position + 1, 1).Any(char.IsLower))
                            name = $"{name.Substring(0, position).Trim()}_{name.Substring(position).Trim()}";
                        else if (!name.Substring(position - 1, 1).Any(char.IsUpper) &&
                            name.Substring(position - 1, 1) != "_")
                            name = $"{name.Substring(0, position).Trim()}_{name.Substring(position).Trim()}";
                    }
                }
            }

            return name;
        }
    }
}
