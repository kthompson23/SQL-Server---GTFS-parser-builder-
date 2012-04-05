using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SQL_Server_GTFS_builder_parser
{
    // GTFSDBBuilder reads in a directory containing several csv text files in GTFS (General Transit Feed Specification) format.
    // Since this data is updated every few months, this program allows the database to quickly be repopulated. If the user would like to delete the
    // existing database first, the -d flag should be passed in after the filename as a command line argument.
    class GTFSDBBuilder
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("No directory or database path passed in. Exiting");
                Environment.Exit(1);
            }

            // args[1] should be the absolute path to the the database
            // it will be used to build a connection string to the database
            DBHelper dbHelper = new DBHelper(args[1]);
                string userInput;
                do
                {
                    Console.WriteLine("Would you like to delete the existing values in your database? y/n");
                    userInput = Console.ReadLine();
                } while (userInput != "n" && userInput != "y");

                if (userInput == "y")
                {
                    dbHelper.DeleteDatabase();
                    Console.WriteLine("Database deleted");
                }
 
            DirectoryReader dr = new DirectoryReader(args[0]);
            dr.OpenDirectory();

            /*
            // alphabetized list of files in the passed in directory.
            foreach (string file in dr.transportFiles)
            {
                Console.WriteLine(file);
            }
            */

            FileParser fileParser = new FileParser();

            // add the entity objects to the database so they persis
            // create the database first if it does not exis
            dbHelper.CreateDataBase();

            // parse each of the files to get a collection of ArrayLists to insert into the database   
            foreach (string file in dr.transportFiles)
            {
                if (file.Contains("agency")) {
                    Agency agencyRow = fileParser.Agencys(file);

                    dbHelper.AddAgency(agencyRow);
                    Console.WriteLine("Agency added");
                }
                else if (file.Contains("calendar_dates"))
                {
 
                }
                else if (file.Contains("calendars"))
                {
                    ArrayList calendarRows = fileParser.Calendars(file);

                    foreach (Calendar calendar in calendarRows)
                    {
                        dbHelper.AddCalendar(calendar);
                    }
                    Console.WriteLine("Calendars added");
                }
                else if (file.Contains("routes"))
                {
                    ArrayList routeRows = fileParser.Routes(file);

                    foreach (Route route in routeRows)
                    {
                        dbHelper.AddRoutes(route);
                    }

                    Console.WriteLine("Routes Added");
                }
                else if (file.Contains("shapes"))
                {
                    /*
                    ArrayList shapeRows = fileParser.Shapes(dr.transportFiles[4]);
                    foreach (Shape shape in shapeRows)
                    {
                        dbHelper.AddShapes(shape);
                    }
            
                    Console.WriteLine("Shapes added");
                    */
                }
                else if (file.Contains("stops"))
                {
                    ArrayList stopRows = fileParser.Stops(file);

                    foreach (Stop stop in stopRows)
                    {
                        dbHelper.AddStops(stop);
                    }

                    Console.WriteLine("Stops added");
                }
                else if (file.Contains("stop_times"))
                {
                    ArrayList stop_timeRows = fileParser.Stop_Times(file);

                    foreach (Stop_time stop_time in stop_timeRows)
                    {
                        dbHelper.AddStop_times(stop_time);
                    }

                    Console.WriteLine("Stop_Times added");
                }
                else if (file.Contains("trips"))
                {
                    ArrayList tripRows = fileParser.Trips(file);

                    foreach (Trip trip in tripRows)
                    {
                        dbHelper.AddTrips(trip);
                    }

                    Console.WriteLine("Trips added");
                }
                else
                {
                    Console.WriteLine(file + " is an unrecognized file.");
                    Console.ReadLine();
                }
            }             
            
            Console.WriteLine("Database construction done.");
        }
    }
}
