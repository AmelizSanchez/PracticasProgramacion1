#nullable disable
using System;
using System.Collections.Generic;

class Excursion
{
	public int Id;
	public string Nombre;
	public string Destino;
	public int Cupos;
}

class Participante
{
	public int Id;
	public string Nombre;
	public string Cedula;
}

class Reserva
{
	public int Id;
	public int IdExcursion;
	public int IdParticipante;
	public bool Pagado;
}

class Program
{
	static List<Excursion> excursiones = new List<Excursion>();
	static List<Participante> participantes = new List<Participante>();
	static List<Reserva> reservas = new List<Reserva>();

	static int contE = 1, contP = 1, contR = 1;

	static void Main()
	{
		string op = "";

		while (op != "4")
		{
			Console.WriteLine("\n==========MENU==========\n");

	
			Console.WriteLine("\n1. Excursiones  2. Participantes  3. Reservas  4. Salir\n");
			Console.Write("Opcion: ");
			op = Console.ReadLine();

			try
			{
				if (op == "1") MenuExcursiones();
				else if (op == "2") MenuParticipantes();
				else if (op == "3") MenuReservas();
				else if (op != "4") Console.WriteLine("Opcion invalida.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}

	static void MenuExcursiones()
	{
		string op = "";
		while (op != "6")
		{
			Console.WriteLine("\n1.Agregar 2.Listar 3.Buscar 4.Modificar 5.Eliminar 6.Volver");
			Console.Write("Opcion: ");
			op = Console.ReadLine();

			try
			{
				if (op == "1")
				{
					Excursion e = new Excursion();
					e.Id = contE++;
					Console.Write("Nombre: "); e.Nombre = Console.ReadLine();
					Console.Write("Destino: "); e.Destino = Console.ReadLine();
					Console.Write("Cupos: "); e.Cupos = int.Parse(Console.ReadLine());
					excursiones.Add(e);
					Console.WriteLine("Excursion agregada.");
				}
				else if (op == "2")
				{
					foreach (Excursion e in excursiones)
						Console.WriteLine(e.Id + ". " + e.Nombre + " - " + e.Destino + " - Cupos: " + e.Cupos);
				}
				else if (op == "3")
				{
					Console.Write("Buscar por nombre: ");
					string buscar = Console.ReadLine();
					foreach (Excursion e in excursiones)
						if (e.Nombre.Contains(buscar))
							Console.WriteLine(e.Id + ". " + e.Nombre + " - " + e.Destino);
				}
				else if (op == "4")
				{
					Console.Write("ID a modificar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Excursion e in excursiones)
					{
						if (e.Id == id)
						{
							Console.Write("Nuevo nombre: "); e.Nombre = Console.ReadLine();
							Console.Write("Nuevo destino: "); e.Destino = Console.ReadLine();
							Console.Write("Nuevos cupos: "); e.Cupos = int.Parse(Console.ReadLine());
							Console.WriteLine("Modificado.");
						}
					}
				}
				else if (op == "5")
				{
					Console.Write("ID a eliminar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Excursion e in excursiones)
					{
						if (e.Id == id) { excursiones.Remove(e); Console.WriteLine("Eliminado."); break; }
					}
				}
			}
			catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
		}
	}

	static void MenuParticipantes()
	{
		string op = "";
		while (op != "6")
		{
			Console.WriteLine("\n1.Agregar 2.Listar 3.Buscar 4.Modificar 5.Eliminar 6.Volver");
			Console.Write("Opcion: ");
			op = Console.ReadLine();

			try
			{
				if (op == "1")
				{
					Participante p = new Participante();
					p.Id = contP++;
					Console.Write("Nombre: "); p.Nombre = Console.ReadLine();
					Console.Write("Cedula: "); p.Cedula = Console.ReadLine();
					participantes.Add(p);
					Console.WriteLine("Participante agregado.");
				}
				else if (op == "2")
				{
					foreach (Participante p in participantes)
						Console.WriteLine(p.Id + ". " + p.Nombre + " - " + p.Cedula);
				}
				else if (op == "3")
				{
					Console.Write("Buscar por cedula: ");
					string cedula = Console.ReadLine();
					foreach (Participante p in participantes)
						if (p.Cedula == cedula)
							Console.WriteLine(p.Id + ". " + p.Nombre + " - " + p.Cedula);
				}
				else if (op == "4")
				{
					Console.Write("ID a modificar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Participante p in participantes)
					{
						if (p.Id == id)
						{
							Console.Write("Nuevo nombre: "); p.Nombre = Console.ReadLine();
							Console.Write("Nueva cedula: "); p.Cedula = Console.ReadLine();
							Console.WriteLine("Modificado.");
						}
					}
				}
				else if (op == "5")
				{
					Console.Write("ID a eliminar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Participante p in participantes)
					{
						if (p.Id == id) { participantes.Remove(p); Console.WriteLine("Eliminado."); break; }
					}
				}
			}
			catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
		}
	}

	static void MenuReservas()
	{
		string op = "";
		while (op != "6")
		{
			Console.WriteLine("\n1.Agregar 2.Listar 3.Buscar 4.Modificar 5.Eliminar 6.Volver");
			Console.Write("Opcion: ");
			op = Console.ReadLine();

			try
			{
				if (op == "1")
				{
					Reserva r = new Reserva();
					r.Id = contR++;
					Console.Write("ID Excursion: "); r.IdExcursion = int.Parse(Console.ReadLine());
					Console.Write("ID Participante: "); r.IdParticipante = int.Parse(Console.ReadLine());
					Console.Write("Pagado? (s/n): "); r.Pagado = Console.ReadLine() == "s";
					reservas.Add(r);
					Console.WriteLine("Reserva agregada.");
				}
				else if (op == "2")
				{
					foreach (Reserva r in reservas)
						Console.WriteLine(r.Id + ". Excursion:" + r.IdExcursion + " Participante:" + r.IdParticipante + " Pagado:" + r.Pagado);
				}
				else if (op == "3")
				{
					Console.Write("Buscar por ID participante: ");
					int idP = int.Parse(Console.ReadLine());
					foreach (Reserva r in reservas)
						if (r.IdParticipante == idP)
							Console.WriteLine(r.Id + ". Excursion:" + r.IdExcursion + " Pagado:" + r.Pagado);
				}
				else if (op == "4")
				{
					Console.Write("ID a modificar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Reserva r in reservas)
					{
						if (r.Id == id)
						{
							Console.Write("Nuevo ID Excursion: "); r.IdExcursion = int.Parse(Console.ReadLine());
							Console.Write("Nuevo ID Participante: "); r.IdParticipante = int.Parse(Console.ReadLine());
							Console.Write("Pagado? (s/n): "); r.Pagado = Console.ReadLine() == "s";
							Console.WriteLine("Modificado.");
						}
					}
				}
				else if (op == "5")
				{
					Console.Write("ID a eliminar: ");
					int id = int.Parse(Console.ReadLine());
					foreach (Reserva r in reservas)
					{
						if (r.Id == id) { reservas.Remove(r); Console.WriteLine("Eliminado."); break; }
					}
				}
			}
			catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
		}
	}
}