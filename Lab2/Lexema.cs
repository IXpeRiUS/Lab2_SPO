using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab2
{
    public class Lexema
    {
        //данные на анализ(входная строка с текстБокса)
        private string _data;
        //длина строки
        private int _lenght;
        //количество конечных автоматов
        private static int LENGHT = 7;
        //автоматы:
        //число
        public const int INT = -19;
        //идентификатор
        public const int ID = -6;
        //пробелы, переносы, табуляции
        public const int SPACE = -13;
        //;
        public const int SEMICOLON = -18;
        public const int FLOAT_KEYWORD = -101;
        public const int DOUBLE_KEYWORD = -102;
        public const int RETURN_KEYWORD = -103;

        //скобки
        public const int OPENING_CURLY_BRACE = -14;
        public const int CLOSING_CURLY_BRACE = -15;
        public const int OPENING_ROUND_BRACE = -16;
        public const int CLOSING_ROUND_BRACE = -17;
        public const int SIGN_EQUAL = -20;
        public const int SIGN_SMALLER = -21;
        public const int SIGN_BIGGER = -22;
        public const int SIGN_PLUS = -23;

        public const int INCREMENT_KEYWORD = -110;

        public const int FOR_KEYWORD = -100;
        public const int DO_KEYWORD = -90;
        public const int INT_KEYWORD = -80;
        public const int ERROR = 0;

        private const int MAX_ID_LENGTH = 7;
        private int i = 0;
        private int lastFinalState = 0;
        private int lastPositionWithFinalState = 0;






        private int startingPosition = 0;
        //состояния автоматов
        private int[] states = new int[LENGHT];
        
        public string Data { get => _data; set => _data = value; }
        public int Lenght { get => _lenght; set => _lenght = value; }

        public Lexema(string data)
        {
            Data = data;
            Lenght = Data.Length;
            initStates(states);
        }

        //проверка Data посимвольно
        public Token ParseToken()
        {
            while (i < Lenght)
            {
                identify(Data[i], states);
                if (AreAllBroken(states))
                {
                    if (lastFinalState == 0)
                    {
                        Token token = new Token(startingPosition, startingPosition + 1,  0);
                        initStates(states);
                        i = startingPosition;
                        startingPosition++;
                        i++;
                        return token;
                    }
                    else
                    {
                        string str = Data.Substring(startingPosition, lastPositionWithFinalState + 1);
                        if (lastFinalState == ID)
                        {
                            str = cutIDString(str);
                        }

                        Token token = new Token(startingPosition, lastPositionWithFinalState + 1,  lastFinalState);
                        lastFinalState = 0;
                        i = lastPositionWithFinalState;
                        startingPosition = lastPositionWithFinalState + 1;
                        initStates(states);
                        i++;
                        return token;
                    }
                }
                else
                {
                    if(getFinalState(states) !=0)
                    {
                        lastFinalState = getFinalState(states);
                        lastPositionWithFinalState = i;
                    }
                    i++;
                }
            }
            if (lastFinalState != 0 && i >= Lenght)
            {
                string str = Data.Substring(startingPosition, lastPositionWithFinalState + 1);
                str = cutIDString(str);
                if (lastFinalState == ID)
                {
                    str = cutIDString(str);
                }
                Token token = new Token(startingPosition, lastPositionWithFinalState + 1, lastFinalState); 
                lastFinalState = 0;
                return token;

            }
            return null;
        }

        private int getFinalState(int[] states)
        {

            //ORDER MATTERS
            //=============

            //keyword
            if (states[2] < 0)
            {
                return states[2];
            }

            //int
            if (states[0] < 0)
            {
                return states[0];
            }

            //identifier
            if (states[1] < 0)
            {
                return states[1];
            }

            //float
            if (states[3] < 0)
            {
                return states[3];
            }

            //double float
            if (states[4] < 0)
            {
                return states[4];
            }

            //space
            if (states[5] < 0)
            {
                return states[5];
            }

            //sign
            if (states[6] < 0)
            {
                return states[6];
            }

            return 0;
        }

        private void identify(char c, int[] states)
        {
            states[1] = IdentifyIdentifier(c, states[1]);
            states[0] = IdentifyInt(c, states[0]);
            states[2] = IdentifyKeyWord(c, states[2]);
            states[3] = IdentifySpace(c, states[3]);
            states[4] = IdentifySign(c, states[4]);
            states[5] = identifyFloat(c, states[5]);
            states[6] = identifyDoubleFloat(c, states[6]);

        }


        private String cutIDString(String id)
        {
            if (id.Length > MAX_ID_LENGTH)
            {
                return id.Substring(0, MAX_ID_LENGTH);
            }
            return id;
        }
        private bool AreAllBroken(int[] states)
        {
            for (int i = 0; i < states.Length; i++)
                if (states[i] != 0)
                {
                    return false;
                }
            return true;
        }


        //инициализация начального состояния автоматов
        private void initStates(int[] s)
        {
            for (int i = 0; i < LENGHT; i++)
            {
                s[i] = 1;
            }
        }
        //идентификация числа
        private int IdentifyInt(char ch, int state)
        {
            switch (state)
            {
                case 1:
                    if (char.IsDigit(ch))
                    {
                        return INT;
                    }
                    return 0;
                case -19:
                    if (char.IsDigit(ch))
                    {
                        return INT;
                    }
                    return 0;
                
            }
            return 0;
        }
        //идентификация ID
        private int IdentifyIdentifier(char c, int state)
        {
            switch (state)
            {
                case 1:
                    if (char.IsLetter(c))
                    {
                        return ID;
                    }
                    return 0;
                case -6:
                    if (char.IsLetter(c))
                    {
                        return ID;
                    }
                    return 0;
            }
            return 0;
        }
        //идентификация ключевых слов for и do
        private int IdentifyKeyWord(char c, int state)
        {
            switch (state) 
            {
                case 1:
                    if (c == 'd')
                    {
                        return 2;
                    }
                    if (c == 'f')
                    {
                        return 2;
                    }
                    if (c == 'i')
                    {
                        return 3;
                    }
                    if (c == '+')
                    {
                        return SIGN_PLUS;
                    }
                    return 0;
                case 2:
                    if (c == 'o')
                    {
                        return DO_KEYWORD;
                    }
                    return 0;
                case DO_KEYWORD:
                    if (c == 'r')
                    {
                        return FOR_KEYWORD;
                    }
                    return 0;
                case 3:
                    if (c == 'n')
                    {
                        return 4;
                    }
                    return 0;
                case 4:
                    if (c == 't')
                    {
                        return INT_KEYWORD;
                    }    
                    return 0;
                case SIGN_PLUS:
                    if (c == '+')
                    {
                        return INCREMENT_KEYWORD;
                    }
                    return 0;
            }
            return 0;

        }
        //идентификация пробела, переноса строки и табуляции
        private int IdentifySpace(char ch, int state)
        {
            switch (state)
            {
                case 1:
                    if (ch == ' ')
                    {
                        return SPACE;
                    }
                    if (ch == '\t')
                    {
                        return SPACE;
                    }
                    if (ch == '\r')
                    {
                        return SPACE;
                    }
                    if (ch == '\n')
                    {
                        return SPACE;
                    }
                    return 0;
                
                case SPACE:
                    if (ch == ' ')
                    {
                        return SPACE;
                    }
                    if (ch == '\t')
                    {
                        return SPACE;
                    }
                    if (ch == '\r')
                    {
                        return SPACE;
                    }
                    if (ch == '\n')
                    {
                        return SPACE;
                    }
                    return 0;
            }
            return 0;
        }
        //идентификация скобок и точки_-запятой
        private int IdentifySign(char ch, int state)
        {
            switch (state)
            {
                case 1:
                    if (ch == ';')
                    {
                        return SEMICOLON;
                    }
                    if (ch == '{')
                    {
                        return OPENING_CURLY_BRACE;
                    }
                    if (ch == '}')
                    {
                        return CLOSING_CURLY_BRACE;
                    }
                    if (ch == '(')
                    {
                        return OPENING_ROUND_BRACE;
                    }
                    if (ch == ')')
                    {
                        return CLOSING_ROUND_BRACE;
                    }
                    if (ch == '>')
                    {
                        return SIGN_BIGGER;
                    }
                    if (ch == '<')
                    {
                        return SIGN_SMALLER;
                    }
                    if (ch == '=')
                    {
                        return SIGN_EQUAL;
                    }
                    if (ch == '+')
                    {
                        return SIGN_PLUS;
                    }

                    return 0;
            }
            return 0;
        }
        private int identifyFloat(char ch, int state)
        {
            switch (state)
            {
                case 1:
                    if (ch == '0')
                    {
                        return 4;
                    }
                    if (char.IsDigit(ch))
                    {
                        return 2;
                    }
                    if (ch == '.')
                    {
                        return 3;
                    }
                    return 0;
                case 2:
                    if (char.IsDigit(ch))
                    {
                        return 2;
                    }
                    if (ch == '.' || ch == ',') return -1;
                    return 0;
                case -1:
                    if (char.IsDigit(ch))
                    {
                        return -2;
                    }
                    return 0;
                case -2:
                    if (char.IsDigit(ch))
                    {
                        return -2;
                    }
                    return 0;
                case 3:
                    if (char.IsDigit(ch))
                    {
                        return -3;
                    }
                    return 0;
                case -3:
                    if (char.IsDigit(ch))
                    {
                        return -3;
                    }
                    return 0;
                case 4:
                    if (ch == '.')
                    {
                        return 5;
                    }
                    return 0;
                case 5:
                    if (char.IsDigit(ch))
                    {
                        return 6;
                    }
                    return 0;
                case 6:
                    if (char.IsDigit(ch))
                    {
                        return 6;
                    }
                    if (ch == 'e')
                    {
                        return 7;
                    }
                    return 0;
                case 7:
                    if (ch == '+' || ch == '-')
                    {
                        return 8;
                    }
                    return 0;
                case 8:
                    if (char.IsDigit(ch))
                    {
                        return -4;
                    }
                    return 0;
                case -4:
                    if (char.IsDigit(ch))
                    {
                        return -4;
                    }
                    return 0;
            }
            return 0;
        }

        private int identifyDoubleFloat(char ch, int state)
        {
            switch (state)
            {
                case 1:
                    if (ch == '0')
                    {
                        return 2;
                    }
                    return 0;
                case 2:
                    if (ch == '.')
                    {
                        return 3;
                    }
                    return 0;
                case 3:
                    if (char.IsDigit(ch))
                    {
                        return 4;
                    }
                    return 0;
                case 4:
                    if (char.IsDigit(ch))
                    {
                        return 4;
                    }
                    if (ch == 'd')
                    {
                        return 5;
                    }
                    return 0;
                case 5:
                    if (ch == '+' || ch == '-')
                    {
                        return 6;
                    }
                    return 0;
                case 6:
                    if (char.IsDigit(ch))
                    {
                        return -5;
                    }
                    return 0;
                case -5:
                    if (char.IsDigit(ch))
                    {
                        return -5;
                    }
                    return 0;
            }
            return 0;
        }


        ////идентификация, передаем символ и состояние автомата
        //private void Identify(char c, int[] states)
        //{
        //    states[0] = IdentifyInt(c, states[0]);
        //    states[1] = IdentifyIdentifier(c, states[1]);
        //    states[2] = IdentifyKeyWord(c, states[2]);
        //    states[3] = IdentifySpace(c, states[3]);
        //    states[4] = IdentifySign(c, states[4]);
        //    states[5] = identifyFloat(c, states[5]);
        //    states[6] = identifyDoubleFloat(c, states[6]);

        //}




    }
}
