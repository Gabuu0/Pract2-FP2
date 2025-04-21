using System;
using System.IO;

namespace WallE
{
    class Program
    {
        static void ProcesaInput(string com, WallE w, Map m)
        {
            string[] partes = com.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0) return; //Si no se ha escrito nada

            switch (partes[0].ToLower())
            {
                case "move":
                    if (partes.Length > 1 && Enum.TryParse<Direction>(partes[1], true, out Direction dir))
                    {
                        w.Move(m, dir);
                    }
                    else
                    {
                        Console.WriteLine("Dirección no válida. Usa: north, south, east, west.");
                    }
                    break;

                case "pick":
                    if (partes.Length > 1 && int.TryParse(partes[1], out int itemIndex))
                    {
                        w.PickItem(m, itemIndex);
                    }
                    else
                    {
                        Console.WriteLine("Índice de ítem no válido.");
                    }
                    break;

                case "drop":
                    if (partes.Length > 1 && int.TryParse(partes[1], out int dropIndex))
                    {
                        w.DropItem(m, dropIndex);
                    }
                    else
                    {
                        Console.WriteLine("Índice de mochila no válido.");
                    }
                    break;

                case "look":
                    Console.WriteLine(m.GetPlaceInfo(w.GetPosition()));
                    Console.WriteLine("Caminos disponibles:");
                    Console.WriteLine(m.GetMoves(w.GetPosition()));
                    Console.WriteLine("Objetos en este lugar:");
                    Console.WriteLine(m.GetItemsPlace(w.GetPosition()));
                    break;

                case "bag":
                    Console.WriteLine("Mochila:");
                    Console.WriteLine(w.Bag(m));
                    break;

                case "quit":
                    Console.WriteLine("Saliendo del juego...");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Comando no reconocido.");
                    break;
            }
        }

        static void Main(string[] args)
        {
            string filePath = "madrid.txt";

            Map map = new Map(10, 10);
            map.ReadMap(filePath);

            WallE w = new WallE();

            Console.WriteLine("¡Bienvenido al mundo de WALL·E!");
            Console.WriteLine("Comandos: move <dir>, pick <idx>, drop <idx>, look, bag, quit");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                ProcesaInput(input, w, map);

                if (w.AtSpaceShip(map))
                {
                    Console.WriteLine("¡Has llegado a la nave espacial!");
                    break;
                }
            }

            Console.WriteLine("Ítems recogidos por WALL·E:");
            Console.WriteLine(w.Bag(map));
        }
    }
}
