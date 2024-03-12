using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ListSerializer
{
    class Program
    {
        /// <summary>
        /// Генерирует ListRand
        /// </summary>
        /// <param name="count">Количество ListNode</param>
        /// <returns>ListRand</returns>
        public static ListRand GetListRand(int count)
        {
            var random = new Random();
            var nodes = new List<ListNode>();
            for (var i = 0; i < count; i++)
            {
                nodes.Add(new ListNode { Prev = null, Next = null, Rand = null, Data = $"{random.Next(0, count)}" });
            }
            var rands = count;
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                if (i > 0)
                    node.Prev = nodes[i - 1];
                if (i != nodes.Count - 1)
                    node.Next = nodes[i + 1];

                if (rands > 0)
                {
                    var randomNodeIndex = random.Next(0, count);
                    node.Rand = nodes[randomNodeIndex];
                    rands--;
                }
            }
            return new ListRand { Head = nodes.First(), Tail = nodes.Last(), Count = nodes.Count };
        }
        /// <summary>
        /// Вывод в консоль данных, хранящихся в ListRand
        /// </summary>
        /// <param name="head">Первый ListNode</param>
        public static void PrintListRand(ListNode head)
        {
            Console.WriteLine($"_______________________________________________{Environment.NewLine}");
            var node = head;
            while (node != null)
            {
                Console.WriteLine($"Node: {node.Data} \tPrev: {node.Prev?.Data ?? "NoData"} \tNext: {node.Next?.Data ?? "NoData"} \tRand: {node?.Data ?? "NoData"}");
                node = node.Next;
            }
            Console.WriteLine($"_______________________________________________{Environment.NewLine}");
        }
        static void Main(string[] args)
        {
            Console.Write("Введите количество ListNode : ");
            var count = Convert.ToInt32(Console.ReadLine());
            var listRand = GetListRand(count);
            
            // Путь к файлу для хранения сериализованных данных
            var fileName = @"data.dat";

            listRand.Serialize(new FileStream(fileName, FileMode.Create));
            PrintListRand(listRand.Head);

            try
            {
                listRand.Deserialize(new FileStream(fileName, FileMode.Open));
                PrintListRand(listRand.Head);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось обработать файл {fileName} : {ex.Message}");
            }
            
            Console.WriteLine($"{Environment.NewLine}Нажмите любую клавишу...{Environment.NewLine}");
            Console.ReadKey();
        }
    }
}
