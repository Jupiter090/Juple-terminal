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
    public class Commands
    {

        public static void GetCommand(String command)
        {
            //Switch for commands without arguments
            switch (command)
            {
                //Help command will show list of all commands
                case "help":
                    Program.WriteCommandsList();
                    Console.WriteLine();
                    Thread.Sleep(50);
                    break;
                //Clear command will clear all of user writings
                case "clear":
                    Console.Clear();
                    Program.WriteIntroduction();
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
                    Program.WriteProjectsList();
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
