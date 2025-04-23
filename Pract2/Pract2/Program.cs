//Gabriel García Bazán
//Alejandro Bueno Curbera
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

                    //si no se puede convertir en un Direction el string que sigue a move se informa al jugador
                    if(!Enum.TryParse(partes[1],true,  out Direction dir))
                    {
                        Console.WriteLine("Dirección no válida. Usa: north(0), south(1), east(2), west(3).");
                    }
                    else
                    {
                        w.Move(m, dir);
                    }
                    break;

                case "pick":
                    try
                    {
                        //si el string que sigue a pick no se puede convertir en int se informa al jugador
                        if (!int.TryParse(partes[1], out int itemIndex))
                        {
                            Console.WriteLine("El índice debe introducirse como  un número.");
                        }
                        else
                        {
                            Console.WriteLine("Cogiste " + m.GetItemName(m.TheItemInPlace(w.GetPosition(), itemIndex)));
                            w.PickItem(m, itemIndex);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case "drop":
                    try
                    {
                        //si el string que sigue a drop no se puede convertir en int se informa al jugador
                        if (!int.TryParse(partes[1], out int dropIndex))
                        {
                            Console.WriteLine("El índice debe introducirse como un número");
                        }
                        else
                        {
                            w.DropItem(m, dropIndex);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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

                case "help":
                    Console.WriteLine("Comandos:\n move <dir> \n Usa: north(0), south(1), east(2), west(3) \n\n pick <idx> \n\n drop <idx> \n\n look \n\n bag \n\n quit \n\n help \n\n clear");
                    break;

                case "clear":
                    Console.Clear();
                    Console.WriteLine("Comandos: move <dir>, pick <idx>, drop <idx>, look, bag, quit, help, clear");
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
            try
            {
                map.ReadMap(filePath);

                WallE w = new WallE();

                Console.WriteLine("¡Bienvenido al mundo de WALL·E!");
                Console.WriteLine("Comandos: move <dir>, pick <idx>, drop <idx>, look, bag, quit, help, clear");

                while (!w.AtSpaceShip(map))
                {
                    Console.Write("> ");
                    string input = Console.ReadLine();
                    ProcesaInput(input, w, map);
                }

                Console.WriteLine("¡Has llegado a la nave espacial!\nÍtems recogidos por WALL·E:");
                Console.WriteLine(w.Bag(map));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
