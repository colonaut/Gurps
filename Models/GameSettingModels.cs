using System.Collections.Generic;

namespace MedienKultur.Gurps.Models
{
    public class GameSetting
    {
        public int Id { get; set; } //is primary key, but we also set it, when we send data to the server ans want to check the id...
        public string Name { get; set; }
        public string System { get; set; }
        public IEnumerable<GameMaster> GameMasters { get; set; }
        public GameCalendar Calendar { get; set; }
    }


    public class GameCalendar
    {
        public int Id { get; set; } //is primary key, but we also set it, when we send data to the server ans want to check the id...
        public string Name { get; set; }
    }
}