using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public  class Validator
    {
        public static string IsInteger(string strTestValue, string strTextBox) //checking for numerical integer value
        {
            string strMessage = "";

            int intTestValue = 0;
            if (!Int32.TryParse(strTestValue, out intTestValue)) //parses input to integer
            {

                strMessage += strTextBox + " must contain a valid whole number. \n";
            }
            return strMessage;
        }

        public static string IsWithinRange(string value, string name, decimal DECmin, decimal DECmax) //checking for numerical range
        {
            // Creates a blank string variable - this is step one for all the validation methods
            string msg = "";

            // Loop happens if the value can be parsed
            if (Decimal.TryParse(value, out decimal number))
            {
                // If the value is too large or small then it catches it and makes an error message
                if (number < DECmin || number > DECmax)
                {
                    msg += name + " Must be between " +
                    DECmin + " and " + DECmax + ".\n";
                }
            }
            //the final boolean return of the method when it is called
            return msg;
        }


        public static string IsPresent(string strTestValue, string strTextBoxName)
        {
            string strMessage = "";

            //This if loop will run if there is no input beyond blank space in the textbox and it will throw an error
            if (strTestValue.Trim() == "")
            {
                strMessage = strTextBoxName + " is a required field.\n";
            }

            //the final boolean return of the method when it is called
            return strMessage;
        }

        public static string IsStatRange(string value, string name)
        {
            string msg = "";

            if (Decimal.TryParse(value, out decimal number))
            {
                // If the value is too large or small then it catches it and makes an error message
                if (number < 0 || number > 999)
                {
                    msg += name + " Must be between 0-999";
                }
            }

            return msg;

        }

    }
}
