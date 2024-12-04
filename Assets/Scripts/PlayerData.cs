[System.Serializable]
public class PlayerData
{
    public string name;
    public string storeName;
    public bool isAdmin;
    public int shiftsCompleted;
    public int customersServed;
    public int itemsScanned;
    public MistakesMade mistakesMade;
    public float profitsEarned;
    public int highScore;
    public int proficiencyScore;
    public int averageTimePerTransaction;

    [System.Serializable]
    public class MistakesMade
    {
        public int restrictedSalesToMinors;
        public int excessChangeGiven;
        public int insufficientChangeGiven;
        public int customersOvercharged;
        public int customersUndercharged;
        public int insufficientCustomerPayment;

        public MistakesMade(int restrictedSalesToMinors, int excessChangeGiven, int insufficientChangeGiven,
                            int customersOvercharged, int customersUndercharged, int insufficientCustomerPayment)
        {
            this.restrictedSalesToMinors = restrictedSalesToMinors;
            this.excessChangeGiven = excessChangeGiven;
            this.insufficientChangeGiven = insufficientChangeGiven;
            this.customersOvercharged = customersOvercharged;
            this.customersUndercharged = customersUndercharged;
            this.insufficientCustomerPayment = insufficientCustomerPayment;
        }
    }

    public PlayerData(string name, string storeName, bool isAdmin, int shiftsCompleted, int customersServed, 
                      int itemsScanned, MistakesMade mistakesMade, float profitsEarned, int highScore, 
                      int proficiencyScore, int averageTimePerTransaction)
    {
        this.name = name;
        this.storeName = storeName;
        this.isAdmin = isAdmin;
        this.shiftsCompleted = shiftsCompleted;
        this.customersServed = customersServed;
        this.itemsScanned = itemsScanned;
        this.mistakesMade = mistakesMade;
        this.profitsEarned = profitsEarned;
        this.highScore = highScore;
        this.proficiencyScore = proficiencyScore;
        this.averageTimePerTransaction = averageTimePerTransaction;
    }
}
