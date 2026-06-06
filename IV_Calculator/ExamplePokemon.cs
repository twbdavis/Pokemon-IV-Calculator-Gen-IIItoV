using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public class ExamplePokemon
    {
        public int ExampleID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Nature { get; set; }

        // Stats (the actual stat values shown in-game)
        public int StatHP { get; set; }
        public int StatAttack { get; set; }
        public int StatDefense { get; set; }
        public int StatSpAttack { get; set; }
        public int StatSpDefense { get; set; }
        public int StatSpeed { get; set; }

        // EVs
        public int EVHP { get; set; }
        public int EVAttack { get; set; }
        public int EVDefense { get; set; }
        public int EVSpAttack { get; set; }
        public int EVSpDefense { get; set; }
        public int EVSpeed { get; set; }

        public override string ToString()
        {
            return $"{Name} (Lv. {Level})";
        }
    }
}
