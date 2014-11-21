using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using CollectionJsonExtended.Core;
using CollectionJsonExtended.Core.Attributes;
using Raven.Client;

namespace MedienKultur.Gurps.Models
{

    public static class IDocumentSessionExtensions
    {
        public static T Load<T>(this IDocumentSession session, DenormalizedReference<T> reference)
            where T : IDenormalizedReference
        {
            return session.Load<T>(reference.Id);
        }
    }

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
            public int Id { get; set; }

            public string Name { get; set; }
        }

    public class DenormalizedGurpsCharacter : DenormalizedReference<GurpsCharacter>
    {
        //public int Id { get; set; }
        //public string Name { get; set; }

        public static implicit operator DenormalizedGurpsCharacter(GurpsCharacter character)
        {
            return new DenormalizedGurpsCharacter
            {
                Id = character.Id,
                Name = character.Name
                
            };
        }
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
        public DenormalizedReference<Character> Character { get; set; }

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
            Characters = new List<DenormalizedReference<GurpsCharacter>>();            
        }

        public int Id { get; set; }
        
        [CollectionJsonProperty(TemplateValueHandling = TemplateValueHandling.Ignore)]
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset PlayedAt { get; set; }

        public GameSettingReference Setting { get; set; }

        
        public CharacterReference GameMaster { get; set; }

        public DenormalizedReference<GurpsCharacter> DRGurpsCharacter { get; set; }

        public DenormalizedGurpsCharacter DCGurpsCharacter { get; set; }

        public IEnumerable<DenormalizedReference<GurpsCharacter>> Characters { get; set; }


        public class GameSettingReference
        {
            [CollectionJsonReference(typeof(GameSetting))]
            public int SettingId { get; set; }

            public string Name { get; set; }
        }
        
    }
    
}