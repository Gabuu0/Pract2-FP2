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
                    try
                    {
                        Direction dir = (Direction)Enum.Parse(typeof(Direction), partes[1], true);
                        w.Move(m, dir);
                        Console.WriteLine("Te moviste ha " + m.GetPlaceName(w.GetPosition()));
                    }
                    catch
                    {
                        Console.WriteLine("Dirección no válida. Usa: north, south, east, west.");
                    }
                    break;

                case "pick":
                    try
                    {
                        int itemIndex = int.Parse(partes[1]);
                        w.PickItem(m, itemIndex);
                        Console.WriteLine("Dejaste " + m.GetItemName(m.TheItemInPlace(w.GetPosition(), itemIndex)));
                    }
                    catch
                    {
                        Console.WriteLine("Índice de ítem no válido.");
                    }
                    break;

                case "drop":
                    try
                    {
                        int dropIndex = int.Parse(partes[1]);
                        w.DropItem(m, dropIndex);
                        Console.WriteLine("Dejaste " + m.GetItemName(m.TheItemInPlace(w.GetPosition(), dropIndex)));
                    }
                    catch 
                    { 
                        Console.WriteLine("Índice de mochila no válido.");
                    }
                    break;

                case "look":
                    Console.WriteLine(m.GetPlaceName(w.GetPosition()) + ":");
                    Console.WriteLine(m.GetPlaceInfo(w.GetPosition()));
                    Console.WriteLine();
                    Console.WriteLine("-Caminos disponibles:");
                    Console.WriteLine(m.GetMoves(w.GetPosition()));
                    Console.WriteLine();
                    Console.WriteLine("-Objetos en este lugar:");
                    Console.WriteLine(m.GetItemsPlace(w.GetPosition()));
                    break;

                case "bag":
                    if(w.Bag(m) == "")
                    {
                        Console.WriteLine("Mochila vacia");
                    }
                    else
                    {
                        Console.WriteLine("Mochila:");
                        Console.WriteLine(w.Bag(m));
                    }
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
                }
            }

            Console.WriteLine("Ítems recogidos por WALL·E:");
            Console.WriteLine(w.Bag(map));
        }
    }
}
