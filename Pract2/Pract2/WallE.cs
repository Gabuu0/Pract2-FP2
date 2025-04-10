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
            StreamReader entrada = new StreamReader(file);
            while (!entrada.EndOfStream)
            {
                string s = entrada.ReadLine();
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
                        CreateItem(v);
                        break;
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
            Direction direction = Enum.Parse<Direction>(v[2]);
            places[pos].connections[(int)direction] = int.Parse(v[3]);
        }
        private void CreateItem(string[] v)
        {
            int pos = int.Parse(v[2]);
            string description = string.Join(' ', v.Skip(4)).Trim('"');

            places[pos].itemsInPlace.InsertaFinal(int.Parse(v[3]));

            int posArray = 0;
            while (items[posArray].name != null)
            {
                posArray++;
            }

            items[posArray].name = v[3];
            items[posArray].description = v[4];
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
        }
    }
}
