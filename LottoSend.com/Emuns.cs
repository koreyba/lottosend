namespace LottoSend.com
{
    /// <summary>
    /// Consists of ways to pay (merchants names)
    /// </summary>
    public enum WayToPay
    {
        Offline,
        Neteller,
        TrustPay,
        Skrill,
        InternalBalance
    }

    /// <summary>
    /// Ways to choose an element in "select" 
    /// </summary>
    public enum SelectBy
    {
        Value,
        Text,
        Index
    }
}
