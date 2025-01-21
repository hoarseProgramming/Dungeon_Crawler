
class Dice
{
    public int NumberOfDice { get; set; }
    public int SidesPerDie { get; set; }
    public int Modifer { get; set; }
    public string[] AnimatedDice { get; set; }

    public int Throw(Character thrower)
    {
        int sum = 0;

        for (int i = 0; i < NumberOfDice; i++)
        {
            Random r = new();
            int throwResult = r.Next(1, SidesPerDie + 1);
            sum += throwResult;
        }

        return sum + Modifer;
    }
    public void AnimatedAttack(Character attacker, Character defender)
    {
        int diceThrowersTextLength = 0;
        ClearDiceText();

        diceThrowersTextLength = PrintDiceThrower(attacker);
        int attackRoll = AnimatedThrow(attacker);

        diceThrowersTextLength = PrintDiceThrower(defender, diceThrowersTextLength);
        int defenseRoll = defender.DefenceDice.AnimatedThrow(defender, attacker.AttackDice.NumberOfDice, diceThrowersTextLength);

        int damage = attacker.CalculateDamage(attackRoll, defenseRoll);

        defender.HP -= damage;
        defender.WasAttackedThisTurn = true;

        if (defender.HP <= 0)
        {
            defender.IsAlive = false;
        }

        attacker.GenerateVoiceLine(attacker, defender, attackRoll, defenseRoll, damage);

    }
    public int AnimatedThrow(Character thrower, int thrownDices = 0, int diceThrowersTextLength = 0)
    {
        int sum = 0;

        for (int i = 0; i < NumberOfDice; i++)
        {
            Random r = new();
            int throwResult = r.Next(1, SidesPerDie + 1);
            PrintAnimatedThrow(thrower, throwResult, thrownDices, diceThrowersTextLength);
            thrownDices++;
            sum += throwResult;
        }

        return sum + Modifer;
    }
    public void PrintAnimatedThrow(Character thrower, int throwResult, int throwCount, int diceThrowersTextLength)
    {
        int x = throwCount * 10;
        int y = 23;
        int currentX = x;
        int currentY = y;

        Random r = new();
        for (int i = 0; i < 5; i++)
        {
            string currentDice = AnimatedDice[r.Next(0, SidesPerDie - 1)];
            for (int j = 0; j < currentDice.Length; j++)
            {
                if (currentDice[j] != '\r' && currentDice[j] != '\n')
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(currentDice[j]);
                    currentX++;
                }
                else if (currentDice[j] == '\n')
                {
                    if (j != 1)
                    {
                        Console.SetCursorPosition(currentX, currentY);
                        Console.Write(currentDice[j]);
                        currentX = x;
                        currentY++;
                    }
                }
            }
            Thread.Sleep(100);
            currentX = x;
            currentY = y;
        }
        string dieToPrint = AnimatedDice[throwResult - 1];

        for (int i = 0; i < dieToPrint.Length; i++)
        {

            if (dieToPrint[i] != '\n' && dieToPrint[i] != '\r')
            {
                Console.SetCursorPosition(currentX, currentY);
                Console.Write(dieToPrint[i]);
                currentX++;
            }
            else if (dieToPrint[i] == '\n')
            {
                if (i != 1)
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(dieToPrint[i]);
                    currentX = x;
                    currentY++;
                }
            }
        }

    }
    public int PrintDiceThrower(Character thrower, int diceThrowersTextLength = 0)
    {
        Console.ForegroundColor = thrower.SpriteColor;

        if (diceThrowersTextLength == 0)
        {
            Console.SetCursorPosition(0, 21);
            if (thrower is Hero)
            {
                diceThrowersTextLength = $"{thrower.Name} rolls for attack!".Length;
                Console.WriteLine($"{thrower.Name} rolls for attack!".PadRight(120));
            }
            else
            {
                diceThrowersTextLength = $"The {thrower.Name} rolls for attack!".Length;
                Console.WriteLine($"The {thrower.Name} rolls for attack!".PadRight(120));
            }
        }
        else
        {
            Console.SetCursorPosition(0, 22);
            if (thrower is Hero)
            {
                Console.WriteLine($"{thrower.Name} rolls for defence!");
            }
            else
            {
                Console.WriteLine($"The {thrower.Name} rolls for defence!");
            }
        }
        return diceThrowersTextLength;
    }
    public void ClearDiceText()
    {
        Console.SetCursorPosition(0, 21);
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(" ".PadLeft(120));
        }
    }
    public override string ToString()
    {
        return $"{NumberOfDice}d{SidesPerDie}+{Modifer}";
    }
    public static string[] LoadAnimatedDice()
    {
        string pathToAnimatedDice = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net8.0", $"\\Dice\\AnimatedDice.txt");

        string[] animatedDice = new string[8];

        using (StreamReader reader = new StreamReader(pathToAnimatedDice))
        {
            var dice = reader.ReadToEnd().Split('/');
            for (int i = 0; i < dice.Length; i++)
            {
                animatedDice[i] = dice[i];
            }
        }

        return animatedDice;
    }
    public Dice(int numberOfDice, int sidesPerDie, int modifer)
    {
        NumberOfDice = numberOfDice;
        SidesPerDie = sidesPerDie;
        Modifer = modifer;
        AnimatedDice = LoadAnimatedDice();
    }
}
