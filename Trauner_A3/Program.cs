using System.Formats.Asn1;
using Trauner_A1;
using Trauner2;
using Trauner_A3;

/**
Name: Derek Trauner
COP2362 - Assignment 4
May 4, 2025
Summary Statement: I worked on this alone.

<summary>
    This is a console style program to create tests, students, and make students take tests.
    All in the console and saved to .txt files.
</summary>
 */
namespace Trauner_A1 // Namespace including interfaces from version 1.
{
    public interface ITestPaper
    {
        string Subject { get; set; }
        string[] MarkScheme { get; }
        string PassMark { get; }
    }

    public class TestPaper : ITestPaper
    {
        public string Subject { get; set; }
        public string[] MarkScheme { get; set; }
        // Confused on what this means in the instructions
        // c.	A Minimum Mark or passing grade: Assignment one uses a string representation. However, the Mark or Passing Grade should match the grading scheme.  
        public string PassMark { get; set; }

        public TestPaper(string subject, string[] markScheme, string passMark)
        {
            Subject = subject;
            MarkScheme = markScheme;
            PassMark = passMark;
        }

        public override string ToString()
        {
            return $"{Subject} | {string.Join(",", MarkScheme)} | {PassMark}";
        }
    }

    public interface IStudent
    {
        string Name { get; set; }
        string[] TestsTaken { get; set; }
        int id { get; set; }
        void TakeTest(TestPaper paper, string[] answers);
    }

    public class Student : IStudent
    {
        private string _name;
        private int _id;
        private List<string> testsTaken = new List<string>();

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int id
        {
            get => _id;
            set => _id = value;
        }

        public Student(string name)
        {
            _id = new Random().Next(1, 10000);
            _name = name;
        }

        public Student(int id, string name, IEnumerable<string> existingTests)
        {
            _id = id;
            _name = name;
            testsTaken = new List<string>(existingTests);
        }

        public string[] TestsTaken
        {
            get
            {
                if (testsTaken.Count == 0)
                {
                    return new string[] { "No tests taken" };
                }
                string[] sortedTests = testsTaken.ToArray();
                Array.Sort(sortedTests);
                return sortedTests;
            }
            set
            {
                if (value != null)
                {
                    testsTaken.AddRange(value);
                }
            }
        }

        public void TakeTest(TestPaper paper, string[] answers)
        {
            int score = 0;
            int i = 0;
            int passMark = int.Parse(paper.PassMark.TrimEnd('%'));
            foreach (string answer in paper.MarkScheme)
            {
                if (answer == answers[i++])
                {
                    score++;
                }
            }
            int percent = (int)Math.Round((double)score * 100 / paper.MarkScheme.Length);
            string result = percent >= passMark ? "Passed!" : "Failed!";
            testsTaken.Add($"{paper.Subject}: {result} ({percent}%)");
        }

        public override string ToString()
        {
            return $"ID:{id} | {Name} | Tests: {string.Join(" • ", TestsTaken)}";
        }
    }
}



namespace Trauner2 // Namespace containins code for version 2, menu items and read/write functions for students and tests
{
    public interface IMenuItem
    {
        public string Name { get; }
        public bool IsSubMenu { get; }
        public IMenuItem? ParentItem { get; }
        public List<IMenuItem> SubMenuItems { get; }
    }

    public abstract class MenuItem : IMenuItem
    {
        public string Name => _name;
        public bool IsSubMenu => _subMenu;
        public IMenuItem? ParentItem => _parent;
        public List<IMenuItem> SubMenuItems => _subMenuItems;

        public string _name;
        public bool _subMenu;
        public IMenuItem? _parent;
        public List<IMenuItem> _subMenuItems;

        public MenuItem(string name, bool sub, IMenuItem? parentItem)
        {
            _name = name;
            _subMenu = sub;
            _parent = parentItem!;
            _subMenuItems = [];
        }
    }

    public class ConsoleMenuItem : MenuItem
    {
        private TextReader _in;
        private TextWriter _out;

        public ConsoleMenuItem(TextReader reader, TextWriter writer, string name, bool sub, IMenuItem? parentItem) : base(name, sub, parentItem)
        {
            _parent?.SubMenuItems.Add(this);
            _in = reader;
            _out = writer;
        }

