using System;
using System.Collections.Generic;
using CollectionJsonExtended.Core.Attributes;
using MedienKultur.Gurps.Models.Extensions;
using Newtonsoft.Json;

namespace MedienKultur.Gurps.Models
{
    

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
            CreatedAt = DateTimeOffset.Now;
            Characters = new List<CharacterReference>();
            SlogEntries = new List<SlogEntry>();
        }

        public int Id { get; set; }
        
        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset PlayedAt { get; set; }

        public GameSettingReference Setting { get; set; }

        public CharacterReference GameMaster { get; set; }

        public IEnumerable<CharacterReference> Characters { get; set; }
        
        public IEnumerable<SlogEntry> SlogEntries { get; set; }


        public class CharacterReference
        {
            [CollectionJsonReference(typeof(Character))]
            public int CharacterId { get; set; }

            public string Name { get; set; }
        }

        public class SlogEntry
        {
            public SlogEntry()
            {
                CreatedAt = DateTimeOffset.Now;
            }
            
            [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
            public DateTimeOffset CreatedAt { get; set; }
            
            public string AlienDateTime { get; set; }

            public UserReference Author { get; set; }

            public string Body { get; set; }
        }

        
    }


    public class Slog
    {
        public string foo { get; set; }
    }
    
}