using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Globalization;

namespace FINALS_APP_BARANGAYHEALTHCENTERSYSTEM
{
    internal class Program
    {
        static Queue<string> consultationRegularQueue = new Queue<string>();
        static Queue<string> consultationPriorityQueue = new Queue<string>();
        static Queue<string> vaccinationRegularQueue = new Queue<string>();
        static Queue<string> vaccinationPriorityQueue = new Queue<string>();
        static Queue<string> maternalcareRegularQueue = new Queue<string>();
        static Queue<string> maternalcarePriorityQueue = new Queue<string>();
        static Queue<string> medicineclaimRegularQueue = new Queue<string>();
        static Queue<string> medicineclaimPriorityQueue = new Queue<string>();
        static Stack<string> actionHistory = new Stack<string>();
        static Dictionary<string, int> serviceCount = new Dictionary<string, int>()
        {
            { "Consultation", 0 },
            { "Vaccination", 0 },
            { "Maternal Care", 0 },
            { "Medicine Claim", 0 }
        };
        static Dictionary<string, int> priorityStats = new Dictionary<string, int>()
        {
            { "Senior Citizen", 0 },
            { "PWD", 0 },
            { "Pregnant", 0 },
            { "Emergency", 0 }
        };
        static int[] hourlyPatients = new int[24];
        static string currentUser = "";
        static string currentConsultationPatient = "";
        static string currentVaccinationPatient = "";
        static string currentMaternalCarePatient = "";
        static string currentMedicineClaimPatient = "";

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
                            if (Login("patients.txt"))
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

                if (user.userName == userName && user.userPassword == userPassword)
                {
                    Console.WriteLine("Login Successful!");
                    currentUser = userName;
                    return true;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR]");
            Console.WriteLine("Account doesn't exist. Please try again or register an account");
            Console.ResetColor();
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
                            {
                                string patientType = PriorityType();
                                var schedule = GetSchedule();
                                JoinQueue(consultationRegularQueue,consultationPriorityQueue, "Consultation", patientType, schedule.date, schedule.time);

                                Console.WriteLine("\nSuccessfully added to queue.");
                                Console.ReadKey();
                                return;
                            }

                        case 2:
                            {
                                string patientType = PriorityType();
                                var schedule = GetSchedule();
                                JoinQueue(vaccinationRegularQueue, vaccinationPriorityQueue, "Vaccination", patientType, schedule.date, schedule.time);

                                Console.WriteLine("\nSuccessfully added to queue.");
                                Console.ReadKey();
                                return;
                            }

                        case 3:
                            {
                                string patientType = PriorityType();
                                var schedule = GetSchedule();
                                JoinQueue(maternalcareRegularQueue, maternalcarePriorityQueue, "Maternal Care", patientType, schedule.date, schedule.time);

                                Console.WriteLine("\nSuccessfully added to queue.");
                                Console.ReadKey();
                                return;
                            }

                        case 4:
                            {
                                string patientType = PriorityType();
                                var schedule = GetSchedule();
                                JoinQueue(medicineclaimRegularQueue, medicineclaimPriorityQueue, "Medicine Claim", patientType, schedule.date, schedule.time);

                                Console.WriteLine("\nSuccessfully added to queue.");
                                Console.ReadKey();
                                return;
                            }

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

        static int GetQueuePosition(Queue<string> queue, string userName)
        {
            List<string> record = queue.ToList();

            for (int i = 0; i < record.Count; i++)
            {
                if (Queue(record[i]).userName == userName)
                {
                    return i + 1;
                }
            }

            return -1;
        }

