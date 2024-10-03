
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
        string heroIsAttacking = $"You (ATK: {AttackDice} => {attack}) attacked the {opponent.Name} " +
                    $"(Def: {opponent.DefenceDice} => {defence}) for {damage} damage";
        string enemyIsAttacking = $"The {Name} (ATK: {AttackDice} => {attack}) attacked you " +
                    $"(Def: {DefenceDice} => {defence}) for {damage} damage";


        if (attacker is Hero)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (opponent.HP <= 0)
            {
                Console.WriteLine($"{heroIsAttacking}, sending it to a gruesome death.".PadRight(120));
            }
            else if (damage == 0)
            {
                Console.WriteLine($"{heroIsAttacking}, not even giving it a scratch.".PadRight(120));
            }
            else if (damage < 5)
            {
                Console.WriteLine($"{heroIsAttacking}, giving it a small wound.".PadRight(120));
            }
            else
            {
                Console.WriteLine($"{heroIsAttacking}, severely wounding it!".PadRight(120));
            }
        }
        else
        {
            Console.ForegroundColor = this.SpriteColor;
            if (opponent.HP <= 0)
            {
                Console.WriteLine($"{enemyIsAttacking}! You die.".PadRight(120));
            }
            if (damage == 0)
            {
                Console.WriteLine($"{enemyIsAttacking}! {opponent.Name}: \"What a fool!\"".PadRight(120));
            }
            else if (damage < 5)
            {
                Console.WriteLine($"{enemyIsAttacking}! {opponent.Name}: \"Bah! It's only a fleshwound!\"".PadRight(120));
            }
            else
            {
                Console.WriteLine($"{enemyIsAttacking}! {opponent.Name}: \"Hmph! You're making me angry\"".PadRight(120));
                
            }
        }
    }

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
