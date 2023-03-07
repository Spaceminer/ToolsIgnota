namespace ToolsIgnota.Data.Models;

public record CombatManagerResponse<T>(
    string Name,
    int Id,
    T Data);

public record CMState(
    int? Round,
    string CR,
    int XP,
    int RulesSystem,
    IEnumerable<CMCreature> CombatList,
    Guid CurrentCharacterId,
    CMInitiativeCount CurrentInitiativeCount);

public record CMCreature(
    string Name,
    Guid ID,
    CMInitiativeCount InitiativeCount,
    int Hp,
    int MaxHp,
    bool IsMonster,
    bool IsActive,
    bool IsHidden,
    IEnumerable<CMCondition> activeConditions);

public record CMInitiativeCount(
    int Base,
    int Dex,
    int Tiebreaker) 
    : IComparable<CMInitiativeCount>
{
    public int CompareTo(CMInitiativeCount other)
    {
        var b = Base.CompareTo(other.Base);
        var d = Dex.CompareTo(other.Dex);
        var t = Tiebreaker.CompareTo(other.Tiebreaker);

        if (b != 0) return b;
        if (d != 0) return d;
        if (t != 0) return t;

        return 0;
    }
}

public record CMCondition(
    string Name,
    string Text,
    string Image,
    int? Turns,
    CMInitiativeCount InitiativeCount);
