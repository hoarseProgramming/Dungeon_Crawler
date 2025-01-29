using Dungeon_Crawler.GameMacro;
using MongoDB.Bson.Serialization.Attributes;

[BsonKnownTypes(typeof(Hero), typeof(Enemy))]
abstract class Character : LevelElement
{
    public bool IsAlive { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }
    public bool WasAttackedThisTurn { get; set; }
    public bool ShouldAnimateDiceThrows { get; set; }

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
    public void EnterCombatPhaseWith(Character opponent)
    {
        Attack(opponent);
        Retaliate(opponent);
        ResetCharactersWasAttackedStatus(opponent);
    }
    public void EnterAnimatedCombatPhaseWith(Character opponent)
    {
        AttackDice.AnimatedAttack(this, opponent);
        AnimatedRetaliate(opponent);
        ResetCharactersWasAttackedStatus(opponent);
    }
    public void Attack(Character opponent)
    {
        int attack = AttackDice.Throw(this);
        int defence = opponent.DefenceDice.Throw(opponent);
        int damage = CalculateDamage(attack, defence);
        opponent.HP -= damage;
        opponent.WasAttackedThisTurn = true;

        if (opponent.HP <= 0)
        {
            opponent.IsAlive = false;
        }
        GenerateVoiceLine(this, opponent, attack, defence, damage);
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
    public void GenerateVoiceLine(Character attacker, Character opponent, int attack, int defence, int damage)
    {
        string heroIsAttacking = $"You (ATK: {AttackDice} => {attack}) attacked the {opponent.Name} " +
                    $"(Def: {opponent.DefenceDice} => {defence}) for {damage} damage.";

        string enemyIsAttacking = $"The {Name} (ATK: {AttackDice} => {attack}) attacked you " +
                    $"(Def: {opponent.DefenceDice} => {defence}) for {damage} damage.";

        MessageType messageType = attacker.WasAttackedThisTurn ? MessageType.Retaliation : MessageType.Attack;

        bool isKillingBlow = !opponent.IsAlive;

        if (attacker is Hero)
        {
            if (!opponent.IsAlive)
            {
                heroIsAttacking += " \"May you writhe in agony in thy grave!\"";
            }
            else if (damage == 0)
            {
                heroIsAttacking += " \"Hmm, something's wrong with this sword?\"";
            }
            else if (damage < 5)
            {
                heroIsAttacking += " \"Got you good feeble monster!\"";
            }
            else
            {
                heroIsAttacking += " \"Mmm, I love the smell of blood in the morning!\"";
            }
            OnLogEvent(new LogMessageSentEventArgs(new LogMessage(this, heroIsAttacking, messageType, damage, isKillingBlow, opponent)));
        }
        else
        {
            if (!opponent.IsAlive)
            {
                enemyIsAttacking += " You die!";
            }
            else if (damage == 0)
            {
                enemyIsAttacking += " \"What a fool!\"";
            }
            else if (damage < 5)
            {
                enemyIsAttacking += " \"Bah! It's only a fleshwound!\"";
            }
            else
            {
                enemyIsAttacking += " \"Hmph! You're making me angry!\"";
            }

            OnLogEvent(new LogMessageSentEventArgs(new LogMessage(this, enemyIsAttacking, messageType, damage, isKillingBlow)));
        }
    }
    public void ClearAttackText()
    {
        if (!WasAttackedThisTurn)
        {
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(" ".PadLeft(Console.BufferWidth));
            Console.WriteLine(" ".PadLeft(Console.BufferWidth));
            Console.SetCursorPosition(0, 1);
        }
        else
        {
            Console.SetCursorPosition(0, 2);
        }
    }
    public void Retaliate(Character opponent)
    {
        if (this.IsAlive && opponent.IsAlive)
        {
            opponent.Attack(this);
        }
    }
    public void AnimatedRetaliate(Character opponent)
    {
        if (this.IsAlive && opponent.IsAlive)
        {
            opponent.AttackDice.AnimatedAttack(opponent, this);
        }
    }
    public void ResetCharactersWasAttackedStatus(Character opponent)
    {
        WasAttackedThisTurn = false;
        opponent.WasAttackedThisTurn = false;
    }
    public void Move(Position potentialPosition)
    {
        RemoveFromPlayingField();
        Position = potentialPosition;
        if (IsInsideVisionRange)
        {
            Draw();
            if (this is Snake)
            {
                OnLogEvent(new LogMessageSentEventArgs(new LogMessage(this, $"The {Name} is running from you!", MessageType.Movement)));
            }
            else if (this is Rat)
            {
                OnLogEvent(new LogMessageSentEventArgs(new LogMessage(this, $"The {Name} is skittering about.", MessageType.Movement)));
            }
            else
            {
                OnLogEvent(new LogMessageSentEventArgs(new LogMessage(this, $"You move", MessageType.Movement)));
            }
        }
    }
    // public void RemoveFromPlayingField()
    // {
    //     Console.SetCursorPosition(Position.X, Position.Y);
    //     Console.Write(' ');
    // }

}
