using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FINALS_APP_BARANGAYHEALTHCENTERSYSTEM
{
    internal class Program
    {
        static Queue<string> consultationQueue = new Queue<string>();
        static Queue<string> vaccinationQueue = new Queue<string>();
        static Queue<string> maternalcareQueue = new Queue<string>();
        static Queue<string> medicineclaimQueue = new Queue<string>();
        static Stack<string> actionHistory = new Stack<string>();
        static Dictionary<string, int> serviceCount = new Dictionary<string, int>();
        static Dictionary<string, int> priorityStats = new Dictionary<string, int>();
        static int[] hourlyPatients = new int[24];
        static string currentUser = "";

        static void Main(string[] args)
        {
            InitializeFiles();
            MainMenu();
            

        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"
                 _  _  ____   __   __   ____  _  _  __    __  __ _  ____    __    ____ 
                / )( \(  __) / _\ (  ) (_  _)/ )( \(  )  (  )(  ( \(  __)  (  )  (  _ \
                ) __ ( ) _) /    \/ (_/\ )(  ) __ (/ (_/\ )( /    / ) _)   / (_/\ ) __/
                \_)(_/(____)\_/\_/\____/(__) \_)(_/\____/(__)\_)__)(____)  \____/(__)  
");
                Console.ResetColor();
                Console.WriteLine("\n[1] Patient");
                Console.WriteLine("[2] Health Worker");
                Console.WriteLine("[3] Exit");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int roleChoice))
                {
                    switch (roleChoice)
                    {
                        case 1:
                            PatientPortal();
                            break;

                        case 2:
                            HealthWorkerPortal();
                            break;

                        case 3:
                            return;

                        default:
                            InvalidInput();
                            break;
                    }
                }

                else
                {
                    InvalidInput();
                }
            }
        }

        static void PatientPortal()
        {
            while (true)
            {
                HeaderDisplay("PATIENT PORTAL");
                Console.WriteLine("\n[1] Login");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] Exit");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int patientPortal))
                {
                    switch (patientPortal)
                    {
                        case 1:
                            if(Login("patients.txt"))
                            {
                                PatientDashboard();
                            }

                            break;

                        case 2:
                            Register("patients.txt");
                            break;

                        case 3:
                            return;

                        default:
                            InvalidInput();
                            break;
                    }
                }
            }
        }

        static void HealthWorkerPortal() 
        {
            while (true) 
            {
                HeaderDisplay("HEALTH WORKER PORTAL");
                Console.WriteLine("\n[1] Login");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] Exit");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int healthworkerPortal))
                {
                    switch (healthworkerPortal)
                    {
                        case 1:
                            if (Login("workers.txt"))
                            {
                                HealthWorkerDashboard();
                            }
                            break;

                        case 2:
                            Register("workers.txt");
                            break;

                        case 3:
                            return;

                        default:
                            InvalidInput();
                            break;
                    }
                }
            }
        }

        static bool Login(string fileName)  
        {
            List<string> users = Load(fileName);

            HeaderDisplay("LOGIN MENU");
            Console.Write("\nEnter your name: "); string userName = Console.ReadLine();
            Console.Write("Enter your Password: "); string userPassword = Console.ReadLine();

            for (int i = 0; i < users.Count; i++)
            {
                var user = User(users[i]);

                if (user.userName ==  userName && user.userPassword == userPassword)
                {
                    Console.WriteLine("Login Successful!");
                    currentUser = userName;
                    return true;
                }
            }
            Console.WriteLine("Invalid Login");
            Console.ReadKey();
            return false;
        }

        static void Register(string fileName)
        {
            List<string> users = Load(fileName);

            HeaderDisplay("REGISTER MENU");
            Console.Write("\nEnter your Name: "); string userName = Console.ReadLine();

            if (NameExists(fileName, userName))
            {
                Console.WriteLine("Name already exists.");
                return;
            }

            Console.Write("Enter your Password: "); string userPassword = Console.ReadLine();

            users.Add(UserFormat(userName, userPassword));
            Save(fileName, users);
            Console.WriteLine("User registered successfully.");
            Console.ReadKey();
        }

        static void PatientDashboard()
        {

        }

        static void HealthWorkerDashBoard()
        {

        }

        static void CreateFileIfNotExisting(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
        }

        static void InitializeFiles()
        {
            CreateFileIfNotExisting("patients.txt");
            CreateFileIfNotExisting("workers.txt");
            CreateFileIfNotExisting("visits.txt");
            CreateFileIfNotExisting("queues.txt");
            CreateFileIfNotExisting("analytics.txt");
            CreateFileIfNotExisting("reports.txt");
        }

        static void InvalidInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR]");
            Console.WriteLine("Invalid input. Please try again");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        static bool NameExists(string fileName, string name) 
        {
            List<string> users = Load(fileName);

            for (int i = 0; i < users.Count; i++)
            {
                var user = User(users[i]);

                if (user.userName == name)
                {
                    return true;
                }
            }

            return false;
        }

        static List<string> Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            return File.ReadAllLines(fileName).ToList();
        }
        static void Save(string fileName, List<string> data)
        {
            File.WriteAllLines(fileName, data);
        }

        static (string userName, string userPassword) User(string line)
        {
            string[] split = line.Split('|');
            return (split[0], split[1]);
        }

        static string UserFormat(string userName, string userPassword) 
        {
            return $"{userName}|{userPassword}";
        }

        static void HeaderDisplay(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+==================================================+");
            Console.WriteLine($"|{title.PadLeft((50 + title.Length) / 2).PadRight(50)}|");
            Console.WriteLine("+==================================================+");
            Console.ResetColor();
        }
    }
}
