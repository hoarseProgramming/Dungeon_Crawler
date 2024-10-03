

using System.ComponentModel;

abstract class Character : LevelElement
{
    public string Name { get; set; }
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public void Attack(Character opponent)
    {
        int attack = AttackDice.Throw();
        int defence = opponent.DefenceDice.Throw();
        int damage = CalculateDamage(attack, defence);
        opponent.HP -= damage;

        PrintVoiceLine(this, opponent, attack, defence, damage);
        
    }
    public int CalculateDamage(int attack, int defence)
    {
        int damage = attack - defence;
        if (damage > 0) 
        { 
            return damage; 
        }
        else 
        { 
            return 0; 
        }
    }
    public void PrintVoiceLine(Character attacker, Character opponent, int attack, int defence, int damage)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (attacker is Hero)
        {
            if (damage == 0)
            {
                Console.WriteLine($"You (ATK: {AttackDice} => {attack}) attacked the {((opponent is Rat) ? "rat" : "snake")} " +
                    $"(Def: {opponent.DefenceDice} => {defence}), not even giving it a scratch.");
            }
            else if (damage < 5)
            {
                Console.WriteLine($"You (ATK: {AttackDice} => {attack}) attacked the {((opponent is Rat) ? "rat" : "snake")} " +
                    $"(Def: {opponent.DefenceDice} => {defence}), giving it a small wound.");
            }
            else
            {
                Console.WriteLine($"You (ATK: {AttackDice} => {attack}) attacked the {((opponent is Rat) ? "rat" : "snake")} " +
                    $"(Def: {opponent.DefenceDice} => {defence}), severely wounding it!");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (damage == 0)
            {
                Console.WriteLine($"The {this} (ATK: {AttackDice} => {attack}) attacked you " +
                    $"(Def: {DefenceDice} => {defence}) but missed terribly. \"What a fool!\"");
            }
            else if (damage < 5)
            {
                Console.WriteLine($"The {this} (ATK: {AttackDice} => {attack}) attacked you " +
                    $"(Def: {DefenceDice} => {defence}). \"Bah! It's only a fleshwound!\"");
            }
            else
            {
                Console.WriteLine($"The {this} (ATK: {AttackDice} => {attack}) attacked you " +
                    $"(Def: {DefenceDice} => {defence}). \"Hmph! You're making me angry\"");
            }
        }

    }

    //public abstract void Move(List<LevelElement> currentLevel);
    //public abstract Position GetPotentialPosition();
    public LevelElement CheckCollision(List<LevelElement> currentLevel, Position potentialPosition)
    {
        foreach (LevelElement element in currentLevel)
        {
            if (element.Position.X == potentialPosition.X && element.Position.Y == potentialPosition.Y)
            {
                return element;         
            }
        }
        return null;
    }
}
