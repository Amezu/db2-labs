using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Lab6
{
    class Exercise3
    {
        static string connectionText = @"DATA SOURCE=MSSQLServer;" +
        "INITIAL CATALOG=Lab6db; INTEGRATED SECURITY=SSPI;";

        static void Main(string[] args)
        {
            CreateTables();
            InsertDataFromCSV("student.csv", "student");
            InsertDataFromCSV("wykladowca.csv", "wykladowca");
            InsertDataFromCSV("przedmiot.csv", "przedmiot");
            InsertDataFromCSV("grupa.csv", "grupa");
            Console.WriteLine();
            CountStudentsForInstructors();
            Console.WriteLine();
            CountStudentsForSubject();

            Console.Write("Press a Key to Continue...");
            Console.ReadKey();
        }

        static void CreateTables()
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            string[] tableCreatingCommands =
            {
                @"CREATE TABLE student
                (
	                id int PRIMARY KEY,
	                fname varchar(30) NOT NULL,
	                lname varchar(30) NOT NULL
                )",
                @"CREATE TABLE wykladowca
                (
	                id int PRIMARY KEY,
	                fname varchar(30) NOT NULL,
	                lname varchar(30) NOT NULL
                )",
                @"CREATE TABLE przedmiot
                (
	                id int PRIMARY KEY,
	                name varchar(50) NOT NULL
                )",
                @"CREATE TABLE grupa
                (
	                id_wykl int REFERENCES wykladowca(id),
	                id_stud int REFERENCES student(id),
	                id_przed int REFERENCES przedmiot(id),
	                PRIMARY KEY(id_wykl, id_stud, id_przed)
                )"
            };

            try
            {
                connection = new SqlConnection(connectionText);
                connection.Open();

                foreach (string commandText in tableCreatingCommands)
                {
                    try
                    {
                        command = new SqlCommand(commandText, connection);
                        command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            { connection.Close(); }
        }

        static void InsertDataFromCSV(string filePath, string tableName)
        {
            SqlConnection connection = null;
            string commandText;
            StringBuilder sb = new StringBuilder("(");
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                    sb.Append(line + "), (");
            }
            sb.Replace('"', '\'');
            commandText = "INSERT INTO " + tableName + " VALUES " + sb.ToString(0, sb.Length - 3);

            try
            {
                connection = new SqlConnection(connectionText);
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            { connection.Close(); }
        }

        static void CountStudentsForInstructors()
        {
            SqlConnection connection = null;
            string commandText =
                @"SELECT fname
                        ,lname
                        ,COUNT(DISTINCT id_stud) AS students
                  FROM wykladowca w
                  JOIN grupa ON id_wykl = w.id
                  GROUP BY w.fname, w.lname";

            try
            {
                connection = new SqlConnection(connectionText);
                connection.Open();
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Console.WriteLine("{0}\t{1}\t{2}",
                    datareader["fname"].ToString(),
                    datareader["lname"].ToString(),
                    datareader["students"].ToString());
                }
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            finally
            { connection.Close(); }
        }

        static void CountStudentsForSubject()
        {
            SqlConnection connection = null;
            string commandText =
                @"SELECT name
                        ,COUNT(id_stud) AS students
                  FROM przedmiot p
                  JOIN grupa ON id_przed = p.id
                  GROUP BY p.name";

            try
            {
                connection = new SqlConnection(connectionText);
                connection.Open();
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Console.WriteLine("{0}\t{1}",
                    datareader["name"].ToString(),
                    datareader["students"].ToString());
                }
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            finally
            { connection.Close(); }
        }
    }
}
