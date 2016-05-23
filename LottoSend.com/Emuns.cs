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
        InternalBalance,
        Moneta,
        eKonto,
        Poli
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

    /// <summary>
    /// Ways to search information
    /// </summary>
    public enum SearchBy
    {
        Key,
        Content
    }

    /// <summary>
    /// Statuses of charge back in the order processing
    /// </summary>
    public enum ChargeBackStatus
    {
        CHB, 
        CHBR,
        RR
    }
}
