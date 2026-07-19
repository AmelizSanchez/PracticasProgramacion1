#nullable disable
using System;
using System.Collections.Generic;

class Program
{
	// ---------- EXCURSIONES ----------
	static List<string> exNombres = new List<string>();
	static List<string> exLugares = new List<string>();
	static List<string> exDias = new List<string>();
	static Dictionary<string, int> cuposPorExcursion = new Dictionary<string, int>();

	// ---------- PARTICIPANTES ----------
	static List<string> partNombres = new List<string>();
	static List<string> partDocumentos = new List<string>();
	static List<string> partTelefonos = new List<string>();
	static List<string> partExcursion = new List<string>();
	static List<int> partNumeroCupo = new List<int>();
	static List<string> partPago = new List<string>();

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

		for (int i = 0; i < partPago.Count; i++)
		{
			if (partPago[i] == "si")
				pagadas++;
			else
				pendientes++;
		}

		Console.WriteLine("\n--- RESUMEN DE PAGOS ---");
		Console.WriteLine("Total de reservas: " + partPago.Count);
		Console.WriteLine("Reservas pagas: " + pagadas);
		Console.WriteLine("Reservas con pago pendiente: " + pendientes);
	}

	// ---------- FUNCIONES AUXILIARES DE VALIDACION ----------

	static bool EsTextoValido(string texto)
	{
		return !string.IsNullOrWhiteSpace(texto);
	}

	static bool EsSoloNumeros(string texto)
	{
		if (string.IsNullOrWhiteSpace(texto)) return false;

		foreach (char c in texto)
		{
			if (!char.IsDigit(c)) return false;
		}
		return true;
	}

	static bool EsAlfanumerico(string texto)
	{
		if (string.IsNullOrWhiteSpace(texto)) return false;

		foreach (char c in texto)
		{
			if (!char.IsLetterOrDigit(c)) return false;
		}
		return true;
	}

	// busca una excursion por nombre sin importar mayusculas/minusculas
	// y devuelve el nombre EXACTO tal como esta guardado (o null si no existe)
	static string BuscarNombreExcursionExacto(string texto)
	{
		foreach (string nombre in exNombres)
		{
			if (nombre.Equals(texto, StringComparison.OrdinalIgnoreCase))
				return nombre;
		}
		return null;
	}

	// busca un participante por documento sin importar mayusculas/minusculas
	// y devuelve su posicion en las listas (o -1 si no existe)
	static int BuscarIndicePorDocumento(string documento)
	{
		for (int i = 0; i < partDocumentos.Count; i++)
		{
			if (partDocumentos[i].Equals(documento, StringComparison.OrdinalIgnoreCase))
				return i;
		}
		return -1;
	}

	// =====================================================
	//                   EXCURSIONES
	// =====================================================
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

	static void AgregarExcursion()
	{
		Console.Write("Nombre de la excursion: ");
		string nombre = Console.ReadLine();

		if (!EsTextoValido(nombre))
		{
			Console.WriteLine("El nombre no puede estar vacio.");
			return;
		}

		if (BuscarNombreExcursionExacto(nombre) != null)
		{
			Console.WriteLine("Ya existe una excursion con ese nombre.");
			return;
		}

		Console.Write("Lugar: ");
		string lugar = Console.ReadLine();

		if (!EsTextoValido(lugar))
		{
			Console.WriteLine("El lugar no puede estar vacio.");
			return;
		}

		Console.Write("Dia: ");
		string dia = Console.ReadLine();

		if (!EsTextoValido(dia))
		{
			Console.WriteLine("El dia no puede estar vacio.");
			return;
		}

		Console.Write("Cupos disponibles: ");
		int cupos;
		if (!int.TryParse(Console.ReadLine(), out cupos) || cupos < 0)
		{
			Console.WriteLine("Cupos invalidos. No se agrego la excursion.");
			return;
		}

		exNombres.Add(nombre);
		exLugares.Add(lugar);
		exDias.Add(dia);
		cuposPorExcursion.Add(nombre, cupos);

		Console.WriteLine("Excursion agregada.");
	}

	static void ListarExcursiones()
	{
		if (exNombres.Count == 0)
		{
			Console.WriteLine("No hay excursiones registradas.");
			return;
		}

		for (int i = 0; i < exNombres.Count; i++)
		{
			string nombre = exNombres[i];

			int pagados = 0;
			int pendientes = 0;
			for (int j = 0; j < partExcursion.Count; j++)
			{
				if (partExcursion[j] == nombre)
				{
					if (partPago[j] == "si")
						pagados++;
					else
						pendientes++;
				}
			}

			Console.WriteLine(nombre + " - " + exLugares[i] + " - " + exDias[i] +
				" - Cupos disponibles: " + cuposPorExcursion[nombre] +
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

		for (int i = 0; i < exNombres.Count; i++)
		{
			bool coincide = false;

			if (criterio == "1" && exNombres[i].ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "2" && exLugares[i].ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "3" && exDias[i].ToLower().Contains(texto))
				coincide = true;

			if (coincide)
			{
				string nombre = exNombres[i];

				int pagados = 0;
				int pendientes = 0;
				for (int j = 0; j < partExcursion.Count; j++)
				{
					if (partExcursion[j] == nombre)
					{
						if (partPago[j] == "si")
							pagados++;
						else
							pendientes++;
					}
				}

				Console.WriteLine(nombre + " - " + exLugares[i] + " - " + exDias[i] +
					" - Cupos disponibles: " + cuposPorExcursion[nombre] +
					" - Pago completado: " + pagados + " - Pago pendiente: " + pendientes);
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

		string nombreReal = BuscarNombreExcursionExacto(nombre);
		if (nombreReal == null)
		{
			Console.WriteLine("No existe una excursion con ese nombre.");
			return;
		}

		int index = exNombres.IndexOf(nombreReal);

		Console.Write("Nuevo lugar: ");
		string nuevoLugar = Console.ReadLine();
		if (EsTextoValido(nuevoLugar))
			exLugares[index] = nuevoLugar;
		else
			Console.WriteLine("Lugar vacio, se dejo el valor anterior.");

		Console.Write("Nuevo dia: ");
		string nuevoDia = Console.ReadLine();
		if (EsTextoValido(nuevoDia))
			exDias[index] = nuevoDia;
		else
			Console.WriteLine("Dia vacio, se dejo el valor anterior.");

		Console.Write("Nuevos cupos disponibles: ");
		int cupos;
		if (!int.TryParse(Console.ReadLine(), out cupos) || cupos < 0)
		{
			Console.WriteLine("Cupos invalidos, se dejo el valor anterior.");
		}
		else
		{
			cuposPorExcursion[nombreReal] = cupos;
		}

		Console.WriteLine("Excursion modificada.");
	}

	static void EliminarExcursion()
	{
		Console.Write("Nombre de la excursion a eliminar: ");
		string nombre = Console.ReadLine();

		string nombreReal = BuscarNombreExcursionExacto(nombre);
		if (nombreReal == null)
		{
			Console.WriteLine("No existe una excursion con ese nombre.");
			return;
		}

		int index = exNombres.IndexOf(nombreReal);

		int contador = 0;
		for (int i = 0; i < partExcursion.Count; i++)
		{
			if (partExcursion[i] == nombreReal) contador++;
		}

		if (contador > 0)
		{
			Console.WriteLine("Hay " + contador + " participante(s) anotados en esta excursion.");
			Console.Write("¿Seguro que desea eliminarla junto con esos participantes? (s/n): ");
			string resp = Console.ReadLine().ToLower();
			if (resp != "s")
			{
				Console.WriteLine("Operacion cancelada.");
				return;
			}

			// recorremos de atras hacia adelante para no saltarnos elementos al borrar
			for (int i = partExcursion.Count - 1; i >= 0; i--)
			{
				if (partExcursion[i] == nombreReal)
				{
					partNombres.RemoveAt(i);
					partDocumentos.RemoveAt(i);
					partTelefonos.RemoveAt(i);
					partExcursion.RemoveAt(i);
					partNumeroCupo.RemoveAt(i);
					partPago.RemoveAt(i);
				}
			}
		}

		exNombres.RemoveAt(index);
		exLugares.RemoveAt(index);
		exDias.RemoveAt(index);
		cuposPorExcursion.Remove(nombreReal);

		Console.WriteLine("Excursion eliminada.");
	}

	// =====================================================
	//                   PARTICIPANTES
	// =====================================================
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

	static void AgregarParticipante()
	{
		Console.Write("Documento (cedula o pasaporte): ");
		string documento = Console.ReadLine();

		if (!EsAlfanumerico(documento))
		{
			Console.WriteLine("El documento no puede estar vacio y solo debe tener letras y numeros.");
			return;
		}

		if (BuscarIndicePorDocumento(documento) != -1)
		{
			Console.WriteLine("Ya existe un participante con ese documento.");
			return;
		}

		Console.Write("Nombre: ");
		string nombre = Console.ReadLine();

		if (!EsTextoValido(nombre))
		{
			Console.WriteLine("El nombre no puede estar vacio.");
			return;
		}

		Console.Write("Telefono: ");
		string telefono = Console.ReadLine();

		if (!EsSoloNumeros(telefono) || telefono.Length < 7)
		{
			Console.WriteLine("Telefono invalido, debe tener solo numeros (minimo 7 digitos).");
			return;
		}

		Console.Write("Excursion a la que se inscribe: ");
		string excursionTexto = Console.ReadLine();
		string excursion = BuscarNombreExcursionExacto(excursionTexto);

		if (excursion == null)
		{
			Console.WriteLine("Esa excursion no existe.");
			return;
		}

		if (cuposPorExcursion[excursion] <= 0)
		{
			Console.WriteLine("No hay cupos disponibles para esa excursion.");
			return;
		}

		int numeroCupo = 1;
		for (int i = 0; i < partExcursion.Count; i++)
		{
			if (partExcursion[i] == excursion) numeroCupo++;
		}

		partNombres.Add(nombre);
		partDocumentos.Add(documento);
		partTelefonos.Add(telefono);
		partExcursion.Add(excursion);
		partNumeroCupo.Add(numeroCupo);
		partPago.Add("no");

		cuposPorExcursion[excursion]--;

		Console.WriteLine("Participante inscrito. Numero de cupo: " + numeroCupo);
	}

	static void ListarParticipantes()
	{
		if (partNombres.Count == 0)
		{
			Console.WriteLine("No hay participantes registrados.");
			return;
		}

		for (int i = 0; i < partNombres.Count; i++)
		{
			Console.WriteLine(partNombres[i] + " - Doc: " + partDocumentos[i] + " - Tel: " + partTelefonos[i] +
				" - Excursion: " + partExcursion[i] + " - Cupo #" + partNumeroCupo[i] + " - Pagado: " + partPago[i]);
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

		for (int i = 0; i < partNombres.Count; i++)
		{
			bool coincide = false;

			if (criterio == "1" && partDocumentos[i].ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "2" && partNombres[i].ToLower().Contains(texto))
				coincide = true;
			else if (criterio == "3" && partExcursion[i].ToLower().Contains(texto))
				coincide = true;

			if (coincide)
			{
				Console.WriteLine(partNombres[i] + " - Doc: " + partDocumentos[i] + " - Tel: " + partTelefonos[i] +
					" - Excursion: " + partExcursion[i] + " - Cupo #" + partNumeroCupo[i] + " - Pagado: " + partPago[i]);
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

		int index = BuscarIndicePorDocumento(documento);
		if (index == -1)
		{
			Console.WriteLine("No existe un participante con ese documento.");
			return;
		}

		Console.Write("Nuevo nombre: ");
		string nuevoNombre = Console.ReadLine();
		if (EsTextoValido(nuevoNombre))
			partNombres[index] = nuevoNombre;
		else
			Console.WriteLine("Nombre vacio, se dejo el valor anterior.");

		Console.Write("Nuevo telefono: ");
		string nuevoTelefono = Console.ReadLine();
		if (EsSoloNumeros(nuevoTelefono) && nuevoTelefono.Length >= 7)
			partTelefonos[index] = nuevoTelefono;
		else
			Console.WriteLine("Telefono invalido, se dejo el valor anterior.");

		Console.Write("Pagado? (s/n): ");
		partPago[index] = Console.ReadLine().ToLower() == "s" ? "si" : "no";

		Console.Write("¿Desea cambiar de excursion? (s/n): ");
		if (Console.ReadLine().ToLower() == "s")
		{
			string excursionVieja = partExcursion[index];

			Console.Write("Nueva excursion: ");
			string excursionTexto = Console.ReadLine();
			string excursionNueva = BuscarNombreExcursionExacto(excursionTexto);

			if (excursionNueva == null)
			{
				Console.WriteLine("Esa excursion no existe. Se mantuvo la excursion anterior.");
				return;
			}

			if (excursionNueva == excursionVieja)
			{
				Console.WriteLine("Es la misma excursion, no hay cambios de excursion.");
				return;
			}

			if (cuposPorExcursion[excursionNueva] <= 0)
			{
				Console.WriteLine("No hay cupos en la nueva excursion. Se mantuvo la excursion anterior.");
				return;
			}

			// devolver el cupo de la excursion vieja
			cuposPorExcursion[excursionVieja]++;

			// calcular nuevo numero de cupo en la excursion nueva
			int numeroCupo = 1;
			for (int i = 0; i < partExcursion.Count; i++)
			{
				if (partExcursion[i] == excursionNueva) numeroCupo++;
			}

			// tomar el cupo en la excursion nueva
			cuposPorExcursion[excursionNueva]--;

			partExcursion[index] = excursionNueva;
			partNumeroCupo[index] = numeroCupo;
		}

		Console.WriteLine("Participante modificado.");
	}

	static void EliminarParticipante()
	{
		Console.Write("Documento del participante a eliminar: ");
		string documento = Console.ReadLine();

		int index = BuscarIndicePorDocumento(documento);
		if (index == -1)
		{
			Console.WriteLine("No existe un participante con ese documento.");
			return;
		}

		Console.WriteLine("Participante encontrado: " + partNombres[index] + " - Excursion: " + partExcursion[index]);
		Console.Write("¿Seguro que desea eliminarlo? (s/n): ");
		string resp = Console.ReadLine().ToLower();
		if (resp != "s")
		{
			Console.WriteLine("Operacion cancelada.");
			return;
		}

		string excursion = partExcursion[index];

		partNombres.RemoveAt(index);
		partDocumentos.RemoveAt(index);
		partTelefonos.RemoveAt(index);
		partExcursion.RemoveAt(index);
		partNumeroCupo.RemoveAt(index);
		partPago.RemoveAt(index);

		// devolver el cupo, solo si la excursion todavia existe
		if (cuposPorExcursion.ContainsKey(excursion))
			cuposPorExcursion[excursion]++;

		Console.WriteLine("Participante eliminado, cupo liberado.");
	}
}