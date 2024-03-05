using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string TextOnDefault => "for(int i = 0; i > 10; i++)\n" +
                                        "\tdo\n" +
                                        "\tint a = i;\n" +
                                        "\ta++;\n";
        public MainWindow()
        {
            InitializeComponent();
            StartText startText = new StartText();
            startText.SText = TextOnDefault;
            DataContext = startText;

        }

        
        private string _data ;
        public string Data { get => _data; set => _data = value; }

        private void LexButton_Click(object sender, RoutedEventArgs e)
        {
            textOut.Text = "";
            Data = TextInput.Text + TextInput.Text + " ";
            if (Data != null)
            {
                Lexema lexema = new Lexema(Data);
                List<Token> tokens = new List<Token>();
                try
                {
                    while (true)
                    {
                        Token token = lexema.ParseToken();
                        tokens.Add(token);
                        
                    }
                }
                catch 
                {
                    foreach (Token token in tokens)
                    {
                        if (token.State == Lexema.SPACE)
                        {
                            continue;
                        }
                        textOut.Text += printText(token, Data) + "\n";
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет текста для анализа.");
            }
        }

        public static String printText(Token token, string data)
        {
            string str = "";
            int stPos = token.StartingPosition;
            int endPos = token.EndingPosition;
            for (int i = stPos; i < endPos; i++)
            {
                str += data[i];
            }
            string Lexeme = str + "\t"+ getNameOfTableByState(token.State);
            return Lexeme;
        }



        public static string getNameOfTableByState(int state)
        {
            switch (state)
            {
                case Lexema.FOR_KEYWORD:
                    return "Кл. слово FOR";
                case Lexema.DO_KEYWORD:
                    return "Кл. слово DO";
                case Lexema.INT_KEYWORD:
                    return "Кл. слово INT";

                case Lexema.INT:
                    return "Целочисл. число";
                case Lexema.ID:
                    return "Идентификатор";
                case Lexema.FLOAT_KEYWORD | Lexema.DOUBLE_KEYWORD:
                case Lexema.DOUBLE_KEYWORD:
                case Lexema.RETURN_KEYWORD:
                    return "Ключ. слово";
                case -1:
                case -2:
                case -3:
                case -4:
                    return "Число с пл. точкой";
                case -5:
                    return "DOUBLE FLOAT";
                //case Lexema.SPACE:
                //    return "SPACE";
                case Lexema.CLOSING_CURLY_BRACE:
                    return "Фиг. скобка закр.";
                case Lexema.OPENING_CURLY_BRACE:
                    return "Фиг. скобка откр.";
                case Lexema.OPENING_ROUND_BRACE:
                    return "Скобка откр.";
                case Lexema.CLOSING_ROUND_BRACE:
                    return "Скобка закр.";
                case Lexema.SEMICOLON:
                    return "Точка зпт.";
                case Lexema.SIGN_SMALLER:
                    return "Знак сравн. меньше";
                case Lexema.SIGN_BIGGER:
                    return "Знак сравн. больше";
                case Lexema.SIGN_EQUAL:
                    return "Знак присвоения";
                case Lexema.SPACE:
                    return "Пробел, перенос троки, табуляция";
                case Lexema.SIGN_PLUS:
                    return "Знак плюс";
                case Lexema.INCREMENT_KEYWORD:
                    return "Инкремент";
                case Lexema.ERROR:
                    return "ERROR";

            }
            return Lexema.ERROR.ToString();
        }

    }









}

