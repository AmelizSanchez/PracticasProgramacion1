#nullable disable
using System;
using System.Collections.Generic;

class Excursion
{
	public string Nombre;
	public string Lugar;
	public string Dia;
	public int Cupos;

	public Excursion(string nombre, string lugar, string dia, int cupos)
	{
		Nombre = nombre;
		Lugar = lugar;
		Dia = dia;
		Cupos = cupos;
	}
}

class Participante
{
	public string Nombre;
	public string Documento;
	public string Telefono;
	public Excursion Excursion;
	public int NumeroCupo;
	public bool Pagado;

	public Participante(string nombre, string documento, string telefono, Excursion excursion, int numeroCupo)
	{
		Nombre = nombre;
		Documento = documento;
		Telefono = telefono;
		Excursion = excursion;
		NumeroCupo = numeroCupo;
		Pagado = false;
	}
}

class Program
{
	static List<Excursion> excursiones = new List<Excursion>();
	static List<Participante> participantes = new List<Participante>();

	static void Main()
	{
		string opcion = "";

		do
		{
			Console.WriteLine("\n===== SISTEMA DE GESTION DE EXCURSIONES =====");
			Console.WriteLine("1. Excursiones");
			Console.WriteLine("2. Participantes");
			Console.WriteLine("3. Resumen de pagos");
			Console.WriteLine("4. Salir");
			Console.Write("Opcion: ");
			opcion = Console.ReadLine();

			try
			{
				switch (opcion)
				{
					case "1":
						MenuExcursiones();
						break;
					case "2":
						MenuParticipantes();
						break;
					case "3":
						ResumenPagos();
						break;
					case "4":
						Console.WriteLine("Saliendo del programa...");
						break;
					default:
						Console.WriteLine("Opcion invalida.");
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ocurrio un error: " + ex.Message);
			}

		} while (opcion != "4");
	}

	static void ResumenPagos()
	{
		int pagadas = 0;
		int pendientes = 0;

		for (int i = 0; i < participantes.Count; i++)
		{
			if (participantes[i].Pagado)
				pagadas++;
			else
				pendientes++;
		}

		Console.WriteLine("\n--- RESUMEN DE PAGOS ---");
		Console.WriteLine("Total de reservas: " + participantes.Count);
		Console.WriteLine("Reservas pagas: " + pagadas);
		Console.WriteLine("Reservas con pago pendiente: " + pendientes);
	}

	static void MenuExcursiones()
	{
		string opcion = "";
		do
		{
			Console.WriteLine("\n--- EXCURSIONES ---");
			Console.WriteLine("1. Agregar");
			Console.WriteLine("2. Listar");
			Console.WriteLine("3. Buscar");
			Console.WriteLine("4. Modificar");
			Console.WriteLine("5. Eliminar");
			Console.WriteLine("6. Volver");
			Console.Write("Opcion: ");
			opcion = Console.ReadLine();

			try
			{
				switch (opcion)
				{
					case "1": AgregarExcursion(); break;
					case "2": ListarExcursiones(); break;
					case "3": BuscarExcursion(); break;
					case "4": ModificarExcursion(); break;
					case "5": EliminarExcursion(); break;
					case "6": break;
					default: Console.WriteLine("Opcion invalida."); break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ocurrio un error: " + ex.Message);
			}

		} while (opcion != "6");
	}

	static Excursion BuscarExcursionPorNombre(string texto)
	{
		for (int i = 0; i < excursiones.Count; i++)
		{
			if (excursiones[i].Nombre.ToLower() == texto.ToLower())
				return excursiones[i];
		}
		return null;
	}

	static void AgregarExcursion()
	{
		Console.Write("Nombre de la excursion: ");
		string nombre = Console.ReadLine();

		if (string.IsNullOrWhiteSpace(nombre))
		{
			Console.WriteLine("El nombre no puede estar vacio.");
			return;
		}

		if (BuscarExcursionPorNombre(nombre) != null)
		{
			Console.WriteLine("Ya existe una excursion con ese nombre.");
			return;
		}

		Console.Write("Lugar: ");
		string lugar = Console.ReadLine();

		Console.Write("Dia: ");
		string dia = Console.ReadLine();

		Console.Write("Cupos disponibles: ");
		int cupos;
		if (!int.TryParse(Console.ReadLine(), out cupos) || cupos < 0)
		{
			Console.WriteLine("Cupos invalidos. No se agrego la excursion.");
			return;
		}

		Excursion nueva = new Excursion(nombre, lugar, dia, cupos);
		excursiones.Add(nueva);

		Console.WriteLine("Excursion agregada.");
	}

	static void ListarExcursiones()
	{
		if (excursiones.Count == 0)
		{
			Console.WriteLine("No hay excursiones registradas.");
			return;
		}

		for (int i = 0; i < excursiones.Count; i++)
		{
			Excursion e = excursiones[i];

			int pagados = 0;
			int pendientes = 0;
			for (int j = 0; j < participantes.Count; j++)
			{
				if (participantes[j].Excursion == e)
				{
					if (participantes[j].Pagado)
						pagados++;
					else
						pendientes++;
				}
			}

			Console.WriteLine(e.Nombre + " - " + e.Lugar + " - " + e.Dia +
				" - Cupos disponibles: " + e.Cupos +
				" - Pago completado: " + pagados + " - Pago pendiente: " + pendientes);
		}
	}

	static void BuscarExcursion()
	{
		Console.WriteLine("Buscar por: 1.Nombre  2.Lugar  3.Dia");
		Console.Write("Criterio: ");
		string criterio = Console.ReadLine();

		if (criterio != "1" && criterio != "2" && criterio != "3")
		{
			Console.WriteLine("Criterio invalido.");
			return;
		}

		Console.Write("Texto a buscar: ");
		string texto = Console.ReadLine().ToLower();

		bool encontrado = false;

		for (int i = 0; i < excursiones.Count; i++)
		{
			Excursion e = excursiones[i];
			bool coincide = false;

			if (criterio == "1" && e.Nombre.ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "2" && e.Lugar.ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "3" && e.Dia.ToLower().Contains(texto))
				coincide = true;

			if (coincide)
			{
				Console.WriteLine(e.Nombre + " - " + e.Lugar + " - " + e.Dia + " - Cupos disponibles: " + e.Cupos);
				encontrado = true;
			}
		}

		if (!encontrado)
			Console.WriteLine("No se encontro ninguna excursion con ese criterio.");
	}

	static void ModificarExcursion()
	{
		Console.Write("Nombre de la excursion a modificar: ");
		string nombre = Console.ReadLine();

		Excursion e = BuscarExcursionPorNombre(nombre);
		if (e == null)
		{
			Console.WriteLine("No existe una excursion con ese nombre.");
			return;
		}

		Console.Write("Nuevo lugar: ");
		string nuevoLugar = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(nuevoLugar))
			e.Lugar = nuevoLugar;
		else
			Console.WriteLine("Lugar vacio, se dejo el valor anterior.");

		Console.Write("Nuevo dia: ");
		string nuevoDia = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(nuevoDia))
			e.Dia = nuevoDia;
		else
			Console.WriteLine("Dia vacio, se dejo el valor anterior.");

		Console.Write("Nuevos cupos disponibles: ");
		int cupos;
		if (!int.TryParse(Console.ReadLine(), out cupos) || cupos < 0)
			Console.WriteLine("Cupos invalidos, se dejo el valor anterior.");
		else
			e.Cupos = cupos;

		Console.WriteLine("Excursion modificada.");
	}

