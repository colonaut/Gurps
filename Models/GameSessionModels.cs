using System;
using System.Collections.Generic;
using CollectionJsonExtended.Core.Attributes;
using MedienKultur.Gurps.Models.Extensions;

namespace MedienKultur.Gurps.Models
{

    public class GameSession
    {
        public GameSession()
        {
            Characters = new List<CharacterReference>();
        }

        public int Id { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset PlayedAt { get; set; }

        [CollectionJsonReference(typeof(GameSetting))]
        public int SettingId { get; set; }

        [CollectionJsonReference(typeof(Slog))]
        public int SlogId { get; set; }

        public CharacterReference GameMaster { get; set; }

        public IEnumerable<CharacterReference> Characters { get; set; }


        public class CharacterReference
        {
            [CollectionJsonReference(typeof(Character))]
            public int CharacterId { get; set; }
            
            public DateTimeOffset PlayedAt { get; set; }
        }
    }


    public class Slog
    {
        public Slog()
        {
            
        }

        public int Id { get; set; }

        [CollectionJsonReference(typeof(GameSession))]
        public int GameSessionId { get; set; }
        
        public List<SlogEntry> Entries { get; set; }

        
        public class SlogEntry
        {
            public int Id { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public string AlienDateTime { get; set; }

            public string Body { get; set; }
            public string Author { get; set; }
            

        }

    }

    
}