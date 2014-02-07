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

        public GameSetting Setting { get; set; }
        
        public GameMaster GameMaster { get; set; }
        
        public IEnumerable<CharacterReference> Characters { get; set; }

        [CollectionJsonReference(typeof(Slog))]
        public int SlogId { get; set; }


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


    


    
    public class GameSetting
    {
        public string Name { get; set; }
        public string System { get; set; }
        public IEnumerable<GameMaster> GameMasters { get; set; }
        public GameCalendar Calendar { get; set; }
    }


    /*
{"template":{"data":[
{"name":"name","value":"The GameMaster"},
{"name":"gameCalendar","object":{"data":[
{"name":"name","value":"The Calendar"}
]}}
]}}
     */


    public class GameCalendar
    {
        public int Id { get; set; } //is primary key, but we also set it, when we send data to the server ans want to check the id...
        public string Name { get; set; }
    }

    
}