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
        static Stack<string> servicedPatients = new Stack<string>();
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
                ╦ ╦┌─┐┌─┐┬ ┌┬┐┬ ┬╦  ┬┌┐┌┌─┐  ╔═╗╦ ╦
                ╠═╣├┤ ├─┤│  │ ├─┤║  ││││├┤   ╠═╝╠═╣
                ╩ ╩└─┘┴ ┴┴─┘┴ ┴ ┴╩═╝┴┘└┘└─┘  ╩  ╩ ╩
");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("          Barangay Health Center Queue Management System");
                Console.ResetColor();

                Console.WriteLine();
                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  Select your role to continue");
                Console.ResetColor();
                Console.WriteLine();

                PrintMenuItem("1", "Patient");
                PrintMenuItem("2", "Health Worker");
                PrintMenuItem("3", "Exit");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n  Welcome! Please login or create an account.\n");
                Console.ResetColor();

                PrintMenuItem("1", "Login");
                PrintMenuItem("2", "Register");
                PrintMenuItem("3", "Back");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n  Staff access only. Please login to continue.\n");
                Console.ResetColor();

                PrintMenuItem("1", "Login");
                PrintMenuItem("2", "Register");
                PrintMenuItem("3", "Back");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("  Logged in as  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  {currentUser}");
                Console.ResetColor();
                Console.WriteLine();
                Divider();
                Console.WriteLine();

                PrintMenuItem("1", "Select Services");
                PrintMenuItem("2", "Queue Progress Tracker");
                PrintMenuItem("3", "Health Visit History");
                PrintMenuItem("4", "Logout");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("  Logged in as  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  {currentUser}");
                Console.ResetColor();
                Console.WriteLine();
                Divider();
                Console.WriteLine();

                PrintMenuItem("1", "View Queue Board");
                PrintMenuItem("2", "Queue Handling");
                PrintMenuItem("3", "Priority Patient Tracking");
                PrintMenuItem("4", "Community Health Analytics");
                PrintMenuItem("5", "Logout");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                PrintError("You are already in a queue.");
                PressAnyKey();
                return;
            }

            while (true)
            {
                HeaderDisplay("SELECT SERVICES");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n  Choose a service to queue in for:\n");
                Console.ResetColor();

                PrintMenuItem("1", "Consultation");
                PrintMenuItem("2", "Vaccination");
                PrintMenuItem("3", "Maternal Care");
                PrintMenuItem("4", "Medicine Claim");
                PrintMenuItem("5", "Back");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
                PrintAlert("You are not currently in any queue.");
                PressAnyKey();
                return; ;
            }

            var patient = Queue(foundRecord);
            int totalInQueue = GetTotalInQueue(serviceType);

            SectionHeader("YOUR QUEUE DETAILS");
            Console.WriteLine();

            PrintField("Patient", patient.userName);
            PrintField("Queue Number", patient.queueNumber, ConsoleColor.Cyan);
            PrintField("Service", patient.chosenService, ConsoleColor.Yellow);
            PrintField("Patient Type", patient.patientType);
            PrintField("Schedule", $"{patient.date}  {patient.time}");
            PrintField("Position", $"{position} of {totalInQueue}");

            Console.WriteLine();
            Divider();

            if (position == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(">> You are next in line! Please proceed to the counter.");
                Console.ResetColor();
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($">> There is/are {position - 1} patient(s) ahead of you.");
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
                PrintAlert("No visit history found.");
                PressAnyKey();
                return;
            }

            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Patient");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  {currentUser}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("    Total Visits");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  {patientVisits.Count}");
            Divider();

            int count = 1;
            foreach (string line in patientVisits)
            {
                var visit = Visit(line);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  Visit #{count}");
                Console.ResetColor();

                PrintField("Queue Number", visit.queueNumber, ConsoleColor.Cyan);
                PrintField("Service", visit.chosenService, ConsoleColor.Yellow);
                PrintField("Patient Type", visit.patientType);
                PrintField("Scheduled", $"{visit.date}  {visit.time}");
                PrintField("Serviced By", visit.workerName, ConsoleColor.Green);

                Divider();
                count++;
            }

            PressAnyKey();
        }

        //====================PATIENT HELPERS====================
        static string PriorityType()
        {
            while (true)
            {
                SectionHeader("SELECT PATIENT TYPE");
                Console.WriteLine();

                PrintMenuItem("1", "Regular");
                PrintMenuItem("2", "Senior Citizen");
                PrintMenuItem("3", "PWD");
                PrintMenuItem("4", "Pregnant");
                PrintMenuItem("5", "Emergency");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
            SectionHeader("SCHEDULE");

            string date = "";
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Date (MM/DD/YY)    : ");
                Console.ForegroundColor = ConsoleColor.White;
                date = Console.ReadLine();
                Console.ResetColor();

                if (DateTime.TryParse(date, out DateTime result))
                {
                    break;
                }

                PrintError("Invalid date. Please use MM/DD/YY (e.g. 06/28/26).");
            }


            string time = "";
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("  Time (HH:MM AM/PM) : ");
                Console.ForegroundColor = ConsoleColor.White;
                time = Console.ReadLine();
                Console.ResetColor();

                if (DateTime.TryParse(time, out DateTime result))
                {
                    break;
                }

                PrintError("Invalid time. Please use HH:MM AM/PM (e.g. 09:30 AM).");
            }

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
                    return $"CO-{NumberFormat(consultationCounter++)}";

                case "Vaccination":
                    return $"VA-{NumberFormat(vaccinationCounter++)}";

                case "Maternal Care":
                    return $"MA-{NumberFormat(maternalcareCounter++)}";

                case "Medicine Claim":
                    return $"ME-{NumberFormat(medicineclaimCounter++)}";
            }

            return "";
        }

        static int GetTotalInQueue(string service)
        {
            switch (service)
            {
                case "Consultation": 
                    return consultationPriorityQueue.Count + consultationRegularQueue.Count;

                case "Vaccination": 
                    return vaccinationPriorityQueue.Count + vaccinationRegularQueue.Count;

                case "Maternal Care": 
                    return maternalcarePriorityQueue.Count + maternalcareRegularQueue.Count;

                case "Medicine Claim": 
                    return medicineclaimPriorityQueue.Count + medicineclaimRegularQueue.Count;

                default: 
                    return 0;
            }
        }

        static void ProcessService(string chosenService, Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            string patientType = PriorityType();
            var schedule = GetSchedule();
            string queueNumber = JoinQueue(regularQueue, priorityQueue, chosenService, patientType, schedule.date, schedule.time);

            Console.WriteLine();
            Divider();
            PrintSuccess("Successfully added to queue!");
            Console.WriteLine();
            PrintField("Service", chosenService, ConsoleColor.Yellow);
            PrintField("Patient Type", patientType);
            PrintField("Queue Number", queueNumber, ConsoleColor.Cyan);
            Console.WriteLine();
            Divider();

            PressAnyKey();
        }

        //====================HEALTH WORKER FEATURES====================
        static void ViewQueueBoard()
        {
            HeaderDisplay("QUEUE BOARD");
            QueueDisplay("CONSULTATION QUEUE", consultationRegularQueue, consultationPriorityQueue);
            QueueDisplay("VACCINATION QUEUE", vaccinationRegularQueue, vaccinationPriorityQueue);
            QueueDisplay("MATERNAL CARE QUEUE", maternalcareRegularQueue, maternalcarePriorityQueue);
            QueueDisplay("MEDICINE CLAIM QUEUE", medicineclaimRegularQueue, medicineclaimPriorityQueue);

            PressAnyKey();
        }

        static void QueueHandling()
        {
            while (true)
            {
                HeaderDisplay("QUEUE HANDLING");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n  Manage patient queues across all services.\n");
                Console.ResetColor();

                PrintMenuItem("1", "Call Next Patient");
                PrintMenuItem("2", "Serve Patient");
                PrintMenuItem("3", "Skip Patient");
                PrintMenuItem("4", "View Completed Patients");
                PrintMenuItem("5", "Back");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

                if (int.TryParse(Console.ReadLine(), out int queueChoice))
                {
                    switch (queueChoice)
                    {
                        case 1:
                            CallNextPatient();
                            break;

                        case 2:
                            ServePatient();
                            break;

                        case 3:
                            SkipService();
                            break;

                        case 4:
                            ViewServicedPatients();
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
            string serving = "";

            switch (service)
            {
                case 1:
                    {
                        if (currentConsultationPatient != "")
                        {
                            PrintAlert("A patient is already being served.");
                            PressAnyKey();
                            return;
                        }

                        string next = PeekNextPatient(consultationRegularQueue, consultationPriorityQueue);

                        if (next == "")
                        {
                            PrintAlert("No patients in queue.");
                            PressAnyKey();
                            return;
                        }

                        Console.Clear();
                        QueueDisplay("CONSULTATION QUEUE", consultationRegularQueue, consultationPriorityQueue);

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"\n  Call {Queue(next).userName}? (Y/N): ");
                            Console.ForegroundColor = ConsoleColor.White;
                            string confirm = Console.ReadLine().ToLower();
                            Console.ResetColor();

                            if (confirm == "y")
                            {
                                currentConsultationPatient = next;
                                serving = Queue(currentConsultationPatient).userName;
                                break;
                            }
                            else if (confirm == "n")
                            {
                                return;
                            }
                            else
                            {
                                InvalidInput();
                                Console.Clear();
                                QueueDisplay("CONSULTATION QUEUE", consultationRegularQueue, consultationPriorityQueue);
                            }
                        }

                        break;
                    }

                case 2:
                    {
                        if (currentVaccinationPatient != "")
                        {
                            PrintAlert("A patient is already being served.");
                            PressAnyKey();
                            return;
                        }

                        string next = PeekNextPatient(vaccinationRegularQueue, vaccinationPriorityQueue);

                        if (next == "")
                        {
                            PrintAlert("No patients in queue.");
                            PressAnyKey();
                            return;
                        }

                        Console.Clear();
                        QueueDisplay("VACCINATION QUEUE", vaccinationRegularQueue, vaccinationPriorityQueue);

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"\n  Call {Queue(next).userName}? (Y/N): ");
                            Console.ForegroundColor = ConsoleColor.White;
                            string confirm = Console.ReadLine().ToLower();
                            Console.ResetColor();

                            if (confirm == "y")
                            {
                                currentVaccinationPatient = next;
                                serving = Queue(currentVaccinationPatient).userName;
                                break;
                            }
                            else if (confirm == "n")
                            {
                                return;
                            }
                            else
                            {
                                InvalidInput();
                                Console.Clear();
                                QueueDisplay("VACCINATION QUEUE", vaccinationRegularQueue, vaccinationPriorityQueue);
                            }
                        }

                        break;
                    }

                case 3:
                    {
                        if (currentMaternalCarePatient != "")
                        {
                            PrintAlert("A patient is already being served.");
                            PressAnyKey();
                            return;
                        }

                        string next = PeekNextPatient(maternalcareRegularQueue, maternalcarePriorityQueue);

                        if (next == "")
                        {
                            PrintAlert("No patients in queue.");
                            PressAnyKey();
                            return;
                        }

                        Console.Clear();
                        QueueDisplay("MATERNAL CARE QUEUE", maternalcareRegularQueue, maternalcarePriorityQueue);

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"\n  Call {Queue(next).userName}? (Y/N): ");
                            Console.ForegroundColor = ConsoleColor.White;
                            string confirm = Console.ReadLine().ToLower();
                            Console.ResetColor();

                            if (confirm == "y")
                            {
                                currentMaternalCarePatient = next;
                                serving = Queue(currentMaternalCarePatient).userName;
                                break;
                            }
                            else if (confirm == "n")
                            {
                                return;
                            }
                            else
                            {
                                InvalidInput();
                                Console.Clear();
                                QueueDisplay("MATERNAL CARE QUEUE", maternalcareRegularQueue, maternalcarePriorityQueue);
                            }
                        }

                        break;
                    }

                case 4:
                    {
                        if (currentMedicineClaimPatient != "")
                        {
                            PrintAlert("A patient is already being served.");
                            PressAnyKey();
                            return;
                        }

                        string next = PeekNextPatient(medicineclaimRegularQueue, medicineclaimPriorityQueue);

                        if (next == "")
                        {
                            PrintAlert("No patients in queue.");
                            PressAnyKey();
                            return;
                        }

                        Console.Clear();
                        QueueDisplay("MEDICINE CLAIM QUEUE", medicineclaimRegularQueue, medicineclaimPriorityQueue);

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"\n  Call {Queue(next).userName}? (Y/N): ");
                            Console.ForegroundColor = ConsoleColor.White;
                            string confirm = Console.ReadLine().ToLower();
                            Console.ResetColor();

                            if (confirm == "y")
                            {
                                currentMedicineClaimPatient = next;
                                serving = Queue(currentMedicineClaimPatient).userName;
                                break;
                            }
                            else if (confirm == "n")
                            {
                                return;
                            }
                            else
                            {
                                InvalidInput();
                                Console.Clear();
                                QueueDisplay("MEDICINE CLAIM QUEUE", medicineclaimRegularQueue, medicineclaimPriorityQueue);
                            }
                        }

                        break;
                    }

                case 5:
                    return;

                default:
                    InvalidInput();
                    return;
            }

            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  NOW SERVING");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  {serving}");
            Console.ResetColor();
            Divider();

            PressAnyKey();
        }

        static void ServePatient()
        {
            HeaderDisplay("SERVE PATIENT");
            ShowCurrentPatients();

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

                case 5:
                    return;

                default:
                    InvalidInput();
                    break;
            }
        }

        static void SkipService()
        {
            HeaderDisplay("SKIP PATIENT");
            ShowCurrentPatients();

            int service = ServiceHandling();
            string skippedPatient = "";

            switch (service)
            {
                case 1:
                    {
                        if (currentConsultationPatient == "")
                        {
                            PrintAlert("No patient is currently being served."); 
                            PressAnyKey(); 
                            return;
                        }

                        skippedPatient = SkipPatient(consultationRegularQueue, consultationPriorityQueue);


                        currentConsultationPatient = "";

                        break;
                    }

                case 2:
                    {
                        if (currentVaccinationPatient == "")
                        {
                            PrintAlert("No patient is currently being served.");
                            PressAnyKey();
                            return;
                        }

                        skippedPatient = SkipPatient(vaccinationRegularQueue, vaccinationPriorityQueue);


                        currentVaccinationPatient = "";

                        break;
                    }

                case 3:
                    {
                        if (currentMaternalCarePatient == "")
                        {
                            PrintAlert("No patient is currently being served.");
                            PressAnyKey();
                            return;
                        }

                        skippedPatient = SkipPatient(maternalcareRegularQueue, maternalcarePriorityQueue);


                        currentMaternalCarePatient = "";

                        break;
                    }

                case 4:
                    {
                        if (currentMedicineClaimPatient == "")
                        {
                            PrintAlert("No patient is currently being served.");
                            PressAnyKey();
                            return;
                        }

                        skippedPatient = SkipPatient(medicineclaimRegularQueue, medicineclaimPriorityQueue);


                        currentMedicineClaimPatient = "";

                        break;
                    }

                case 5:
                    return;

                default:
                    InvalidInput();
                    return;
            }

            PrintSuccess("Patient skipped and moved to the back of the queue.");
            PressAnyKey();
        }

        static void ViewServicedPatients()
        {
            HeaderDisplay("SERVICED PATIENTS");

            if (servicedPatients.Count == 0)
            {
                PrintAlert("No patients have been serviced yet.");
                PressAnyKey();
                return;
            }

            var grouped = new Dictionary<string, List<(string queueNumber, string userName, string chosenService, string patientType, string date, string time, string workerName)>>();

            foreach (string record in servicedPatients)
            {
                string[] split = record.Split('|');
                bool isVisit = split.Length == 7;

                string queueNumber = split[0];
                string userName = split[1];
                string chosenService = split[2];
                string patientType = split[3];
                string date = split[4];
                string time = split[5];
                string workerName;

                if (isVisit)
                {
                    workerName = split[6];
                }
                else
                {
                    workerName = currentUser;
                }

                if (!grouped.ContainsKey(date))
                {
                    grouped[date] = new List<(string, string, string, string, string, string, string)>();
                }

                grouped[date].Add((queueNumber, userName, chosenService, patientType, date, time, workerName));
            }

            var sortedDates = grouped.Keys
                    .OrderBy(currentDate => DateTime.TryParse(currentDate, out DateTime parsedDate) ? parsedDate : DateTime.MaxValue)
                    .ToList();

            foreach (string currentDate in sortedDates)
            {
                SectionHeader(currentDate);
                Console.WriteLine();

                var sortedPatients = grouped[currentDate]
                    .OrderBy(currentPatient => DateTime.TryParse(currentPatient.time, out DateTime parsedTime) ? parsedTime : DateTime.MaxValue)
                    .ToList();

                int count = 1;

                foreach (var currentPatient in sortedPatients)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  {count,2}. ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"[{currentPatient.queueNumber}]{""}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"  {currentPatient.userName,-8}");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"  {currentPatient.chosenService,-15}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  {currentPatient.patientType,-14}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  served by {currentPatient.workerName}");
                    Console.ResetColor();
                    count++;
                }
            }

            Console.WriteLine();
            Divider();
            PressAnyKey();
        }

        static void PriorityPatientTracking()
        {
            HeaderDisplay("PRIORITY PATIENT TRACKING");

            int totalPriority = 0;

            foreach (var kvp in priorityStats)
            {
                totalPriority += kvp.Value;
            }

            SectionHeader("TOTAL HISTORY");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Total Priority Patients Serviced  ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{totalPriority}");
            Console.ResetColor();
            Console.WriteLine();

            PrintField("Senior Citizen", $"{priorityStats["Senior Citizen"]}", ConsoleColor.Magenta);
            PrintField("PWD", $"{priorityStats["PWD"]}", ConsoleColor.Blue);
            PrintField("Pregnant", $"{priorityStats["Pregnant"]}", ConsoleColor.Green);
            PrintField("Emergency", $"{priorityStats["Emergency"]}", ConsoleColor.Red);

            SectionHeader("CURRENTLY WAITING");

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
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  {service}");
                    Console.ResetColor();

                    int pos = 1;
                    foreach (string record in priority)
                    {
                        var patient = Queue(record);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write($"    {pos,2}. ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"[{patient.queueNumber}]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"  {patient.userName,-20}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"({patient.patientType})");
                        Console.ResetColor();
                        pos++;
                    }

                    anyWaiting = true;
                }
            }

            if (!anyWaiting)
            {
                PrintAlert("No priority patients currently waiting.");
            }

            PressAnyKey();
        }

        static void CommunityHealthAnalytics()
        {
            while (true)
            {
                HeaderDisplay("COMMUNITY HEALTH ANALYTICS");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n  View health service trends and statistics.\n");
                Console.ResetColor();

                PrintMenuItem("1", "Service Trends");
                PrintMenuItem("2", "Most Requested Service");
                PrintMenuItem("3", "Peak Hours");
                PrintMenuItem("4", "Back");

                Divider();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n  Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.White;

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
            SectionHeader("SELECT SERVICE");
            Console.WriteLine();

            PrintMenuItem("1", "Consultation");
            PrintMenuItem("2", "Vaccination");
            PrintMenuItem("3", "Maternal Care");
            PrintMenuItem("4", "Medicine Claim");
            PrintMenuItem("5", "Back");

            Divider();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\n  Select Service: ");
            Console.ForegroundColor = ConsoleColor.White;

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }

            return -1;
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
                PrintAlert("No patient is currently being served.");
                PressAnyKey();
                return false;
            }

            string completedPatient = CompletePatient(regularQueue, priorityQueue);
            List<string> queuedPatients = Load("queues.txt");

            queuedPatients.Remove(completedPatient);

            Save("queues.txt", queuedPatients);

            servicedPatients.Push(completedPatient);

            var patient = Queue(completedPatient);
            UpdateServiceCount(patient.chosenService);
            UpdatePriorityStats(patient.patientType);
            UpdateHourlyPatients(patient.time);

            List<string> visits = Load("visits.txt");
            visits.Add(VisitFormat(completedPatient, currentUser));
            Save("visits.txt", visits);

            PrintSuccess("Service completed successfully.");
            PressAnyKey();
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

        static void ShowCurrentPatients()
        {
            SectionHeader("CURRENTLY BEING SERVED");
            Console.WriteLine();

            PrintCurrentPatient("Consultation", currentConsultationPatient);
            PrintCurrentPatient("Vaccination", currentVaccinationPatient);
            PrintCurrentPatient("Maternal Care", currentMaternalCarePatient);
            PrintCurrentPatient("Medicine Claim", currentMedicineClaimPatient);

            Console.WriteLine();
            Divider();
        }

        static void PrintCurrentPatient(string service, string record)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {service,-15} : ");

            if (record == "")
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("none");
            }
            else
            {
                var patient = Queue(record);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{patient.userName,-20}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"  [{patient.queueNumber}]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"  ({patient.patientType})");
            }

            Console.ResetColor();
        }

        static void ServiceTrends()
        {
            HeaderDisplay("SERVICE TRENDS");

            int total = 0;

            foreach (var kvp in serviceCount)
            {
                total += kvp.Value;
            }

            int maxCount = serviceCount.Values.Max();

            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Total Patients Serviced  ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{total}");
            Divider();
            Console.WriteLine();

            foreach (var kvp in serviceCount)
            {
                int count = kvp.Value;
                double percent = total > 0 ? (double)count / total * 100 : 0;
                string bar = BuildBar(count, maxCount, 20);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  {kvp.Key,-15}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"  {count,4} patients  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[{bar}]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  {percent:0.0}%");
                Console.ResetColor();

            }

            Console.WriteLine();
            Divider();
            PressAnyKey();
        }

        static void MostRequestedService()
        {
            HeaderDisplay("MOST REQUESTED SERVICE");

            int total = 0;

            foreach (var kvp in serviceCount)
            {
                total += kvp.Value;
            }

            if (total == 0)
            {
                PrintAlert("No service data available yet.");
                PressAnyKey();
                return;
            }

            var ranked = serviceCount.OrderByDescending(kvp => kvp.Value).ToList();

            SectionHeader("SERVICE RANKING");
            Console.WriteLine();

            int rank = 1;
            foreach (var kvp in ranked)
            {
                double percent = (double)kvp.Value / total * 100;
                bool isTop = rank == 1;
                Console.ForegroundColor = isTop ? ConsoleColor.Yellow : ConsoleColor.Gray;
                Console.Write($"  #{rank}  ");
                Console.ForegroundColor = isTop ? ConsoleColor.White : ConsoleColor.Gray;
                Console.Write($"{kvp.Key,-15}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"  {kvp.Value,4} patients  ({percent:0.0}%)");

                if (isTop)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  << TOP");
                }

                Console.WriteLine();
                Console.ResetColor();
                rank++;
            }

            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  Most Requested:  {ranked[0].Key}  ({ranked[0].Value} patients)");
            Console.ResetColor();
            Divider();

            PressAnyKey();
        }

        static void PeakHours()
        {
            HeaderDisplay("PEAK HOURS");

            int totalPatients = hourlyPatients.Sum();

            if (totalPatients == 0)
            {
                PrintAlert("No hourly data available yet.");
                PressAnyKey();
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

            SectionHeader("PATIENTS PER HOUR");
            Console.WriteLine();

            for (int i = 0; i < hourlyPatients.Length; i++)
            {
                if (hourlyPatients[i] == 0) continue;

                string label = DateTime.Today.AddHours(i).ToString("hh:00 tt");
                string bar = BuildBar(hourlyPatients[i], peakCount, 20);
                bool isPeak = i == peakHour;

                Console.ForegroundColor = isPeak ? ConsoleColor.Yellow : ConsoleColor.Gray;
                Console.Write($"  {label}  ");
                Console.ForegroundColor = isPeak ? ConsoleColor.White : ConsoleColor.Gray;
                Console.Write($"{hourlyPatients[i],4} patients  ");
                Console.ForegroundColor = isPeak ? ConsoleColor.Yellow : ConsoleColor.Cyan;
                Console.Write($"[{bar}]");

                if (isPeak)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  << PEAK");
                }

                Console.WriteLine();
                Console.ResetColor();
            }

            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  Peak Hour:  {DateTime.Today.AddHours(peakHour):hh:00 tt}  ({peakCount} patients)");
            Console.ResetColor();
            Divider();

            PressAnyKey();
        }

        static string BuildBar(int value, int max, int width)
        {
            if (max == 0) return new string('-', width);
            int filled = (int)((double)value / max * width);
            return new string('#', filled) + new string('.', width - filled);
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
            if (DateTime.TryParse(time, out DateTime parsed))
            {
                hourlyPatients[parsed.Hour]++;
                SaveHourlyPatients();
            }

        }

        //====================GLOBAL HELPERS====================
        static bool Login(string fileName)
        {
            List<string> users = Load(fileName);

            HeaderDisplay("LOGIN");
            SectionHeader("ENTER YOUR CREDENTIALS");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\n  Name      : ");
            Console.ForegroundColor = ConsoleColor.White;
            string userName = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Password  : ");
            Console.ForegroundColor = ConsoleColor.White;
            string userPassword = Console.ReadLine();
            Console.ResetColor();

            for (int i = 0; i < users.Count; i++)
            {
                var user = User(users[i]);

                if (user.userName == userName && user.userPassword == userPassword)
                {
                    PrintSuccess("Login successful!");
                    currentUser = userName;
                    PrintSuccess("Loading...");
                    Thread.Sleep(800);
                    return true;
                }
            }

            PrintError("Account not found. Please check your credentials or register.");
            PressAnyKey();
            return false;
        }

        static void Register(string fileName)
        {
            List<string> users = Load(fileName);

            HeaderDisplay("REGISTER");
            SectionHeader("CREATE A NEW ACCOUNT");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\n  Name      : ");
            Console.ForegroundColor = ConsoleColor.White;
            string userName = Console.ReadLine();
            Console.ResetColor();

            if(userName.Trim() == "")
            {
                PrintError("Name cannot be empty. Please try again.");
                PressAnyKey();
                return;
            }

            if (NameExists(fileName, userName))
            {
                PrintError("Name already exists. Please choose a different name.");
                PressAnyKey();
                return;
            }


            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Password  : ");
            Console.ForegroundColor = ConsoleColor.White;
            string userPassword = Console.ReadLine();
            Console.ResetColor();

            if (userPassword.Trim() == "")
            {
                PrintError("Password cannot be empty. Please try again.");
                PressAnyKey();
                return;
            }

            users.Add(UserFormat(userName, userPassword));
            Save(fileName, users);

            PrintSuccess("Account created successfully! You may now login.");
            PressAnyKey();

        }

        static void PromptDataSetup()
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  +==================================================+
  |           FIRST TIME SETUP                       |
  +==================================================+");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n  Welcome! This appears to be a fresh installation.");
        Console.WriteLine("  Would you like to load pre-defined demo data to");
        Console.WriteLine("  simulate a real health center experience?\n");
        Console.ResetColor();

        PrintMenuItem("1", "Load Demo Data (recommended for testing)");
        PrintMenuItem("2", "Start Clean (empty system)");

        Divider();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("\n  Enter your choice: ");
        Console.ForegroundColor = ConsoleColor.White;

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DefaultWorkers();
                    DefaultPatients();
                    DefaultQueues();
                    DefaultVisits();
                    DefaultStats();
                    DefaultCounters();
                    DefaultHourly();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n  Demo data loaded successfully!");
                    Console.WriteLine("\n  Check program files for more information!");
                    Console.WriteLine("\n  Loading");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    return;

                case 2:
                    DefaultWorkers();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n  Starting with a clean system.");
                    Console.WriteLine("  Default health worker accounts have been created.");
                    Console.WriteLine("\n  Check program files for more information!");
                    Console.WriteLine("\n  Loading");
                    Console.ResetColor();
                    Thread.Sleep(3000);
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
            CreateFileIfNotExisting("counters.txt");
            CreateFileIfNotExisting("stats.txt");
            CreateFileIfNotExisting("hourly.txt");

            bool isFirstRun = Load("workers.txt").Count == 0 && Load("patients.txt").Count == 0;

            if (isFirstRun)
            {
                PromptDataSetup();
            }

        }

        static void InvalidInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  Invalid input. Please try again.");
            Console.WriteLine("\n  Loading...");
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

            List<string> visitedPatients = Load("visits.txt");
            foreach (string visit in visitedPatients)
            {
                servicedPatients.Push(visit);
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

        static void DefaultWorkers()
        {
            var defaultWorkers = new List<string>
            {
                "Rose|Tumayan",
                "Arnel|Montablan",
                "Shayne|Buenaventura",
                "Aldin|Osena",
                "Kyle|Gloriani"
            };
           
            Save("workers.txt", defaultWorkers);
        }

        static void DefaultPatients()
        {
            var defaultPatients = new List<string>
            {
                "Gian|Topacio",
                "Simon|Songco",
                "Clark|Labay",
                "Zak|Guevarra",
                "Keeyan|Segismundo",
                "Art|Cuaresma",
                "Keith|Santos",
                "Rezdee|Navarro",
                "Ariane|Abante",
                "Marco|Secreto",
                "Amrisse|Tagoc",
                "Kyle|Luces"
            };

            Save("patients.txt", defaultPatients);
        }

        static void DefaultQueues()
        {
            var defaultQueues = new List<string>
            {
                "CO-001|Keeyan|Consultation|Emergency|06/30/26|08:00 AM",
                "CO-002|Keith|Consultation|Senior Citizen|06/30/26|09:00 AM",
                "CO-003|Gian|Consultation|Regular|06/30/26|09:30 AM",
                "CO-004|Rezdee|Consultation|Regular|06/30/26|10:00 AM",

        
                "VA-001|Marco|Vaccination|PWD|06/30/26|09:00 AM",
                "VA-002|Simon|Vaccination|Regular|06/30/26|09:30 AM",
                "VA-003|Ariane|Vaccination|Regular|06/30/26|10:00 AM",

        
                "MA-001|Amrisse|Maternal Care|Pregnant|06/30/26|08:30 AM",
                "MA-002|Keith|Maternal Care|Regular|06/30/26|09:15 AM",

        
                "ME-001|Art|Medicine Claim|Senior Citizen|06/30/26|10:30 AM",
                "ME-002|Zak|Medicine Claim|Regular|06/30/26|11:00 AM",
                "ME-003|Kyle|Medicine Claim|Regular|06/30/26|11:30 AM",
            };

            Save("queues.txt", defaultQueues);
        }

        static void DefaultVisits()
        {
            var defaultVisits = new List<string>
            {
                "CO-001|Clark|Consultation|Emergency|06/29/26|08:00 AM|Rose",
                "CO-002|Gian|Consultation|Regular|06/29/26|09:00 AM|Arnel",
                "CO-003|Rezdee|Consultation|Senior Citizen|06/29/26|09:30 AM|Shayne",

                "VA-001|Simon|Vaccination|Regular|06/29/26|10:00 AM|Aldin",
                "VA-002|Ariane|Vaccination|PWD|06/29/26|10:30 AM|Kyle",

                "MA-001|Amrisse|Maternal Care|Pregnant|06/29/26|08:30 AM|Rose",
                "MA-002|Keith|Maternal Care|Regular|06/29/26|09:15 AM|Shayne",

                "ME-001|Art|Medicine Claim|Senior Citizen|06/29/26|11:00 AM|Arnel",
                "ME-002|Zak|Medicine Claim|Regular|06/29/26|11:30 AM|Kyle",
            };

            Save("visits.txt", defaultVisits);
        }

        static void DefaultStats()
        {
            var defaultStats = new List<string>
            {
                "Consultation|3",
                "Vaccination|2",
                "Maternal Care|2",
                "Medicine Claim|2",
                "Senior Citizen|2",
                "PWD|1",
                "Pregnant|1",
                "Emergency|1"
            };

            Save("stats.txt", defaultStats);
        }

        static void DefaultCounters()
        {
            var defaultCounters = new List<string>
            {
                "Consultation|5",
                "Vaccination|4",
                "MaternalCare|3",
                "MedicineClaim|4"
            };

            Save("counters.txt", defaultCounters);
        }

        static void DefaultHourly()
        {
            var defaultHourly = new List<string>();

            int[] counts = new int[24];
            counts[8] = 2;  // Clark and Amrisse
            counts[9] = 4;  // Gian, Rezdee, Keith, Ariane
            counts[10] = 2; // Simon, Ariane
            counts[11] = 2; // Art, Zak

            for (int i = 0; i < 24; i++)
            {
                defaultHourly.Add($"{i}|{counts[i]}");
            }

            Save("hourly.txt", defaultHourly);
        }

        //====================UI HELPERS====================
        static void HeaderDisplay(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  +==================================================+");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  |{title.PadLeft((50 + title.Length) / 2).PadRight(50)}|");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  +==================================================+");
            Console.ResetColor();
        }

        static void QueueDisplay(string title, Queue<string> regularQueue, Queue<string> priorityQueue)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  +-- ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(title);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  ({priorityQueue.Count + regularQueue.Count} waiting)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  |");
            Console.ResetColor();

            int position = 1;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  |  PRIORITY");
            Console.ResetColor();

            if (priorityQueue.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  |    none");
                Console.ResetColor();
            }

            else
            {
                foreach (string record in priorityQueue)
                {
                    var patient = Queue(record);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  |  {position,2}. ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"[{patient.queueNumber}]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"  {patient.userName,-20}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"({patient.patientType})");
                    Console.ResetColor();
                    position++;
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  |");
            Console.WriteLine("  |  REGULAR");
            Console.ResetColor();

            if (regularQueue.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  |    none");
                Console.ResetColor();
            }
            else
            {
                foreach (string record in regularQueue)
                {
                    var patient = Queue(record);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  |  {position,2}. ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"[{patient.queueNumber}]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"  {patient.userName,-20}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"({patient.patientType})");
                    Console.ResetColor();
                    position++; ;
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  +" + new string('-', 50));
            Console.ResetColor();
        }

        static void Divider(int width = 54)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  " + new string('-', width));
            Console.ResetColor();
        }

        static void PrintMenuItem(string number, string label)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"  [{number}]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {label}");
            Console.ResetColor();
        }

        static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  {msg}");
            Console.ResetColor();
        }

        static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  {msg}");
            Console.ResetColor();
        }

        static void PrintAlert(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  {msg}");
            Console.ResetColor();
        }

        static void PrintField(string label, string value, ConsoleColor valueColor = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"  {label,-16}: ");
            Console.ForegroundColor = valueColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        static void PressAnyKey()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void SectionHeader(string title)
        {
            Console.WriteLine();
            Divider();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  {title}");
            Divider();
            Console.ResetColor();
        }   
    }
}
