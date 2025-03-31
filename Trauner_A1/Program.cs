/**
Name: Derek Trauner
COP2362 - Assignment 1
Mar 29, 2025
Summary Statement: I worked on this alone, used ai to help debug type conversions;

<summary>
    This program contains Types to create Student and TestPaper objects.
    Testpaper implements ITestPaper, an interface for general TestPaper properties.
    Student implements IStudent, an interface for general Student properties and methods.
        -Causes every instance of Student to have those properties and Student must implement those methods from IStudent.
</summary>
 */

namespace Trauner_A1;

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
            if (testsTaken.Count == 0){
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


class Program
{
    static void Main(string[] args)
    {
        TestPaper paper1 = new TestPaper("Maths", new string[] { "1A", "2C", "3D", "4A", "5A" }, "60%");
        TestPaper paper2 = new TestPaper("Chemistry", new string[] { "1C", "2C", "3D", "4A" }, "75%");
        TestPaper paper3 = new TestPaper("Computing", new string[] { "1D", "2C", "3C", "4B", "5D", "6C", "7A" }, "75%");


        Student student1 = new Student();
        Student student2 = new Student();

        Console.WriteLine(string.Join(", ", student1.TestsTaken));
        student1.TakeTest(paper1, new string[] { "1A", "2D", "3D", "4A", "5A" });
        Console.WriteLine(string.Join(", ", student1.TestsTaken));

        student2.TakeTest(paper2, new string[] { "1C", "2D", "3A", "4C" });
        student2.TakeTest(paper3, new string[] { "1A", "2C", "3A", "4C", "5D", "6C", "7B" });
        Console.WriteLine(string.Join(", ", student2.TestsTaken));
    }
}
