using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckSortingAlgorithm
{
    class Card
    {
        private string name;
        private string type;
        private string race;
        private string level;
        private string atk;

        public Card()
        {

        }

        public Card(string name, string type, string race, string level, string atk)
        {
            this.name = name;
            this.type = type;
            this.race = race;
            this.level = level;
            this.atk = atk;
        }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Race { get => race; set => race = value; }
        public string Level { get => level; set => level = value; }
        public string Atk { get => atk; set => atk = value; }
    }
}
