
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
			Console.WriteLine("\n===== SISTEMA DE EXCURSIONES =====");
			Console.WriteLine("1. Agregar excursion");
			Console.WriteLine("2. Listar excursiones");
			Console.WriteLine("3. Salir");
			Console.Write("Opcion: ");
			opcion = Console.ReadLine();

			switch (opcion)
			{
				case "1":
					AgregarExcursion();
					break;

				case "2":
					ListarExcursiones();
					break;

				case "3":
					Console.WriteLine("Saliendo...");
					break;

				default:
					Console.WriteLine("Opcion invalida.");
					break;
			}

		} while (opcion != "3");
	}

	static void AgregarExcursion()
	{
		Console.Write("Nombre: ");
		string nombre = Console.ReadLine();

		Console.Write("Lugar: ");
		string lugar = Console.ReadLine();

		Console.Write("Dia: ");
		string dia = Console.ReadLine();

		Console.Write("Cupos: ");
		int cupos = int.Parse(Console.ReadLine());

		using (SqlConnection con = new SqlConnection(conexion))
		{
			con.Open();

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

			string sql = "SELECT * FROM Excursiones";

			SqlCommand cmd = new SqlCommand(sql, con);

			SqlDataReader reader = cmd.ExecuteReader();

			Console.WriteLine("\n--- LISTA DE EXCURSIONES ---");

			while (reader.Read())
			{
				Console.WriteLine(
					reader["Id"] + " - " +
					reader["Nombre"] + " - " +
					reader["Lugar"] + " - " +
					reader["Dia"] + " - Cupos: " +
					reader["Cupos"]);
			}

			reader.Close();
		}
	}
}