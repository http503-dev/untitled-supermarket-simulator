[System.Serializable]
public class SessionData
{
    public bool gameConnected;
    public bool webConnected;
    public string createdOn;
    public Customer customer;

    [System.Serializable]
    public class Customer
    {
        public string name;
        public string dateOfBirth;
        public bool isBlacklisted;
        public int spriteIndex;
    }

    public SessionData(bool gameConnected, bool webConnected, string createdOn, Customer customer)
    {
        this.gameConnected = gameConnected;
        this.webConnected = webConnected;
        this.createdOn = createdOn;
        this.customer = customer;
    }

    public void SetCustomer(string name, string dateOfBirth, bool isBlacklisted, int spriteIndex)
    {
        customer = new Customer
        {
            name = name,
            dateOfBirth = dateOfBirth,
            isBlacklisted = isBlacklisted,
            spriteIndex = spriteIndex
        };
    }
}