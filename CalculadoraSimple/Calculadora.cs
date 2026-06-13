using System;

bool keepRunning = true;

while (keepRunning)
{

	Console.WriteLine("\n------------------NEW OPERATION-------------------------------------------");
	Console.WriteLine("\n");


	Console.WriteLine("Welcome to your calculator!\n");
	Console.WriteLine("\nPlease select the operation you want to perform:\n");
	Console.WriteLine("1. Addition");
	Console.WriteLine("2. Subtraction");
	Console.WriteLine("3. Multiplication");
	Console.WriteLine("4. Division");
	Console.WriteLine("5. Student result");
	Console.WriteLine("6. Exit\n");

	Console.Write("Choose an option: ");

	int option;

	try
	{
		option = Convert.ToInt32(Console.ReadLine());
	}
	catch
	{
		Console.WriteLine("Error: invalid option");
		continue;
	}

	if (option == 6)
	{
		keepRunning = false;
		Console.WriteLine("Program ended");
	}
	else if (option >= 1 && option <= 4)
	{
		decimal num1;
		decimal num2;

		Console.Write("Enter first number: ");
		try
		{
			num1 = Convert.ToDecimal(Console.ReadLine());
		}
		catch
		{
			Console.WriteLine("Error: invalid number");
			continue;
		}

		Console.Write("Enter second number: ");
		try
		{
			num2 = Convert.ToDecimal(Console.ReadLine());
		}
		catch
		{
			Console.WriteLine("Error: invalid number");
			continue;
		}

		if (option == 1)
			Console.WriteLine("Result: " + (num1 + num2));
		else if (option == 2)
			Console.WriteLine("Result: " + (num1 - num2));
		else if (option == 3)
			Console.WriteLine("Result: " + (num1 * num2));
		else if (option == 4)
		{
			if (num2 == 0)
				Console.WriteLine("Cannot divide by 0");
			else
				Console.WriteLine("Result: " + (num1 / num2));
		}
	}
	else if (option == 5)
	{
		decimal grade;

		Console.Write("Enter result: ");
		try
		{
			grade = Convert.ToDecimal(Console.ReadLine());
		}
		catch
		{
			Console.WriteLine("Error: invalid grade");
			continue;
		}

		if (grade >= 70)
			Console.WriteLine("Passed");
		else if (grade >= 0)
			Console.WriteLine("Failed");
		else
			Console.WriteLine("Invalid grade");
	}
	else
	{
		Console.WriteLine("Invalid option");
	}

}