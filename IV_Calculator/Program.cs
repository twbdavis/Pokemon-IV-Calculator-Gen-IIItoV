using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IV_Calculator
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string dataDir = AppDataPaths.DirectoryPath;
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                DatabaseInitializer.SeedDatabase();
                Application.Run(new FRMCalculator());
            }
            catch (Exception ex)
            {
                MessageBox.Show("The calculator could not start. Verify that SQL Server Express LocalDB is installed and that your data folder is writable.\n\n" + ex.Message,
                    "Unable to start IV Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
