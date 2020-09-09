namespace RawCoding.Shop.UI
{
    // stripe listen --forward-to https://localhost:5001/api/stripe
    public class StripeSettings
    {
        public string PublicKey { get; set; }
        public string SecretKey { get; set; }
        public string SigningSecret { get; set; }
    }
}