using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Listas;
using System.ComponentModel;

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
                        CreatePlace(v);
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

        private void CreatePlace(string[] v)
        {
            places[int.Parse(v[1])].name = v[2];
            places[int.Parse(v[1])].spaceShip = (v[3]=="spaceShip");
        }
        private void CreateStreet(string[] v)
        {
            Direction direction = Enum.Parse<Direction>(v[2]);
            places[int.Parse(v[1])].connections[(int)direction] = int.Parse(v[3]);
        }
        private void CreateItem(string[] v)
        {

        }
        /*private string ReadDescription(StreamReader f) 
        {

        }*/
    }
}
