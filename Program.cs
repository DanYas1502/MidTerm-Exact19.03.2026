using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalTaskTracker
{
    // Спільний контракт для всіх типів задач
    abstract class TaskItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; private set; }

        protected TaskItem(string title)
        {
            Title = title;
            IsCompleted = false;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public abstract string GetInfo();
    }

    // Базова звичайна задача
    class SimpleTask : TaskItem
    {
        public SimpleTask(string title) : base(title) { }

        public override string GetInfo()
        {
            string status = IsCompleted ? "Виконано" : "Не виконано";
            return $"[Звичайна задача] Назва: {Title} | Стан: {status}";
        }
    }

    // Задача з дедлайном
    class DeadlineTask : TaskItem
    {
        public DateTime Deadline { get; set; }

        public DeadlineTask(string title, DateTime deadline) : base(title)
        {
            Deadline = deadline;
        }

        public override string GetInfo()
        {
            string status = IsCompleted ? "Виконано" : "Не виконано";
            return $"[Задача з дедлайном] Назва: {Title} | Дедлайн: {Deadline:dd.MM.yyyy HH:mm} | Стан: {status}";
        }
    }

    // Задача з пріоритетом
    class PriorityTask : TaskItem
    {
        public int Priority { get; set; }

        public PriorityTask(string title, int priority) : base(title)
        {
            Priority = priority;
        }

        public override string GetInfo()
        {
            string status = IsCompleted ? "Виконано" : "Не виконано";
            return $"[Пріоритетна задача] Назва: {Title} | Пріоритет: {Priority} | Стан: {status}";
        }
    }

    // Додатковий третій тип задачі з категорією
    class CategoryTask : TaskItem
    {
        public string Category { get; set; }

        public CategoryTask(string title, string category) : base(title)
        {
            Category = category;
        }

        public override string GetInfo()
        {
            string status = IsCompleted ? "Виконано" : "Не виконано";
            return $"[Категоризована задача] Назва: {Title} | Категорія: {Category} | Стан: {status}";
        }
    }

    class TaskTracker
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(TaskItem task)
        {
            tasks.Add(task);
        }

        public void ShowAllTasks()
        {
            Console.WriteLine("\n=== Усі задачі ===");

            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач порожній.");
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].GetInfo()}");
            }
        }

        public void MarkTaskAsCompleted(int index)
        {
            if (index >= 0 && index < tasks.Count)
            {
                tasks[index].MarkAsCompleted();
                Console.WriteLine($"Задачу '{tasks[index].Title}' позначено як виконану.");
            }
            else
            {
                Console.WriteLine("Некоректний індекс задачі.");
            }
        }

        public void ShowIncompleteTasks()
        {
            Console.WriteLine("\n=== Лише невиконані задачі ===");

            var incompleteTasks = tasks.Where(t => !t.IsCompleted).ToList();

            if (incompleteTasks.Count == 0)
            {
                Console.WriteLine("Невиконаних задач немає.");
                return;
            }

            foreach (var task in incompleteTasks)
            {
                Console.WriteLine(task.GetInfo());
            }
        }

        public int CountCompletedTasks()
        {
            return tasks.Count(t => t.IsCompleted);
        }

        public void SearchTasksByKeyword(string keyword)
        {
            Console.WriteLine($"\n=== Пошук задач за ключовим словом: {keyword} ===");

            var foundTasks = tasks
                .Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundTasks.Count == 0)
            {
                Console.WriteLine("Нічого не знайдено.");
                return;
            }

            foreach (var task in foundTasks)
            {
                Console.WriteLine(task.GetInfo());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TaskTracker tracker = new TaskTracker();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ===== Рівень 1 =====
            Console.WriteLine("ДЕМОНСТРАЦІЯ РІВНЯ 1");
            tracker.AddTask(new SimpleTask("Зробити домашнє завдання"));
            tracker.AddTask(new SimpleTask("Прибрати кімнату"));

            tracker.ShowAllTasks();
            tracker.MarkTaskAsCompleted(0);
            tracker.ShowAllTasks();

            // ===== Рівень 2 =====
            Console.WriteLine("\nДЕМОНСТРАЦІЯ РІВНЯ 2");
            tracker.AddTask(new DeadlineTask("Здати лабораторну", new DateTime(2026, 3, 25, 23, 59, 0)));
            tracker.AddTask(new PriorityTask("Підготуватися до екзамену", 1));
            tracker.AddTask(new CategoryTask("Прочитати книгу", "Навчання"));

            tracker.ShowAllTasks();

            // ===== Рівень 3 =====
            Console.WriteLine("\nДЕМОНСТРАЦІЯ РІВНЯ 3");
            tracker.MarkTaskAsCompleted(3); 

            tracker.ShowIncompleteTasks();

            int completedCount = tracker.CountCompletedTasks();
            Console.WriteLine($"\nКількість виконаних задач: {completedCount}");

            tracker.SearchTasksByKeyword("зда");
            tracker.SearchTasksByKeyword("книгу");
        }
    }
}
