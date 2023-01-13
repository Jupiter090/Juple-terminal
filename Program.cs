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
using System.ComponentModel;
using System.Security;

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

        static bool DownloadCompleted = false;
        static bool FirstTime = true;

        static int currentTimes = 1;
        static int Times = 0;

        static WebClient client = new WebClient();
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

                //Changename
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
                                if (answer == "y") { username = work.Replace(" ", "-").ToLower(); break; }
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
                        //Enter
                        Console.WriteLine();
                    }
                }

                //Download project
                else if (command.Contains("--d") && command.Contains("projects "))
                {
                    WebClient webClient = new WebClient();
                    //Quiet argument
                    if (command.Contains("--q"))
                    {
                        string work1 = command.Replace("projects ", "").Replace("--d ", "").Replace("--q ", "");
                        
                        switch (work1)
                        {
                            

                            //Calculator download
                            case "calculator":
                                try
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDoneCalcQuiet);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=1Qkj3KnFFGOMPbtSB47NFaDuGgE6Umt3K&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                        "Please check your internet connection and try again\n");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                break;
                            //PC Components Monitor download
                            case "pc-components-monitor":
                                try
                                {
                                    
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDonePCMonitQuiet);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=1gygNC51rgtR5kBvWJnObpy6UYtfEe01P&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;

                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                        "Please check your internet connection and try again\n");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                break;
                            //Tic-Tac download
                            case "tictactoe":
                                Console.WriteLine("Downloading...");
                                try
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDoneTicTacToeQuiet);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=18z80CD1k88EwReQzky7iAZju8dDdNeGe&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                        "Please check your internet connection and try again\n");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                break;

                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Coulnd't find project!\n" +
                                    "For list of all projects write '");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("projects");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("'!");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;

                        }
                    }
                    //Without quiet argument
                    else
                    {
                        string work = command.Replace("projects --d ", "");
                        work.ToLower();


                        //Without quite argument
                        switch (work)
                        {
                            //Calculator download
                            case "calculator":
                                try
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDoneCalculator);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=1Qkj3KnFFGOMPbtSB47NFaDuGgE6Umt3K&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;
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
                            //PC Components Monitor download
                            case "pc-components-monitor":
                                try
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDonePCMonit);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=1gygNC51rgtR5kBvWJnObpy6UYtfEe01P&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                        "Please check your internet connection and try again\n");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                break;
                            //Tic-Tac download
                            case "tictactoe":
                                try
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadDoneTicTacToe);
                                    Uri uri = new Uri("https://drive.google.com/uc?id=18z80CD1k88EwReQzky7iAZju8dDdNeGe&export=download");
                                    client.DownloadFileAsync(uri, "Calc-setup.zip");
                                    Console.WriteLine("Downloading...");

                                    while (!DownloadCompleted)
                                    {

                                    }

                                    DownloadCompleted = false;
                                    FirstTime = true;
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something happend while we tried to download installator for this program.\n" +
                                        "Please check your internet connection and try again\n");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Coulnd't find project!\n" +
                                    "For list of all projects write '");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("projects");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("'!");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                        }
                        
                    }
                }
                else if (command.Contains("open"))
                {
                    string work = command.Replace("open ", "");
                    if (!(work.Contains("http://") && work.Contains("www.")))
                    {
                        work = "https://www." + work;
                    }
                    else if (!(work.Contains("http://")) && work.Contains("www."))
                    {
                        work = "https://" + work;
                    }
                    try
                    {
                        try
                        {
                            Process.Start(work);
                        }
                        catch
                        {
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                work = work.Replace("&", "^&");
                                Process.Start(new ProcessStartInfo(work) { UseShellExecute = true });
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            {
                                Process.Start("xdg-open", work);
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            {
                                Process.Start("open", work);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.WriteLine("The URL wasn't in correct format. \n" +
                            "Correct format: www.website.com!");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                }


                else
                {
                    Commands.GetCommand(command);
                }
            }
        }


        public static void WriteIntroduction()
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
            string[] commands = {"Command", "help", "ver", "clear", "projects", "echo", "changename", "open" , "github", "terminal.exit"};
            string[] descriptions = {"Description", "Will show you this list of all commands", "Shows you version of terminal you are running", "Clear all commands you wrote", "Gives you list of all my projects" ,"Repeats message you write", "Change your name to what you want", "Opens website you input",  "Bring you to my GitHub page", "Exits the Juple terminal"};
            string[] args = { "Arguments\n", "", "", "", "--d --q <project>", "<message>", "<name>", "<URL>", "", "", ""};
            for (int i = 0; i < commands.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(commands[i]);
                Thread.Sleep(50);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(Console.WindowWidth / 8, Console.CursorTop);
                Console.Write(args[i]);
                Thread.Sleep(50);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(Console.WindowWidth / 4, Console.CursorTop);
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
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        private static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            Console.Write("[");
            for (int i = 0; i < 10; i++)
            {
                if(i < Math.Round(percentage / 10))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("-");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("-");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
            }
            Console.WriteLine("] " + Math.Round(percentage / 10) * 10 + "%");
            currentTimes++;
        }
        private static void DownloadDoneCalcQuiet(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try
            {
                ZipFile.ExtractToDirectory("Calc-setup.zip", Directory.GetCurrentDirectory());
            }catch
            { }
            File.Delete("Calc-setup.zip");
            Process process = new Process();
            process.StartInfo.FileName = "msiexec";
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process.StartInfo.Arguments = "/qb /i Calc-Setup.msi";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit(60000);
            File.Delete("Calc-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
        private static void DownloadDonePCMonitQuiet(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try {
                ZipFile.ExtractToDirectory("Calc-Setup.zip", Directory.GetCurrentDirectory());
            }
            catch
            {

            }
            File.Delete("Calc-Setup.zip");
            Process process2 = new Process();
            process2.StartInfo.FileName = "msiexec";
            process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process2.StartInfo.Arguments = "/qb /i PC-Components-Monitor-Setup.msi";
            process2.StartInfo.Verb = "runas";
            process2.Start();
            process2.WaitForExit(60000);
            File.Delete("PC-Components-Monitor-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
        private static void DownloadDoneTicTacToeQuiet(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try
            {
                ZipFile.ExtractToDirectory("Calc-Setup.zip", Directory.GetCurrentDirectory());
            }
            catch
            {

            }
            File.Delete("Calc-Setup.zip");
            Process process2 = new Process();
            process2.StartInfo.FileName = "msiexec";
            process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process2.StartInfo.Arguments = "/qb /i TicTacToe-Setup.msi";
            process2.StartInfo.Verb = "runas";
            process2.Start();
            process2.WaitForExit(60000);
            File.Delete("TicTacToe-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
        private static void DownloadDoneTicTacToe(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try
            {
                ZipFile.ExtractToDirectory("Calc-Setup.zip", Directory.GetCurrentDirectory());
            }
            catch
            {

            }
            File.Delete("Calc-Setup.zip");
            Process process2 = new Process();
            process2.StartInfo.FileName = "msiexec";
            process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process2.StartInfo.Arguments = "/i TicTacToe-Setup.msi";
            process2.StartInfo.Verb = "runas";
            process2.Start();
            process2.WaitForExit(60000);
            File.Delete("TicTacToe-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
        private static void DownloadDoneCalculator(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try
            {
                ZipFile.ExtractToDirectory("Calc-Setup.zip", Directory.GetCurrentDirectory());
            }
            catch
            {

            }
            File.Delete("Calc-Setup.zip");
            Process process2 = new Process();
            process2.StartInfo.FileName = "msiexec";
            process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process2.StartInfo.Arguments = "/i Calc-Setup.msi";
            process2.StartInfo.Verb = "runas";
            process2.Start();
            process2.WaitForExit(60000);
            File.Delete("Calc-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
        private static void DownloadDonePCMonit(object sender, AsyncCompletedEventArgs e)
        {
            Times++;
            int time = Times;
            while (currentTimes != time)
            {

            }
            try
            {
                ZipFile.ExtractToDirectory("Calc-Setup.zip", Directory.GetCurrentDirectory());
            }
            catch
            {

            }
            File.Delete("Calc-Setup.zip");
            Process process2 = new Process();
            process2.StartInfo.FileName = "msiexec";
            process2.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process2.StartInfo.Arguments = "/i PC-Components-Stats-Setup.msi";
            process2.StartInfo.Verb = "runas";
            process2.Start();
            process2.WaitForExit(60000);
            File.Delete("PC-Components-Stats-Setup.msi");
            Times = 0;
            currentTimes = 1;
            DownloadCompleted = true;
            Console.WriteLine("Download completed!");
            Console.WriteLine();
        }
    }
}