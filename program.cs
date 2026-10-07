using System.Drawing;

namespace Threads;

class Program
{
    static void Main(string[] args)
    {
        object NielsonLock_balance = new();
        int balance = 1000;
        void Atm()
        {
            lock (NielsonLock_balance)                      // makes code accurate -> balance will always be 1000
            {
                for (int i = 0; i < 1_000_000; i++)
                {
                    balance++;
                    balance--;

                }
            }
        }
        var t1 = new Thread(Atm);
        var t2 = new Thread(Atm);
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
        Console.WriteLine($"Final Balance: {balance}");
    }
}