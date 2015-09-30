namespace LottoSend.com
{
    /// <summary>
    /// Consists of ways to pay (merchants names)
    /// </summary>
    public enum WayToPay
    {
        Offline,
        Neteller
    }

    /// <summary>
    /// Includes ways to run tests (web or mobile)
    /// </summary>
    public enum Environment
    {
        Web,
        Mobile
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
