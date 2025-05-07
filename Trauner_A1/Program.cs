

using Trauner2;

/**
Name: Derek Trauner
COP2362 - Assignment 1
Mar 31, 2025
Summary Statement: I worked on this alone, used ai to help debug type conversions;

<summary>
    This program contains Types to create Student and TestPaper objects.
    Testpaper implements ITestPaper, an interface for general TestPaper properties.
    Student implements IStudent, an interface for general Student properties and methods.
        -Causes every instance of Student to have those properties and Student must implement those methods from IStudent.
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
        string[] TestsTaken { get; }
        void TakeTest(TestPaper paper, string[] answers);
    }

    public class Student : IStudent
    {
        private List<string> testsTaken = new List<string>();

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



namespace Trauner2 // Namespace containins code for version 2, menu items and read/write functions
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

        public void HandleUserChoice(string userChoice)
        {

        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        ConsoleMenuItem option1 = new ConsoleMenuItem(Console.In, Console.Out, "1. TESTS", false, null);
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
        while (running)
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
                case "1":
                    Console.WriteLine();
                    option1.PrintMenuItem();
                    Console.WriteLine("Choose a sub option (a,b,c,d): ");
                    string? option1SubChoice = Console.ReadLine();
                    switch (option1SubChoice)
                    {
                        case "a":
                            Console.WriteLine("Enter the name for the test: ");
                            string? testName = Console.ReadLine();
                            Console.WriteLine("Enter the markScheme for the test (1A, 2B, 3C, 4D, 5E, 6F,...): ");
                            string? preFormatMarkScheme = Console.ReadLine();
                            string[] markScheme = preFormatMarkScheme == null ? [] : preFormatMarkScheme.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine("Enter the passMark for the test (ex: 75%): ");
                            string? passMark = Console.ReadLine();
                            Trauner_A1.TestPaper paperMade = new Trauner_A1.TestPaper(testName, markScheme, passMark);
                            break;
                        case "b":
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "c":
                            Console.WriteLine("Not finished yet.");
                            break;
                        case "d":

                            break;
                        default:
                            break;
                    }
                    break;
                case "2":
                    Console.WriteLine();
                    option2.PrintMenuItem();
                    Console.WriteLine("Choose a sub option (a,b,c,d,e): ");
                    string? option2SubChoice = Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine();
                    Console.WriteLine($"{option3._name}");
                    Console.WriteLine("Select test: ");
                    string? option3TestChoice = Console.ReadLine();
                    break;
                case "4":
                    Console.WriteLine();
                    option4.PrintMenuItem();
                    Console.WriteLine("Select student: ");
                    string? option3StudentChoice = Console.ReadLine();
                    break;
                case "5":
                    Console.WriteLine();
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;
                case "6":
                    Trauner_A1.TestPaper paper1 = new Trauner_A1.TestPaper("Maths", new string[] { "1A", "2C", "3D", "4A", "5A" }, "60%");
                    Trauner_A1.TestPaper paper2 = new Trauner_A1.TestPaper("Chemistry", new string[] { "1C", "2C", "3D", "4A" }, "75%");
                    Trauner_A1.TestPaper paper3 = new Trauner_A1.TestPaper("Computing", new string[] { "1D", "2C", "3C", "4B", "5D", "6C", "7A" }, "75%");

                    Trauner_A1.Student student1 = new Trauner_A1.Student();
                    Trauner_A1.Student student2 = new Trauner_A1.Student();

                    Console.WriteLine(string.Join(", ", student1.TestsTaken));
                    student1.TakeTest(paper1, new string[] { "1A", "2D", "3D", "4A", "5A" });
                    Console.WriteLine(string.Join(", ", student1.TestsTaken));

                    student2.TakeTest(paper2, new string[] { "1C", "2D", "3A", "4C" });
                    student2.TakeTest(paper3, new string[] { "1A", "2C", "3A", "4C", "5D", "6C", "7B" });
                    Console.WriteLine(string.Join(", ", student2.TestsTaken));
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid option, retry.");
                    break;
            }
        }



    }
}
