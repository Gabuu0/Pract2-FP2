using System;
using Listas;

namespace Main
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ListaEnlazada l = new ListaEnlazada ();
			int op;
			do {
				int e;
				op = menu ();
				switch (op){
				case 0:
					Console.WriteLine("Bye");
					break;
				case 1:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					l.InsertaPpio(e);
					break;
				case 2:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					l.InsertaFinal(e);
					break;
				case 3:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					if (l.BuscaDato(e))
						Console.WriteLine("Esta!");
					else
						Console.WriteLine("No esta!");
					break;
				case 4:
					Console.Write("Dato: ");
					e=int.Parse(Console.ReadLine());
					if (l.EliminaElto(e)) Console.WriteLine("Eliminado!");
					else Console.WriteLine("No esta!");
					break;
				case 5:					
					Console.WriteLine($"Num elems: {l.NumElems()}");
					break;

				}
				Console.WriteLine(l);
			} while (op!=0);


		}

		static int menu(){
			Console.WriteLine("0. Salir");
			Console.WriteLine("1. Inserta ppio");
			Console.WriteLine("2. Inserta final");
			Console.WriteLine("3. Busca elto");
			Console.WriteLine("4. Elimina elto");
			Console.WriteLine("5. Num elems");

			int op;
			do {
				Console.Write("Opcion: ");
				op = int.Parse(Console.ReadLine());
			} while (op<0 || op> 5);
			return op;
		}


	}
}
