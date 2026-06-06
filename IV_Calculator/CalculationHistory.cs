using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public class CalculationHistory
    {
        public DateTime CalculationTime { get; set; }
        public string Pokemon { get; set; }
        public int Level { get; set; }
        public string Nature { get; set; }
        public string HPIVResult { get; set; }
        public string AttackIVResult { get; set; }
        public string DefenseIVResult { get; set; }
        public string SpAttackIVResult { get; set; }
        public string SpDefenseIVResult { get; set; }
        public string SpeedIVResult { get; set; }
        public string HiddenPowerType { get; set; }

      
        public int StatHP { get; set; }
        public int StatAttack { get; set; }
        public int StatDefense { get; set; }
        public int StatSpAttack { get; set; }
        public int StatSpDefense { get; set; }
        public int StatSpeed { get; set; }

   
        public int EVHP { get; set; }
        public int EVAttack { get; set; }
        public int EVDefense { get; set; }
        public int EVSpAttack { get; set; }
        public int EVSpDefense { get; set; }
        public int EVSpeed { get; set; }
    }
}
