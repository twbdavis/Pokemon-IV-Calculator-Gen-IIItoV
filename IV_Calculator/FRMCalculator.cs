using IV_Calculator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IV_Calculator
{
    public partial class FRMCalculator : Form
    {


        public FRMCalculator()
        {
            InitializeComponent();
            WireHiddenPowerEvents();
            LoadPokemon();
            LoadNatures();
            LoadExamplePokemon();
            Pokedex_Load();
        }

        
        private static List<CalculationHistory> calculationHistory = new List<CalculationHistory>();


        //private List<Pokemon> pokemonList;
        //private List<Nature> natureList;
        private List<ExamplePokemon> examplePokemonList;
        //private List<PokemonBaseStats> PokemonBaseStats;

        private readonly Random random = new Random();


        


        private readonly string[] hiddenPowerTypes = new string[]
        {
          "Fighting", "Flying", "Poison", "Ground",
          "Rock", "Bug", "Ghost", "Steel",
          "Fire", "Water", "Grass", "Electric",
          "Psychic", "Ice", "Dragon", "Dark"
        };


      

       

      

        private void Pokedex_Load()
        {
            

            // Load existing history from XML file
            calculationHistory = CalculationHistoryDB.GetHistory();

            TXTEVHP.Text = "0";
            TXTEVAttack.Text = "0";
            TXTEVDefense.Text = "0";
            TXTEVSpAttack.Text = "0";
            TXTEVSpDefense.Text = "0";
            TXTEVSpeed.Text = "0";

            TXTIVAttack.ReadOnly = true;
            TXTIVDefense.ReadOnly = true;
            TXTIVSpAttack.ReadOnly = true;
            TXTIVSpeed.ReadOnly = true;
            TXTIVHP.ReadOnly = true;
            TXTIVSpDefense.ReadOnly = true;


        }

       

        public string CalculateHPIVRange(decimal observedHP, decimal level, decimal ev, decimal baseHP)
        {
            // 1. Calculate all possible IVs that could produce this HP
            List<int> validIVs = new List<int>();

            for (int iv = 0; iv <= 31; iv++)
            {
                // Pokémon's exact HP calculation formula:
                decimal hp = Math.Floor(
                    Math.Floor((2 * baseHP + iv + Math.Floor(ev / 4m)) * level / 100m
                ) + level + 10);

                if (hp == observedHP)
                    validIVs.Add(iv);
            }

            // 2. Return formatted range
            if (validIVs.Count == 0)
                return "Impossible";
            else if (validIVs.Count == 1)
                return validIVs[0].ToString();
            else
                return $"{validIVs.Min()}-{validIVs.Max()}";
        }

        private void SaveToHistory(string pokemonName)
        {
            Nature selectedNature = CBXNature.SelectedItem as Nature;


            var historyEntry = new CalculationHistory
            {
                Pokemon = pokemonName,
                Level = int.Parse(TXTLevel.Text),
                Nature = selectedNature?.NatureName ?? "",
                HPIVResult = TXTIVHP.Text,
                AttackIVResult = TXTIVAttack.Text,
                DefenseIVResult = TXTIVDefense.Text,
                SpAttackIVResult = TXTIVSpAttack.Text,
                SpDefenseIVResult = TXTIVSpDefense.Text,
                SpeedIVResult = TXTIVSpeed.Text,
                HiddenPowerType = lblHiddenPowerType.Text,
                CalculationTime = DateTime.Now,


                StatHP = int.Parse(TXTStatHP.Text),
                StatAttack = int.Parse(TXTStatAttack.Text),
                StatDefense = int.Parse(TXTStatDefense.Text),
                StatSpAttack = int.Parse(TXTStatSpAttack.Text),
                StatSpDefense = int.Parse(TXTStatSpDefense.Text),
                StatSpeed = int.Parse(TXTStatSpeed.Text),

              
                EVHP = int.Parse(TXTEVHP.Text),
                EVAttack = int.Parse(TXTEVAttack.Text),
                EVDefense = int.Parse(TXTEVDefense.Text),
                EVSpAttack = int.Parse(TXTEVSpAttack.Text),
                EVSpDefense = int.Parse(TXTEVSpDefense.Text),
                EVSpeed = int.Parse(TXTEVSpeed.Text)
            };

            // Check for duplicate before adding
            bool isDuplicate = calculationHistory.Any(h =>
                h.Pokemon == historyEntry.Pokemon &&
                h.Level == historyEntry.Level &&
                h.Nature == historyEntry.Nature &&
                h.StatHP == historyEntry.StatHP &&
                h.StatAttack == historyEntry.StatAttack &&
                h.StatDefense == historyEntry.StatDefense &&
                h.StatSpAttack == historyEntry.StatSpAttack &&
                h.StatSpDefense == historyEntry.StatSpDefense &&
                h.StatSpeed == historyEntry.StatSpeed &&
                h.EVHP == historyEntry.EVHP &&
                h.EVAttack == historyEntry.EVAttack &&
                h.EVDefense == historyEntry.EVDefense &&
                h.EVSpAttack == historyEntry.EVSpAttack &&
                h.EVSpDefense == historyEntry.EVSpDefense &&
                h.EVSpeed == historyEntry.EVSpeed);

            if (isDuplicate)
            {
                // Optionally show a message, or just silently skip
                // MessageBox.Show("This calculation already exists in history.", "Duplicate");
                return;
            }

            calculationHistory.Add(historyEntry);
            CalculationHistoryDB.SaveHistory(calculationHistory);

        }

        private void CalculateIVs(PokemonBaseStats baseStats)
        {
            if (IsDataValid())
            {
                try
                {
                    decimal decLVL = Convert.ToDecimal(TXTLevel.Text);

                    string hpIVRange = CalculateHPIVRange(Convert.ToDecimal(TXTStatHP.Text), Convert.ToDecimal(TXTLevel.Text), Convert.ToDecimal(TXTEVHP.Text), baseStats.HP);

                    TXTIVHP.Text = hpIVRange.ToString();

                    string atkIV = CalculateExactIVRange(Convert.ToDecimal(TXTStatAttack.Text), Convert.ToDecimal(TXTLevel.Text)
                        , Convert.ToDecimal(TXTEVAttack.Text), baseStats.Attack, "Atk");

                    TXTIVAttack.Text = atkIV.ToString();

                    string spatkIV = CalculateExactIVRange(Convert.ToDecimal(TXTStatSpAttack.Text), Convert.ToDecimal(TXTLevel.Text)
                       , Convert.ToDecimal(TXTEVSpAttack.Text), baseStat: baseStats.SpAttack, statName: "SpAtk");

                    TXTIVSpAttack.Text = spatkIV.ToString();

                    string defIV = CalculateExactIVRange(Convert.ToDecimal(TXTStatDefense.Text), Convert.ToDecimal(TXTLevel.Text)
                       , Convert.ToDecimal(TXTEVDefense.Text), baseStats.Defense, "Def");

                    TXTIVDefense.Text = defIV.ToString();

                    string spdefIV = CalculateExactIVRange(Convert.ToDecimal(TXTStatSpDefense.Text), Convert.ToDecimal(TXTLevel.Text)
                      , Convert.ToDecimal(TXTEVSpDefense.Text), baseStats.SpDefense, "SpDef");

                    TXTIVSpDefense.Text = spdefIV.ToString();

                    string spdIV = CalculateExactIVRange(Convert.ToDecimal(TXTStatSpeed.Text), Convert.ToDecimal(TXTLevel.Text)
                       , Convert.ToDecimal(TXTEVSpeed.Text), baseStats.Speed, "Speed");

                    TXTIVSpeed.Text = spdIV.ToString();



                }
                catch
                {
                    MessageBox.Show("Please enter information in all fields");

                }



            }

        }


        

        public string CalculateExactIVRange(decimal observedStat, decimal level, decimal ev, decimal baseStat, string statName, bool isHP = false)
        {
            // 1. Get pure nature effects (no multipliers yet)
            var nature = GetNatureEffects();

            string boosted = NormalizeStatName(nature.boosted);
            string lowered = NormalizeStatName(nature.lowered);
            string current = NormalizeStatName(statName);

            bool isNeutral = boosted == lowered;
            bool isBoosted = boosted == current;
            bool isHindered = lowered == current;

            //MessageBox.Show("Boosted=" + boosted + "\nLowered=" + lowered + "\nCurrent=" + current);

            // 2. Calculate possible IVs
            List<int> validIVs = new List<int>();
            for (int iv = 0; iv <= 31; iv++)
            {
                // Pokémon's stat calculation steps:
                decimal stat = (2 * baseStat + iv + Math.Floor(ev / 4m)) * level;
                stat = Math.Floor(stat / 100m) + (isHP ? level + 10 : 5);

                // Apply nature ONLY if not neutral
                if (!isNeutral)
                {
                    if (isBoosted) stat = Math.Floor(stat * 1.1m);
                    if (isHindered) stat = Math.Floor(stat * 0.9m);
                }

                if (stat == observedStat)
                    validIVs.Add(iv);
            }
            
         
            if (validIVs.Count == 0) return "Impossible";
            if (validIVs.Count == 1) return validIVs[0].ToString();
            return $"{validIVs.Min()}-{validIVs.Max()}";

        }



        private (string boosted, string lowered) GetNatureEffects()
        {
            Nature selectedNature = CBXNature.SelectedItem as Nature;

            if (selectedNature == null)
                return ("", "");

            return (selectedNature.BoostedStat, selectedNature.LoweredStat);
        }


        private string NormalizeStatName(string s)
        {
            if (s == null) return "";

            s = s.Trim();

     
            s = s.Replace(".", "");
            s = s.Replace(" ", "");
            s = s.Replace("_", "");

            // Make it case-insensitive by converting to lower
            s = s.ToLower();

         
            if (s == "hp") return "HP";

            if (s == "attack" || s == "atk") return "Atk";
            if (s == "defense" || s == "def") return "Def";

         
            if (s == "spatk" || s == "spattack" || s == "specialattack") return "SpAtk";

      
            if (s == "spdef" || s == "spdefense" || s == "specialdefense") return "SpDef";


            if (s == "speed" || s == "spe") return "Speed";

            // If nothing matched, return original trimmed string
            return s;
        }



        private bool IsDataValid()
        {
            bool success = true; //the value is true

            string errorMessage = ""; //create blank message box


            // Use += for ALL validations to accumulate errors
            errorMessage += Validator.IsPresent(TXTLevel.Text, "Level");
            errorMessage += Validator.IsInteger(TXTLevel.Text, "Level");
            errorMessage += Validator.IsWithinRange(TXTLevel.Text, "Level", 1, 100);

            errorMessage += Validator.IsPresent(TXTStatHP.Text, "HP Stat");
            errorMessage += Validator.IsInteger(TXTStatHP.Text, "HP Stat");
            errorMessage += Validator.IsStatRange(TXTStatHP.Text, "HP Stat");

            errorMessage += Validator.IsPresent(TXTStatAttack.Text, "ATK Stat");
            errorMessage += Validator.IsInteger(TXTStatAttack.Text, "ATK Stat");
            errorMessage += Validator.IsStatRange(TXTStatAttack.Text, "ATK Stat");

            errorMessage += Validator.IsPresent(TXTStatDefense.Text, "DEF Stat");
            errorMessage += Validator.IsInteger(TXTStatDefense.Text, "DEF Stat");
            errorMessage += Validator.IsStatRange(TXTStatDefense.Text, "DEF Stat");

            errorMessage += Validator.IsPresent(TXTStatSpAttack.Text, "SP ATK Stat");
            errorMessage += Validator.IsInteger(TXTStatSpAttack.Text, "SP ATK Stat");
            errorMessage += Validator.IsStatRange(TXTStatSpAttack.Text, "SP ATK Stat");

            errorMessage += Validator.IsPresent(TXTStatSpDefense.Text, "SP DEF Stat");
            errorMessage += Validator.IsInteger(TXTStatSpDefense.Text, "SP DEF Stat");
            errorMessage += Validator.IsStatRange(TXTStatSpDefense.Text, "SP DEF Stat");

            errorMessage += Validator.IsPresent(TXTStatSpeed.Text, "SPD Stat");
            errorMessage += Validator.IsInteger(TXTStatSpeed.Text, "SPD Stat");
            errorMessage += Validator.IsStatRange(TXTStatSpeed.Text, "SPD Stat");

            // EV Validations
            errorMessage += Validator.IsInteger(TXTEVHP.Text, "HP EV");
            errorMessage += Validator.IsWithinRange(TXTEVHP.Text, "HP EV", 0, 255);

            errorMessage += Validator.IsInteger(TXTEVAttack.Text, "ATK EV");
            errorMessage += Validator.IsWithinRange(TXTEVAttack.Text, "ATK EV", 0, 255);

            errorMessage += Validator.IsInteger(TXTEVDefense.Text, "DEF EV");
            errorMessage += Validator.IsWithinRange(TXTEVDefense.Text, "DEF EV", 0, 255);

            errorMessage += Validator.IsInteger(TXTEVSpAttack.Text, "SP ATK EV");
            errorMessage += Validator.IsWithinRange(TXTEVSpAttack.Text, "SP ATK EV", 0, 255);

            errorMessage += Validator.IsInteger(TXTEVSpDefense.Text, "SP DEF EV");
            errorMessage += Validator.IsWithinRange(TXTEVSpDefense.Text, "SP DEF EV", 0, 255);

            errorMessage += Validator.IsInteger(TXTEVSpeed.Text, "SPD EV");
            errorMessage += Validator.IsWithinRange(TXTEVSpeed.Text, "SPD EV", 0, 255);




            if (errorMessage != "") //if the message is not blank (aka we generated an error)
            {
                success = false; //something is wrong
                MessageBox.Show(errorMessage, "Entry Error"); //show error message
            }


            return success; //otherwise all data is valid
        }



        

        private bool eventsWired = false;

   
        private void WireHiddenPowerEvents()
        {
            if (eventsWired) return;  // ADD THIS CHECK
            eventsWired = true;

            TXTIVHP.TextChanged += (_, __) => UpdateHiddenPowerUI();
            TXTIVAttack.TextChanged += (_, __) => UpdateHiddenPowerUI();
            TXTIVDefense.TextChanged += (_, __) => UpdateHiddenPowerUI();
            TXTIVSpeed.TextChanged += (_, __) => UpdateHiddenPowerUI();
            TXTIVSpAttack.TextChanged += (_, __) => UpdateHiddenPowerUI();
            TXTIVSpDefense.TextChanged += (_, __) => UpdateHiddenPowerUI();

            UpdateHiddenPowerUI();
        }

        /// <summary>
        /// Updates Hidden Power UI output. Keeps HPGroupBox in play.
        /// </summary>
        private void UpdateHiddenPowerUI()
        {
            var possibleTypes = CalculateHiddenPowerTypesFromTextBoxes();

            if (possibleTypes == null || possibleTypes.Count == 0)
            {
                lblHiddenPowerType.Text = "";
                return;
            }

            if (possibleTypes.Count == 1)
            {
                lblHiddenPowerType.Text = possibleTypes[0];
                return;
            }

            int maxEntries = Math.Min(possibleTypes.Count, 14);

            // 2 types per line, then break
            var lines = new List<string>();
            for (int i = 0; i < maxEntries; i += 2)
            {
                if (i + 1 < maxEntries)
                {
                    // Pair two types together
                    lines.Add(possibleTypes[i] + " / " + possibleTypes[i + 1]);
                }
                else
                {
                    // Odd number - last one stands alone
                    lines.Add(possibleTypes[i]);
                }
            }

            lblHiddenPowerType.Text = string.Join(Environment.NewLine, lines);
        }
        /// <summary>
        /// Returns a sorted list of possible Hidden Power types given the IV textboxes.
        /// Gen 3–5 type uses LSB parity in order: HP, Atk, Def, Spe, SpA, SpD.
        /// If any textbox is invalid, returns null.
        /// </summary>
        private List<string> CalculateHiddenPowerTypesFromTextBoxes()
        {
            if (!TryParseIvRange(TXTIVHP.Text, out int hpMin, out int hpMax)) return null;
            if (!TryParseIvRange(TXTIVAttack.Text, out int atkMin, out int atkMax)) return null;
            if (!TryParseIvRange(TXTIVDefense.Text, out int defMin, out int defMax)) return null;
            if (!TryParseIvRange(TXTIVSpeed.Text, out int speMin, out int speMax)) return null;
            if (!TryParseIvRange(TXTIVSpAttack.Text, out int spaMin, out int spaMax)) return null;
            if (!TryParseIvRange(TXTIVSpDefense.Text, out int spdMin, out int spdMax)) return null;

            // If any fields are blank, you can choose how to handle it.
            // Here, blank => invalid (null). If you'd rather treat blank as "0-31", change TryParseIvRange behavior.
            // (Current TryParseIvRange returns false for blank.)

            // Hidden Power depends ONLY on parity (even/odd), so we reduce each IV range to possible parities.
            var hpP = PossibleParities(hpMin, hpMax);   // [evenPossible, oddPossible]
            var atkP = PossibleParities(atkMin, atkMax);
            var defP = PossibleParities(defMin, defMax);
            var speP = PossibleParities(speMin, speMax);
            var spaP = PossibleParities(spaMin, spaMax);
            var spdP = PossibleParities(spdMin, spdMax);

            var possible = new HashSet<string>();

            // Enumerate parity combinations (max 64). Much faster than enumerating IVs (up to 32^6).
            for (int a = 0; a <= 1; a++) // HP
                for (int b = 0; b <= 1; b++) // Atk
                    for (int c = 0; c <= 1; c++) // Def
                        for (int d = 0; d <= 1; d++) // Spe
                            for (int e = 0; e <= 1; e++) // SpA
                                for (int f = 0; f <= 1; f++) // SpD
                                {
                                    if ((a == 0 && !hpP[0]) || (a == 1 && !hpP[1])) continue;
                                    if ((b == 0 && !atkP[0]) || (b == 1 && !atkP[1])) continue;
                                    if ((c == 0 && !defP[0]) || (c == 1 && !defP[1])) continue;
                                    if ((d == 0 && !speP[0]) || (d == 1 && !speP[1])) continue;
                                    if ((e == 0 && !spaP[0]) || (e == 1 && !spaP[1])) continue;
                                    if ((f == 0 && !spdP[0]) || (f == 1 && !spdP[1])) continue;

                                    int typeIndex = HiddenPowerTypeIndex_Gen3to5(a, b, c, d, e, f);
                                    if (typeIndex < 0 || typeIndex >= hiddenPowerTypes.Length) continue;

                                    possible.Add(hiddenPowerTypes[typeIndex]);
                                }

            // Stable, deterministic ordering
            return possible.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Gen 3–5 Hidden Power type index:
        /// typeIndex = floor( ( (a + 2b + 4c + 8d + 16e + 32f) * 15 ) / 63 )
        /// where bits are the LSB parity in order: HP, Atk, Def, Spe, SpA, SpD.
        /// </summary>
        private int HiddenPowerTypeIndex_Gen3to5(int hpParity, int atkParity, int defParity, int speParity, int spaParity, int spdParity)
        {
            int value = hpParity
                        + 2 * atkParity
                        + 4 * defParity
                        + 8 * speParity
                        + 16 * spaParity
                        + 32 * spdParity;

            return (value * 15) / 63;
        }

        /// <summary>
        /// Parses an IV textbox value as either:
        ///  - "31" (single IV)
        ///  - "12-31" (range)
        /// Whitespace ok. Clamps to [0,31]. Returns false for blank/invalid.
        /// </summary>
        private bool TryParseIvRange(string text, out int min, out int max)
        {
            min = max = 0;

            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();

            // Single value
            if (int.TryParse(text, out int v))
            {
                v = ClampIv(v);
                min = max = v;
                return true;
            }

            // Range "x-y"
            var parts = text.Split('-');
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), out int a) &&
                int.TryParse(parts[1].Trim(), out int b))
            {
                a = ClampIv(a);
                b = ClampIv(b);
                min = Math.Min(a, b);
                max = Math.Max(a, b);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns whether a range can be even and/or odd.
        /// Output: bool[2] => [evenPossible, oddPossible]
        /// </summary>
        private bool[] PossibleParities(int min, int max)
        {
            bool even = false, odd = false;

            // Quick parity detection without looping:
            // - If min == max -> fixed parity
            // - If range length >= 1 -> it includes at least two consecutive ints => both parities possible
            if (min == max)
            {
                if ((min & 1) == 0) even = true;
                else odd = true;
                return new[] { even, odd };
            }

            // If min != max, since the range is inclusive and integers are consecutive,
            // it must contain both parities.
            return new[] { true, true };
        }

        private int ClampIv(int v) => Math.Max(0, Math.Min(31, v));



        private void LoadExamplePokemon()
        {
            try
            {
                examplePokemonList = ExamplePokemonDB.GetAllExamples();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading example Pokemon: " + ex.Message);
                examplePokemonList = new List<ExamplePokemon>();
            }
        }




        private void BTNLoad_Click_1(object sender, EventArgs e)
        {
            // Check if examples were loaded
            if (examplePokemonList == null || examplePokemonList.Count == 0)
            {
                MessageBox.Show("No example Pokemon available.", "Error");
                return;
            }

            // Get a random example from the database list
            int randomIndex = random.Next(examplePokemonList.Count);
            ExamplePokemon example = examplePokemonList[randomIndex];

            // Set the Pokémon selection - search by Name property
            int pokemonIndex = -1;
            for (int i = 0; i < CBXPokemon.Items.Count; i++)
            {
                Pokemon p = CBXPokemon.Items[i] as Pokemon;
                if (p != null && p.Name == example.Name)
                {
                    pokemonIndex = i;
                    break;
                }
            }
            if (pokemonIndex >= 0)
            {
                CBXPokemon.SelectedIndex = pokemonIndex;
            }

            // Set the Nature selection - search by NatureName property
            int natureIndex = -1;
            for (int i = 0; i < CBXNature.Items.Count; i++)
            {
                Nature n = CBXNature.Items[i] as Nature;
                if (n != null && n.NatureName == example.Nature)
                {
                    natureIndex = i;
                    break;
                }
            }
            if (natureIndex >= 0)
            {
                CBXNature.SelectedIndex = natureIndex;
            }

            // Set Level
            TXTLevel.Text = example.Level.ToString();

            // Set Stats
            TXTStatHP.Text = example.StatHP.ToString();
            TXTStatAttack.Text = example.StatAttack.ToString();
            TXTStatDefense.Text = example.StatDefense.ToString();
            TXTStatSpAttack.Text = example.StatSpAttack.ToString();
            TXTStatSpDefense.Text = example.StatSpDefense.ToString();
            TXTStatSpeed.Text = example.StatSpeed.ToString();

            // Set EVs
            TXTEVHP.Text = example.EVHP.ToString();
            TXTEVAttack.Text = example.EVAttack.ToString();
            TXTEVDefense.Text = example.EVDefense.ToString();
            TXTEVSpAttack.Text = example.EVSpAttack.ToString();
            TXTEVSpDefense.Text = example.EVSpDefense.ToString();
            TXTEVSpeed.Text = example.EVSpeed.ToString();

            // Clear previous IV results
            TXTIVHP.Text = "";
            TXTIVAttack.Text = "";
            TXTIVDefense.Text = "";
            TXTIVSpAttack.Text = "";
            TXTIVSpDefense.Text = "";
            TXTIVSpeed.Text = "";

        }

        private void BTNClear_Click_1(object sender, EventArgs e)
        {
            TXTLevel.Text = null;
            TXTStatHP.Text = null;
            TXTStatAttack.Text = null;
            TXTStatDefense.Text = null;
            TXTStatSpAttack.Text = null;
            TXTStatSpDefense.Text = null;
            TXTStatSpeed.Text = null;

            TXTEVAttack.Text = Convert.ToString(0);
            TXTEVDefense.Text = Convert.ToString(0);
            TXTEVSpAttack.Text = Convert.ToString(0);
            TXTEVHP.Text = Convert.ToString(0);
            TXTEVSpDefense.Text = Convert.ToString(0);
            TXTEVSpeed.Text = Convert.ToString(0);

            TXTIVAttack.Text = null;
            TXTIVDefense.Text = null;
            TXTIVSpeed.Text = null;
            TXTIVHP.Text = null;
            TXTIVSpDefense.Text = null;
            TXTIVSpAttack.Text = null;

            CBXPokemon.SelectedIndex = 0;

            

        }

        private void BTNCalculate_Click(object sender, EventArgs e)
        {
            Pokemon selectedPokemon = CBXPokemon.SelectedItem as Pokemon;

            if (selectedPokemon != null)
            {
                // Create PokemonBaseStats from the database Pokemon object
                PokemonBaseStats baseStats = new PokemonBaseStats
                {
                    HP = selectedPokemon.BaseHP,
                    Attack = selectedPokemon.BaseAttack,
                    Defense = selectedPokemon.BaseDefense,
                    SpAttack = selectedPokemon.BaseSpAttack,
                    SpDefense = selectedPokemon.BaseSpDefense,
                    Speed = selectedPokemon.BaseSpeed
                };

                CalculateIVs(baseStats);

                if (!string.IsNullOrWhiteSpace(TXTIVHP.Text) &&
                    !string.IsNullOrWhiteSpace(TXTIVAttack.Text))
                {
                    SaveToHistory(selectedPokemon.Name);
                }
            }
            if (selectedPokemon == null)
            {
                MessageBox.Show("No Pokémon selected");
            }
        }

        private void BTNClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BTNHistory_Click(object sender, EventArgs e)
        {
            History historyForm = new History();
            historyForm.ShowDialog();
            if (historyForm.HistoryWasCleared)
            {
                calculationHistory.Clear();  // Clear the main form's list too!
            }

            LoadExamplePokemon();
        }
    
        private void LoadPokemon()
        {
            CBXPokemon.Items.Clear();

            using (SqlConnection connection = PokemonDB.GetConnection())
            {
                connection.Open();

                string sql = @"
            SELECT PokemonID,
                   Name,
                   BaseHP,
                   BaseAttack,
                   BaseDefense,
                   BaseSpAttack,
                   BaseSpDefense,
                   BaseSpeed
            FROM Pokemon
            ORDER BY Name";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CBXPokemon.Items.Add(new Pokemon
                        {
                            PokemonID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            BaseHP = reader.GetInt32(2),
                            BaseAttack = reader.GetInt32(3),
                            BaseDefense = reader.GetInt32(4),
                            BaseSpAttack = reader.GetInt32(5),
                            BaseSpDefense = reader.GetInt32(6),
                            BaseSpeed = reader.GetInt32(7),
                       
                        });
                    }
                }
            }

            CBXPokemon.DisplayMember = "Name";
            if (CBXPokemon.Items.Count > 0)
                CBXPokemon.SelectedIndex = 0;
        }

        private void LoadNatures()
        {
            CBXNature.Items.Clear();

            using (SqlConnection connection = PokemonDB.GetConnection())
            {
                connection.Open();

                // Load ALL nature data, not just the name
                string sql = @"
            SELECT NatureID, NatureName, BoostedStat, LoweredStat 
            FROM Natures 
            ORDER BY NatureName";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add full Nature OBJECT, not just the string
                        CBXNature.Items.Add(new Nature
                        {
                            NatureID = reader.GetInt32(0),
                            NatureName = reader.GetString(1),
                            BoostedStat = reader.GetString(2),
                            LoweredStat = reader.GetString(3)
                        });
                    }
                }
            }

            CBXNature.DisplayMember = "NatureName";  // Display the name in dropdown

            if (CBXNature.Items.Count > 0)
                CBXNature.SelectedIndex = 0;
        }

        private void TXTEVHP_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVHP.Text);
                if (value == 0)
                {
                    TXTEVHP.Text = "";
                }
            }
            catch
            {
                // If it can't be parsed, leave it alone
            }
            
        }

        private void TXTEVHP_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVHP.Text))
            {
                TXTEVHP.Text = "0";
            }


        }

        private void TXTEVAttack_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVAttack.Text);
                if (value == 0)
                {
                    TXTEVAttack.Text = "";
                }
            }
            catch
            {
              
            }
        }

        private void TXTEVAttack_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVAttack.Text))
            {
                TXTEVAttack.Text = "0";
            }
        }

        private void TXTEVDefense_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVDefense.Text);
                if (value == 0)
                {
                    TXTEVDefense.Text = "";
                }
            }
            catch
            {

            }
        }

        private void TXTEVDefense_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVDefense.Text))
            {
                TXTEVDefense.Text = "0";
            }
        }

        private void TXTEVSpAttack_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVSpAttack.Text);
                if (value == 0)
                {
                    TXTEVSpAttack.Text = "";
                }
            }
            catch
            {

            }
        }

        private void TXTEVSpAttack_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVSpAttack.Text))
            {
                TXTEVSpAttack.Text = "0";
            }
        }

        private void TXTEVSpDefense_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVSpDefense.Text);
                if (value == 0)
                {
                    TXTEVSpDefense.Text = "";
                }
            }
            catch
            {

            }
        }

        private void TXTEVSpDefense_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVSpDefense.Text))
            {
                TXTEVSpDefense.Text = "0";
            }
        }

        private void TXTEVSpeed_Enter_1(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(TXTEVSpeed.Text);
                if (value == 0)
                {
                    TXTEVSpeed.Text = "";
                }
            }
            catch
            {

            }
        }

        private void TXTEVSpeed_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXTEVSpeed.Text))
            {
                TXTEVSpeed.Text = "0";
            }
        }


        private async void LoadPokemonSprite(string pokemonName)
        {
            try
            {
                // PokeAPI accepts lowercase Pokemon names!
                string name = pokemonName.ToLower().Replace(" ", "-").Replace(".", "").Replace("'", "");

                // Special cases for problematic names
                if (name == "nidoran♀" || name == "nidoran-f") name = "nidoranF";
                if (name == "nidoran♂" || name == "nidoran-m") name = "nidoranM";
                if (name == "mr-mime") name = "mr-mime";
                if (name == "farfetchd") name = "farfetchd";

                string url = $"https://pokeapi.co/api/v2/pokemon/{name}";

                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(url);

                    // Parse sprite URL from JSON
                    int spriteStart = json.IndexOf("\"front_default\":\"") + 17;
                    int spriteEnd = json.IndexOf("\"", spriteStart);
                    string spriteUrl = json.Substring(spriteStart, spriteEnd - spriteStart);

                    // Download and display sprite
                    byte[] imageBytes = await client.GetByteArrayAsync(spriteUrl);
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        PBXSprite.Image = Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                PBXSprite.Image = null; // Clear if not found
            }
        }

        private void CBXPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBXPokemon.SelectedItem != null)
            {
                string pokemonName = CBXPokemon.SelectedItem.ToString();
                LoadPokemonSprite(pokemonName);
            }           }

       
    }
}
