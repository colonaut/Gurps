using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MedienKultur.Gurps.Models.Extensions;

namespace MedienKultur.Gurps.Models
{

    public abstract class Character
    {
        public int Id { get; set; } //is primary key, but we also set it, when we send data to the server ans want to check the id...
        public string Name { get; set; }
        public int SettingId { get; private set; }
        public int PlayerId { get; private set; }
    }

    public class GameMaster : Character
    {
        
    }

    public class GurpsCharacter : Character
    {

        public GurpsCharacter()
        {
            Stats = new List<Stat>();
            Cultures = new List<Culture>();
            Languages = new List<Language>();
            Advantages = new List<Advantage>();
            Disadvantages = new List<Advantage>();
            Techniques = new List<Technique>();
            Skills = new List<Skill>();
            Melee = new List<Melee>();
            Ranged = new List<Ranged>();
        }

        public Decimal Age { get; set; }
        public string Race { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }

        public IEnumerable<Stat> Stats { get; set; }
        public IEnumerable<Culture> Cultures { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<Advantage> Advantages { get; set; }
        public IEnumerable<Advantage> Disadvantages { get; set; }
        public IEnumerable<Technique> Techniques { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
        public IEnumerable<Melee> Melee { get; set; }
        public IEnumerable<Ranged> Ranged { get; set; }

    }

    public class Stat
    {
        public string Name { get; set; }
        public Decimal Score { get; set; }
        public int Points { get; set; }
    }

    public class Culture
    {

    }

    public class Language
    {
        public string Name { get; set; }
        public int Points { get; set; }
    }

    public class Advantage
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string Page { get; set; }
    }

    public class Technique
    {
    }

    public class Skill
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public string Step { get; set; }
        public string Page { get; set; }
    }

    public class Weapon
    {
        public Weapon()
        {
            Modes = new List<Mode>();
        }

        public string Name { get; set; }
        public string Page { get; set; }
        public Decimal Weight { get; set; }
        public IEnumerable<Mode> Modes { get; set; }

        public class Mode
        {
            public string Type { get; set; }
            public string Damage { get; set; }
            public int Level { get; set; }
            public int Parry { get; set; }
            public string Reach { get; set; }
            public int MinStr { get; set; }
            public string Notes { get; set; }
        }

    }

    public class Melee : Weapon
    {
        
    }

    public class Ranged : Weapon
    {
        
    }

}