        public void PrintMenuItem()
        {
            if (!_subMenu)
            {
                _out.WriteLine($"{_name}");
                foreach (MenuItem subItem in this._subMenuItems)
                {
                    _out.WriteLine($"   {subItem.Name}");
                }
            }
            else
            {
                _out.WriteLine($"Printing a sub-Menu-Item: {_name}");
            }
        }
    }
}

namespace Trauner_A3
{

}

class Program
{
    static List<TestPaper> testPapersList = new List<TestPaper>(); // global static lists
    static List<Student> studentsList = new List<Student>();
    // string testsPath = "C:\\Users\\derek\\Documents\\GitHub Repositories\\COP2362\\Trauner_A2\\tests.txt"; 
    // ^^^Use this but insert full path if below is not working.
    static string testsPath = "../../../tests.txt"; // Used this because when project is ran its directory is in the bin/debug/net8.0 folder.
    static string studentsPath = "../../../students.txt";

    static ConsoleMenuItem option1 = new ConsoleMenuItem(Console.In, Console.Out, "1. TESTS", false, null); // Initialize all menu options
    static ConsoleMenuItem option1a = new ConsoleMenuItem(Console.In, Console.Out, "a. ADD", true, option1);
    static ConsoleMenuItem option1b = new ConsoleMenuItem(Console.In, Console.Out, "b. EDIT", true, option1);
    static ConsoleMenuItem option1c = new ConsoleMenuItem(Console.In, Console.Out, "c. DELETE", true, option1);
    static ConsoleMenuItem option1d = new ConsoleMenuItem(Console.In, Console.Out, "d. VIEW", true, option1);
    static ConsoleMenuItem option2 = new ConsoleMenuItem(Console.In, Console.Out, "2. STUDENTS", false, null);
    static ConsoleMenuItem option2a = new ConsoleMenuItem(Console.In, Console.Out, "a. ADD", true, option2);
    static ConsoleMenuItem option2b = new ConsoleMenuItem(Console.In, Console.Out, "b. EDIT", true, option2);
    static ConsoleMenuItem option2c = new ConsoleMenuItem(Console.In, Console.Out, "c. DELETE", true, option2);
    static ConsoleMenuItem option2d = new ConsoleMenuItem(Console.In, Console.Out, "d. GIVE TEST", true, option2);
    static ConsoleMenuItem option2e = new ConsoleMenuItem(Console.In, Console.Out, "e. VIEW SCORES", true, option2);
    static ConsoleMenuItem option3 = new ConsoleMenuItem(Console.In, Console.Out, "3. GIVE ALL STUDENTS A TEST", false, null);
    static ConsoleMenuItem option3a = new ConsoleMenuItem(Console.In, Console.Out, "a. SELECT TEST", true, option3);
    static ConsoleMenuItem option4 = new ConsoleMenuItem(Console.In, Console.Out, "4. HAVE STUDENT TAKE ALL TESTS", false, null);
    static ConsoleMenuItem option4a = new ConsoleMenuItem(Console.In, Console.Out, "a. SELECT STUDENT", true, option4);
    static ConsoleMenuItem option5 = new ConsoleMenuItem(Console.In, Console.Out, "5. EXIT", false, null);

    static void Main(string[] args)
    {
        LoadTests();
        LoadStudents();

        bool running = true;
        while (running) // looping menu
        {
            option1.PrintMenuItem(); // print all menu items
            option2.PrintMenuItem();
            option3.PrintMenuItem();
            option4.PrintMenuItem();
            option5.PrintMenuItem();
            Console.WriteLine("Choose a main option (1,2,3,4,5): ");
            int mainChoice = int.Parse(Console.ReadLine()!.Trim()); // get user choice and error-check
            switch (mainChoice) // handle user choice
            {
                case 1:
                    HandleTestsMenu();
                    break;
                case 2:
                    HandleStudentsMenu();
                    break;
                case 3:
                    Console.WriteLine("=== GIVE ALL STUDENTS A TEST  (TODO) ===\n");
                    break;
                case 4:
                    Console.WriteLine("=== HAVE STUDENT TAKE ALL TESTS  (TODO) ===\n");
                    break;
                case 5:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.\n");
                    break;
            }
        }
    }

    //───────────────────────────────────────────────────────
    // ────────────────────VERSION 4 CODE────────────────────
    //───────────────────────────────────────────────────────

