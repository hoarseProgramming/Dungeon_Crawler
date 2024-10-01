//Dungeon Crawler.


//Game Loop
class Dice
{
    public int NumberOfDice { get; set; }
    public int SidesPerDie { get; set; }
    public int Modifer { get; set; }

    public int Throw()
    {
        Random r = new();
        int sum = 0;
        for (int i = 0; i < NumberOfDice; i++)
        {
            sum += r.Next(1, SidesPerDie + 1);
        }
        return sum + Modifer;
    }
    public override string ToString()
    {
        return $"{NumberOfDice}d{SidesPerDie}+{Modifer}";
    }
    public  Dice(int numberOfDice, int sidesPerDie, int modifer)
    {
        NumberOfDice = numberOfDice;
        SidesPerDie = sidesPerDie;
        Modifer = modifer;
    }
}
