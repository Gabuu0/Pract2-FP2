//Gabriel García Bazán
//Alejandro Bueno Curbera
using Listas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public enum Direction { North, South, East, West };

    class Map
    {
        // items basursa
        struct Item
        {
            public string name, description;
        }
        // lugares del mapa
        struct Place
        {
            public string name, description;
            public bool spaceShip;
            public int[] connections; // vector de 4 componentes
                                      // con el lugar al norte, sur, este y oeste
                                      // -1 si no hay conexión en esa dirección
            public ListaEnlazada itemsInPlace; // lista de índices al vector ítems,
                                               // correspondientes a los ítems que hay en este lugar
        }
        Place[] places; // vector de lugares del mapa
        Item[] items; // vector de ítems que hay en el juego
        int nPlaces, nItems; // número de lugares y número de ítems


        //constructora del map, se establecen la cantidad de places e items
        public Map(int numPlaces, int numItems)
        {
            nPlaces = numPlaces;
            nItems = numItems;
            places = new Place[nPlaces];
            items = new Item[nItems];
        }
        public void ReadMap(string file)
        {
            int itemsCreados = 0;
            string s = "";
            StreamReader entrada = null;
            try
            {
                entrada = new StreamReader(file);
            }
            catch //Si salta excepción por el archivo se relanza esta excepción para informar que hay problemas con el archivo
            {
                throw new Exception("Nombre de archivo no encontrado o incorrecto");
            }

            try
            {
                while (!entrada.EndOfStream)
                {
                    s = entrada.ReadLine();
                    if (!string.IsNullOrWhiteSpace(s))       //Si la linea esta vacia pasa a la siguiente
                    {
                        string[] v = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        switch (v[0])
                        {
                            case "place":
                                CreatePlace(v, entrada);
                                break;
                            case "street":
                                CreateStreet(v);
                                break;
                            case "garbage":
                                CreateItem(v, itemsCreados);
                                itemsCreados++;
                                break;
                        }
                    }
                }
            }
            catch (Exception e) //Si hay alguna excepcion al leer el mapa se informa de en que hay error,
                                //del formato esperado y del lugar del archivo donde se ha detectado el error
            {
                throw new Exception($"{e.Message} \n\n Error encontrado en la línea del archivo con el siguiente aspecto: \n\n {s}");
            }
        }

        private void CreatePlace(string[] v, StreamReader texto)
        {
            //Solo entra si el formato de Place está bien: hay solo 4 "palabras" y la segunda se 
            //puede converitr en int
            if (v.Length == 4 && int.TryParse(v[1], out int pos))
            {
                places[pos].name = v[2];
                places[pos].spaceShip = (v[3] == "spaceShip");
                places[pos].itemsInPlace = new ListaEnlazada();
                places[pos].connections = new int[] { -1, -1, -1, -1 };
                places[pos].description = ReadDescription(texto);
            }
            else //se lanza excepción en caso de mal formato introducido
            {
                throw new Exception("Formato de creción de Places no válido.\nEl formato correcto es:\n place     <índice del place(0  -   nº places-1)> " +
                    "     Nombre    spaceShip o noSpaceShip\nseguido de una descripción que empiece en la línea siguiente.");
            }
        }
        private void CreateStreet(string[] v)
        {
            //Solo entra si el formato de street está bien: hay solo 4 "palabras", la segunda y cuarta
            //se pueden convertir en int, la tercera en un enum Direction y si los indices de los places
            //están dentro del array de Places

            if (v.Length == 4 && int.TryParse(v[1], out int posOri) && int.TryParse(v[3], out int posDes) &&
                Enum.TryParse<Direction>(v[2], true, out Direction direction) && (posOri >= 0 && posOri < nPlaces) && (posDes >= 0 && posDes < nPlaces))
            {
                places[posOri].connections[(int)direction] = posDes;


                //según la direccion dada se establece la conexion inversa del lugar destino con el origen
                switch (direction)
                {
                    case Direction.North:
                        places[posDes].connections[(int)Direction.South] = posOri;
                        break;
                    case Direction.South:
                        places[posDes].connections[(int)Direction.North] = posOri;
                        break;
                    case Direction.East:
                        places[posDes].connections[(int)Direction.West] = posOri;
                        break;
                    case Direction.West:
                        places[posDes].connections[(int)Direction.East] = posOri;
                        break;
                }
            }
            else //se lanza excepción en caso de mal formato introducido
            {
                throw new Exception("Formato de creación de Streets incorrecto\n El formato correcto es:\n street      <indice del placeOrigen(0  -   nº places-1)>     direccion       <indice del placeDestino(0  -   nº places-1)>");
            }
        }
        private void CreateItem(string[] v, int itemIndex)
        {
            //Solo entra si el formato de street está bien: hay minimo 4 "palabras", la tercera
            //se puede convertir en int (corresponde al lugar en que se encuentra el item) y si tanto
            //el indice del place y del item están dentro del array de Places e Items respectivamente
            if (v.Length > 4 && int.TryParse(v[2], out int placePos) && (placePos >= 0 && placePos < nPlaces) && (itemIndex >= 0 && itemIndex < nItems))
            {
                items[itemIndex].name = v[3];
                items[itemIndex].description = string.Join(' ', v.Skip(4)).Trim('"');

                places[placePos].itemsInPlace.InsertaFinal(itemIndex);
            }
            else //se lanza excepción en caso de mal formato introducido
            {
                throw new Exception("Formato de creación de Items incorrecto\n El formato correcto es:\n garbage      place     <indice del place(0  -   nº places-1)>     Nombre       Descripción entre comillas");
            }

        }

        private string ReadDescription(StreamReader f)
        {
            string description;
            string line;
            string[] w;

            //se leen líneas hasta que una empiece por comillas
            do
            {
                line = f.ReadLine();
                line = line.Trim();
                w = line.Split(' ');
            } while (!w[0].StartsWith('"'));

            line = line.Trim('"');
            description = line;
            //se leen lineas y se añaden a la descripción mientras que no termine por comillas
            while (!w[w.Length - 1].EndsWith('"'))
            {
                line = f.ReadLine();
                //se eliminan los espacios en blanco de los laterales de la linea
                line = line.Trim();
                //se divide la linea en un array con todas las palabras separadas por espacios
                w = line.Split(' ');
                //en caso de que pudiera tener se quitan las comillas de los laterales
                //y se añade la linea a la descripción junto a un salto de línea
                line = line.Trim('"');
                description += "\n" + line;
            }
            return description;

        }

        

        public string GetMoves(int pl)
        {
            // Inicializo variables
            string north = "", south = "", east = "", west = "";

            // Añado el nombre de las calles conectados en caso de que existan
            for (int i = 0; i < places[pl].connections.Length; i++)
            {
                if (places[pl].connections[i] != -1)
                {
                    switch (i)
                    {
                        case 0:
                            north = places[places[pl].connections[i]].name;
                            break;
                        case 1:
                            south = places[places[pl].connections[i]].name;
                            break;
                        case 2:
                            east = places[places[pl].connections[i]].name;
                            break;
                        case 3:
                            west = places[places[pl].connections[i]].name;
                            break;
                    }
                }
            }

            // Las meto en el return
            string result = "";

            if (north != "") result += $"north: {north}\n";
            if (east != "") result += $"east: {east}\n";
            if (south != "") result += $"south: {south}\n";
            if (west != "") result += $"west: {west}\n";

            return result;
        }

        public string GetItemsPlace(int pl)
        {
            //se mira cuantos elementos hay en el lugar dado
            int elems = places[pl].itemsInPlace.NumElems();
            string itemsPlace = "";


            //se van añadiendo al string que se devuelve el índice en la lista del lugar, el nombre
            //y la descripción de todos los items de la lista de este lugar
            for (int i = 0; i < elems; i++)
            {
                itemsPlace += $"{i} {items[places[pl].itemsInPlace.Nesimo(i)].name}  {items[places[pl].itemsInPlace.Nesimo(i)].description}\n";
            }

            return itemsPlace;
        }

        /// <summary>
        /// Este método se encarga de eliminar el elemento nº placeItemIndex de la lista del lugar dado
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="placeItemIndex"></param>
        public void PickItemPlace(int pl, int placeItemIndex)
        {
            places[pl].itemsInPlace.EliminaElto(places[pl].itemsInPlace.Nesimo(placeItemIndex));
        }

        /// <summary>
        /// Este método se encarga de añadir el elemento it a la lista del lugar dado
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="it"></param>
        public void DropItemPlace(int pl, int it)
        {
            places[pl].itemsInPlace.InsertaFinal(it);
        }


       
        public int Move(int pl, Direction dir)
        {
            int direc = (int)dir;
            //se ve con que place conecta en la direccion dada y lo devuelve
            int dest = places[pl].connections[direc];
            return dest;
        }


        /// <summary>
        /// Devuelve el valor de spaceShip del lugar dado
        /// </summary>
        /// <param name="pl"></param>
        /// <returns></returns>
        public bool IsSpaceShip(int pl)
        {
            return places[pl].spaceShip;
        }



        /// <summary>
        /// Devuelve el nombre del nº de item dado
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public string GetItemName(int itemIndex)
        {
            return items[itemIndex].name;
        }
        /// <summary>
        /// Devuelve la descripción del nº de item dado
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public string GetItemDescription(int itemIndex)
        {
            return items[itemIndex].description;
        }


        /// <summary>
        /// Devuelve el dato del item nº itemIndex de la lista del lugar dado
        /// </summary>
        /// <param name="placeId"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public int TheItemInPlace(int placeId, int itemIndex)
        {
            return places[placeId].itemsInPlace.Nesimo(itemIndex);
        }

        //devuelve el nombre del lugar dado
        public string GetPlaceName(int pl)  //Voluntario
        {
            return places[pl].name;
        }


        //devuelve la descripcion del lugar dado
        public string GetPlaceInfo(int pl)
        {
            return places[pl].description;
        }
    }//class Map
}