	static void EliminarExcursion()
	{
		Console.Write("Nombre de la excursion a eliminar: ");
		string nombre = Console.ReadLine();

		Excursion e = BuscarExcursionPorNombre(nombre);
		if (e == null)
		{
			Console.WriteLine("No existe una excursion con ese nombre.");
			return;
		}

		int contador = 0;
		for (int i = 0; i < participantes.Count; i++)
		{
			if (participantes[i].Excursion == e)
				contador++;
		}

		if (contador > 0)
		{
			Console.WriteLine("Hay " + contador + " participante(s) anotados en esta excursion.");
			Console.Write("Seguro que desea eliminarla junto con esos participantes? (s/n): ");
			string resp = Console.ReadLine().ToLower();
			if (resp != "s")
			{
				Console.WriteLine("Operacion cancelada.");
				return;
			}

			for (int i = participantes.Count - 1; i >= 0; i--)
			{
				if (participantes[i].Excursion == e)
					participantes.RemoveAt(i);
			}
		}

		excursiones.Remove(e);
		Console.WriteLine("Excursion eliminada.");
	}

	static void MenuParticipantes()
	{
		string opcion = "";
		do
		{
			Console.WriteLine("\n--- PARTICIPANTES ---");
			Console.WriteLine("1. Agregar");
			Console.WriteLine("2. Listar");
			Console.WriteLine("3. Buscar");
			Console.WriteLine("4. Modificar");
			Console.WriteLine("5. Eliminar");
			Console.WriteLine("6. Volver");
			Console.Write("Opcion: ");
			opcion = Console.ReadLine();

			try
			{
				switch (opcion)
				{
					case "1": AgregarParticipante(); break;
					case "2": ListarParticipantes(); break;
					case "3": BuscarParticipante(); break;
					case "4": ModificarParticipante(); break;
					case "5": EliminarParticipante(); break;
					case "6": break;
					default: Console.WriteLine("Opcion invalida."); break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ocurrio un error: " + ex.Message);
			}

		} while (opcion != "6");
	}

	static Participante BuscarParticipantePorDocumento(string documento)
	{
		for (int i = 0; i < participantes.Count; i++)
		{
			if (participantes[i].Documento.ToLower() == documento.ToLower())
				return participantes[i];
		}
		return null;
	}

	static void AgregarParticipante()
	{
		Console.Write("Documento (cedula o pasaporte): ");
		string documento = Console.ReadLine();

		if (string.IsNullOrWhiteSpace(documento))
		{
			Console.WriteLine("El documento no puede estar vacio.");
			return;
		}

		if (BuscarParticipantePorDocumento(documento) != null)
		{
			Console.WriteLine("Ya existe un participante con ese documento.");
			return;
		}

		Console.Write("Nombre: ");
		string nombre = Console.ReadLine();

		Console.Write("Telefono: ");
		string telefono = Console.ReadLine();

		Console.Write("Excursion a la que se inscribe: ");
		string excursionTexto = Console.ReadLine();
		Excursion excursion = BuscarExcursionPorNombre(excursionTexto);

		if (excursion == null)
		{
			Console.WriteLine("Esa excursion no existe.");
			return;
		}

		if (excursion.Cupos <= 0)
		{
			Console.WriteLine("No hay cupos disponibles para esa excursion.");
			return;
		}

		int numeroCupo = 1;
		for (int i = 0; i < participantes.Count; i++)
		{
			if (participantes[i].Excursion == excursion)
				numeroCupo++;
		}

		Participante nuevo = new Participante(nombre, documento, telefono, excursion, numeroCupo);
		participantes.Add(nuevo);

		excursion.Cupos = excursion.Cupos - 1;

		Console.WriteLine("Participante inscrito. Numero de cupo: " + numeroCupo);
	}

	static void ListarParticipantes()
	{
		if (participantes.Count == 0)
		{
			Console.WriteLine("No hay participantes registrados.");
			return;
		}

		for (int i = 0; i < participantes.Count; i++)
		{
			Participante p = participantes[i];
			string estado = p.Pagado ? "si" : "no";

			Console.WriteLine(p.Nombre + " - Doc: " + p.Documento + " - Tel: " + p.Telefono +
				" - Excursion: " + p.Excursion.Nombre + " - Cupo #" + p.NumeroCupo + " - Pagado: " + estado);
		}
	}

	static void BuscarParticipante()
	{
		Console.WriteLine("Buscar por: 1.Documento  2.Nombre  3.Excursion");
		Console.Write("Criterio: ");
		string criterio = Console.ReadLine();

		if (criterio != "1" && criterio != "2" && criterio != "3")
		{
			Console.WriteLine("Criterio invalido.");
			return;
		}

		Console.Write("Texto a buscar: ");
		string texto = Console.ReadLine().ToLower();

		bool encontrado = false;

		for (int i = 0; i < participantes.Count; i++)
		{
			Participante p = participantes[i];
			bool coincide = false;

			if (criterio == "1" && p.Documento.ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "2" && p.Nombre.ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "3" && p.Excursion.Nombre.ToLower().Contains(texto))
				coincide = true;

			if (coincide)
			{
				string estado = p.Pagado ? "si" : "no";
				Console.WriteLine(p.Nombre + " - Doc: " + p.Documento + " - Tel: " + p.Telefono +
					" - Excursion: " + p.Excursion.Nombre + " - Cupo #" + p.NumeroCupo + " - Pagado: " + estado);
				encontrado = true;
			}
		}

		if (!encontrado)
			Console.WriteLine("No se encontro ningun participante con ese criterio.");
	}

	static void ModificarParticipante()
	{
		Console.Write("Documento del participante a modificar: ");
		string documento = Console.ReadLine();

		Participante p = BuscarParticipantePorDocumento(documento);
		if (p == null)
		{
			Console.WriteLine("No existe un participante con ese documento.");
			return;
		}

		Console.Write("Nuevo nombre: ");
		string nuevoNombre = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(nuevoNombre))
			p.Nombre = nuevoNombre;

		Console.Write("Nuevo telefono: ");
		string nuevoTelefono = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(nuevoTelefono))
			p.Telefono = nuevoTelefono;

