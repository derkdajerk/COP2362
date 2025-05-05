using System.Formats.Asn1;
using Trauner_A1;
using Trauner2;

/**
Name: Derek Trauner
COP2362 - Assignment 1
May 4, 2025
Summary Statement: I worked on this alone.

<summary>
    This program contains Types to create Student and TestPaper objects.
    Testpaper implements ITestPaper, an interface for general TestPaper properties.
    Student implements IStudent, an interface for general Student properties and methods.
        -Causes every instance of Student to have those properties and Student must implement those methods from IStudent.

    Now implements creating,writing, and viewing both students and tests to a txt file to read into the console.
</summary>
 */
namespace Trauner_A1 // Namespace including interfaces from version 1.
{
    public interface ITestPaper
    {
        string Subject { get; }
        string[] MarkScheme { get; }
        string PassMark { get; }
    }

    public class TestPaper : ITestPaper
    {
        public string Subject { get; }
        public string[] MarkScheme { get; }
        // Confused on what this means in the instructions
        // c.	A Minimum Mark or passing grade: Assignment one uses a string representation. However, the Mark or Passing Grade should match the grading scheme.  
        public string PassMark { get; }

        public TestPaper(string subject, string[] markScheme, string passMark)
        {
            Subject = subject;
            MarkScheme = markScheme;
            PassMark = passMark;
        }
    }

    public interface IStudent
    {
        string Name { get; }
        string[] TestsTaken { get; }
        int id { get; }
        void TakeTest(TestPaper paper, string[] answers);
    }

    public class Student : IStudent
    {
        private List<string> testsTaken = new List<string>();
        string IStudent.Name => _name;
        int IStudent.id => _id;
        public string _name;
        public int _id;

        public Student(string name)
        {
            _id = new Random().Next(1, 10000);
            _name = name;
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
        }

        public void TakeTest(TestPaper paper, string[] answers)
        {
            int score = 0;
            int i = 0;
            int passMark = int.Parse(paper.PassMark.TrimEnd('%'));
            foreach (var answer in paper.MarkScheme)
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
                foreach (var subItem in this._subMenuItems)
                {
                    _out.WriteLine($"   {subItem.Name}");
                }
            }
            else
            {
                _out.WriteLine($"Printing a sub-Menu-Item: {_name}");
            }
        }

