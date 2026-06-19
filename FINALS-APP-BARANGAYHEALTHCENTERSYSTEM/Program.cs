using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        static void Main(string[] args)
        {
            Console.WriteLine(@"
 _  _  ____   __   __   ____  _  _  __    __  __ _  ____    __    ____ 
/ )( \(  __) / _\ (  ) (_  _)/ )( \(  )  (  )(  ( \(  __)  (  )  (  _ \
) __ ( ) _) /    \/ (_/\ )(  ) __ (/ (_/\ )( /    / ) _)   / (_/\ ) __/
\_)(_/(____)\_/\_/\____/(__) \_)(_/\____/(__)\_)__)(____)  \____/(__)  
");
            Console.ReadKey();
        }

        static void RegisterUser(string userRole)
        {
            List<string> users  = new List<string>();
            if (userRole == "patient")
            {
                users = Load("patients.txt");
            }
            
            else if (userRole == "worker")
            {
                users = Load("workers.txt");
            }

            Console.Write("Enter your Username: "); string userName = Console.ReadLine().Trim();
            Console.Write("Enter your Password: "); string userPassword = Console.ReadLine().Trim();

            for (int i = 0; i < users.Count; i++)
            {
                var user = User(users[i]);

                if (user.userName.Trim().ToLower() == userName.Trim().ToLower() && user.userPassword.Trim() == userPassword)
                {
                    Console.WriteLine("Account already exists.");
                    Console.ReadKey();
                    return;
                }
            }

            users.Add(UserFormat(userName, userPassword));
            Save("")

            
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
            Console.WriteLine("+==================================================+");
            Console.WriteLine($"|                     {title}                      |");
            Console.WriteLine("+==================================================+");
        }
    }
}
