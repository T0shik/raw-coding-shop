namespace RawCoding.Shop.UI
{
    public struct ShopConstants
    {
        public struct Policies
        {
            public const string Customer = nameof(Customer);
        }
        public struct Schemas
        {
            public const string Guest = "GuestCookie";
        }

        public struct Roles
        {
            public const string Guest = nameof(Guest);
            public const string Admin = nameof(Admin);
        }
    }
}