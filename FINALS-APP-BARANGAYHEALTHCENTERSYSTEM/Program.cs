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
        //====================GLOBAL VARIABLES====================
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
        static int consultationCounter = 1;
        static int vaccinationCounter = 1;
        static int maternalcareCounter = 1;
        static int medicineclaimCounter = 1;

        //====================MAIN====================
        static void Main(string[] args)
        {
            InitializeFiles();
            LoadState();
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

        //====================PORTALS====================
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

        //====================DASHBOARDS====================
        static void PatientDashboard()
        {
            while (true)
            {
                HeaderDisplay("PATIENT DASHBOARD");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\nWelcome, {currentUser}");
                Console.ResetColor();
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

                        case 2:
                             QueueProgressTracker();
                             break;

                        case 3:
                             HealthVisitHistory();
                             break;

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

        static void HealthWorkerDashBoard()
        {
            while (true)
            {
                HeaderDisplay("HEALTH WORKER DASHBOARD");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"\nWelcome, {currentUser}");
                Console.ResetColor();
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

                        case 3:
                            PriorityPatientTracking();
                            break;

                        case 4:
                            CommunityHealthAnalytics();
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

        //====================PATIENT FEATURES====================
        static void SelectServices()
        {
            if (IsAlreadyQueued())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou are already in a queue.");
                Console.ReadKey();
                return;
            }

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
                    switch (servicesChoice)
                    {
                        case 1:
                            {
                                ProcessService("Consultation", consultationRegularQueue, consultationPriorityQueue);
                                return;
                            }

                        case 2:
                            {
                                ProcessService("Vaccination", vaccinationRegularQueue, vaccinationPriorityQueue);
                                return;
                            }

                        case 3:
                            {
                                ProcessService("Maternal Care", maternalcareRegularQueue, maternalcarePriorityQueue);
                                return;
                            }

                        case 4:
                            {
                                ProcessService("Medicine Claim", medicineclaimRegularQueue, medicineclaimPriorityQueue);
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

        static void QueueProgressTracker()
        {
            HeaderDisplay("QUEUE PROGRESS TRACKER");

            string foundRecord = "";
            int position = 0;
            string serviceType = "";

            var allQueues = new (string service, Queue<string> priority, Queue<string> regular)[]
            {
            ("Consultation",   consultationPriorityQueue,   consultationRegularQueue),
            ("Vaccination",    vaccinationPriorityQueue,    vaccinationRegularQueue),
            ("Maternal Care",  maternalcarePriorityQueue,   maternalcareRegularQueue),
            ("Medicine Claim", medicineclaimPriorityQueue,  medicineclaimRegularQueue),
            };

            foreach (var (service, priority, regular) in allQueues)
            {
                int pos = 1;

                foreach (string record in priority)
                {
                    if (Queue(record).userName == currentUser)
                    {
                        foundRecord = record;
                        position = pos;
                        serviceType = service;
                        break;
                    }
                    pos++;
                }

                if (foundRecord != "") break;

                foreach (string record in regular)
                {
                    if (Queue(record).userName == currentUser)
                    {
                        foundRecord = record;
                        position = pos;
                        serviceType = service;
                        break;
                    }
                    pos++;
                }

                if (foundRecord != "") break;
            }

            if (foundRecord == "")
            {
                Console.WriteLine("\nYou are not currently in any queue.");
                Console.ReadKey();
                return;
            }

            var patient = Queue(foundRecord);
            int totalInQueue = GetTotalInQueue(serviceType);

            Console.WriteLine($"\nPatient       : {patient.userName}");
            Console.WriteLine($"Queue Number  : {patient.queueNumber}");
            Console.WriteLine($"Service       : {patient.chosenService}");
            Console.WriteLine($"Patient Type  : {patient.patientType}");
            Console.WriteLine($"Schedule      : {patient.date} {patient.time}");
            Console.WriteLine($"Position      : {position} of {totalInQueue}");

            Console.WriteLine();

            if (position == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(">> You are next in line! Please proceed to the counter.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($">> There are {position - 1} patient(s) ahead of you.");
                Console.ResetColor();
            }

            Console.ReadKey();
        }

        static void HealthVisitHistory()
        {
            HeaderDisplay("HEALTH VISIT HISTORY");

            List<string> visits = Load("visits.txt");
            List<string> patientVisits = new List<string>();

            foreach (string line in visits)
            {
                var visit = Visit(line);
                if (visit.userName == currentUser)
                {
                    patientVisits.Add(line);
                }
            }

            if (patientVisits.Count == 0)
            {
                Console.WriteLine("\nNo visit history found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nVisit History for: {currentUser}");
            Console.WriteLine($"Total Visits: {patientVisits.Count}");
            Console.WriteLine("\n------------------------------");

            int count = 1;
            foreach (string line in patientVisits)
            {
                var visit = Visit(line);

                Console.WriteLine($"\nVisit #{count}");
                Console.WriteLine($"  Queue Number  : {visit.queueNumber}");
                Console.WriteLine($"  Service       : {visit.chosenService}");
                Console.WriteLine($"  Patient Type  : {visit.patientType}");
                Console.WriteLine($"  Scheduled     : {visit.date} {visit.time}");
                Console.WriteLine($"  Serviced By   : {visit.workerName}");
                Console.WriteLine("  ------------------------------");

                count++;
            }

            Console.ReadKey();
        }

        //====================PATIENT HELPERS====================
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

        static string JoinQueue(Queue<string> regularQueue, Queue<string> priorityQueue, string chosenService, string patientType, string date, string time)
        {
            List<string> queuedPatients = Load("queues.txt");
            string queueNumber = GenerateQueueNumber(chosenService);
            string record = QueueFormat(queueNumber, currentUser, chosenService, patientType, date, time);

            if (patientType == "Regular")
            {
                regularQueue.Enqueue(record);
            }

            else
            {
                priorityQueue.Enqueue(record);
            }

            queuedPatients.Add(record);
            Save("queues.txt", queuedPatients);
            SaveCounters();

            return queueNumber;
        }

        static string GenerateQueueNumber(string service)
        {
            switch (service)
            {
                case "Consultation":
                    return $"C-{NumberFormat(consultationCounter++)}";

                case "Vaccination":
                    return $"V-{NumberFormat(vaccinationCounter++)}";

                case "Maternal Care":
                    return $"MAC-{NumberFormat(maternalcareCounter++)}";

                case "Medicine Claim":
                    return $"MEC-{NumberFormat(medicineclaimCounter++)}";
            }

            return "";
        }

        static int GetTotalInQueue(string service)
        {
            switch (service)
            {
                case "Consultation": return consultationPriorityQueue.Count + consultationRegularQueue.Count;
                case "Vaccination": return vaccinationPriorityQueue.Count + vaccinationRegularQueue.Count;
                case "Maternal Care": return maternalcarePriorityQueue.Count + maternalcareRegularQueue.Count;
                case "Medicine Claim": return medicineclaimPriorityQueue.Count + medicineclaimRegularQueue.Count;
                default: return 0;
            }
        }

        static void ProcessService(string chosenService, Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            string patientType = PriorityType();
            var schedule = GetSchedule();
            string queueNumber = JoinQueue(regularQueue, priorityQueue, chosenService, patientType, schedule.date, schedule.time);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSuccessfully added to queue.");
            Console.ResetColor();
            Console.WriteLine($"Your Queue Number: {queueNumber}");
            Console.ReadKey();
        }

        //====================HEALTH WORKER FEATURES====================
        static void ViewQueueBoard()
        {
            HeaderDisplay("QUEUE BOARD");
            QueueDisplayer("CONSULTATION QUEUE", consultationRegularQueue, consultationPriorityQueue);
            QueueDisplayer("VACCINATION QUEUE", vaccinationRegularQueue, vaccinationPriorityQueue);
            QueueDisplayer("MATERNAL CARE QUEUE", maternalcareRegularQueue, maternalcarePriorityQueue);
            QueueDisplayer("MEDICINE CLAIM QUEUE", medicineclaimRegularQueue, medicineclaimPriorityQueue);

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey();
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

                Console.Write("\nEnter your choice: ");

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

                        case 3:
                            SkipService();
                            break;

                        case 4:
                            UndoLastAction();
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

        static void CallNextPatient()
        {
            HeaderDisplay("CALL NEXT PATIENT");

            int service = ServiceHandling();

            switch (service)
            {
                case 1:
                    {
                        if (currentConsultationPatient != "")
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
                        if (currentVaccinationPatient != "")
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
                        if (currentMaternalCarePatient != "")
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
                        if (currentMedicineClaimPatient != "")
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

        static void SkipService()
        {
            HeaderDisplay("SKIP PATIENT");

            int service = ServiceHandling();
            string skippedPatient = "";

            switch (service)
            {
                case 1:
                    {
                        if (currentConsultationPatient == "")
                        {
                            Console.WriteLine("\nNo patient is currently being served.");
                            Console.ReadKey();
                            return;
                        }

                        skippedPatient = SkipPatient(consultationRegularQueue, consultationPriorityQueue);
                        actionHistory.Push($"SKIP|{skippedPatient}");

                        List<string> actions = Load("actions.txt");
                        actions.Add($"SKIP|{skippedPatient}");
                        Save("actions.txt", actions);

                        currentConsultationPatient = "";

                        Console.WriteLine("\nPatient skipped.");
                        Console.ReadKey();
                        break;
                    }

                case 2:
                    {
                        if (currentVaccinationPatient == "")
                        {
                            Console.WriteLine("\nNo patient is currently being served.");
                            Console.ReadKey();
                            return;
                        }

                        skippedPatient = SkipPatient(vaccinationRegularQueue, vaccinationPriorityQueue);
                        actionHistory.Push($"SKIP|{skippedPatient}");

                        List<string> actions = Load("actions.txt");
                        actions.Add($"SKIP|{skippedPatient}");
                        Save("actions.txt", actions);

                        currentVaccinationPatient = "";

                        Console.WriteLine("\nPatient skipped.");
                        Console.ReadKey();
                        break;
                    }

                case 3:
                    {
                        if (currentMaternalCarePatient == "")
                        {
                            Console.WriteLine("\nNo patient is currently being served.");
                            Console.ReadKey();
                            return;
                        }

                        skippedPatient = SkipPatient(maternalcareRegularQueue, maternalcarePriorityQueue);
                        actionHistory.Push($"SKIP|{skippedPatient}");

                        List<string> actions = Load("actions.txt");
                        actions.Add($"SKIP|{skippedPatient}");
                        Save("actions.txt", actions);

                        currentMaternalCarePatient = "";

                        Console.WriteLine("\nPatient skipped.");
                        Console.ReadKey();
                        break;
                    }

                case 4:
                    {
                        if (currentMedicineClaimPatient == "")
                        {
                            Console.WriteLine("\nNo patient is currently being served.");
                            Console.ReadKey();
                            return;
                        }

                        skippedPatient = SkipPatient(medicineclaimRegularQueue, medicineclaimPriorityQueue);
                        actionHistory.Push($"SKIP|{skippedPatient}");

                        List<string> actions = Load("actions.txt");
                        actions.Add($"SKIP|{skippedPatient}");
                        Save("actions.txt", actions);

                        currentMedicineClaimPatient = "";

                        Console.WriteLine("\nPatient skipped.");
                        Console.ReadKey();
                        break;
                    }

                default:
                    InvalidInput();
                    return;
            }
        }

        static void UndoLastAction()
        {
            HeaderDisplay("UNDO LAST ACTION");

            if (actionHistory.Count == 0)
            {
                Console.WriteLine("\nNo actions to undo.");
                Console.ReadKey();
                return;
            }

            string lastAction = actionHistory.Peek();

            Console.WriteLine($"\nLast Action:");
            Console.WriteLine(lastAction);
            Console.Write("Undo? (Y/N): "); string undo = Console.ReadLine().ToLower();

            if (undo == "y")
            {
                string action = actionHistory.Pop();

                List<string> actions = Load("actions.txt");

                if (actions.Count > 0)
                {
                    actions.RemoveAt(actions.Count - 1);
                }

                Save("actions.txt", actions);

                string[] split = action.Split('|');
                string actionType = split[0];
                string record = $"{split[0]},{split[1]}|{split[2]}|{split[3]}|{split[4]}|{split[5]}|{split[6]}";

                switch (actionType)
                {
                    case "COMPLETE":

                        RestorePatient(record);
                        List<string> queuedPatients = Load("queues.txt");

                        queuedPatients.Add(record);

                        Save("queues.txt", queuedPatients);

                        RemoveVisitRecord(record);

                        Console.WriteLine("\nCompleted service has been restored.");
                        break;

                    case "SKIP":
                        Console.WriteLine("\nSkip action removed from history.");
                        break;
                }
            }

            else if (undo == "n")
            {
                return;
            }

            else
            {
                InvalidInput();
            }

            Console.ReadKey();
        }

        static void PriorityPatientTracking()
        {
            HeaderDisplay("PRIORITY PATIENT TRACKING");

            int totalPriority = 0;

            foreach (var kvp in priorityStats)
            {
                totalPriority += kvp.Value;
            }

            Console.WriteLine($"\nTotal Priority Patients Serviced: {totalPriority}");
            Console.WriteLine("\n------------------------------");
            Console.WriteLine("\nBreakdown by Type:");

            Console.WriteLine($"\n  Senior Citizen : {priorityStats["Senior Citizen"]}");
            Console.WriteLine($"  PWD            : {priorityStats["PWD"]}");
            Console.WriteLine($"  Pregnant       : {priorityStats["Pregnant"]}");
            Console.WriteLine($"  Emergency      : {priorityStats["Emergency"]}");

            Console.WriteLine("\n------------------------------");
            Console.WriteLine("\nCurrently Waiting Priority Patients:");

            var allQueues = new (string service, Queue<string> priority)[]
            {
            ("Consultation",   consultationPriorityQueue),
            ("Vaccination",    vaccinationPriorityQueue),
            ("Maternal Care",  maternalcarePriorityQueue),
            ("Medicine Claim", medicineclaimPriorityQueue),
            };

            bool anyWaiting = false;

            foreach (var (service, priority) in allQueues)
            {
                if (priority.Count > 0)
                {
                    Console.WriteLine($"\n  {service}:");

                    int pos = 1;
                    foreach (string record in priority)
                    {
                        var patient = Queue(record);
                        Console.WriteLine($"    {pos}. [{patient.queueNumber}] {patient.userName} ({patient.patientType})");
                        pos++;
                    }

                    anyWaiting = true;
                }
            }

            if (!anyWaiting)
            {
                Console.WriteLine("\n  No priority patients currently waiting.");
            }

            Console.ReadKey();
        }

        static void CommunityHealthAnalytics()
        {
            while (true)
            {
                HeaderDisplay("COMMUNITY HEALTH ANALYTICS");
                Console.WriteLine("\n[1] Service Trends");
                Console.WriteLine("[2] Most Requested Service");
                Console.WriteLine("[3] Peak Hours");
                Console.WriteLine("[4] Back");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ServiceTrends();
                            break;

                        case 2:
                            MostRequestedService();
                            break;

                        case 3:
                            PeakHours();
                            break;

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

        //====================HEALTH WORKER HELPERS====================
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

        static void CompleteService()
        {
            HeaderDisplay("COMPLETE SERVICE");

            int service = ServiceHandling();

            switch (service)
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
                        if (FinishService(vaccinationRegularQueue, vaccinationPriorityQueue, currentVaccinationPatient))
                        {
                            currentVaccinationPatient = "";
                        }

                        break;
                    }

                case 3:
                    {
                        if (FinishService(maternalcareRegularQueue, maternalcarePriorityQueue, currentMaternalCarePatient))
                        {
                            currentMaternalCarePatient = "";
                        }

                        break;
                    }

                case 4:
                    {
                        if (FinishService(medicineclaimRegularQueue, medicineclaimPriorityQueue, currentMedicineClaimPatient))
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

        static void RestorePatient(string record)
        {
            var patient = Queue(record);

            if (patient.patientType == "Regular")
            {
                switch (patient.chosenService)
                {
                    case "Consultation":
                        consultationRegularQueue.Enqueue(record);
                        break;

                    case "Vaccination":
                        vaccinationRegularQueue.Enqueue(record);
                        break;

                    case "Maternal Care":
                        maternalcareRegularQueue.Enqueue(record);
                        break;

                    case "Medicine Claim":
                        medicineclaimRegularQueue.Enqueue(record);
                        break;
                }
            }
            else
            {
                switch (patient.chosenService)
                {
                    case "Consultation":
                        consultationPriorityQueue.Enqueue(record);
                        break;

                    case "Vaccination":
                        vaccinationPriorityQueue.Enqueue(record);
                        break;

                    case "Maternal Care":
                        maternalcarePriorityQueue.Enqueue(record);
                        break;

                    case "Medicine Claim":
                        medicineclaimPriorityQueue.Enqueue(record);
                        break;
                }
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

        static bool FinishService(Queue<string> regularQueue, Queue<string> priorityQueue, string currentPatient)
        {
            if (currentPatient == "")
            {
                Console.WriteLine("\nNo patient is currently being served.");
                Console.ReadKey();
                return false;
            }

            string completedPatient = CompletePatient(regularQueue, priorityQueue);
            List<string> queuedPatients = Load("queues.txt");

            queuedPatients.Remove(completedPatient);

            Save("queues.txt", queuedPatients);

            actionHistory.Push($"COMPLETE|{completedPatient}");

            var patient = Queue(completedPatient);
            UpdateServiceCount(patient.chosenService);
            UpdatePriorityStats(patient.patientType);
            UpdateHourlyPatients(patient.time);

            List<string> actions = Load("actions.txt");
            actions.Add($"COMPLETE|{completedPatient}");
            Save("actions.txt", actions);

            List<string> visits = Load("visits.txt");
            visits.Add(VisitFormat(completedPatient, currentUser));
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

        static string CompletePatient(Queue<string> regularQueue, Queue<string> priorityQueue)
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

        static void RemoveVisitRecord(string record)
        {
            List<string> visits = Load("visits.txt");

            visits.Remove(record);

            Save("visits.txt", visits);
        }

        static void ServiceTrends()
        {
            HeaderDisplay("SERVICE TRENDS");

            int total = 0;
            foreach (var kvp in serviceCount)
                total += kvp.Value;

            int maxCount = serviceCount.Values.Max();

            Console.WriteLine($"\nTotal Patients Serviced: {total}");
            Console.WriteLine("\n------------------------------");

            foreach (var kvp in serviceCount)
            {
                int count = kvp.Value;
                double percent = total > 0 ? (double)count / total * 100 : 0;
                string bar = BuildBar(count, maxCount, 20);

                Console.WriteLine($"\n  {kvp.Key,-15}: {count,4} patients  [{bar}] {percent:0.0}%");
            }

            Console.ReadKey();
        }

        static void MostRequestedService()
        {
            HeaderDisplay("MOST REQUESTED SERVICE");

            int total = 0;
            foreach (var kvp in serviceCount)
                total += kvp.Value;

            if (total == 0)
            {
                Console.WriteLine("\nNo service data available yet.");
                Console.ReadKey();
                return;
            }

            var ranked = serviceCount.OrderByDescending(kvp => kvp.Value).ToList();

            Console.WriteLine("\nService Ranking:");
            Console.WriteLine("\n------------------------------");

            int rank = 1;
            foreach (var kvp in ranked)
            {
                double percent = (double)kvp.Value / total * 100;
                string medal = rank == 1 ? " <<" : "";

                Console.WriteLine($"\n  #{rank} {kvp.Key,-15}: {kvp.Value,4} patients  ({percent:0.0}%){medal}");
                rank++;
            }

            Console.WriteLine("\n------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  Most Requested: {ranked[0].Key} ({ranked[0].Value} patients)");
            Console.ResetColor();

            Console.ReadKey();
        }

        static void PeakHours()
        {
            HeaderDisplay("PEAK HOURS");

            int totalPatients = hourlyPatients.Sum();

            if (totalPatients == 0)
            {
                Console.WriteLine("\nNo hourly data available yet.");
                Console.ReadKey();
                return;
            }

            int peakHour = 0;
            int peakCount = 0;
            for (int i = 0; i < hourlyPatients.Length; i++)
            {
                if (hourlyPatients[i] > peakCount)
                {
                    peakCount = hourlyPatients[i];
                    peakHour = i;
                }
            }

            Console.WriteLine("\nPatients per Hour:");
            Console.WriteLine("\n------------------------------");

            for (int i = 0; i < hourlyPatients.Length; i++)
            {
                if (hourlyPatients[i] == 0) continue;

                string label = DateTime.Today.AddHours(i).ToString("hh:00 tt");
                string bar = BuildBar(hourlyPatients[i], peakCount, 20);
                bool isPeak = i == peakHour;

                if (isPeak) Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {label} : {hourlyPatients[i],4} patients  [{bar}]{(isPeak ? " << PEAK" : "")}");
                Console.ResetColor();
            }

            Console.WriteLine("\n------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  Peak Hour: {DateTime.Today.AddHours(peakHour):hh:00 tt} ({peakCount} patients)");
            Console.ResetColor();

            Console.ReadKey();
        }

        static string BuildBar(int value, int max, int width)
        {
            if (max == 0) return new string('-', width);
            int filled = (int)((double)value / max * width);
            return new string('█', filled) + new string('░', width - filled);
        }

        static void UpdatePriorityStats(string patientType)
        {
            if (patientType != "Regular")
            {
                priorityStats[patientType]++;
                SaveStats();
            }
        }

        static void UpdateServiceCount(string service)
        {
            serviceCount[service]++;
            SaveStats();
        }

        static void UpdateHourlyPatients(string time)
        {
            if (DateTime.TryParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
            {
                hourlyPatients[parsed.Hour]++;
                SaveHourlyPatients();
            }
        }

        //====================GLOBAL HELPERS====================
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nName already exists.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.Write("Enter your Password: "); string userPassword = Console.ReadLine();

            users.Add(UserFormat(userName, userPassword));
            Save(fileName, users);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUser registered successfully.");
            Console.ResetColor();
            Console.ReadKey();
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
            CreateFileIfNotExisting("actions.txt");
            CreateFileIfNotExisting("counters.txt");
            CreateFileIfNotExisting("stats.txt");
            CreateFileIfNotExisting("hourly.txt");

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

        static void LoadState()
        {
            List<string> queuedPatients = Load("queues.txt");

            foreach (string record in queuedPatients)
            {
                var patient = Queue(record);
                bool isPriority = patient.patientType != "Regular";

                switch (patient.chosenService)
                {
                    case "Consultation":
                        if (isPriority) consultationPriorityQueue.Enqueue(record);
                        else consultationRegularQueue.Enqueue(record);
                        break;
                    case "Vaccination":
                        if (isPriority) vaccinationPriorityQueue.Enqueue(record);
                        else vaccinationRegularQueue.Enqueue(record);
                        break;
                    case "Maternal Care":
                        if (isPriority) maternalcarePriorityQueue.Enqueue(record);
                        else maternalcareRegularQueue.Enqueue(record);
                        break;
                    case "Medicine Claim":
                        if (isPriority) medicineclaimPriorityQueue.Enqueue(record);
                        else medicineclaimRegularQueue.Enqueue(record);
                        break;
                }
            }

            List<string> actions = Load("actions.txt");
            foreach (string action in actions)
            {
                actionHistory.Push(action);
            }

            List<string> counters = Load("counters.txt");
            foreach (string line in counters)
            {
                string[] split = line.Split('|');
                if (split.Length != 2) continue;
                if (!int.TryParse(split[1], out int val)) continue;

                switch (split[0])
                {
                    case "Consultation": consultationCounter = val; break;
                    case "Vaccination": vaccinationCounter = val; break;
                    case "MaternalCare": maternalcareCounter = val; break;
                    case "MedicineClaim": medicineclaimCounter = val; break;
                }
            }

            List<string> stats = Load("stats.txt");
            foreach (string line in stats)
            {
                string[] split = line.Split('|');
                if (split.Length != 2) continue;
                if (!int.TryParse(split[1], out int val)) continue;

                if (serviceCount.ContainsKey(split[0]))
                    serviceCount[split[0]] = val;
                else if (priorityStats.ContainsKey(split[0]))
                    priorityStats[split[0]] = val;
            }

            List<string> hourly = Load("hourly.txt");
            foreach (string line in hourly)
            {
                string[] split = line.Split('|');
                if (split.Length != 2) continue;
                if (!int.TryParse(split[0], out int hour)) continue;
                if (!int.TryParse(split[1], out int count)) continue;
                hourlyPatients[hour] = count;
            }
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

        static void SaveCounters()
        {
            var lines = new List<string>
    {
        $"Consultation|{consultationCounter}",
        $"Vaccination|{vaccinationCounter}",
        $"MaternalCare|{maternalcareCounter}",
        $"MedicineClaim|{medicineclaimCounter}"
    };
            Save("counters.txt", lines);
        }

        static void SaveStats()
        {
            var lines = new List<string>();
            foreach (var kvp in serviceCount)
                lines.Add($"{kvp.Key}|{kvp.Value}");
            foreach (var kvp in priorityStats)
                lines.Add($"{kvp.Key}|{kvp.Value}");
            Save("stats.txt", lines);
        }

        static void SaveHourlyPatients()
        {
            var lines = new List<string>();
            for (int i = 0; i < hourlyPatients.Length; i++)
            {
                lines.Add($"{i}|{hourlyPatients[i]}");
            }
            Save("hourly.txt", lines);
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

        static (string queueNumber, string userName, string chosenService, string patientType, string date, string time) Queue(string line)
        {
            string[] split = line.Split('|');
            return (split[0], split[1], split[2], split[3], split[4], split[5]);
        }

        static string QueueFormat(string queueNumber, string userName, string chosenService, string patientType, string date, string time)
        {
            return $"{queueNumber}|{userName}|{chosenService}|{patientType}|{date}|{time}";
        }

        static string VisitFormat(string queueRecord, string workerName)
        {
            return $"{queueRecord}|{workerName}";
        }

        static (string queueNumber, string userName, string chosenService, string patientType, string date, string time, string workerName) Visit(string line)
        {
            string[] split = line.Split('|');
            return (split[0], split[1], split[2], split[3], split[4], split[5], split[6]);
        }

        static string NumberFormat(int number)
        {
            string num = number.ToString();

            while (num.Length < 3)
            {
                num = "0" + num;
            }

            return num;
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
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n\n{title}");
            Console.ResetColor();
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
                    Console.WriteLine($"{position}. [{patient.queueNumber}] {patient.userName} ({patient.patientType})");
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
                    Console.WriteLine($"{position}. [{patient.queueNumber}] {patient.userName} ({patient.patientType})");
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
