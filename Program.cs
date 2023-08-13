using System;
using System.Collections.Generic;
using System.Threading;

namespace CountdownTimerConsoleApp
{
    class Program
    {
        static List<CountdownEvent> countdownEvents = new List<CountdownEvent>();
        static Timer reminderTimer;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            reminderTimer = new Timer(CheckReminders, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\u001b[32mCountdown Timer Application\u001b[0m\n"); // Green text

                Console.WriteLine("1. \u001b[33mAdd Event\u001b[0m"); // Yellow text
                Console.WriteLine("2. \u001b[34mShow Countdowns\u001b[0m"); // Blue text
                Console.WriteLine("3. \u001b[31mExit\u001b[0m"); // Red text

                Console.Write("\nSelect an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddEvent();
                        break;
                    case "2":
                        ShowCountdowns();
                        break;
                    case "3":
                        reminderTimer.Dispose();
                        return;
                    default:
                        Console.WriteLine("\u001b[31mInvalid option. Please try again.\u001b[0m"); // Red text
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void AddEvent()
        {
            Console.Clear();
            Console.WriteLine("\u001b[33mAdd Event\u001b[0m\n");

            Console.Write("Enter event name: ");
            string eventName = Console.ReadLine();

            Console.Write("Enter event date and time (yyyy-MM-dd HH:mm:ss): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime eventDate))
            {
                countdownEvents.Add(new CountdownEvent(eventName, eventDate));
                Console.WriteLine("\u001b[32mEvent added successfully!\u001b[0m"); // Green text
            }
            else
            {
                Console.WriteLine("\u001b[31mInvalid date and time format.\u001b[0m"); // Red text
            }
        }

        static void ShowCountdowns()
        {
            Console.Clear();
            Console.WriteLine("\u001b[34mCountdowns\u001b[0m\n"); // Blue text

            foreach (CountdownEvent ev in countdownEvents)
            {
                string timeLeft = ev.TimeLeft.ToString(@"dd\.hh\:mm\:ss");
                if (ev.TimeLeft <= TimeSpan.Zero)
                {
                    timeLeft = "\u001b[31mEvent Expired\u001b[0m"; // Red text
                }

                Console.WriteLine($"\u001b[33m{ev.EventName}\u001b[0m - Time left: {timeLeft}");
            }
        }

        static void CheckReminders(object state)
        {
            foreach (CountdownEvent ev in countdownEvents)
            {
                if (ev.TimeLeft > TimeSpan.Zero && ev.TimeLeft <= TimeSpan.FromMinutes(5))
                {
                    Console.WriteLine($"\u001b[31mReminder: {ev.EventName} is about to occur!\u001b[0m");
                }
            }
        }
    }

    public class CountdownEvent
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan TimeLeft => EventDate - DateTime.Now;

        public CountdownEvent(string eventName, DateTime eventDate)
        {
            EventName = eventName;
            EventDate = eventDate;
        }
    }
}
