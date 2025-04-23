//Gabriel García Bazán
//Alejandro Bueno Curbera
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
    class WallE
    {
        int pos;            //pos de Wall-E en el mapa
        ListaEnlazada bag;  //lista de items recogidos por Wall-E (indices a la lista de items del mapa)


        //constructora
        public WallE() 
        {
            //se establece como posicion inicial la 0
            pos = 0;
            //se crea una lista nueva
            bag = new ListaEnlazada();
        }

        /// <summary>
        /// Devuelve la posición actual de WallE
        /// </summary>
        /// <returns></returns>
        public int GetPosition()
        {
            return pos;
        }

        public void Move(Map m, Direction dir)
        {
            int newPos = m.Move(pos, dir);
            if (newPos == -1) //Si no hay ningun lugar hacia esa dirección te informa
            {
                Console.WriteLine("No puedes moverte hacia esa dirección");
            }
            else //Si es posible moverse hacia esa dirección mueve a WallE e informa de la nueva localización
            {
                pos = newPos;
                Console.WriteLine("Te moviste ha " + m.GetPlaceName(pos));
            }
        }

        public void PickItem(Map m ,int placeItemIndex)
        {
            //Inserta en la bolsa de WallE el item
            bag.InsertaFinal(m.TheItemInPlace(pos, placeItemIndex));
            m.PickItemPlace(pos, placeItemIndex);
        }


        public void DropItem(Map m, int bagIndex)
        {
            m.DropItemPlace(pos, bag.Nesimo(bagIndex));
            //Te informa de que objeto has dejado
            Console.WriteLine("Dejaste " + m.GetItemName(bag.Nesimo(bagIndex)));
            //Elimina el objeto de la bag de WallE
            bag.EliminaElto(bag.Nesimo(bagIndex));
        }

        /// <summary>
        /// Este método devuelve un string con los objetos recogidos por WallE hasta el momento
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string Bag(Map m)
        {
            //Se mira cuantos objetos tiene actualmente WallE
            int elems = bag.NumElems();
            string items = "";
            //Se recorre la bag y se escribe el nombre y descripcion de cada objeto que tiene
            for (int i = 0; i< elems; i++)
            {
                items += $"{i}  {m.GetItemName(bag.Nesimo(i))}  {m.GetItemDescription(bag.Nesimo(i))}\n";
            }
            return items;
        }


        /// <summary>
        /// Comprueba si según el mapa dado en la posicion actual de WallE hay o no nave.
        /// Devuelve True si hay nave, False en caso contrario
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool AtSpaceShip(Map map)
        {
            return map.IsSpaceShip(pos);
        }
    }//class WallE
}
