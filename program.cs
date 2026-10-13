using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Threads;

class Course
{
    public Dictionary<int, int> course_availseats = new();
    public Dictionary<int, object> courseLocks = new();

    private readonly object courseLock = new();

    public Course(int courses = 100, int availseats = 10_000)
    {
        for (int i = 1; i <= courses; i++)
        {
            course_availseats[i] = availseats;

            courseLocks[i] = new object();
        }
    }

    public void DecrementSeat(int courseId)
    {
        lock (courseLock)
        {
            course_availseats[courseId]--;
        }
    }
}
class Student
{
    public long id;
    public List<int> courses;

    public Student(long in_id)
    {
        id = in_id;
        courses = new();
    }

    public void SelectCoursesForRegistration(int num)
    {
        for (int i = 0; i < num; i++)
        {
            courses.Add(Random.Shared.Next(1, 100));
        }

    }

    public bool RegisterMyCourses()
    {
        // var orderedCourses = courses.OrderBy(c => c).ToList();

        foreach (var c in courses)
        {
            int first = Math.Min(courses[0], courses[1]);
            int second = Math.Max(courses[0], courses[1]);
            
            lock (Program.c.courseLocks[courses[0]])
            {
                Thread.Sleep(10);
                lock (Program.c.courseLocks[courses[1]])
                {
                    Program.c.course_availseats[courses[0]]--;
                    Program.c.course_availseats[courses[1]]--;
                }
            }
        }
        Console.WriteLine($"Student {id}: Finished registrations");
        return true;
    }
}
class Program
{
    public static Course c = new();
    // 1) Create a thread to do N (1000) registrations
    //create student
    //student.SelectCoursesForRegistration
    //student.RegisterMyCourses
    static void Main(string[] args)
    {
        int studentCount = 100;
        List<Thread> threads = new();

        for (int i = 1; i < studentCount; i++)
        {
            Student student = new Student(i);
            student.SelectCoursesForRegistration(2);

            Thread t = new Thread(() =>
            {
                student.RegisterMyCourses();
            });

            threads.Add(t);
            t.Start();

        }
        foreach (var t in threads)
        {
            t.Join();
        }

        Console.WriteLine("\n All registrations completed. \n");
    }
}