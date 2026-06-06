using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace IV_Calculator
{
    public static class CalculationHistoryDB
    {
        // Path to the XML file (stored in project directory)
        private const string Path = @"..\..\CalculationHistory.xml";

        /// <summary>
        /// Loads all calculation history entries from the XML file
        /// </summary>
        public static List<CalculationHistory> GetHistory()
        {
            List<CalculationHistory> history = new List<CalculationHistory>();

            if (!System.IO.File.Exists(Path))
            {
                return history;
            }

            try
            {
                XDocument doc = XDocument.Load(Path);

                foreach (XElement entry in doc.Descendants("Entry"))
                {
                    CalculationHistory item = new CalculationHistory();

                    // Original fields (always present)
                    item.Pokemon = (string)entry.Element("Pokemon") ?? "";
                    item.Level = (int?)entry.Element("Level") ?? 0;
                    item.Nature = (string)entry.Element("Nature") ?? "";
                    item.HPIVResult = (string)entry.Element("HPIVResult") ?? "";
                    item.AttackIVResult = (string)entry.Element("AttackIVResult") ?? "";
                    item.DefenseIVResult = (string)entry.Element("DefenseIVResult") ?? "";
                    item.SpAttackIVResult = (string)entry.Element("SpAttackIVResult") ?? "";
                    item.SpDefenseIVResult = (string)entry.Element("SpDefenseIVResult") ?? "";
                    item.SpeedIVResult = (string)entry.Element("SpeedIVResult") ?? "";
                    item.HiddenPowerType = (string)entry.Element("HiddenPowerType") ?? "";

                    string timeStr = (string)entry.Element("CalculationTime");
                    if (!string.IsNullOrEmpty(timeStr))
                        item.CalculationTime = DateTime.Parse(timeStr);

                    // NEW fields (may not exist in old entries - defaults to 0)
                    item.StatHP = (int?)entry.Element("StatHP") ?? 0;
                    item.StatAttack = (int?)entry.Element("StatAttack") ?? 0;
                    item.StatDefense = (int?)entry.Element("StatDefense") ?? 0;
                    item.StatSpAttack = (int?)entry.Element("StatSpAttack") ?? 0;
                    item.StatSpDefense = (int?)entry.Element("StatSpDefense") ?? 0;
                    item.StatSpeed = (int?)entry.Element("StatSpeed") ?? 0;
                    item.EVHP = (int?)entry.Element("EVHP") ?? 0;
                    item.EVAttack = (int?)entry.Element("EVAttack") ?? 0;
                    item.EVDefense = (int?)entry.Element("EVDefense") ?? 0;
                    item.EVSpAttack = (int?)entry.Element("EVSpAttack") ?? 0;
                    item.EVSpDefense = (int?)entry.Element("EVSpDefense") ?? 0;
                    item.EVSpeed = (int?)entry.Element("EVSpeed") ?? 0;

                    history.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Log or handle error - return empty list rather than crash
                System.Diagnostics.Debug.WriteLine("Error loading history: " + ex.Message);
            }

            return history;
        }

        /// <summary>
        /// Saves all calculation history entries to the XML file
        /// </summary>
        public static void SaveHistory(List<CalculationHistory> history)
        {
            // Create the XmlWriterSettings object
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            // Create the XmlWriter object
            XmlWriter xmlOut = XmlWriter.Create(Path, settings);

            // Write the start of the document
            xmlOut.WriteStartDocument();
            xmlOut.WriteStartElement("CalculationHistory");

            // Write each history entry to the XML file
            foreach (CalculationHistory entry in history)
            {
                xmlOut.WriteStartElement("Entry");
                xmlOut.WriteElementString("Pokemon", entry.Pokemon);
                xmlOut.WriteElementString("Level", entry.Level.ToString());
                xmlOut.WriteElementString("Nature", entry.Nature);
                xmlOut.WriteElementString("HPIVResult", entry.HPIVResult);
                xmlOut.WriteElementString("AttackIVResult", entry.AttackIVResult);
                xmlOut.WriteElementString("DefenseIVResult", entry.DefenseIVResult);
                xmlOut.WriteElementString("SpAttackIVResult", entry.SpAttackIVResult);
                xmlOut.WriteElementString("SpDefenseIVResult", entry.SpDefenseIVResult);
                xmlOut.WriteElementString("SpeedIVResult", entry.SpeedIVResult);
                xmlOut.WriteElementString("HiddenPowerType", entry.HiddenPowerType ?? "");
                xmlOut.WriteElementString("CalculationTime", entry.CalculationTime.ToString("o"));
             

                // In SaveHistory method - add these inside the foreach loop after existing WriteElementString calls:
                xmlOut.WriteElementString("StatHP", entry.StatHP.ToString());
                xmlOut.WriteElementString("StatAttack", entry.StatAttack.ToString());
                xmlOut.WriteElementString("StatDefense", entry.StatDefense.ToString());
                xmlOut.WriteElementString("StatSpAttack", entry.StatSpAttack.ToString());
                xmlOut.WriteElementString("StatSpDefense", entry.StatSpDefense.ToString());
                xmlOut.WriteElementString("StatSpeed", entry.StatSpeed.ToString());

                xmlOut.WriteElementString("EVHP", entry.EVHP.ToString());
                xmlOut.WriteElementString("EVAttack", entry.EVAttack.ToString());
                xmlOut.WriteElementString("EVDefense", entry.EVDefense.ToString());
                xmlOut.WriteElementString("EVSpAttack", entry.EVSpAttack.ToString());
                xmlOut.WriteElementString("EVSpDefense", entry.EVSpDefense.ToString());
                xmlOut.WriteElementString("EVSpeed", entry.EVSpeed.ToString());



            }

            // Write the end tag for the root element
            xmlOut.WriteEndElement();

            // Close the XmlWriter object
            xmlOut.Close();
        }
    }
}
