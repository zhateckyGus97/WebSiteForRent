namespace Application
{
    public static class ValidationConstants
    {
        public const int MaxFullNameLength = 100;
        public const int MaxEmailLength = 50;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 64;
        public const int MaxPhoneNumberLength = 50;
        public const int MaxRoleLength = 30;

        public const int MaxTitleLength = 100;
        public const int MaxDescriptionLength = 1000;
        public const int MaxAddressLength = 150;

        public const int MaxCommentLength = 500;

        public const string PassportPattern = "^\\d{4}-\\d{6}$";
    }
}
