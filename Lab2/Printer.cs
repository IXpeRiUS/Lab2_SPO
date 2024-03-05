using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Printer
    {
        public static String printText(Token token)
        {
            string Lexeme =  getNameOfTableByState(token.State) + " (" + token.State + ")" + " INDEX " ; 
            return Lexeme;
        }



        public static String getNameOfTableByState(int state)
        {
            switch (state)
            {

                case Lexema.INT:
                    return "INT";
                case Lexema.ID:
                    return "ID";
                case Lexema.FOR_KEYWORD | Lexema.DO_KEYWORD:
                    return "KEYWORD";
                case -1:
                case -2:
                case -3:
                case -4:
                    return "FLOAT";
                case -5:
                    return "DOUBLE FLOAT";
                //case Lexema.SPACE:
                //    return "SPACE";
                case Lexema.CLOSING_CURLY_BRACE:
                case Lexema.OPENING_CURLY_BRACE:
                case Lexema.OPENING_ROUND_BRACE:
                case Lexema.CLOSING_ROUND_BRACE:
                case Lexema.SEMICOLON:
                    return "SIGN";
                case Lexema.ERROR:
                    return "ERROR";
                case Lexema.SIGN_SMALLER:
                    return "Знак сравн. меньше";
                case Lexema.SIGN_BIGGER:
                    return "Знак сравн. больше";
                case Lexema.SIGN_EQUAL:
                    return "Знак присвоения";

            }
            return "";
        }

    }
}
