#nullable disable
using System;
using Microsoft.Data.SqlClient;

class Program
{
	static string conexion =
		@"Server=localhost\SQLEXPRESS01;Database=ExcursionesDB;Trusted_Connection=True;TrustServerCertificate=True;";

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
					case "1": MenuExcursiones(); break;
					case "2": MenuParticipantes(); break;
					case "3": ResumenPagos(); break;
					case "4": Console.WriteLine("Saliendo del programa..."); break;
					default: Console.WriteLine("Opcion invalida."); break;
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
		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlTotal = "SELECT COUNT(*) FROM Participantes";
			SqlCommand cmdTotal = new SqlCommand(sqlTotal, con);
			int total = (int)cmdTotal.ExecuteScalar();

			string sqlPagadas = "SELECT COUNT(*) FROM Participantes WHERE Pagado = 1";
			SqlCommand cmdPagadas = new SqlCommand(sqlPagadas, con);
			int pagadas = (int)cmdPagadas.ExecuteScalar();

			string sqlPendientes = "SELECT COUNT(*) FROM Participantes WHERE Pagado = 0";
			SqlCommand cmdPendientes = new SqlCommand(sqlPendientes, con);
			int pendientes = (int)cmdPendientes.ExecuteScalar();

			Console.WriteLine("\n--- RESUMEN DE PAGOS ---");
			Console.WriteLine("Total de reservas: " + total);
			Console.WriteLine("Reservas pagas: " + pagadas);
			Console.WriteLine("Reservas con pago pendiente: " + pendientes);
		}
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

	static void AgregarExcursion()
	{
		Console.Write("Nombre: ");
		string nombre = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(nombre))
		{
			Console.WriteLine("El nombre no puede estar vacio.");
			return;
		}

		Console.Write("Lugar: ");
		string lugar = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(lugar))
		{
			Console.WriteLine("El lugar no puede estar vacio.");
			return;
		}

		Console.Write("Dia: ");
		string dia = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(dia))
		{
			Console.WriteLine("El dia no puede estar vacio.");
			return;
		}

		Console.Write("Cupos: ");
		int cupos;
		if (!int.TryParse(Console.ReadLine(), out cupos) || cupos < 0)
		{
			Console.WriteLine("Cupos invalidos. No se agrego la excursion.");
			return;
		}

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlVerificar = "SELECT COUNT(*) FROM Excursiones WHERE Nombre = @Nombre";
			SqlCommand cmdVerificar = new SqlCommand(sqlVerificar, con);
			cmdVerificar.Parameters.AddWithValue("@Nombre", nombre);
			int existe = (int)cmdVerificar.ExecuteScalar();

			if (existe > 0)
			{
				Console.WriteLine("Ya existe una excursion con ese nombre.");
				return;
			}

			string sql = "INSERT INTO Excursiones (Nombre, Lugar, Dia, Cupos) VALUES (@Nombre, @Lugar, @Dia, @Cupos)";
			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.Parameters.AddWithValue("@Nombre", nombre);
			cmd.Parameters.AddWithValue("@Lugar", lugar);
			cmd.Parameters.AddWithValue("@Dia", dia);
			cmd.Parameters.AddWithValue("@Cupos", cupos);
			cmd.ExecuteNonQuery();
		}

		Console.WriteLine("Excursion agregada correctamente.");
	}

	static void ListarExcursiones()
	{
		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sql = "SELECT Id, Nombre, Lugar, Dia, Cupos FROM Excursiones";
			SqlCommand cmd = new SqlCommand(sql, con);
			SqlDataReader reader = cmd.ExecuteReader();

			bool hayDatos = false;
			Console.WriteLine("\n--- EXCURSIONES ---");
			while (reader.Read())
			{
				hayDatos = true;
				Console.WriteLine(reader["Nombre"] + " - " + reader["Lugar"] + " - " + reader["Dia"] +
					" - Cupos disponibles: " + reader["Cupos"]);
			}
			reader.Close();

			if (!hayDatos)
				Console.WriteLine("No hay excursiones registradas.");
		}
	}

	static void BuscarExcursion()
	{
		Console.WriteLine("Buscar por: 1.Nombre  2.Lugar  3.Dia");
		Console.Write("Criterio: ");
		string criterio = Console.ReadLine();

		Console.Write("Texto a buscar: ");
		string texto = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sql;
			if (criterio == "1")
				sql = "SELECT Nombre, Lugar, Dia, Cupos FROM Excursiones WHERE Nombre LIKE @Texto";
			else if (criterio == "2")
				sql = "SELECT Nombre, Lugar, Dia, Cupos FROM Excursiones WHERE Lugar LIKE @Texto";
			else if (criterio == "3")
				sql = "SELECT Nombre, Lugar, Dia, Cupos FROM Excursiones WHERE Dia LIKE @Texto";
			else
			{
				Console.WriteLine("Criterio invalido.");
				return;
			}

			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.Parameters.AddWithValue("@Texto", "%" + texto + "%");

			SqlDataReader reader = cmd.ExecuteReader();

			bool encontrado = false;
			while (reader.Read())
			{
				Console.WriteLine(reader["Nombre"] + " - " + reader["Lugar"] + " - " + reader["Dia"] +
					" - Cupos disponibles: " + reader["Cupos"]);
				encontrado = true;
			}
			reader.Close();

			if (!encontrado)
				Console.WriteLine("No se encontro ninguna excursion con ese criterio.");
		}
	}

	static void ModificarExcursion()
	{
		Console.Write("Nombre de la excursion a modificar: ");
		string nombre = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlBuscar = "SELECT Id, Lugar, Dia, Cupos FROM Excursiones WHERE Nombre = @Nombre";
			SqlCommand cmdBuscar = new SqlCommand(sqlBuscar, con);
			cmdBuscar.Parameters.AddWithValue("@Nombre", nombre);

			SqlDataReader reader = cmdBuscar.ExecuteReader();

			int id;
			string lugarActual, diaActual;
			int cuposActual;

			if (reader.Read())
			{
				id = (int)reader["Id"];
				lugarActual = reader["Lugar"].ToString();
				diaActual = reader["Dia"].ToString();
				cuposActual = (int)reader["Cupos"];
			}
			else
			{
				reader.Close();
				Console.WriteLine("No existe una excursion con ese nombre.");
				return;
			}
			reader.Close();

			Console.Write("Nuevo lugar: ");
			string nuevoLugar = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(nuevoLugar))
			{
				Console.WriteLine("Lugar vacio, se dejo el valor anterior.");
				nuevoLugar = lugarActual;
			}

			Console.Write("Nuevo dia: ");
			string nuevoDia = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(nuevoDia))
			{
				Console.WriteLine("Dia vacio, se dejo el valor anterior.");
				nuevoDia = diaActual;
			}

			Console.Write("Nuevos cupos: ");
			int nuevoCupos;
			if (!int.TryParse(Console.ReadLine(), out nuevoCupos) || nuevoCupos < 0)
			{
				Console.WriteLine("Cupos invalidos, se dejo el valor anterior.");
				nuevoCupos = cuposActual;
			}

			string sqlUpdate = "UPDATE Excursiones SET Lugar = @Lugar, Dia = @Dia, Cupos = @Cupos WHERE Id = @Id";
			SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con);
			cmdUpdate.Parameters.AddWithValue("@Lugar", nuevoLugar);
			cmdUpdate.Parameters.AddWithValue("@Dia", nuevoDia);
			cmdUpdate.Parameters.AddWithValue("@Cupos", nuevoCupos);
			cmdUpdate.Parameters.AddWithValue("@Id", id);
			cmdUpdate.ExecuteNonQuery();
		}

		Console.WriteLine("Excursion modificada.");
	}

	static void EliminarExcursion()
	{
		Console.Write("Nombre de la excursion a eliminar: ");
		string nombre = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlBuscar = "SELECT Id FROM Excursiones WHERE Nombre = @Nombre";
			SqlCommand cmdBuscar = new SqlCommand(sqlBuscar, con);
			cmdBuscar.Parameters.AddWithValue("@Nombre", nombre);
			object resultado = cmdBuscar.ExecuteScalar();

			if (resultado == null)
			{
				Console.WriteLine("No existe una excursion con ese nombre.");
				return;
			}

			int id = (int)resultado;

			string sqlContar = "SELECT COUNT(*) FROM Participantes WHERE IdExcursion = @Id";
			SqlCommand cmdContar = new SqlCommand(sqlContar, con);
			cmdContar.Parameters.AddWithValue("@Id", id);
			int contador = (int)cmdContar.ExecuteScalar();

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

				string sqlBorrarParticipantes = "DELETE FROM Participantes WHERE IdExcursion = @Id";
				SqlCommand cmdBorrarParticipantes = new SqlCommand(sqlBorrarParticipantes, con);
				cmdBorrarParticipantes.Parameters.AddWithValue("@Id", id);
				cmdBorrarParticipantes.ExecuteNonQuery();
			}

			string sqlBorrar = "DELETE FROM Excursiones WHERE Id = @Id";
			SqlCommand cmdBorrar = new SqlCommand(sqlBorrar, con);
			cmdBorrar.Parameters.AddWithValue("@Id", id);
			cmdBorrar.ExecuteNonQuery();
		}

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

	static void AgregarParticipante()
	{
		Console.Write("Documento (cedula o pasaporte): ");
		string documento = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(documento))
		{
			Console.WriteLine("El documento no puede estar vacio.");
			return;
		}

		Console.Write("Nombre: ");
		string nombre = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(nombre))
		{
			Console.WriteLine("El nombre no puede estar vacio.");
			return;
		}

		Console.Write("Telefono: ");
		string telefono = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(telefono))
		{
			Console.WriteLine("El telefono no puede estar vacio.");
			return;
		}

		Console.Write("Excursion a la que se inscribe: ");
		string excursionTexto = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlVerificarDoc = "SELECT COUNT(*) FROM Participantes WHERE Documento = @Documento";
			SqlCommand cmdVerificarDoc = new SqlCommand(sqlVerificarDoc, con);
			cmdVerificarDoc.Parameters.AddWithValue("@Documento", documento);
			int existeDoc = (int)cmdVerificarDoc.ExecuteScalar();

			if (existeDoc > 0)
			{
				Console.WriteLine("Ya existe un participante con ese documento.");
				return;
			}

			string sqlExcursion = "SELECT Id, Cupos FROM Excursiones WHERE Nombre = @Nombre";
			SqlCommand cmdExcursion = new SqlCommand(sqlExcursion, con);
			cmdExcursion.Parameters.AddWithValue("@Nombre", excursionTexto);
			SqlDataReader reader = cmdExcursion.ExecuteReader();

			int idExcursion;
			int cuposDisponibles;

			if (reader.Read())
			{
				idExcursion = (int)reader["Id"];
				cuposDisponibles = (int)reader["Cupos"];
			}
			else
			{
				reader.Close();
				Console.WriteLine("Esa excursion no existe.");
				return;
			}
			reader.Close();

			if (cuposDisponibles <= 0)
			{
				Console.WriteLine("No hay cupos disponibles para esa excursion.");
				return;
			}

			string sqlContar = "SELECT COUNT(*) FROM Participantes WHERE IdExcursion = @IdExcursion";
			SqlCommand cmdContar = new SqlCommand(sqlContar, con);
			cmdContar.Parameters.AddWithValue("@IdExcursion", idExcursion);
			int numeroCupo = (int)cmdContar.ExecuteScalar() + 1;

			string sqlInsertar = "INSERT INTO Participantes (Nombre, Documento, Telefono, IdExcursion, NumeroCupo, Pagado) " +
				"VALUES (@Nombre, @Documento, @Telefono, @IdExcursion, @NumeroCupo, 0)";
			SqlCommand cmdInsertar = new SqlCommand(sqlInsertar, con);
			cmdInsertar.Parameters.AddWithValue("@Nombre", nombre);
			cmdInsertar.Parameters.AddWithValue("@Documento", documento);
			cmdInsertar.Parameters.AddWithValue("@Telefono", telefono);
			cmdInsertar.Parameters.AddWithValue("@IdExcursion", idExcursion);
			cmdInsertar.Parameters.AddWithValue("@NumeroCupo", numeroCupo);
			cmdInsertar.ExecuteNonQuery();

			string sqlActualizarCupos = "UPDATE Excursiones SET Cupos = Cupos - 1 WHERE Id = @Id";
			SqlCommand cmdActualizarCupos = new SqlCommand(sqlActualizarCupos, con);
			cmdActualizarCupos.Parameters.AddWithValue("@Id", idExcursion);
			cmdActualizarCupos.ExecuteNonQuery();

			Console.WriteLine("Participante inscrito. Numero de cupo: " + numeroCupo);
		}
	}

	static void ListarParticipantes()
	{
		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sql = "SELECT p.Nombre, p.Documento, p.Telefono, e.Nombre AS Excursion, p.NumeroCupo, p.Pagado " +
				"FROM Participantes p INNER JOIN Excursiones e ON p.IdExcursion = e.Id";
			SqlCommand cmd = new SqlCommand(sql, con);
			SqlDataReader reader = cmd.ExecuteReader();

			bool hayDatos = false;
			Console.WriteLine("\n--- PARTICIPANTES ---");
			while (reader.Read())
			{
				hayDatos = true;
				string estado = (bool)reader["Pagado"] ? "si" : "no";
				Console.WriteLine(reader["Nombre"] + " - Doc: " + reader["Documento"] + " - Tel: " + reader["Telefono"] +
					" - Excursion: " + reader["Excursion"] + " - Cupo #" + reader["NumeroCupo"] + " - Pagado: " + estado);
			}
			reader.Close();

			if (!hayDatos)
				Console.WriteLine("No hay participantes registrados.");
		}
	}

	static void BuscarParticipante()
	{
		Console.WriteLine("Buscar por: 1.Documento  2.Nombre  3.Excursion");
		Console.Write("Criterio: ");
		string criterio = Console.ReadLine();

		Console.Write("Texto a buscar: ");
		string texto = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sql;
			if (criterio == "1")
				sql = "SELECT p.Nombre, p.Documento, p.Telefono, e.Nombre AS Excursion, p.NumeroCupo, p.Pagado " +
					"FROM Participantes p INNER JOIN Excursiones e ON p.IdExcursion = e.Id WHERE p.Documento LIKE @Texto";
			else if (criterio == "2")
				sql = "SELECT p.Nombre, p.Documento, p.Telefono, e.Nombre AS Excursion, p.NumeroCupo, p.Pagado " +
					"FROM Participantes p INNER JOIN Excursiones e ON p.IdExcursion = e.Id WHERE p.Nombre LIKE @Texto";
			else if (criterio == "3")
				sql = "SELECT p.Nombre, p.Documento, p.Telefono, e.Nombre AS Excursion, p.NumeroCupo, p.Pagado " +
					"FROM Participantes p INNER JOIN Excursiones e ON p.IdExcursion = e.Id WHERE e.Nombre LIKE @Texto";
			else
			{
				Console.WriteLine("Criterio invalido.");
				return;
			}

			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.Parameters.AddWithValue("@Texto", "%" + texto + "%");

			SqlDataReader reader = cmd.ExecuteReader();

			bool encontrado = false;
			while (reader.Read())
			{
				string estado = (bool)reader["Pagado"] ? "si" : "no";
				Console.WriteLine(reader["Nombre"] + " - Doc: " + reader["Documento"] + " - Tel: " + reader["Telefono"] +
					" - Excursion: " + reader["Excursion"] + " - Cupo #" + reader["NumeroCupo"] + " - Pagado: " + estado);
				encontrado = true;
			}
			reader.Close();

			if (!encontrado)
				Console.WriteLine("No se encontro ningun participante con ese criterio.");
		}
	}

	static void ModificarParticipante()
	{
		Console.Write("Documento del participante a modificar: ");
		string documento = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlBuscar = "SELECT Nombre, Telefono, IdExcursion FROM Participantes WHERE Documento = @Documento";
			SqlCommand cmdBuscar = new SqlCommand(sqlBuscar, con);
			cmdBuscar.Parameters.AddWithValue("@Documento", documento);

			SqlDataReader reader = cmdBuscar.ExecuteReader();

			string nombreActual, telefonoActual;
			int idExcursionActual;

			if (reader.Read())
			{
				nombreActual = reader["Nombre"].ToString();
				telefonoActual = reader["Telefono"].ToString();
				idExcursionActual = (int)reader["IdExcursion"];
			}
			else
			{
				reader.Close();
				Console.WriteLine("No existe un participante con ese documento.");
				return;
			}
			reader.Close();

			Console.Write("Nuevo nombre: ");
			string nuevoNombre = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(nuevoNombre))
				nuevoNombre = nombreActual;

			Console.Write("Nuevo telefono: ");
			string nuevoTelefono = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(nuevoTelefono))
				nuevoTelefono = telefonoActual;

			Console.Write("Pagado? (s/n): ");
			bool pagado = Console.ReadLine().ToLower() == "s";

			string sqlUpdate = "UPDATE Participantes SET Nombre = @Nombre, Telefono = @Telefono, Pagado = @Pagado WHERE Documento = @Documento";
			SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con);
			cmdUpdate.Parameters.AddWithValue("@Nombre", nuevoNombre);
			cmdUpdate.Parameters.AddWithValue("@Telefono", nuevoTelefono);
			cmdUpdate.Parameters.AddWithValue("@Pagado", pagado);
			cmdUpdate.Parameters.AddWithValue("@Documento", documento);
			cmdUpdate.ExecuteNonQuery();

			Console.Write("Desea cambiar de excursion? (s/n): ");
			if (Console.ReadLine().ToLower() == "s")
			{
				Console.Write("Nueva excursion: ");
				string excursionTexto = Console.ReadLine();

				string sqlNuevaExcursion = "SELECT Id, Cupos FROM Excursiones WHERE Nombre = @Nombre";
				SqlCommand cmdNuevaExcursion = new SqlCommand(sqlNuevaExcursion, con);
				cmdNuevaExcursion.Parameters.AddWithValue("@Nombre", excursionTexto);
				SqlDataReader readerNueva = cmdNuevaExcursion.ExecuteReader();

				int idExcursionNueva;
				int cuposNueva;

				if (readerNueva.Read())
				{
					idExcursionNueva = (int)readerNueva["Id"];
					cuposNueva = (int)readerNueva["Cupos"];
				}
				else
				{
					readerNueva.Close();
					Console.WriteLine("Esa excursion no existe. Se mantuvo la excursion anterior.");
					return;
				}
				readerNueva.Close();

				if (idExcursionNueva == idExcursionActual)
				{
					Console.WriteLine("Es la misma excursion, no hay cambios.");
					return;
				}

				if (cuposNueva <= 0)
				{
					Console.WriteLine("No hay cupos en la nueva excursion. Se mantuvo la excursion anterior.");
					return;
				}

				string sqlLiberar = "UPDATE Excursiones SET Cupos = Cupos + 1 WHERE Id = @Id";
				SqlCommand cmdLiberar = new SqlCommand(sqlLiberar, con);
				cmdLiberar.Parameters.AddWithValue("@Id", idExcursionActual);
				cmdLiberar.ExecuteNonQuery();

				string sqlContar = "SELECT COUNT(*) FROM Participantes WHERE IdExcursion = @Id";
				SqlCommand cmdContar = new SqlCommand(sqlContar, con);
				cmdContar.Parameters.AddWithValue("@Id", idExcursionNueva);
				int numeroCupoNuevo = (int)cmdContar.ExecuteScalar() + 1;

				string sqlOcupar = "UPDATE Excursiones SET Cupos = Cupos - 1 WHERE Id = @Id";
				SqlCommand cmdOcupar = new SqlCommand(sqlOcupar, con);
				cmdOcupar.Parameters.AddWithValue("@Id", idExcursionNueva);
				cmdOcupar.ExecuteNonQuery();

				string sqlActualizarParticipante = "UPDATE Participantes SET IdExcursion = @IdExcursion, NumeroCupo = @NumeroCupo WHERE Documento = @Documento";
				SqlCommand cmdActualizarParticipante = new SqlCommand(sqlActualizarParticipante, con);
				cmdActualizarParticipante.Parameters.AddWithValue("@IdExcursion", idExcursionNueva);
				cmdActualizarParticipante.Parameters.AddWithValue("@NumeroCupo", numeroCupoNuevo);
				cmdActualizarParticipante.Parameters.AddWithValue("@Documento", documento);
				cmdActualizarParticipante.ExecuteNonQuery();
			}
		}

		Console.WriteLine("Participante modificado.");
	}

	static void EliminarParticipante()
	{
		Console.Write("Documento del participante a eliminar: ");
		string documento = Console.ReadLine();

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

			string sqlBuscar = "SELECT Nombre, IdExcursion FROM Participantes WHERE Documento = @Documento";
			SqlCommand cmdBuscar = new SqlCommand(sqlBuscar, con);
			cmdBuscar.Parameters.AddWithValue("@Documento", documento);

			SqlDataReader reader = cmdBuscar.ExecuteReader();

			string nombre;
			int idExcursion;

			if (reader.Read())
			{
				nombre = reader["Nombre"].ToString();
				idExcursion = (int)reader["IdExcursion"];
			}
			else
			{
				reader.Close();
				Console.WriteLine("No existe un participante con ese documento.");
				return;
			}
			reader.Close();

			Console.WriteLine("Participante encontrado: " + nombre);
			Console.Write("Seguro que desea eliminarlo? (s/n): ");
			string resp = Console.ReadLine().ToLower();
			if (resp != "s")
			{
				Console.WriteLine("Operacion cancelada.");
				return;
			}

			string sqlBorrar = "DELETE FROM Participantes WHERE Documento = @Documento";
			SqlCommand cmdBorrar = new SqlCommand(sqlBorrar, con);
			cmdBorrar.Parameters.AddWithValue("@Documento", documento);
			cmdBorrar.ExecuteNonQuery();

			string sqlLiberar = "UPDATE Excursiones SET Cupos = Cupos + 1 WHERE Id = @Id";
			SqlCommand cmdLiberar = new SqlCommand(sqlLiberar, con);
			cmdLiberar.Parameters.AddWithValue("@Id", idExcursion);
			cmdLiberar.ExecuteNonQuery();
		}

		Console.WriteLine("Participante eliminado, cupo liberado.");
	}
}