    // ─── TESTS MENU ───
    static void HandleTestsMenu()
    {
        option1.PrintMenuItem();
        Console.WriteLine("Choose (a-d): ");
        string? option1SubChoice = Console.ReadLine()?.Trim().ToLower();
        switch (option1SubChoice)
        {
            case "a":
                AddTest();
                break;
            case "b":
                EditTest();
                break;
            case "c":
                DeleteTest();
                break;
            case "d":
                ViewTests();
                break;
            default:
                Console.WriteLine("Invalid choice, try again.\n");
                break;
        }
    }

    static void AddTest()
    {
        try
        {
            Console.WriteLine("Subject: ");
            string testSubject = Console.ReadLine()!.Trim();
            Console.WriteLine("Mark scheme (1A,2B,3C,...): ");
            string preFormatMarkScheme = Console.ReadLine()!;
            string[] markScheme = preFormatMarkScheme == null ? [] : preFormatMarkScheme.ToUpper().Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("Pass mark (ex: 75%): ");
            string passMark = Console.ReadLine()!.Trim();

            TestPaper paper = new TestPaper(testSubject, markScheme, passMark);
            testPapersList.Add(paper);
            SaveTests();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding test: {ex.Message}");
        }
        Console.WriteLine("Test saved.\n");
    }

    static void EditTest()
    {
        try
        {
            ViewTests();
            Console.WriteLine("Which test do you want to edit?:");
            int indexToEdit = int.Parse(Console.ReadLine()!.Trim()) - 1;
            TestPaper testToEdit = testPapersList[indexToEdit];
            Console.WriteLine(testToEdit + "\nDo you want to edit the (1)Subject, (2)MarkScheme, or (3)PassMark?: ");
            int userChoice = int.Parse(Console.ReadLine()!);
            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Enter the new subject: ");
                    string newSubject = Console.ReadLine()!;
                    testToEdit.Subject = newSubject;
                    SaveTests();
                    break;
                case 2:
                    Console.WriteLine("Enter the new MarkScheme (1A,2B,3C,...): ");
                    string preFormatMarkScheme = Console.ReadLine()!;
                    string[] newMarkScheme = preFormatMarkScheme == null ? [] : preFormatMarkScheme.ToUpper().Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    testToEdit.MarkScheme = newMarkScheme;
                    SaveTests();
                    break;
                case 3:
                    Console.WriteLine("Enter the new Pass mark (ex: 75%): ");
                    string newPassMark = Console.ReadLine()!.Trim();
                    testToEdit.PassMark = newPassMark;
                    SaveTests();
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing test: {ex.Message}");
        }
    }

    static void DeleteTest()
    {
        try
        {
            ViewTests();
            Console.WriteLine("Which test do you want to delete?:");
            int indexToDelete = int.Parse(Console.ReadLine()!.Trim()) - 1;
            testPapersList.RemoveAt(indexToDelete);
            SaveTests();
            Console.WriteLine("Successfully deleted test.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting test: {ex.Message}");
        }
    }

