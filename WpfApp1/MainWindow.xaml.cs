using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Calculator calc;
        private string previous;
        private bool isPrev = false;
        private char[] validOperators = new char[]
        {
            '+',
            '-',
            '×',
            '÷',
            '^'
        };

        public MainWindow()
        {
            InitializeComponent();
            calc = new Calculator();
            //result.Text = "0";
            //result2.Text = "";
        }

        private bool buttonsVisible = true;

        private void Func_Click(object sender, RoutedEventArgs e)
        {
            if (buttonsVisible)
            {
                pow.Visibility = Visibility.Visible;
                sqrt.Visibility = Visibility.Visible;
                log10.Visibility = Visibility.Visible;
                fac.Visibility = Visibility.Visible;
            }
            else
            {
                pow.Visibility = Visibility.Collapsed;
                sqrt.Visibility = Visibility.Collapsed;
                log10.Visibility = Visibility.Collapsed;
                fac.Visibility = Visibility.Collapsed;
            }

            buttonsVisible = !buttonsVisible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                string input = (string)((Button)e.OriginalSource).Content;
                if (input == "C")
                {
                    result.Text = calc.C();
                    result2.Text = "";
                    isPrev = false;
                }
                else if(input== "CE")
                {
                    int b = -1;
                    try
                    {
                        for (int i = 0; i < calc.PreviousCommand.Length; i++)
                        {
                            for (int j = 0; j < validOperators.Length; j++)
                            {
                                if (calc.PreviousCommand[i] == validOperators[j])
                                {
                                    b = i;
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Попередня команда відсутня!");
                    }
                    result2.Text = calc.PreviousCommand.Substring(0, b + 1);
                    result.Text = calc.PreviousCommand.Substring(b + 1, calc.PreviousCommand.Length - b - 1);
                    previous = result2.Text;
                }
                else if(input== "⌫")
                {
                    result.Text = calc.Erase();
                }
                else if (input == ".")
                {
                    if (NormalizeDot())
                        result.Text += input;
                }

                else if (input == "0" || input == "00")
                {
                    if (NormalizeZero())
                        result.Text += input;
                }
                else if (result.Text == "0")
                    result.Text = input;
                //else if (input == "π")
                //{
                //    if (NormalizePI())
                //        result.Text += input;
                //}
                else
                    result.Text += input;

        }
        private bool NormalizeZero()
        {
            if (result.Text.Length < 2)
                return false;
            return true;
        }
        private bool NormalizeDot()
        {
            for (int i = 0; i < result.Text.Length; i++)
            {
                if (result.Text[i] == '.')
                    return false;
            }
            return true;
        }
        private bool NormalizePI()
        {
            for (int i = 0; i < result.Text.Length; i++)
            {
                if (result.Text[i] == 'π')
                    return false;
            }
            return true;
        }

        //private void Button_Pow_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(result.Text))
        //    {
        //        double num;
        //        if (double.TryParse(result.Text, out num))
        //        {
        //            calc.ExecuteCommand(new PowCommand(calc.receiver_, num));
        //            result.Text = calc.receiver_.Info.ToString();
        //            previous = null;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Невiрно введено значення!");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Помилка!");
        //    }
        //}

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string input = (string)((Button)e.OriginalSource).Content;

            if (input == "=")
            {
                if (!string.IsNullOrEmpty(previous) && !string.IsNullOrEmpty(result.Text))
                {
                    calc.receiver_.Info = Convert.ToDouble(previous.Substring(0, previous.Length - 1));

                    double num;
                    if (double.TryParse(result.Text, out num))
                    {
                        switch (previous[previous.Length - 1])
                        {
                            case '+':
                                Convert.ToString(calc.AddCommand(num));
                                break;
                            case '-':
                                Convert.ToString(calc.SubCommand(num));
                                break;
                            case '×':
                                Convert.ToString(calc.MulCommand(num));
                                break;
                            case '÷':
                                Convert.ToString(calc.DivCommand(num));
                                break;
                            case '^':
                                Convert.ToString(calc.PowCommand(num));
                                break;
                        }
                        result.Text = calc.receiver_.Info.ToString();
                        result2.Text = "";
                        previous = null;
                        isPrev = false;
                    }
                }
            }
            else if (input == "√")
            {
                result.Text = Convert.ToString(calc.SqrtCommand(Convert.ToDouble(result.Text)));
            }
            else if (input == "ln")
            {
                result.Text = Convert.ToString(calc.Log10(Convert.ToDouble(result.Text)));
            }
            else if (!isPrev)
            {
                previous = result.Text + input;
                result2.Text = previous;
                result.Text = "0";
                isPrev = true;
            }
        }
        //private void Button_Log_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(result.Text))
        //    {
        //        double num;
        //        if (double.TryParse(result.Text, out num))
        //        {
        //            if (num > 0)
        //            {
        //                calc.receiver_.Info = Math.Log10(num);
        //                result.Text = calc.receiver_.Info.ToString(); 
        //                previous = null;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Логарифм нуля або вiд'ємного числа не визначено!");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Невiрно введено значення!");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Помилка!");
        //    }
        //}
        //private async void Button_Factorial_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(result.Text))
        //    {
        //        int num;
        //        if (int.TryParse(result.Text, out num))
        //        {
        //            // Перевірка чи число не від'ємне
        //            if (num >= 0)
        //            {
        //                //метод для обчислення факторіала
        //                if (num <= 999) // Обмеження на величину числа для обчислення факторіала
        //                {
        //                    double factorial = await Task.Run(() => CalculateFactorial(num));
        //                    result.Text = factorial.ToString();
        //                    previous = null;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Число занадто велике для обчислення факторіала.");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Факторіал можна визначити лише для не від'ємних цілих чисел!");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Введено не ціле значення!");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Помилка!");
        //    }
        //}

        private double CalculateFactorial(int n)
        {
            double result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        abstract class Command
        {
            protected Receiver receiver;
            protected double operand;
            public Command(Receiver receiver)
            {
                this.receiver = receiver;
            }
            public abstract void Execute();
            public abstract void UnExecute();
        }

        class AddCommand : Command
        {
            public AddCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('+', operand);
            }

            public override void UnExecute()
            {
                receiver.Run('-', operand);
            }
        }

        class SubCommand : Command
        {
            public SubCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('-', operand);
            }

            public override void UnExecute()
            {
                receiver.Run('+', operand);
            }
        }

        class MulCommand : Command
        {

            public MulCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('*', operand);
            }

            public override void UnExecute()
            {
                receiver.Run('/', operand);
            }
        }

        class DivCommand : Command
        {
            public DivCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('/', operand);
            }

            public override void UnExecute()
            {
                receiver.Run('*', operand);
            }
        }

        class PowCommand : Command
        {
            public PowCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }
            public override void Execute()
            {
                receiver.Run('^', operand);
            }
            public override void UnExecute()
            {
               
            }
        }

        class SqrtCommand : Command
        {
            public SqrtCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('√', 0);
            }

            public override void UnExecute()
            {
               
            }
        }

        class Log10Command : Command
        {
            public Log10Command(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('l', 0);
            }

            public override void UnExecute()
            {
               
            }
        }

        class FactorialCommand : Command
        {
            public FactorialCommand(Receiver receiver, double operand) : base(receiver)
            {
                this.receiver = receiver;
                this.operand = operand;
            }

            public override void Execute()
            {
                receiver.Run('!', operand);
            }

            public override void UnExecute()
            {
              
            }
        }
        class Receiver
        {
            public double Info;

            public void Run(char operationCode, double operand)
            {
                switch (operationCode)
                {
                    case '+':
                        Info += operand;
                        break;
                    case '-':
                        Info -= operand;
                        break;
                    case '*':
                        Info *= operand;
                        break;
                    case '/':
                        if (operand != 0)  
                            Info /= operand;
                        else
                            MessageBox.Show("Неможливо подiлити на 0");
                        break;
                    case '^':
                        Info = Math.Pow(Info, operand);
                        break;
                    case '√':
                        Info = Math.Sqrt(operand);
                        break;
                    case 'l':
                        Info = Math.Log10(operand); 
                        break;
                    case '!':
                        Info = Factorial((int)operand); 
                        break;
                }
            }

            private double Factorial(int n)
            {
                if (n == 0)
                    return 1;
                return n * Factorial(n - 1);
            }
        }

        class Invoker
        {
            private List<Command> commands = new List<Command>();
            private int curr = 0;

            public void StoreComm(Command command)
            {
                commands.Add(command);
            }

            public void ExecuteComm()
            {
                commands[curr].Execute();
                curr++;
            }

            public void Undo(int levels)
            {
                for (int i = 0; i < levels; i++)
                {
                    if (curr > 0)
                        commands[--curr].UnExecute();
                }
            }

            public void Redo(int levels)
            {
                for (int i = 0; i < levels; i++)
                {
                    if (curr < commands.Count - 1)
                        commands[curr++].Execute();
                }
            }
        }

        class Calculator
        {
            public Receiver receiver_;
            private Invoker invoker_;

            public Calculator()
            {
                receiver_ = new Receiver();
                invoker_ = new Invoker();
            }
            private double Run(Command command)
            {
                invoker_.StoreComm(command);
                invoker_.ExecuteComm();
                return receiver_.Info;
            }
            public void ExecuteCommand(Command command)
            {
                invoker_.StoreComm(command);
                invoker_.ExecuteComm();
            }
            public double AddCommand(double operand)
            {
                return Run(new AddCommand(receiver_, operand));
            }
            public double SubCommand(double operand)
            {
                return Run(new SubCommand(receiver_, operand));
            }
            public double MulCommand(double operand)
            {
                return Run(new MulCommand(receiver_, operand));
            }
            public double DivCommand(double operand)
            {
                return Run(new DivCommand(receiver_, operand));
            }
            public double PowCommand(double operand)
            {
                return Run(new PowCommand(receiver_, operand));
            }
            public double SqrtCommand (double operand)
            {
                return Run(new SqrtCommand(receiver_, operand));
            }
            public double Log10(double operand)
            {
                return Run(new Log10Command(receiver_, operand));
            }
            public string C()
            {
                receiver_.Info = 0;
                return "0";
            }
            public string Erase()
            {
                string erasedInfo = Convert.ToString(receiver_.Info);
                erasedInfo.Substring(0, erasedInfo.Length - 1);
                receiver_.Info = Convert.ToDouble(erasedInfo);
                return erasedInfo;
            }
            public string PreviousCommand { get; set; }
        }
    }
}
