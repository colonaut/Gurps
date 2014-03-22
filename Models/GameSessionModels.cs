using System;
using System.Collections.Generic;
using CollectionJsonExtended.Core.Attributes;
using MedienKultur.Gurps.Models.Extensions;

namespace MedienKultur.Gurps.Models
{
    public class CharacterReference
    {
        [CollectionJsonReference(typeof(Character))]
        public int CharacterId { get; set; }

        public DateTimeOffset PlayedAt { get; set; }
    }

    public class UserReference
    {
        [CollectionJsonReference(typeof(ApplicationUser))]
        public int UserId { get; set; }

        public string Name { get; set; }
    }

    public class GameSettingReference
    {
        [CollectionJsonReference(typeof(GameSetting))]
        public int SettingId { get; set; }

        public string Name { get; set; }
    }


    public class GameSession
    {
        public GameSession()
        {
            //Characters = new List<CharacterReference>();
            SlogEntries = new List<SlogEntry>();
        }

        public int Id { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset PlayedAt { get; set; }

        public GameSettingReference Setting { get; set; }

        public CharacterReference GameMaster { get; set; }

        //public IEnumerable<CharacterReference> Characters { get; set; }
        
        public List<SlogEntry> SlogEntries { get; set; }


        public class SlogEntry
        {
            public DateTimeOffset CreatedAt { get; set; }
            
            public string AlienDateTime { get; set; }

            public UserReference Author { get; set; }

            public string Body { get; set; }
        }

        
    }



    
}