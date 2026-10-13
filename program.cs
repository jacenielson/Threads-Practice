using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Threads;

class Course
{
    public Dictionary<int, int> course_availseats = new();
    public Dictionary<int, object> courseLocks = new();

    // private readonly object courseLock = new();

    public Course(int courses = 100, int availseats = 10_000)
    {
        for (int i = 1; i <= courses; i++)
        {
            course_availseats[i] = availseats;

            courseLocks[i] = new object();
        }
    }

    // public void DecrementSeat(int courseId)
    // {
    //     lock (courseLock)
    //     {
    //         course_availseats[courseId]--;
    //     }
    // }
}
class Student
{
    public int id;
    public List<int> courses;

    public Student(int in_id)
    {
        id = in_id;
        courses = new();
    }

    public void SelectCoursesForRegistration(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int pick = Random.Shared.Next(1, 100);
            if (!courses.Contains(pick))
                courses.Add(pick);
        }

    }

    public bool RegisterMyCourses()
    {

        // foreach (var c in courses)
        // {
        // int first = Math.Min(courses[0], courses[1]);
        // int second = Math.Max(courses[0], courses[1]);

        // lock (Program.c.courseLocks[courses[first]])
        // {
        //     Thread.Sleep(10);
        //     lock (Program.c.courseLocks[courses[second]])
        //     {
        //         Program.c.course_availseats[courses[0]]--;
        //         Program.c.course_availseats[courses[1]]--;
        //     }
        // }
        // }
        // Console.WriteLine($"Student {id}: Finished registrations");
        // return true;

        var ordered = courses.OrderBy(c => c).ToList();
        if (GetAndRegister(ordered, 0))
        {
            Console.WriteLine($"Student {id}: Finished registrations");
            return true;
        }
        return false;

    }
    private bool GetAndRegister(List<int> ordered, int index)
    {

        if (index == ordered.Count) //once i = course count, end
        {
            bool allRegistered = true;
            foreach (var c in ordered)
            {
                if (Program.c.course_availseats[c] > 0)
                    Program.c.course_availseats[c]--;
                else
                    allRegistered = false; 
            }
            return allRegistered;
        }
        lock (Program.c.courseLocks[ordered[index]])
        {
            // Thread.Sleep(10);
            return GetAndRegister(ordered, index + 1);
        }
    }
    class Program
    {
        public static Course c = new();
        static void Main(string[] args)
        {
            int studentCount = 100;
            List<Thread> threads = new();

            for (int i = 1; i < studentCount; i++)
            {
                Student student = new Student(i);
                student.SelectCoursesForRegistration(5);

                Thread t = new Thread(() =>
                {
                    if(!student.RegisterMyCourses())
                    Console.WriteLine($"Student {student.id} failed to register course list.");
                });

                threads.Add(t);
                t.Start();

            }
            foreach (var t in threads)
            {
                t.Join();
            }
            Console.WriteLine("\nAll seats assigned\n");
            foreach (var x in c.course_availseats)
            {
                Console.WriteLine($"Course {x.Key} has {x.Value} seats still aailable.");
            }

            Console.WriteLine("\n All registrations completed. \n");
        }
    }
}