    static void ViewTests()
    {
        try
        {
            Console.WriteLine("\nTests:");
            for (int i = 0; i < testPapersList.Count; i++)
            {
                TestPaper test = testPapersList[i];
                Console.WriteLine($"{i + 1}. {test.Subject} | {string.Join(",", test.MarkScheme)} | {test.PassMark}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading tests: {ex.Message}");
        }
        Console.WriteLine();
    }

    static void LoadTests()
    {
        testPapersList.Clear();
        if (!File.Exists(testsPath)) return;
        try
        {
            foreach (string line in File.ReadAllLines(testsPath))
            {
                string[] parts = line.Split(',', 3);
                if (parts.Length < 3) continue;
                string[] scheme = parts[1].Split('|', StringSplitOptions.RemoveEmptyEntries);
                testPapersList.Add(new TestPaper(parts[0], scheme, parts[2]));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
        }
    }

    static void SaveTests()
    {
        try
        {
            using StreamWriter writer = new StreamWriter(testsPath, false);
            foreach (TestPaper test in testPapersList)
            {
                string markScheme = string.Join("|", test.MarkScheme);
                writer.WriteLine($"{test.Subject},{markScheme},{test.PassMark}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file, try again: {ex.Message}");
        }
        Console.WriteLine("Tests has been saved.");
    }

    // ─── STUDENTS MENU ───
    static void HandleStudentsMenu()
    {
        option2.PrintMenuItem();
        Console.WriteLine("Choose (a-e): ");
        string option2SubChoice = Console.ReadLine()!.Trim().ToLower();
        switch (option2SubChoice)
        {
            case "a":
                AddStudent();
                break;
            case "b":
                EditStudent();
                break;
            case "c":
                DeleteStudent();
                break;
            case "d":
                GiveTest();
                break;
            case "e":
                ViewStudents();
                break;
            default:
                Console.WriteLine("Invalid choice, try again.\n");
                break;
        }
    }

    static void AddStudent()
    {
        try
        {
            Console.WriteLine("Name: ");
            string studentName = Console.ReadLine()!.Trim();
            Student s = new Student(studentName);
            studentsList.Add(s);
            SaveStudents();
            Console.WriteLine($"Student: {s.Name} ({s.id}) saved.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding student: {ex.Message}");
        }
    }

    static void EditStudent()
    {
        try
        {
            ViewStudents();
            Console.WriteLine("Which student do you want to edit?:");
            int indexToEdit = int.Parse(Console.ReadLine()!.Trim()) - 1;
            Student studentToEdit = studentsList[indexToEdit];
            Console.WriteLine(studentToEdit + "\nDo you want to edit the (1)ID, (2)Name, or (3)Tests Taken?: ");
            int userChoice = int.Parse(Console.ReadLine()!);
            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Enter the new ID: ");
                    int newID = int.Parse(Console.ReadLine()!.Trim());
                    studentToEdit.id = newID;
                    SaveStudents();
                    break;
                case 2:
                    Console.WriteLine("Enter the new name: ");
                    string newName = Console.ReadLine()!.Trim();
                    studentToEdit.Name = newName;
                    SaveStudents();
                    break;
                case 3:
                    Console.WriteLine("notdone - Enter the new Tests Taken (ex: 75%): ");
                    SaveStudents();
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding student: {ex.Message}");
        }
    }

    static void DeleteStudent()
    {
        try
        {
            ViewStudents();
            Console.WriteLine("Which student do you want to delete?:");
            int indexToDelete = int.Parse(Console.ReadLine()!.Trim()) - 1;
            studentsList.RemoveAt(indexToDelete);
            SaveStudents();
            Console.WriteLine("Successfully deleted student.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting student: {ex.Message}");
        }
    }

    static void GiveTest()
    {
        ViewStudents();
        Console.WriteLine("Which student would you like to give a test to?:");
        int studentIndex = int.Parse(Console.ReadLine()!.Trim()) - 1;
        Student student = studentsList[studentIndex];
        ViewTests();
        Console.WriteLine("Which test would you like to give them?:");


    }

    static void ViewStudents()
    {
        try
        {
            Console.WriteLine("\nStudents:");
            for (int i = 0; i < studentsList.Count; i++)
            {
                Student student = studentsList[i];
                Console.WriteLine($"{i + 1}. ID:{student.id} | {student.Name} | Tests: {string.Join(" • ", student.TestsTaken)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error viewing students: {ex.Message}");
        }

        Console.WriteLine();
    }

    static void LoadStudents()
    {
        studentsList.Clear();
        if (!File.Exists(studentsPath)) return;

        try
        {
            foreach (string line in File.ReadAllLines(studentsPath))
            {
                string[] parts = line.Split(',', StringSplitOptions.None);
                if (parts.Length < 2) continue;

                if (!int.TryParse(parts[0], out int id)) continue;
                string name = parts[1];

                List<string> records = parts.Length > 2
                    ? parts.Skip(2).ToList()
                    : new List<string>();

                studentsList.Add(new Student(id, name, records));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading students: {ex.Message}");
        }

    }

    static void SaveStudents()
    {
        try
        {
            using StreamWriter writer = new StreamWriter(studentsPath, false);
            foreach (Student s in studentsList)
            {
                string line = $"{s.id},{s.Name}";
                foreach (string record in s.TestsTaken)
                    line += $",{record}";
                writer.WriteLine(line);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving students: {ex.Message}");
        }
        Console.WriteLine("Student has been saved.");
    }
}