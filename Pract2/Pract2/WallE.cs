using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Listas;
using System.ComponentModel;
using System.Xml.Linq;

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

        public Map(int numPlaces, int numItems)
        {
            nPlaces = numPlaces;
            nItems = numItems;
            places = new Place[nPlaces];
            items = new Item[nItems];
        }
        public void ReadMap(string file)
        {
            int itemsCreated = 0;
            StreamReader entrada = new StreamReader(file);
            while (!entrada.EndOfStream)
            {
                string s = entrada.ReadLine();

                if (!string.IsNullOrWhiteSpace(s))       //Si la linea esta vacia pasa a la siguiente
                {
                    string[] v = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    switch (v[0])
                    {
                        case "place":
                            CreatePlace(v, entrada);
                            //ReadDescription(entrada);
                            break;
                        case "street":
                            CreateStreet(v);
                            break;
                        case "garbage":
                            CreateItem(v, itemsCreated);
                            break;
                            itemsCreated++;
                    }
                }
            }
        }

        private void CreatePlace(string[] v, StreamReader texto)
        {
            int i = 1;
            int pos=0;
            foreach (string s in v)
            {
                try
                {
                    pos = int.Parse(s);
                }
                catch { }
            }

            places[pos].name = v[2];
            places[pos].spaceShip = (v[3]=="spaceShip");
            places[pos].itemsInPlace = new ListaEnlazada();
            places[pos].connections = new int [] { -1, -1, -1,-1};
            places[pos].description = ReadDescription(texto);
        }
        private void CreateStreet(string[] v)
        {
            int pos = int.Parse(v[1]);
            Direction direction = Enum.Parse<Direction>(v[2], ignoreCase: true);
            places[pos].connections[(int)direction] = int.Parse(v[3]);

            pos = int.Parse(v[3]);
            int nDirection = (int)direction;
            if (nDirection % 2 == 0)
            {
                places[pos].connections[(int)direction + 1] = int.Parse(v[1]);
            }
            else
            {
                places[pos].connections[(int)direction - 1] = int.Parse(v[1]);
            }
        }
        private void CreateItem(string[] v, int itemIndex)
        {
            int placePos = int.Parse(v[2]);

            items[itemIndex].name = v[3];
            items[itemIndex].description = string.Join(' ', v.Skip(4)).Trim('"');

            places[placePos].itemsInPlace.InsertaFinal(itemIndex);
        }

        private string ReadDescription(StreamReader f) 
        {
            string description;
            string line;
            string[] w;
            do
            {
                line = f.ReadLine();
                line = line.Trim();
                w = line.Split(' ');
            } while (!w[0].StartsWith('"'));

            line = line.Trim('"');
            description = line;
            while (!w[w.Length -1].EndsWith('"'))
            {
                line = f.ReadLine();
                line = line.Trim();
                w = line.Split(' ');
                line = line.Trim('"');
                description += "\n" + line;
            }
            return description;
            
        }

        public string GetPlaceName(int pl)  //Voluntario
        {
            return places[pl].name;
        }

        public string GetPlaceInfo(int pl)
        {
            return places[pl].description;
        }

        public string GetMoves(int pl)
        {
            // Inicializo variables
            string north = "", south = "", east = "", west = "";

            // Añado el nombre de las calles conectados en caso de que existan
            for (int i = 0; i < places[pl].connections.Length ; i++)
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
            int elems = places[pl].itemsInPlace.NumElems();
            string itemsPlace = "";

            for (int i =0; i<elems; i ++)
            {
                itemsPlace += $"{i} {items[places[pl].itemsInPlace.Nesimo(i)].name}  {items[places[pl].itemsInPlace.Nesimo(i)].description}\n";
            }

            return itemsPlace;
        }

        public void PickItemPlace(int pl, int placeItemIndex)
        {
            places[pl].itemsInPlace.EliminaElto(places[pl].itemsInPlace.Nesimo(placeItemIndex));
        }

        public void DropItemPlace(int pl, int it)
        {
            places[pl].itemsInPlace.InsertaFinal(it);
        }

        public int Move(int pl, Direction dir)
        {
            int direc = (int)dir;
            int dest = places[pl].connections[direc];

            if (dest == -1)
                throw new InvalidOperationException("No hay camino en esa dirección.");

            return dest;
        }

        public bool IsSpaceShip(int pl)
        {
            return places[pl].spaceShip;
        }

        public string GetItemName(int itemIndex)
        {
            return items[itemIndex].name;
        }
        public string GetItemDescription(int itemIndex)
        {
            return items[itemIndex].description;
        }
        public int TheItemInPlace(int placeId, int itemIndex)
        {
            return places[placeId].itemsInPlace.Nesimo(itemIndex);
        }
    }//class Map

    class WallE
    {
        int pos;            //pos de Wall-E en el mapa
        ListaEnlazada bag;  //lista de items recogidos por Wall-E (indices a la lista de items del mapa)

        public WallE() 
        {
            pos = 0;
            bag = new ListaEnlazada();
        }

        public int GetPosition()
        {
            return pos;
        }

        public void Move(Map m, Direction dir)
        {
            try
            {
                int newPos = m.Move(pos, dir);
                pos = newPos;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("No puedes moverte en esa dirección: " + ex.Message);
            }
        }

        public void PickItem(Map m ,int placeItemIndex)
        {
            bag.InsertaFinal(m.TheItemInPlace(pos, placeItemIndex));
            m.PickItemPlace(pos, placeItemIndex);
        }


        public void DropItem(Map m, int bagIndex)
        {
            m.DropItemPlace(pos, bagIndex);
            bag.EliminaElto(bag.Nesimo(bagIndex));
        }

        public string Bag(Map m)
        {
            int elems = bag.NumElems();
            string items = "";
            for (int i = 0; i< elems; i++)
            {
                items += $"{i}  {m.GetItemName(bag.Nesimo(i))}  {m.GetItemDescription(bag.Nesimo(i))}";
            }
            return items;
        }

        public bool AtSpaceShip(Map map)
        {
            return map.IsSpaceShip(pos);
        }
    }//class WallE
}