        static string GetCurrentService()
        {
            List<string> queuedPatients = Load("queues.txt");

            for(int i = 0; i < queuedPatients.Count; i++)
            {
                var patient = Queue(queuedPatients[i]);

                if (patient.userName == currentUser)
                {
                    return patient.chosenService;
                }
            }

            return "";
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
                        case 1:
                            ViewQueueBoard();
                            break;

                        case 2:
                            QueueHandling();
                            break;

                        /*case 3:
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

        static void ViewQueueBoard()
        {
            HeaderDisplay("QUEUE BOARD");
            QueueDisplayer("CONSULTATION QUEUE", consultationRegularQueue, consultationPriorityQueue);
            QueueDisplayer("VACCINATION QUEUE", vaccinationRegularQueue, vaccinationPriorityQueue);
            QueueDisplayer("MATERNAL CARE QUEUE", maternalcareRegularQueue, maternalcarePriorityQueue);
            QueueDisplayer("MEDICINE CLAIM QUEUE", medicineclaimRegularQueue, medicineclaimPriorityQueue);
        }

        static void QueueHandling()
        {
            while (true)
            {
                HeaderDisplay("QUEUE HANDLING");
                Console.WriteLine("\n[1] Call Next Patient");
                Console.WriteLine("[2] Complete Service");
                Console.WriteLine("[3] Skip Patient");
                Console.WriteLine("[4] Undo Last Action");
                Console.WriteLine("[5] Back");

                if (int.TryParse(Console.ReadLine(), out int queueChoice))
                {
                    switch (queueChoice)
                    {
                        case 1:
                            CallNextPatient();
                            break;

                        case 2:
                            CompleteService();
                            break;

                        /*case 3:
                            SkipPatient;
                            break;

                        case 4:
                            UndoLastAction();
                            break;*/

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

        static int ServiceHandling()
        {
            HeaderDisplay("SERVICE SELECTION");
            Console.WriteLine("\n[1] Consultation");
            Console.WriteLine("[2] Vaccination");
            Console.WriteLine("[3] Maternal Care");
            Console.WriteLine("[4] Medicine Claim");
            Console.Write("\nSelect Service: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }

            return -1;
        }

        static void CallNextPatient()
        {
            HeaderDisplay("CALL NEXT PATIENT");

            int service = ServiceHandling();

            switch (service)
            {
                case 1:
                    {
                        if(currentConsultationPatient != "")
                        {
                            Console.WriteLine("\nA patient is already being served.");
                            Console.ReadKey();
                            return;
                        }

                        currentConsultationPatient = PeekNextPatient(consultationRegularQueue, consultationPriorityQueue);

                        if (currentConsultationPatient == "")
                        {
                            Console.WriteLine("\nNo patients in queue.");
                            Console.ReadKey();
                            return;
                        }

                        var patient = Queue(currentConsultationPatient);

                        Console.WriteLine($"\nNow Serving: {patient.userName}");
                        break;
                    }

                case 2:
                    {
                        if(currentVaccinationPatient != "")
                        {
                            Console.WriteLine("\nA patient is already being served.");
                            Console.ReadKey();
                            return;
                        }

                        currentVaccinationPatient = PeekNextPatient(vaccinationRegularQueue, vaccinationPriorityQueue);

                        if (currentVaccinationPatient == "")
                        {
                            Console.WriteLine("\nNo patients in queue.");
                            Console.ReadKey();
                            return;
                        }

                        var patient = Queue(currentVaccinationPatient);

                        Console.WriteLine($"\nNow Serving: {patient.userName}");
                        break;
                    }

                case 3:
                    {
                        if(currentMaternalCarePatient != "")
                        {
                            Console.WriteLine("\nA patient is already being served.");
                            Console.ReadKey();
                            return;
                        }

                        currentMaternalCarePatient = PeekNextPatient(maternalcareRegularQueue, maternalcarePriorityQueue);

                        if (currentMaternalCarePatient == "")
                        {
                            Console.WriteLine("\nNo patients in queue.");
                            Console.ReadKey();
                            return;
                        }

                        var patient = Queue(currentMaternalCarePatient);

                        Console.WriteLine(
                            $"\nNow Serving: {patient.userName}");
                        break;
                    }

                case 4:
                    {
                        if(currentMedicineClaimPatient != "")
                        {
                            Console.WriteLine("\nA patient is already being served.");
                            Console.ReadKey();
                            return;
                        }

                        currentMedicineClaimPatient = PeekNextPatient(medicineclaimRegularQueue, medicineclaimPriorityQueue);

                        if (currentMedicineClaimPatient == "")
                        {
                            Console.WriteLine("\nNo patients in queue.");
                            Console.ReadKey();
                            return;
                        }

                        var patient = Queue(currentMedicineClaimPatient);

                        Console.WriteLine(
                            $"\nNow Serving: {patient.userName}");
                        break;
                    }

                default:
                    InvalidInput();
                    return;
            }

            Console.WriteLine("\nPatient has been called.");
            Console.ReadKey();
        }

        static void CompleteService()
        {
            HeaderDisplay("COMPLETE SERVICE");

            int service = ServiceHandling();

            switch(service)
            {
                case 1:
                    {
                        if (FinishService(consultationRegularQueue, consultationPriorityQueue, currentConsultationPatient))
                        {
                            currentConsultationPatient = "";
                        }

                        break;
                    }

                case 2:
                    {
                        if (FinishService(vaccinationRegularQueue, vaccinationRegularQueue, currentVaccinationPatient))
                        {
                            currentVaccinationPatient = "";
                        }

                        break;
                    }

                case 3:
                    {
                        if (FinishService(maternalcareRegularQueue, maternalcareRegularQueue, currentMaternalCarePatient))
                        {
                            currentMaternalCarePatient = "";
                        }

                        break;
                    }

                case 4:
                    {
                        if (FinishService(medicineclaimRegularQueue, medicineclaimRegularQueue, currentMedicineClaimPatient))
                        {
                            currentMedicineClaimPatient = "";
                        }

                        break;
                    }

                default:
                    InvalidInput();
                    break;
            }
        }

        static string SkipPatient(Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            if (priorityQueue.Count > 0)
            {
                string patient = priorityQueue.Dequeue();
                priorityQueue.Enqueue(patient);
                return patient;
            }

            if (regularQueue.Count > 0)
            {
                string patient = regularQueue.Dequeue();
                regularQueue.Enqueue(patient);
                return patient;
            }

            return "";
        }

        /*static string Skip
        static string SkipService()
        {
            HeaderDisplay("SKIP PATIENT");

            int service = ServiceHandling();

            switch (service)
            {
                case 1:
                    {
                        if (currentConsultationPatient == "")
                        {

                        }
                    }
            }

        }*/

        static bool FinishService(Queue<string> regularQueue, Queue<string> priorityQueue, string currentPatient)
        {
            if (currentPatient == "")
            {
                Console.WriteLine("\nNo patient is currently being served.");
                Console.ReadKey();
                return false;
            }

            string completedPatient = CompletePatient(regularQueue, priorityQueue);
            actionHistory.Push($"COMPLETE|{completedPatient}");

            List<string> visits = Load("visits.txt");
            visits.Add(completedPatient);
            Save("visits.txt", visits);

            Console.WriteLine("\nService completed successfully.");
            Console.ReadKey();
            return true;
        }

        static string PeekNextPatient(Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            if (priorityQueue.Count > 0)
            {
                return priorityQueue.Peek();
            }

            if (regularQueue.Count > 0)
            {
                return regularQueue.Peek();
            }

            return "";
        }

        static string CompletePatient(Queue<string> priorityQueue, Queue<string> regularQueue)
        {
            if (priorityQueue.Count > 0)
            {
                return priorityQueue.Dequeue();
            }

            if (regularQueue.Count > 0)
            {
                return regularQueue.Dequeue();
            }

            return "";
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

        static bool UserExistsInQueue(Queue<string> queue)
        {
            foreach (string record in queue)
            {
                if (Queue(record).userName == currentUser)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsAlreadyQueued()
        {
            return UserExistsInQueue(consultationPriorityQueue) || UserExistsInQueue(consultationRegularQueue) ||
                   UserExistsInQueue(vaccinationPriorityQueue) || UserExistsInQueue(vaccinationRegularQueue) ||
                   UserExistsInQueue(maternalcarePriorityQueue) || UserExistsInQueue(maternalcareRegularQueue) ||
                   UserExistsInQueue(medicineclaimPriorityQueue) || UserExistsInQueue(medicineclaimRegularQueue);
        }

        static string PriorityType()
        {
            while (true)
            {
                Console.WriteLine("\n=====Patient Type=====");
                Console.WriteLine("[1] Regular");
                Console.WriteLine("[2] Senior Citizen");
                Console.WriteLine("[3] PWD");
                Console.WriteLine("[4] Pregnant");
                Console.WriteLine("[5] Emergency");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int typeChoice))
                {
                    switch (typeChoice)
                    {
                        case 1:
                            return "Regular";

                        case 2:
                            return "Senior Citizen";

                        case 3:
                            return "PWD";

                        case 4:
                            return "Pregnant";

                        case 5:
                            return "Emergency";
                    }
                }

                InvalidInput();
            }
        }

        static (string date, string time) GetSchedule()
        {
            Console.Write("Enter Date (MM/DD/YY): "); string date = Console.ReadLine();
            Console.Write("Enter Time (HH:MM AM/PM): "); string time = Console.ReadLine();

            return (date, time);
        }

        static void JoinQueue(Queue<string> regularQueue, Queue<string> priorityQueue, string chosenService, string patientType, string date, string time)
        {
            List<string> queuedPatients = Load("queues.txt");
            string record = QueueFormat(currentUser, chosenService, patientType, date, time);

            if (patientType == "Regular")
            {
                regularQueue.Enqueue(record);
            }

            else
            {
                priorityQueue.Enqueue(record);
            }

            
            UpdateServiceCount(chosenService);
            UpdatePriorityStats(patientType);
            queuedPatients.Add(record);
            Save("queues.txt", queuedPatients);
        }

        static void UpdatePriorityStats(string patientType)
        {
            if (patientType != "Regular")
            {
                priorityStats[patientType]++;
            }
        }

        static void UpdateServiceCount(string service)
        {
            serviceCount[service]++;
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

        static (string userName, string chosenService, string patientType, string date, string time) Queue(string line)
        {
            string[] split = line.Split('|');
            return (split[0], split[1], split[2], split[3], split[4]);
        }

        static string QueueFormat(string userName, string chosenService, string patientType, string date, string time)
        {
            return $"{userName}|{chosenService}|{patientType}|{date}|{time}";
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

        static void QueueDisplayer(string title, Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine("------------------------------");

            int position = 1;

            Console.WriteLine("\nPRIORITY PATIENTS");

            if (priorityQueue.Count == 0) 
            {
                Console.WriteLine("NONE");
            }

            else
            {
                foreach (string record in priorityQueue)
                {
                    var patient = Queue(record);

                    Console.WriteLine($"{position}. {patient.userName} ({patient.patientType})");

                    position++;
                }
            }

            Console.WriteLine("\nREGULAR PATIENTS");

            if (regularQueue.Count == 0)
            {
                Console.WriteLine("NONE");
            }

            else
            {
                foreach (string record in regularQueue)
                {
                    var patient = Queue(record);

                    Console.WriteLine($"{position}. {patient.userName} ({patient.patientType})");

                    position++;
                }
            }
        }

        static void DefaultWorkers()
        {
            var defaultWorkers = new List<string>();
            defaultWorkers.Add(UserFormat("admin", "1234"));
            Save("workers.txt", defaultWorkers);
        }
    }
}
