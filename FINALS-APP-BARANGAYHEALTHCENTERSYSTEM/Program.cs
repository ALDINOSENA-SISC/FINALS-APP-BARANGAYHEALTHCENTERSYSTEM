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

                else
                {
                    InvalidInput();
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
                                HealthWorkerDashBoard();
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

                else
                {
                    InvalidInput();
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
            while (true)
            {
                HeaderDisplay("PATIENT DASHBOARD");
                Console.WriteLine($"\nWelcome, {currentUser}");
                Console.WriteLine("\n[1] Select Services");
                Console.WriteLine("[2] View Queue Progress Tracker");
                Console.WriteLine("[3] View Health Visit History");
                Console.WriteLine("[4] Logout");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int patientChoice))
                {
                    switch (patientChoice)
                    {
                        case 1:
                            SelectServices();
                            break;

                       /* case 2:
                            QueueProgressTracker();
                            break;

                        case 3:
                            HealthVisitHistory();
                            break;*/

                        case 4:
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

        static void SelectServices()
        {
            List<string> queuedPatients = Load("queues.txt");

            while (true)
            {
                HeaderDisplay("SELECT SERVICES");
                Console.WriteLine("\n[1] Consultation");
                Console.WriteLine("[2] Vaccination");
                Console.WriteLine("[3] Maternal Care");
                Console.WriteLine("[4] Medicine Claim");
                Console.WriteLine("[5] Exit");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int servicesChoice))
                {
                    if (IsAlreadyQueued())
                    {
                        Console.WriteLine("\nYou are already in a queue.");
                        Console.ReadKey();
                        return;
                    }

                    switch (servicesChoice)
                    {
                        case 1:
                            consultationQueue.Enqueue(currentUser);
                            serviceCount["Consultation"]++;
                            queuedPatients.Add(QueueFormat(currentUser, "Consultation"));
                            Save("queues.txt", queuedPatients);
                            break;

                        case 2:
                            vaccinationQueue.Enqueue(currentUser);
                            serviceCount["Vaccination"]++;
                            queuedPatients.Add(QueueFormat(currentUser, "Vaccination"));
                            Save("queues.txt", queuedPatients);
                            break;

                        case 3:
                            maternalcareQueue.Enqueue(currentUser);
                            serviceCount["Maternal Care"]++;
                            queuedPatients.Add(QueueFormat(currentUser, "Maternal Care"));
                            Save("queues.txt", queuedPatients);
                            break;

                        case 4:
                            medicineclaimQueue.Enqueue(currentUser);
                            serviceCount["Medicine Claim"]++;
                            queuedPatients.Add(QueueFormat(currentUser, "Medicine Claim"));
                            Save("queues.txt", queuedPatients);
                            break;

                        case 5:
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

        static void HealthWorkerDashBoard()
        {
            while (true)
            {
                HeaderDisplay("HEALTH WORKER DASHBOARD");
                Console.WriteLine($"\nWelcome, {currentUser}");
                Console.WriteLine("\n[1] View Queue Board");
                Console.WriteLine("[2] Queue Handling");
                Console.WriteLine("[3] Priority Patient Tracking");
                Console.WriteLine("[4] Community Health Analytics");
                Console.WriteLine("[5] Logout");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int workerChoice))
                {
                    switch (workerChoice)
                    {
                        /*case 1:
                            ViewQueueBoard();
                            break;

                        case 2:
                            QueueHandling();
                            break;

                        case 3:
                            PriorityPatientTracking();
                            break;

                        case 4:
                            CommunityHealthAnalytics();
                            break;*/

                        case 5:
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

            if (Load("workers.txt").Count == 0)
            {
                DefaultWorkers();
            }
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

        static bool IsAlreadyQueued()
        {
            return consultationQueue.Contains(currentUser) ||
                   vaccinationQueue.Contains(currentUser) ||
                   maternalcareQueue.Contains(currentUser) ||
                   medicineclaimQueue.Contains(currentUser);
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

        static (string userName, string chosenService) Queue(string line)
        {
            string[] split = line.Split('|');
            return (split[0], split[1]);
        }

        static string QueueFormat(string userName, string chosenService)
        {
            return $"{userName}|{chosenService}";
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

        static void DefaultWorkers()
        {
            var defaultWorkers = new List<string>();
            defaultWorkers.Add(UserFormat("admin", "1234"));
            Save("workers.txt", defaultWorkers);
        }
    }
}
