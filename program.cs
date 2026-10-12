using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Threads;

class Course
{
    public Dictionary<int, int> course_availseats = new();

    public Course(int courses = 100, int availseats = 10_000)
    {
        for (int i = 1; i <= courses; i++)
        {
            course_availseats[i] = availseats;
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

    void SelectCoursesForRegistration(int num)
    {
        for (int i = 0; i < num; i++)
        {
            courses.Add(Random.Shared.Next(1, 100));
        }

    }

    bool RegisterMyCourses()
    {
        foreach (var c in courses)
        {
            course_availseats[c].Value--;
            
        }
    }
}
class Program
{
    public static Course c = new();
    // 1) Create a thread to do N (1000) registrations
        //create student
        //student.SelectCoursesForRegistration
        //student.RegisterMyCourses
        //
    static void Main(string[] args)
    {

    }
}