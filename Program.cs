using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.Win32;
using System.IO.Compression;

namespace console
{
    internal class Program
    {   [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            string username;
            string command;
            WriteIntroduction();


            //Turns text color to gray
            Console.ForegroundColor = ConsoleColor.Gray;

            try
            {
                StreamReader sr = new StreamReader("data/data.txt");
                username = sr.ReadLine();
                sr.Close();
            }
            catch
            {
                try
                {
                    StreamWriter sw = new StreamWriter("data/data.txt");
                    //Tries to get username if is null then will try again
                    Console.Write("Tell us your username: ");
                    username = Convert.ToString(Console.ReadLine()).ToLower(); ;
                    if (username != "") { }
                    else while (username == "")
                        {
                            Console.Write("Tell us your username: ");
                            username = Convert.ToString(Console.ReadLine()).ToLower();
                            if (username != "") { }
                            else { }
                        }
                    sw.WriteLine(username);
                    sw.Close();
                    Console.Clear();
                    WriteIntroduction();
                }
                catch
                {
                    Directory.CreateDirectory("data");

                    StreamWriter sw = new StreamWriter("data/data.txt");
                    //Tries to get username if is null then will try again
                    Console.Write("Tell us your username: ");
                    username = Convert.ToString(Console.ReadLine()).ToLower();
                    if (username != "") { }
                    else while (username == "")
                        {
                            Console.Write("Tell us your username: ");
                            username = Convert.ToString(Console.ReadLine()).ToLower();
                            if (username != "") { }
                            else { }
                        }
                    sw.WriteLine(username);
                    sw.Close();
                    Console.Clear();
                    WriteIntroduction();
                }
            }
            //Commands section
            while (true){ 
                //Prefix
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("<");

                //Name
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{username.ToLower().Replace(" ", "-")}@juple-terminal");

                //Prefix
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ~$ ");

                Console.ForegroundColor = ConsoleColor.Cyan;

                //Commands with arguments
                command = Convert.ToString(Console.ReadLine()).ToLower();
                //Echo command reapeats your arguments
                if (command.Contains("echo"))
                {
                    //Deletes first 'echo' from message
                    var regex = new Regex(Regex.Escape("echo "));
                    if (command == "echo")
                    {
                        Console.ForegroundColor = ConsoleColor.Red; Console.Write("Invalid usage!\n"); Thread.Sleep(50);
                        Console.Write("Right usage:"); Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" echo"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(" <message>"); Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(".\n\n");
                    }
                    else if (regex.Replace(command, "", 1) == "") {
                        Console.ForegroundColor = ConsoleColor.Red; Console.Write("Invalid usage!\n"); Thread.Sleep(50);
                        Console.Write("Right usage:"); Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" echo"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(" <message>"); Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(".\n\n");
                    }
                    else
                    {
                        string work = regex.Replace(command, "", 1);
                        string[] chars = new string[command.Length];
                        //Will put every char of message to individual string to array
                        for (int i = 0; i < command.Length; i++)
                        {
                            if (work == "") break;
                            string letter = work.Substring(0, 1);
                            chars[i] = letter;
                            work = work.Remove(0, 1);
                        }
                        //Will write every letter of message with break 10 miliseconds between them
                        foreach (string lett in chars)
                        {
                            Console.Write(lett);
                            Thread.Sleep(10);
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        Thread.Sleep(50);
                    }
                }
                else if (command.Contains("changename"))
                {
                    //Checks if user wrote command without arguments
                    var regex = new Regex(Regex.Escape("changename "));
                    if (command == "changename")
                    {
                        Console.ForegroundColor = ConsoleColor.Red; Console.Write("Invalid usage!\n"); Thread.Sleep(50);
                        Console.Write("Right usage:"); Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" changename"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(" <name>"); Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(".\n\n");
                    }
                    else if (regex.Replace(command, "", 1) == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red; Console.Write("Invalid usage!\n"); Thread.Sleep(50);
                        Console.Write("Right usage:"); Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" changename"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(" <name>"); Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(".\n\n");
                    }
                    //If user wrote arguments it will change name to what user typed
                    else
                    {
                        //Change username in current session
                        string work = regex.Replace(command, "", 1);

                        Console.Write("Do you really want to change your name to {0}(y/n): ", work);

                        string answer = Convert.ToString(Console.ReadLine().ToLower());
                        if (answer == "y") username = work.Replace(" ", "-").ToLower();
                        else if (answer == "n") { }
                        else
                        {
                            while (true)
                            {
                                Thread.Sleep(50);
                                Console.Write("Do you really want to change your name to {0}(y/n): ", work);
                                answer = Convert.ToString(Console.ReadLine().ToLower());
                                if (answer == "y") { username = work.Replace(" ", "-").ToLower();  break; }
                                else if (answer == "n") { break; }
                            }
                        }
                        //Chnage user
                        try
                        {
                            StreamWriter sw = new StreamWriter("data/data.txt");
                            sw.WriteLine(username);
                            sw.Close();
                        }
                        catch
                        {
                            Directory.CreateDirectory("data");
                            StreamWriter sw = new StreamWriter("data/data.txt");
                            sw.WriteLine(username);
                            sw.Close();
                        }
                        Console.WriteLine();
                    }
                }
                else if (command.Contains("projects --d"))
                {
                    string work = command.Replace("projects --d ", "");
                    WebClient webClient = new WebClient();
                    
                    switch (work)
                    {
                        case "calculator":
                            Console.WriteLine("Donwloading...");
                            try
                            {
                                webClient.DownloadFile("https://drive.google.com/uc?id=1Qkj3KnFFGOMPbtSB47NFaDuGgE6Umt3K&export=download", "Calc-setup.zip");
                                ZipFile.ExtractToDirectory("Calc-setup.zip", Directory.GetCurrentDirectory());
                                File.Delete("Calc-setup.zip");
                                Console.WriteLine("Donwloaded sucessfully! Running...\n");
                                Process process = new Process();
                                process.StartInfo.FileName = "msiexec";
                                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                                process.StartInfo.Arguments = "/i Calc-Setup.msi";
                                process.StartInfo.Verb = "runas";
                                process.Start();
                                process.WaitForExit(60000);
                                File.Delete("Calc-Setup.msi");
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                    "Please check your internet connection and try again\n");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
;
                            break;
                        case "pc-components-monitor":
                            Console.WriteLine("Donwloading...");
                            try
                            {
                                webClient.DownloadFile("https://drive.google.com/uc?id=1gygNC51rgtR5kBvWJnObpy6UYtfEe01P&export=download", "PC-Components-Stats-Setup.zip");
                                ZipFile.ExtractToDirectory("PC-Components-Stats-Setup.zip", Directory.GetCurrentDirectory());
                                File.Delete("PC-Components-Stats-Setup.zip");
                                Console.WriteLine("Donwloaded sucessfully! Running...\n");
                                Process process2 = new Process();
                                process2.StartInfo.FileName = "msiexec";
                                process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                                process2.StartInfo.Arguments = "/i PC-Components-Stats-Setup.msi";
                                process2.StartInfo.Verb = "runas";
                                process2.Start();
                                process2.WaitForExit(60000);
                                File.Delete("PC-Components-Stats-Setup.msi");
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                    "Please check your internet connection and try again\n");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
                            break;
                        case "tictactoe":
                            Console.WriteLine("Donwloading...");
                            try
                            {
                                webClient.DownloadFile("https://drive.google.com/uc?id=18z80CD1k88EwReQzky7iAZju8dDdNeGe&export=download", "TicTacToe-Setup.zip");
                                ZipFile.ExtractToDirectory("TicTacToe-Setup.zip", Directory.GetCurrentDirectory());
                                File.Delete("TicTacToe-Setup.zip");
                                Console.WriteLine("Donwloaded sucessfully! Running...\n");
                                Process process3 = new Process();
                                process3.StartInfo.FileName = "msiexec";
                                process3.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                                process3.StartInfo.Arguments = "/i TicTacToe-Setup.msi";
                                process3.StartInfo.Verb = "runas";
                                process3.Start();
                                process3.WaitForExit(60000);
                                File.Delete("TicTacToe-Setup.msi");
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                    "Please check your internet connection and try again\n");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
                            break;

                    }
                }
                else
                {
                    //Switch for commands without arguments
                    switch (command)
                    {
                        //Help command will show list of all commands
                        case "help":
                            WriteCommandsList();
                            Console.WriteLine();
                            Thread.Sleep(50);
                            break;
                        //Clear command will clear all of user writings
                        case "clear":
                            Console.Clear();
                            WriteIntroduction();
                            break;
                        //Ver command will show current version
                        case "ver":
                            Console.WriteLine("v0.1-beta");
                            Thread.Sleep(50);
                            Console.WriteLine();
                            Thread.Sleep(50);
                            break;
                        //Projects command which shows all of my projets
                        case "projects":
                            WriteProjectsList();
                            Console.WriteLine();
                            Thread.Sleep(50);
                            break;
                        case "github":
                            string url = "https://github.com/Jupiter090/";
                            try
                            {
                                Process.Start(url);
                            }
                            catch
                            {
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    url = url.Replace("&", "^&");
                                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                                }
                                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                {
                                    Process.Start("xdg-open", url);
                                }
                                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                                {
                                    Process.Start("open", url);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            break;
                        case "test":

                            break;
                        //Terminal.exit command will close the terminal
                        case "terminal.exit":
                            Thread.Sleep(50);
                            Console.Write("Are you sure(y/n): ");
                            string answer = Convert.ToString(Console.ReadLine().ToLower());
                            if (answer == "y") Environment.Exit(0);
                            else if (answer == "n") break;
                            else
                            {
                                while (true)
                                {
                                    Thread.Sleep(50);
                                    Console.Write("Are you sure(y/n): ");
                                    answer = Convert.ToString(Console.ReadLine().ToLower());
                                    if (answer == "y") Environment.Exit(0);
                                    else if (answer == "n") break;
                                }
                            }
                            break;
                        //When command is not found will show tip to use help command
                        default:
                            Console.WriteLine("Command not found.");
                            Thread.Sleep(50);
                            Console.Write("To found all valid commands write '");
                            Thread.Sleep(50);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("help");
                            Thread.Sleep(50);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("'!\n");
                            Console.WriteLine();
                            Thread.Sleep(50);
                            break;
                    }
                }
            }
        }


        static void WriteIntroduction()
        {
            //Changes text color to red
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Juple - The best terminal you will ever see!";

            //String for first dashed line
            string dash = "┌-------------------------------------------------------------------------------------------------------------┐";

            //Centers text
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            //Writes dashed line
            Console.WriteLine(dash);
            Thread.Sleep(50);
            //Centers text
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            //Writes welcome line
            Console.WriteLine("| ,--.   ,--.       ,--.                                    ,--.               ,--.               ,--.        |");
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            Console.WriteLine("| |  |   |  | ,---. |  | ,---. ,---. ,--,--,--. ,---.     ,-'  '-. ,---.       |  |,--.,--. ,---. |  | ,---.  |");
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            Console.WriteLine("| |  |.'.|  || .-. :|  || .--'| .-. ||        || .-. :    '-.  .-'| .-. | ,--. |  ||  ||  || .-. ||  || .-. : |");
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            Console.WriteLine("| |   ,'.   ||   --.|  || `--.' '-' '|  |  |  ||   --.      |  |  ' '-' ' |  '-'  /'  ''  '| '-' '|  ||   --. |");
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            Console.WriteLine("| '--'   '--' `----'`--' `---' `---' `--`--`--' `----'      `--'   `---'   `-----'  `----' |  |-' `--' `----' |");
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            Console.WriteLine("|                                                                                          `--'               |");
            Thread.Sleep(50);
            //Centerws text
            Console.SetCursorPosition((Console.WindowWidth - dash.Length) / 2, Console.CursorTop);
            //Writes bottom dashed line 
            Console.WriteLine("└-------------------------------------------------------------------------------------------------------------┘");
            Thread.Sleep(50);

            //Turns text color to red and writes warning
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n Welcome to Juple - a simple terminal made by Jupiter!\n For list of all commands type '");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("help");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("'.\n");
            Thread.Sleep(50);
            Console.Write("\n");
            Thread.Sleep(50);
        }
        public static void WriteCommandsList()
        {
            string[] commands = {"help", "ver", "clear", "projects", "echo", "changename", "github", "terminal.exit"};
            string[] descriptions = { "Will show you this list of all commands", "Shows you version of terminal you are running", "Clear all commands you wrote", "Gives you list of all my projects" ,"Repeats message you write", "Change your name to what you want", "Bring you to my GitHub page", "Exits the Juple terminal"};
            for (int i = 0; i < commands.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(commands[i]);
                Thread.Sleep(50);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(Console.WindowWidth / 8, Console.CursorTop);
                Console.Write(descriptions[i] + "\n");
                Thread.Sleep(50);
            }
        }
        public static void WriteProjectsList()
        {
            string[] projects = {"Calculator", "PC-Components-Monitor", "TicTacToe", "And more is coming..." };
            string[] descriptions = { "A simple calculator made with windows forms", "An app for monitoring components of your computer", "Simple TicTacToe game made with windows forms", "Find all my projects at https://github.com/Jupiter090" };
            for (int i = 0; i < projects.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(projects[i]);
                Thread.Sleep(50);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(Console.WindowWidth / 8, Console.CursorTop);
                Console.Write(descriptions[i] + "\n");
                Thread.Sleep(50);
            }
        }
    }
}