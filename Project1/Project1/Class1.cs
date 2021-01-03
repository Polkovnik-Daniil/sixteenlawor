using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Project1
{
    class Class1
    {
        static void Factorial(int x)
        {
            int result = 1;

            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            Console.WriteLine($"Выполняется задача {Task.CurrentId}");
            Console.WriteLine($"Факториал числа {x} равен {result}");
            Thread.Sleep(3000);
        }
        static void Create(int x)
        {
            int[] mas = new int[x];
            Console.WriteLine($"Выполняется задача {Task.CurrentId} x = {x}");
            Thread.Sleep(100);
        }
        //Простейший метод, возвращающий результат и не принимающий аргументов,
        static bool MyTask()
        {
            return true;
        }
        static int SumIt(object v)
        {
            int x = (int)v;
            int sum = 0;
            for (; x > 0; x--)
                sum += x;
            return sum;
        }
        public static void Display()
        {
            Console.Write("N = 100\n");
            var n = 100;
            Console.WriteLine("Простые числа из диапазона ({0}, {1})", 0, n);
            for (var i = 0u; i < n; i++)
            {
                if (IsPrimeNumber(i))
                {
                    Console.Write($"{i} ");
                }
            }
        }
        public static bool IsPrimeNumber(uint n)
        {
            var result = true;
            if (n > 1)
            {
                for (var i = 2u; i < n; i++)
                {
                    if (n % i == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }
        static void FirstFunc(int number)
        {
            for (int i = 0; i < number; i++)
                Console.WriteLine(i);
        }
        //Предоставляет возможности блокировки и ограничения для поточно- ориентированных коллекций
        static BlockingCollection<int> bc;
        static void Prod()
        {
            for (int i = 0; i < 10; i++)
            {
                //добваление в коллекцию
                bc.Add(i * i);
                Console.WriteLine("Производится число " + i * i);
            }
            //Заканчиваю добавлять в коллекцию
            bc.CompleteAdding();
        }
        static void Cons()
        {
            int i;
            while (!bc.IsCompleted)            //Получает значение, указывающее, завершена ли задача.
            {
                if (bc.TryTake(out i))                //удаление эл-та из коллекции BlockCollection
                    Console.WriteLine("Потребляется число: " + i);
            }
        }
        static async void FactorialAsync()
        {
            Console.WriteLine("Начало метода FactorialAsync"); // выполняется синхронно
            await Task.Run(() => Factorial1());                // выполняется асинхронно
            Console.WriteLine("Конец метода FactorialAsync");
        }
        static void Factorial1()
        {
            int result = 1;
            for (int i = 1; i <= 6; i++)
            {
                result *= i;
            }
            Thread.Sleep(8000);
            Console.WriteLine($"Факториал равен {result}");
        }
        static void SecondFunc(int number)
        {
            for (int i = number; i > 0; i--)
                Console.WriteLine(i);
        }
        static void Display1(Task t)
        {
            Console.WriteLine($"Id задачи: {Task.CurrentId}");
            Console.WriteLine($"Id предыдущей задачи: {t.Id}");
            Thread.Sleep(3000);
        }
        static void Main(string[] args)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            Task task = new Task(Display);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            task.Start();
            task.Wait();
            stopWatch.Stop();
            //произойдет по истечении интервала времени
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("\nRunTime " + elapsedTime);
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //для прерывания задачи
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            //получаем токен
            CancellationToken token = cancelTokenSource.Token;
            int number = 6;
            Task task1 = new Task(() =>
            {
                Console.Write("N = 100\n");
                var n = 100;
                Console.WriteLine("Простые числа из диапазона ({0}, {1})", 0, n);
                for (var i = 0u; i < n; i++)
                {
                    if (token.IsCancellationRequested)//Получает, была ли запрошена отмена для этого токена.
                    {
                        Console.WriteLine("Операция прервана");
                        Thread.Sleep(1000);
                        return;
                    }
                    if (IsPrimeNumber(i))
                    {
                        Thread.Sleep(200);
                        Console.Write($"{i} ");
                    }
                }
            });
            stopWatch.Start();
            task1.Start();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("Введите Y для отмены операции или другой символ для ее продолжения:");
            string s = Console.ReadLine();
            if (s == "y")
                cancelTokenSource.Cancel();
            task.Wait();
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("\nRunTime " + elapsedTime);
            Console.Clear();
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Основной поток запущен.");
            //Сконструировать объект первой задачи.
            Task<bool> tsk = Task<bool>.Factory.StartNew(MyTask);
            Console.WriteLine("Результат после выполнения задачи 1: "
                     + tsk.Result);
            //Сконструировать объект второй задачи.
            Task<int> tsk2 = Task<int>.Factory.StartNew(SumIt, 3);
            Console.WriteLine("Результат после выполнения задачи 2: "
                     + tsk2.Result);
            //Сконструировать объект второй задачи.
            Task<int> tsk3 = Task<int>.Factory.StartNew(SumIt, 4);
            Console.WriteLine("Результат после выполнения задачи 3: "
                     + tsk3.Result);
            //Сконструировать объект второй задачи.
            Task<int> tsk4 = Task<int>.Factory.StartNew(SumIt, 5);
            Console.WriteLine("Результат после выполнения задачи 4: "
                     + tsk4.Result);
            if (tsk.Result == true)
            {
                int res = tsk2.Result + tsk3.Result + tsk4.Result;
                Console.WriteLine("Результат после выполнения всех задач: " + res);
            }
            tsk.Dispose();
            tsk2.Dispose();
            /////////////////////////////////////////////////////////////////////////////////////////////////////

            Task.Run(() => FirstFunc(10)).ContinueWith(task90 => SecondFunc(10));

            Task task12 = new Task(() =>
            {
                Console.WriteLine($"Id задачи: {Task.CurrentId}");
            });

            //задача продолжения
            Task task2 = task12.ContinueWith(Display1);//позволяют определить задачи, которые выполняются после завершения других задач
            task12.Start();


            //ждем окончания второй задачи
            task2.Wait();

            Parallel.For(1, 12, Factorial);// Класс Parallel предназначен для упрощения параллельного выполнения кода.
            Thread.Sleep(1000);
            Console.Clear();
            ParallelLoopResult result = Parallel.ForEach<int>(new List<int>() { 1, 3, 5, 8 }, Factorial);// Метод Parallel.ForEach осуществляет итерацию по коллекции, реализующей интерфейс IEnumerable, подобно циклу foreach, только осуществляет параллельное выполнение перебора.

             Parallel.For(1, 100, Create);
            var dict1 = new Dictionary<int, string>();//Словарь хранит объекты, которые представляют пару ключ-значение.
            var dict2 = new Dictionary<int, string>();
            DateTime t1 = DateTime.Now;
            Parallel.Invoke( //Выполняет каждое из указанных действий, возможно, параллельно.
                   () =>
                   {
                       for (int i = 0; i < 10; i++)
                       {
                           dict1.Add(i, "Test " + i);
                           Console.WriteLine("Element list(dict1): " + dict1[i]);
                       }
                   },
                   () =>
                   {
                       for (int i = 0; i < 10; i++)
                       {
                           dict2.Add(i, "Test " + i);
                           Console.WriteLine("Element list(dict2): " + dict2[i]);
                       }
                   }
            );
            TimeSpan t2 = DateTime.Now.Subtract(t1);
            Console.WriteLine(t2.TotalMilliseconds);

            bc = new BlockingCollection<int>(4);

            // Создадим задачи поставщика и потребителя
            Task Pr = new Task(Prod);
            Task Cn = new Task(Cons);

            // Запустим задачи
            Pr.Start();
            Cn.Start();

            try
            {
                Task.WaitAll(Cn, Pr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Cn.Dispose();
                Pr.Dispose();
                bc.Dispose();
            }

            FactorialAsync();   // вызов асинхронного метода
            Console.Clear();
            Console.WriteLine("Введите число: ");
            var n1 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Квадрат числа равен {n1 * n1}");


            Console.WriteLine("Завершение Main");
            Console.ReadLine();
        }
    }
}