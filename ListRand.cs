using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ListSerializer
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public void Serialize(Stream s)
        {
            // Словарь "ListNode - Индекс"
            var nodes = new Dictionary<ListNode, int>();
            for (var (node, index) = (Head, 0); node != null; node = node.Next, index++)
            {
                nodes.Add(node, index);
            }

            using (var writer = new BinaryWriter(s, Encoding.UTF8, false))
            {
                for (var node = Head; node != null; node = node.Next)
                {
                    // Пишем ListNode.Data и индекс ListNode.Rand (-1 если ListNode.Rand равен null)
                    writer.Write(node.Data);
                    writer.Write(node.Rand != null ? nodes[node.Rand] : -1);
                }
            }
        }

        public void Deserialize(Stream s)
        {
            // Словарь "Индекс ListNode - (данные, индекс ListNode.Rand )"
            var nodes = new Dictionary<int, (string data, int randIndex)>();
            var count = 0;
            using (var reader = new BinaryReader(s, Encoding.UTF8, false))
            {
                while (reader.PeekChar() != -1)
                {
                    var data = reader.ReadString();
                    var randIndex = reader.ReadInt32();
                    nodes.Add(count++, (data, randIndex));
                }

                Count = count;
                var nodesList = new List<ListNode>(Count);

                Head = new ListNode();
                for (var (node, i) = (Head, 0); node != null && i < Count; node = node.Next, i++)
                {
                    nodesList.Add(node);
                    node.Data = nodes[i].data;
                    // Проверка на Tail ListNode для связи Prev с Next
                    if (i < Count - 1)
                    {
                        node.Next = new ListNode { Prev = node };
                    }
                    else
                    {
                        Tail = node;
                    }
                }

                for (var (node, i) = (Head, 0); node != null && i < Count; node = node.Next, i++)
                {
                    // Если индекс -1, пишем в ListNode.Rand null
                    node.Rand = nodes[i].randIndex != -1 ? nodesList[nodes[i].randIndex] : null;
                }
            }
        }
    }
}
