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
        private UserReference() {}
        
        public UserReference(ApplicationUser applicationUser)
        {
            Name = applicationUser.UserName;
            GravatarUrl = applicationUser.GravatarUrl;
        }
        
        [CollectionJsonReference(typeof(ApplicationUser))]
        public string Name { get; set; }

        public string GravatarUrl { get; set; }
    }

    public class CharacterReference
        {
            [CollectionJsonReference(typeof(Character))]
            public int CharacterId { get; set; }

            public string Name { get; set; }

            public int EarnedCharacterPoints { get; set; }
        }


    public class SlogEntry
    {
        public SlogEntry()
        {
            CreatedAt = DateTimeOffset.Now;            
        }

        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public DateTimeOffset CreatedAt { get; set; }

        public int Id { get; set; }

        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public UserReference User { get; set; }

        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public CharacterReference Character { get; set; }

        public string Content { get; set; }


        public class GameSessionReference
        {
            [CollectionJsonReference(typeof(GameSession))]
            public int GameSessionId { get; set; }

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


        public class GameSettingReference
        {
            [CollectionJsonReference(typeof(GameSetting))]
            public int SettingId { get; set; }

            public string Name { get; set; }
        }
        
    }
    
}