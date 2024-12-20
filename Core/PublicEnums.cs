namespace Core
{
    public enum UserType
    {
        USER = 1,
        ADMIN = 2,
    }

    public enum TrashBinStatus
    {
        PENDING = 0,
        APPROVED = 1,
        REJECTED = 2
    }

    public enum TrashBinLatestFeedback
    {
        EMPTY,
        QUARTER,
        HALF,
        THREEQUARTER,
        FULL
    }
}
