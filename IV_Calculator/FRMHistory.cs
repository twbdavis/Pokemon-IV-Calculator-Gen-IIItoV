using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IV_Calculator
{
    public partial class History : Form
    {

        private List<CalculationHistory> history;

    

        public bool HistoryWasCleared { get; private set; } = false;


        public History()
        {
            InitializeComponent();
            InitializeDataGridView();
     history = CalculationHistoryDB.GetHistory() ?? new List<CalculationHistory>();
            RefreshGrid();
        }



        private void InitializeDataGridView()
        {
            // Configure the DataGridView
            dataGridViewHistory.ReadOnly = true;
            dataGridViewHistory.AllowUserToAddRows = false;
            dataGridViewHistory.AllowUserToDeleteRows = false;
            dataGridViewHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Adds columns to the existing DataGridView
            dataGridViewHistory.Columns.Add("Time", "Time");
            dataGridViewHistory.Columns.Add("Pokemon", "Pokémon");
            dataGridViewHistory.Columns.Add("Level", "Level");
            dataGridViewHistory.Columns.Add("Nature", "Nature");
            dataGridViewHistory.Columns.Add("HP", "HP IV");
            dataGridViewHistory.Columns.Add("Attack", "Atk IV");
            dataGridViewHistory.Columns.Add("Defense", "Def IV");
            dataGridViewHistory.Columns.Add("SpAttack", "SpA IV");
            dataGridViewHistory.Columns.Add("SpDefense", "SpD IV");
            dataGridViewHistory.Columns.Add("Speed", "Spe IV");
            dataGridViewHistory.Columns.Add("HiddenPower", "Hidden Power");

        }

        private void RefreshGrid()
        {
            dataGridViewHistory.Rows.Clear();

            foreach (var entry in history.OrderByDescending(h => h.CalculationTime))
            {
                dataGridViewHistory.Rows.Add(
                    entry.CalculationTime.ToString("MM/dd/yyyy HH:mm:ss"),
                    entry.Pokemon,
                    entry.Level,
                    entry.Nature,
                    entry.HPIVResult,
                    entry.AttackIVResult,
                    entry.DefenseIVResult,
                    entry.SpAttackIVResult,
                    entry.SpDefenseIVResult,
                    entry.SpeedIVResult,
                    entry.HiddenPowerType
                );
            }
            
        }

        

        

        private void BTNClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Are you sure you want to clear all history?",
            "Confirm Clear",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dataGridViewHistory.Rows.Clear();
                CalculationHistoryDB.SaveHistory(new List<CalculationHistory>());
                HistoryWasCleared = true;  
                MessageBox.Show("History cleared successfully.", "Success");
            }

          
        }

        private void BTNBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowAllExamples()
        {
            var examples = ExamplePokemonDB.GetAllExamples();
            string message = "Current Examples:\n";
            foreach (var ex in examples)
            {
                message += $"{ex.Name} Lv.{ex.Level} ({ex.Nature}) - HP:{ex.StatHP}\n";
            }
            MessageBox.Show(message);
        }
        private void BTNClearRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewHistory.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to remove.", "No Selection");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete the selected entry?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            int gridIndex = dataGridViewHistory.CurrentRow.Index;

            // Your grid displays MOST RECENT FIRST, so build the same ordering here
            var ordered = history
                .OrderByDescending(h => h.CalculationTime)
                .ToList();

            if (gridIndex < 0 || gridIndex >= ordered.Count)
            {
                MessageBox.Show("Selection is out of range. Try again.", "Error");
                return;
            }

            var toRemove = ordered[gridIndex];

            history.Remove(toRemove);

            RefreshGrid();
            CalculationHistoryDB.SaveHistory(history);

            MessageBox.Show("Entry deleted successfully.", "Success");
        }

        private void BTNAddExample_Click(object sender, EventArgs e)
        {
         
            // Check if a row is selected
            if (dataGridViewHistory.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to save as an example.", "No Selection");
                return;
            }

            var selectedRow = dataGridViewHistory.SelectedRows[0];

            // Get the selected history entry (matching your grid's sort order)
            int gridIndex = dataGridViewHistory.CurrentRow.Index;

            var ordered = history
                .OrderByDescending(h => h.CalculationTime)
                .ToList();

            if (gridIndex < 0 || gridIndex >= ordered.Count)
            {
                MessageBox.Show("Selection is out of range.", "Error");
                return;
            }

            CalculationHistory selectedEntry = ordered[gridIndex];

            if (selectedEntry.StatHP == 0 && selectedEntry.StatAttack == 0)
            {
                MessageBox.Show(
                     "This history entry was created before stat tracking was added.\n" +
                    "Only new calculations can be saved as examples.",
                  "Cannot Save as Example",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Confirm with user
            DialogResult result = MessageBox.Show(
                $"Save {selectedEntry.Pokemon} (Lv. {selectedEntry.Level}) as an example Pokémon?",
                "Confirm Add Example",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            // Convert CalculationHistory to ExamplePokemon
            ExamplePokemon newExample = new ExamplePokemon
            {
                Name = selectedEntry.Pokemon,
                Level = selectedEntry.Level,
                Nature = selectedEntry.Nature,
                StatHP = selectedEntry.StatHP,
                StatAttack = selectedEntry.StatAttack,
                StatDefense = selectedEntry.StatDefense,
                StatSpAttack = selectedEntry.StatSpAttack,
                StatSpDefense = selectedEntry.StatSpDefense,
                StatSpeed = selectedEntry.StatSpeed,
                EVHP = selectedEntry.EVHP,
                EVAttack = selectedEntry.EVAttack,
                EVDefense = selectedEntry.EVDefense,
                EVSpAttack = selectedEntry.EVSpAttack,
                EVSpDefense = selectedEntry.EVSpDefense,
                EVSpeed = selectedEntry.EVSpeed
            };

            // Save to database
            if (ExamplePokemonDB.AddExample(newExample))
            {
                MessageBox.Show(
                    $"{selectedEntry.Pokemon} has been added to the example Pokémon list!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            ShowAllExamples();
            
    }

        private void BTNResetExamples_Click(object sender, EventArgs e)
        {
            ExamplePokemonDB.ResetToDefaults();

            //ShowMessage("Example database has been reset!", "Success");
            MessageBox.Show("Example database has been reset!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void ShowMessage(string message, string title)
        {
            Form msgForm = new Form();
            msgForm.Text = title;
            msgForm.ControlBox = false;  // Removes the X button
            msgForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            msgForm.StartPosition = FormStartPosition.CenterParent;
            msgForm.ClientSize = new Size(300, 120);
            msgForm.MaximizeBox = false;
            msgForm.MinimizeBox = false;

            Label lbl = new Label();
            lbl.Text = message;
            lbl.Font = new Font("Arial Rounded MT Bold", 12F);  // Bigger font
            lbl.AutoSize = false;
            lbl.Size = new Size(280, 60);
            lbl.Location = new Point(10, 20);
            lbl.TextAlign = ContentAlignment.MiddleCenter;  // Centers the text
            msgForm.Controls.Add(lbl);


            Button btn = new Button();
            btn.Text = "OK";
            btn.DialogResult = DialogResult.OK;
            btn.Location = new Point(110, 70);
            msgForm.Controls.Add(btn);

            msgForm.AcceptButton = btn;
            msgForm.ShowDialog();
        }
    }
}
