using System;
using System.Collections.Generic;
using CollectionJsonExtended.Core.Attributes;
using MedienKultur.Gurps.Models.Extensions;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Newtonsoft.Json;

namespace MedienKultur.Gurps.Models
{
    

    public class UserReference
    {
        [CollectionJsonReference(typeof(ApplicationUser))]
        public int UserId { get; set; }

        public string Name { get; set; }
    }

    public class Slog
    {
        public int Id { get; set; }

        public CharacterReference Character { get; set; }

        public GameSessionReference GameSession { get; set; }

        
        public class CharacterReference
        {
            public int CharacterId { get; set; }

            public string Name { get; set; }
        }

        public class GameSessionReference
        {
            public int GameSessionId { get; set; }

            public AlienDate BeginAlienDate { get; set; }

            public AlienDate EndAlienDate { get; set; }

            public DateTimeOffset PlayedAt { get; set; }
        }
    }

    


    public class GameSession
    {
        public GameSession()
        {
            CreatedAt = DateTimeOffset.Now;
            Characters = new List<CharacterReference>();
        }

        public int Id { get; set; }
        
        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset PlayedAt { get; set; }

        public GameSettingReference Setting { get; set; }

        public CharacterReference GameMaster { get; set; }

        public IEnumerable<CharacterReference> Characters { get; set; }

        public SlogReference Slog { get; set; }


        public class GameSettingReference
        {
            [CollectionJsonReference(typeof(GameSetting))]
            public int SettingId { get; set; }

            public string Name { get; set; }
        }

        public class CharacterReference
        {
            [CollectionJsonReference(typeof(Character))]
            public int CharacterId { get; set; }

            public string Name { get; set; }

            public int EarnedCharacterPoints { get; set; }
        }

        public class SlogReference
        {
            [CollectionJsonReference(typeof(Slog))]
            public int SlogId { get; set; }

             
        }
    }


    public class Slog
    {
        public string foo { get; set; }
    }
    
}