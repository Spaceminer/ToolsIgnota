using System;
using System.Collections.Generic;

namespace ToolsIgnota.Data.Models
{
    public record CombatManagerResponse<T>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public T Data { get; set; }
    }

    public record CMState
    {
        public int? Round { get; set; }
        public string CR { get; set; }
        public int XP { get; set; }
        public int RulesSystem { get; set; }
        public IEnumerable<CMCreature> CombatList { get; set; }
        public Guid CurrentCharacterId { get; set; }
        public CMInitiativeCount CurrentInitiativeCount { get; set; }
    }

    public record CMCreature
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public CMInitiativeCount InitiativeCount { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public bool IsMonster { get; set; }
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<CMCondition> ActiveConditions { get; set; }
    }

    public record CMInitiativeCount : IComparable<CMInitiativeCount>
    {
        public int Base { get; set; }
        public int Dex { get; set; }
        public int Tiebreaker { get; set; }

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

    public record CMCondition
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int? Turns { get; set; }
        public CMInitiativeCount InitiativeCount{ get; set; }
    }
}