        public void HandleUserChoice(string userChoice) // Future use maybe
        {

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
    static void Main(string[] args)
    {
        // string testsPath = "C:\\Users\\derek\\Documents\\GitHub Repositories\\COP2362\\Trauner_A2\\tests.txt"; 
        // ^^^Use this but insert full path if below is not working.
        string testsPath = "../../../tests.txt"; // Used this because when project is ran its directory is in the bin/debug/net8.0 folder.
        string studentsPath = "../../../students.txt";
        // loadTests(testsPath); // did not use these loading methods since once you run program you wouldnt be able to view new students made in the same instance if it loads all students at start
        // loadStudents(studentsPath);
        ConsoleMenuItem option1 = new ConsoleMenuItem(Console.In, Console.Out, "1. TESTS", false, null); // Initialize all menu options
        ConsoleMenuItem option1a = new ConsoleMenuItem(Console.In, Console.Out, "a. ADD", true, option1);
        ConsoleMenuItem option1b = new ConsoleMenuItem(Console.In, Console.Out, "b. EDIT", true, option1);
        ConsoleMenuItem option1c = new ConsoleMenuItem(Console.In, Console.Out, "c. DELETE", true, option1);
        ConsoleMenuItem option1d = new ConsoleMenuItem(Console.In, Console.Out, "d. VIEW", true, option1);
        ConsoleMenuItem option2 = new ConsoleMenuItem(Console.In, Console.Out, "2. STUDENTS", false, null);
        ConsoleMenuItem option2a = new ConsoleMenuItem(Console.In, Console.Out, "a. ADD", true, option2);
        ConsoleMenuItem option2b = new ConsoleMenuItem(Console.In, Console.Out, "b. EDIT", true, option2);
        ConsoleMenuItem option2c = new ConsoleMenuItem(Console.In, Console.Out, "c. DELETE", true, option2);
        ConsoleMenuItem option2d = new ConsoleMenuItem(Console.In, Console.Out, "d. GIVE TEST", true, option2);
        ConsoleMenuItem option2e = new ConsoleMenuItem(Console.In, Console.Out, "e. VIEW SCORES", true, option2);
        ConsoleMenuItem option3 = new ConsoleMenuItem(Console.In, Console.Out, "3. GIVE ALL STUDENTS A TEST", false, null);
        ConsoleMenuItem option3a = new ConsoleMenuItem(Console.In, Console.Out, "a. SELECT TEST", true, option3);
        ConsoleMenuItem option4 = new ConsoleMenuItem(Console.In, Console.Out, "4. HAVE STUDENT TAKE ALL TESTS", false, null);
        ConsoleMenuItem option4a = new ConsoleMenuItem(Console.In, Console.Out, "a. SELECT STUDENT", true, option4);
        ConsoleMenuItem option5 = new ConsoleMenuItem(Console.In, Console.Out, "5. EXIT", false, null);

        bool running = true;
        while (running) // looping menu
        {
            option1.PrintMenuItem();
            option2.PrintMenuItem();
            option3.PrintMenuItem();
            option4.PrintMenuItem();
            option5.PrintMenuItem();
            Console.WriteLine("Choose a main option (1,2,3,4,5): ");
            string? mainChoice = Console.ReadLine();
            switch (mainChoice)
            {
                case "1": // Tests
                    Console.WriteLine();
                    option1.PrintMenuItem();
                    Console.WriteLine("Choose a sub option (a,b,c,d): ");
                    string? option1SubChoice = Console.ReadLine();
                    switch (option1SubChoice)
                    {
                        case "a" or "A": // Add
                            Console.WriteLine("Enter the name for the test: ");
                            string? testName = Console.ReadLine().Trim();
                            Console.WriteLine("Enter the markScheme for the test (1A, 2B, 3C, 4D, 5E, 6F,...): ");
                            string? preFormatMarkScheme = Console.ReadLine();
                            string[] markScheme = preFormatMarkScheme == null ? [] : preFormatMarkScheme.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine("Enter the passMark for the test (ex: 75%): ");
                            string? passMark = Console.ReadLine().Trim();
                            TestPaper paperMade = new TestPaper(testName, markScheme, passMark);
                            // Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}"); //Debug print
                            // Console.WriteLine($"Full file path: {Path.GetFullPath(testsPath)}"); //Debug print
                            try
                            {
                                using StreamWriter writer = new StreamWriter(testsPath, append: true);
                                string markSchemeString = string.Join("|", paperMade.MarkScheme);
                                writer.WriteLine($"{paperMade.Subject},{markSchemeString},{paperMade.PassMark}");
                                Console.WriteLine($"Successfully wrote to {testsPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error writing to file: {ex.Message}");
                            }
                            Console.WriteLine("Test has been saved.\n");
                            break;
                        case "b" or "B": // Edit
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "c" or "C": // Delete
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "d" or "D": // View
                            try
                            {
                                using StreamReader viewTests = new(new FileStream(testsPath, FileMode.Open, FileAccess.Read));
                                string line;
                                int i = 1;
                                Console.WriteLine("\nTests:");
                                while ((line = viewTests.ReadLine()) != null)
                                {
                                    string[] lineParts = line.Split(',');
                                    // lineParts[1].Replace('|',)
                                    Console.WriteLine($"{i++}- Name: {lineParts[0]} MarkScheme: {lineParts[1]} PassMark: {lineParts[2]}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error reading from file: {ex.Message}");
                            }
                            Console.WriteLine();
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, retry.");
                            break;
                    }
                    break;
                case "2": // Students
                    Console.WriteLine();
                    option2.PrintMenuItem();
                    Console.WriteLine("Choose a sub option (a,b,c,d,e): ");
                    string? option2SubChoice = Console.ReadLine().Trim();
                    switch (option2SubChoice)
                    {
                        case "a" or "A": // Add
                            Console.WriteLine("Enter the name of the student: ");
                            string? studentName = Console.ReadLine().Trim();
                            Student studentMade = new Student(studentName);
                            // Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}"); //Debug print
                            // Console.WriteLine($"Full file path: {Path.GetFullPath(testsPath)}"); //Debug print
                            try
                            {
                                using StreamWriter writer = new StreamWriter(studentsPath, append: true);
                                writer.WriteLine($"{studentMade._id},{studentMade._name},{string.Join(",", studentMade.TestsTaken)}");
                                Console.WriteLine($"Successfully wrote to {studentsPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error writing to file: {ex.Message}");
                            }
                            Console.WriteLine("Student has been saved.\n");
                            break;
                        case "b" or "B": // Edit
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "c" or "C": // Delete
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "d" or "D": // Give Test
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "e" or "E": // View Scores
                            try
                            {
                                using StreamReader viewStudents = new(new FileStream(studentsPath, FileMode.Open, FileAccess.Read));
                                string line;
                                int i = 1;
                                Console.WriteLine("\nStudents:");
                                while ((line = viewStudents.ReadLine()) != null)
                                {
                                    string[] lineParts = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                                    Console.WriteLine($"{i++}- ID: {lineParts[0]}| Name: {lineParts[1]}| Tests Taken: {lineParts[2]}");
                                    // Only grabs first part of tests taken string array since the method for making a student taking a test is not needed yet for the program
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error reading from file: {ex.Message}");
                            }
                            Console.WriteLine();
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, retry.");
                            break;
                    }
                    break;
                case "3": // Give all students a test
                    Console.WriteLine();
                    Console.WriteLine($"{option3._name}");
                    Console.WriteLine("Select test: ");
                    string? option3TestChoice = Console.ReadLine();
                    break;
                case "4": // Have student take all tests
                    Console.WriteLine();
                    option4.PrintMenuItem();
                    Console.WriteLine("Select student: ");
                    string? option4StudentChoice = Console.ReadLine();
                    break;
                case "5": // Exit
                    Console.WriteLine();
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid option, retry.");
                    break;
            }
        }

        // Old code form version 1
        // Trauner_A1.TestPaper paper1 = new Trauner_A1.TestPaper("Maths", new string[] { "1A", "2C", "3D", "4A", "5A" }, "60%");
        // Trauner_A1.TestPaper paper2 = new Trauner_A1.TestPaper("Chemistry", new string[] { "1C", "2C", "3D", "4A" }, "75%");
        // TestPaper paper3 = new TestPaper("Computing", new string[] { "1D", "2C", "3C", "4B", "5D", "6C", "7A" }, "75%");

        // Trauner_A1.Student student1 = new Trauner_A1.Student();
        // Student student2 = new Student("student2");

        // Console.WriteLine(string.Join(", ", student1.TestsTaken));
        // student1.TakeTest(paper1, new string[] { "1A", "2D", "3D", "4A", "5A" });
        // Console.WriteLine(string.Join(", ", student1.TestsTaken));

        // student2.TakeTest(paper2, new string[] { "1C", "2D", "3A", "4C" });
        // student2.TakeTest(paper3, new string[] { "1A", "2C", "3A", "4C", "5D", "6C", "7B" });
        // Console.WriteLine(string.Join(", ", student2.TestsTaken));
    }


    // Unused methods, might implement in final version
    static void loadTests(string testsPath) //testPapersList
    {
        try
        {
            using StreamReader viewTests = new(new FileStream(testsPath, FileMode.Open, FileAccess.Read));
            string line;
            while ((line = viewTests.ReadLine()) != null)
            {
                string[] lineParts = line.Split(',');
                string[] markScheme = lineParts[1] == null ? [] : lineParts[1].Split("|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                testPapersList.Add(new TestPaper(lineParts[0], markScheme, lineParts[2]));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tests from file: {ex.Message}");
        }
    }

    static void loadStudents(string studentsPath) //studentsList
    {
        try
        {
            using StreamReader viewStudents = new(new FileStream(studentsPath, FileMode.Open, FileAccess.Read));
            string line;
            while ((line = viewStudents.ReadLine()) != null)
            {
                studentsList.Add(new Student(line));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading students from file: {ex.Message}");
        }
    }
}
