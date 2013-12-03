using System;
using System.Collections.Generic;
using MedienKultur.Gurps.Models.Extensions;

namespace MedienKultur.Gurps.Models
{

    public class GameSession : IJsonSerializeable
    {
        public GameSession()
        {
            Characters = new List<Character>();
            BlogEntries = new List<GameSessionBlogEntry>();
            Date = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime Date { get; private set; }
        
        public GameSetting Setting { get; set; }
        
        public GameMaster GameMaster { get; set; }
        public IEnumerable<Character> Characters { get; set; }

        public IEnumerable<GameSessionBlogEntry> BlogEntries { get; set; }

    }



    public class GameSessionBlogEntry
    {
        public GameSessionBlogEntry()
        {
            
        }

        public string Data { get; set; }
        public string Author { get; set; }
        public AlienDate AlienDate { get; set; }

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