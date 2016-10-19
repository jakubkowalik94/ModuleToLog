using System;
using System.Threading;

namespace ModuleToLog
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Random sleepTime = new Random();
            Test testObject = new Test();
            int x = 0;

            do
            {
                int i = sleepTime.Next(1, 50);

                Thread.Sleep(i);

                Thread oThread = new Thread(new ThreadStart(testObject.Start));

                oThread.Start();
                x++;
            } while (x < 100);
        }
    }

    internal class Test
    {
        private LogToFile ltf;

        public Test()
        {
            ltf = new LogToFile(@"D:\plikTest2.txt", 5);    //sciezka do pliku z logiem
        }

        public void Start()
        {
            ltf.TextToFile("Dziala");
        }
    }
}