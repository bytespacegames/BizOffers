using System;
using System.Collections.Generic;
using System.Text;

namespace BizOffers
{
    class Program
    {
        static string code = "set 104 out h\noutput char h\noperate h + 1 out h\noutput char h";

        static int currentLine = 1;

        static List<string> lines = new List<string>();
        static Dictionary<string, double> vars = new Dictionary<string, double>();
        static string getLine(int i)
        {
            return lines[i - 1];
        }
        static string ranSymb()
        {
            Random ran = new Random();
            int i = ran.Next(0, 5);

            switch (i)
            {
                case 0:
                    return "+";
                case 1:
                    return "-";
                case 2:
                    return "x";
                case 3:
                    return "/";
                case 4:
                    return "%";
                default:
                    return "+";
            }
        }

        static void Main(string[] args)
        {
            //lines = new List<string>(code.Split("\n"));

            Console.WriteLine("Please enter your code line by line. Code must be reentered each time and you will face the challenge of offers by the IDE.");
            Console.WriteLine("When you would like to run your program, enter \"start\"");

            List<string> inittedvars = new List<string>();

            string line = Console.ReadLine();
            while (line != "start")
            {
                string trmd = line.Trim();
                string[] trimargs = trmd.Split(" ");

                if (trimargs[0] == "set")
                {
                    inittedvars.Add(trimargs[3]);
                }

                Random r = new Random();
                if (r.Next(0,6) == 1)
                {
                    if (inittedvars.Count > 0)
                    {
                        string modifvar = inittedvars[r.Next(0, inittedvars.Count)];

                        string symbol = ranSymb();

                        string offerLine = "operate " + modifvar + " " + symbol + " " +  r.Next(1,6) + " out " + modifvar;

                        if (symbol == "+" || symbol == "x")
                        {
                            Console.WriteLine(offerLine + " --Oh wow! The IDE returned some of your money because you overpaid on your taxes! That's good and all, but it messes up your program! Counter the changes the IDEs offer caused!");
                        }
                        else
                        {
                            Console.WriteLine(offerLine + " --Oh no! The IDE taxed one of your variables! Looks like you commited tax fraud! Try to counter this offer by finding a way to revert this variable to its original state.");
                        }

                        lines.Add(offerLine);
                    } else
                    {
                        int lineskip = lines.Count + r.Next(0, 10);
                        lines.Add("goto " + lineskip);
                        Console.WriteLine("goto " + lineskip + " --Oh no! The IDE did not find any variables to tax, so it instead reset your program to line " + lineskip + "! Counter this offer to make your program work properly!");
                    }
                }
                lines.Add(line);

                line = Console.ReadLine();
            }

            Console.Clear();

            Console.Write("Program starting...");
            Console.WriteLine("\n");
            while (currentLine <= lines.Count)
            {
                string codeLine = getLine(currentLine).Trim().ToLower();
                string[] argss = codeLine.Split(" ");


                switch (argss[0])
                {
                    case "input":
                        int inputrec = (int) Console.ReadKey().KeyChar;
                        string var = argss[2];
                        switch (argss[1])
                        {
                            case "out":
                                vars[var] = inputrec;
                                break;
                        }
                        break;
                    case "set":
                        double inputint;
                        try
                        {
                            inputint = int.Parse(argss[1]);
                        } catch (FormatException)
                        {
                            inputint = vars[argss[1]];
                        }
                        string varname = argss[3];
                        switch (argss[2])
                        {
                            case "out":
                                if (vars.ContainsKey(varname))
                                {
                                    vars[varname] = inputint;
                                } else
                                {
                                    vars.Add(varname, inputint);
                                }
                                
                                break;
                        }
                        break;
                    case "output":
                        string type = argss[1];
                        switch (type)
                        {
                            case "string":
                                string index = "string ";
                                string input = codeLine.Substring(codeLine.IndexOf(index) + index.Length);
                                Console.Write(input);
                                break;
                            case "char":
                                try  {
                                    double inputof = int.Parse(argss[2]);
                                    Console.Write(Encoding.ASCII.GetString(new byte[] { (byte) inputof }));
                                } catch (FormatException f)
                                {
                                    double inputof = vars[argss[2]];
                                    Console.Write(Encoding.ASCII.GetString(new byte[] { (byte)inputof }));
                                }
                                break;
                        }
                        break;
                    case "operate":
                        double vara;
                        double varb;
                        // operate a + b out c
                        string opertype = argss[2];

                        double finalres;

                        try
                        {
                            vara = int.Parse(argss[1]);
                        }
                        catch (FormatException)
                        {
                            vara = vars[argss[1]];
                        }
                        try
                        {
                            varb = int.Parse(argss[3]);
                        }
                        catch (FormatException)
                        {
                            varb = vars[argss[3]];
                        }

                        switch (opertype)
                        {
                            case "+":
                                finalres = vara + varb;
                                break;
                            case "-":
                                finalres = vara - varb;
                                break;
                            case "x":
                                finalres = vara * varb;
                                break;
                            case "/":
                                finalres = vara / varb;
                                break;
                            case "%":
                                finalres = vara % varb;
                                break;
                            default:
                                finalres = 69;
                                break;
                        }

                        switch (argss[4])
                        {
                            case "out":
                                vars[argss[5]] = finalres;
                                break;
                        }

                        break;
                    case "if":
                        double condition;
                        try
                        {
                            condition = int.Parse(argss[1]);
                        }
                        catch (FormatException)
                        {
                            condition = vars[argss[1]];
                        }

                        if (condition == 0)
                        {
                            currentLine++;
                        }
                        break;
                    case "ifnot":
                        double conditionnot;
                        try
                        {
                            conditionnot = int.Parse(argss[1]);
                        }
                        catch (FormatException)
                        {
                            conditionnot = vars[argss[1]];
                        }

                        if (conditionnot != 0)
                        {
                            currentLine++;
                        }
                        break;
                    case "goto":
                        double linenumb;
                        try
                        {
                            linenumb = int.Parse(argss[1]);
                        }
                        catch (FormatException)
                        {
                            linenumb = (int) vars[argss[1]];
                        }

                        currentLine = (int) linenumb - 1;
                        break;

                }
                currentLine++;
            }

            Console.Write("\n\nProgram complete.");
            while(true)
            {
                
            }
        }
    }
}
