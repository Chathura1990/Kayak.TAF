using System.Text.RegularExpressions;

namespace Utilities
{
    public static class StringUtils
    {
        public static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
        
        public static string GenerateStringWithLength(int stringLength)
        {
            return GenerateStringWithLengthAndValue(stringLength, 'x');
        }
        
        public static string GenerateStringWithLengthAndValue(int stringLength, char value)
        {
            return new string(value, stringLength);
        }
    }
}