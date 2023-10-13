using System;
using System.Collections.Concurrent;
using System.Threading;

namespace PZ5
{
    
        class Program
        {
            private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            private static BlockingCollection<int> progressionValues = new BlockingCollection<int>(); // Используем BlockingCollection

            static void Main()
            {
                Thread thread1 = new Thread(PrintProgression);
                Thread thread2 = new Thread(GenerateProgression);

                thread1.Start(cancellationTokenSource.Token);
                thread2.Start(cancellationTokenSource.Token);
                Console.ReadLine(); // Даем потокам время выполниться

                // Останавливаем потоки
                cancellationTokenSource.Cancel();
                Console.WriteLine("Поток 2 был завершён.");
            }

            static void GenerateProgression(object token)
            {
                CancellationToken cancellationToken = (CancellationToken)token;


                int t1 = 2; // Первый член прогрессии
                int t2 = 2; // Знаменатель прогрессии

                for (int i = 0; i < 10; i++)
                {
                    int term = t1 * (int)Math.Pow(t2, i);

                    if (term == 16)
                    {
                        Console.WriteLine("Второй поток временно заблокирован.");
                        Thread.Sleep(5000); // Блокировка на 5 секунд
                        Console.WriteLine("Второй поток разблокирован.");
                    }
                //if (term == 32)
                if (term == 64)
                    {
                        cancellationTokenSource.Cancel(); // Запрос на прерывание потока
                        break;
                    }

                    progressionValues.Add(term); // Добавляем в BlockingCollection

                    Thread.Sleep(1000); // Пауза 
                    if (cancellationToken.IsCancellationRequested) break; // Проверка запроса на прерывание потока
            }
                progressionValues.CompleteAdding(); // Сообщаем, что больше не будем добавлять элементы
            }

        static void PrintProgression(object token)
        {
            CancellationToken cancellationToken = (CancellationToken)token;
            int perem = 0;

            foreach (int value in progressionValues.GetConsumingEnumerable())
            {
                Console.WriteLine($"Итерации: число {value}");
                Console.WriteLine("Ждём...");
                Thread.Sleep(1000); // Пауза между выводами
                perem = value;
            }
            Console.WriteLine($"Достигнуто конечное значение - {perem * 2}.");
            Console.WriteLine("Первый поток был завершён.");
            Console.WriteLine("Введите любой символ для завершения");
        }
    }
}

   
