namespace RawCoding.Shop.Domain.Extensions
{
    public static class ValueExtension
    {
        public static string ToMoney(this int v)
        {
            return $"£{v * 0.01:N2}";
        }
    }
}