		Console.Write("Pagado? (s/n): ");
		p.Pagado = Console.ReadLine().ToLower() == "s";

		Console.Write("Desea cambiar de excursion? (s/n): ");
		if (Console.ReadLine().ToLower() == "s")
		{
			Excursion excursionVieja = p.Excursion;

			Console.Write("Nueva excursion: ");
			string excursionTexto = Console.ReadLine();
			Excursion excursionNueva = BuscarExcursionPorNombre(excursionTexto);

			if (excursionNueva == null)
			{
				Console.WriteLine("Esa excursion no existe. Se mantuvo la excursion anterior.");
				return;
			}

			if (excursionNueva == excursionVieja)
			{
				Console.WriteLine("Es la misma excursion, no hay cambios.");
				return;
			}

			if (excursionNueva.Cupos <= 0)
			{
				Console.WriteLine("No hay cupos en la nueva excursion. Se mantuvo la excursion anterior.");
				return;
			}

			excursionVieja.Cupos = excursionVieja.Cupos + 1;

			int numeroCupo = 1;
			for (int i = 0; i < participantes.Count; i++)
			{
				if (participantes[i].Excursion == excursionNueva)
					numeroCupo++;
			}

			excursionNueva.Cupos = excursionNueva.Cupos - 1;

			p.Excursion = excursionNueva;
			p.NumeroCupo = numeroCupo;
		}

		Console.WriteLine("Participante modificado.");
	}

	static void EliminarParticipante()
	{
		Console.Write("Documento del participante a eliminar: ");
		string documento = Console.ReadLine();

		Participante p = BuscarParticipantePorDocumento(documento);
		if (p == null)
		{
			Console.WriteLine("No existe un participante con ese documento.");
			return;
		}

		Console.WriteLine("Participante encontrado: " + p.Nombre + " - Excursion: " + p.Excursion.Nombre);
		Console.Write("Seguro que desea eliminarlo? (s/n): ");
		string resp = Console.ReadLine().ToLower();
		if (resp != "s")
		{
			Console.WriteLine("Operacion cancelada.");
			return;
		}

		p.Excursion.Cupos = p.Excursion.Cupos + 1;
		participantes.Remove(p);

		Console.WriteLine("Participante eliminado, cupo liberado.");
	}
}