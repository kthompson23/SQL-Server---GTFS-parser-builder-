using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace SQL_Server_GTFS_builder_parser
{   

    class FileParser
    {

        public FileParser() {}

        // parses the file and returns an Agency entitiy object. This method differs from the other methods
        // because the Agency table only contains one row.
        public Agency Agencys(string fileName)
        {
            Agency agencyRow = new Agency();
            string tempRow;
            string[] tempLine;

            try
            {
                stream = new StreamReader(fileName);

                // ignore first row
                stream.ReadLine();

                tempRow = stream.ReadLine();

                // split the input based on commas
                tempLine = tempRow.Split(',');
                agencyRow.Agency_name = tempLine[0];
                agencyRow.Agency_timezone = tempLine[1];
                agencyRow.Agency_url = tempLine[2];

            }
            catch (IOException)
            {
                Console.WriteLine("Error Reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return agencyRow;
        }

        public ArrayList Calendars(string fileName)
        {
            ArrayList calendarRows = new ArrayList();
            string[] tempLine;
            string tempRow = "";

            try
            {
                stream = new StreamReader(fileName);
                // skip first row which are table headers
                tempRow = stream.ReadLine();

                while ((tempRow = stream.ReadLine()) != null)
                {
                    Calendar calendar = new Calendar();
                    // Replace the quotes with white space, split the input by commas
                    // the Route shapes.txt does not contain quotes but incase the source changes
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');


                    // This for loop converts all string representations of Boolean value "0" and "1" to the C#
                    // equivalents "false" and true". This is necessary because C# does not allow "0" and "1" to be 
                    // used as Boolean values and will throw an error if there is an attempt to cast
                    for (int i = 0; i < tempLine.Length; i++)
                    {
                        if (tempLine[i] == "0")
                        {
                            tempLine[i] = "false";
                        }
                        else if (tempLine[i] == "1")
                        {
                            tempLine[i] = "true";
                        }
                    }

                    calendar.Service_id = tempLine[0];
                    calendar.Start_date = MakeDateTime(tempLine[1]);
                    calendar.End_date = MakeDateTime(tempLine[2]);
                    calendar.Sunday = Boolean.Parse(tempLine[3]);
                    calendar.Monday = Boolean.Parse(tempLine[4]);
                    calendar.Tuesday = Boolean.Parse(tempLine[5]);
                    calendar.Wednesday = Boolean.Parse(tempLine[6]);
                    calendar.Thursday = Boolean.Parse(tempLine[7]);
                    calendar.Friday = Boolean.Parse(tempLine[8]);
                    calendar.Saturday = Boolean.Parse(tempLine[9]);

                    calendarRows.Add(calendar);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return calendarRows;
        }

        public ArrayList Routes(string fileName)
        {
            ArrayList routeRows = new ArrayList();
            string [] tempLine;
            string tempRow = "";

            try
            {
                stream = new StreamReader(fileName);
                // skip the first row, which are table headers
                tempRow = stream.ReadLine();

                while ((tempRow = stream.ReadLine()) != null)
                {
                    Route route = new Route();
                    // Replace the quotes with white space, split the input by commas
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');

                    // place each value in tempLine into the Route object
                    route.Route_id = tempLine[0];
                    route.Route_short_name = tempLine[1];
                    route.Route_long_name = tempLine[2];
                    route.Route_desc = tempLine[3];
                    route.Route_type = Int32.Parse(tempLine[4]);

                    // add the route object to the routeRows ArrayList
                    routeRows.Add(route);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return routeRows;
        }
        /*
        public ArrayList Shapes(string fileName)
        {
            ArrayList shapeRows = new ArrayList();
            string[] tempLine;
            string tempRow = "";

            try
            {
                stream = new StreamReader(fileName);
                // skip the first row which are table haeaders
                tempRow = stream.ReadLine();

                while ((tempRow = stream.ReadLine()) != null)
                {
                    Shape shape = new Shape();
                    // Replace the quotes with white space, split the input by commas
                    // the Route shapes.txt does not contain quotes but incase the source changes
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');

                    shape.Shape_id = tempLine[0];
                    shape.Shape_pt_lat = float.Parse(tempLine[1]);
                    shape.Shape_pt_lon = float.Parse(tempLine[2]);
                    shape.Shape_pt_sequence = Int32.Parse(tempLine[3]);
                    shape.Shape_dist_traveled = null;

                    // add the shape object to the shapeRows ArrayList
                    shapeRows.Add(shape);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return shapeRows;
        }
        */
        public ArrayList Stops(string fileName)
        {
            ArrayList stopRows = new ArrayList();
            string[] tempLine;
            string tempRow = "";

            try
            {
                stream = new StreamReader(fileName);
                // skip the first row which are table headers
                tempRow = stream.ReadLine();
                while ((tempRow = stream.ReadLine()) != null)
                {
                    Stop stopObject = new Stop();
                    // Replace the quotes with white space, split the input by commas
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');

                    stopObject.Stop_id = tempLine[0];
                    stopObject.Stop_name = tempLine[1];
                    stopObject.Stop_desc = tempLine[2];
                    stopObject.Stop_lat = float.Parse(tempLine[3]);
                    stopObject.Stop_lon = float.Parse(tempLine[4]);

                    // add the Stop object to the stopRows ArrayList
                    stopRows.Add(stopObject);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading the file: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return stopRows;
        }

        public ArrayList Stop_Times(string fileName)
        {
            ArrayList stop_timeRows = new ArrayList();
            string[] tempLine;
            string tempRow = "";
            int secSinceMidnightA = 0;
            int secSinceMidnightD = 0;

            try
            {
                stream = new StreamReader(fileName);
                // skip the first row which are table headers
                tempRow = stream.ReadLine();

                while ((tempRow = stream.ReadLine()) != null)
                {
                    Stop_time stop_time = new Stop_time();
                    // Replace the quotes with white space, split the input by commas
                    // the file stop_times.txt does not contain quotes but left in incase the source changes
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');

                    secSinceMidnightA = ConvertTimeToSeconds(tempLine[1]);
                    secSinceMidnightD = ConvertTimeToSeconds(tempLine[2]);

                    // remove the T from the beginning of the trip_id ex: T134 is now 134
                    stop_time.Trip_id = Int32.Parse(tempLine[0].Substring(1));
                    stop_time.Arrival_time = tempLine[1];
                    stop_time.Departure_time = tempLine[2];
                    stop_time.Stop_id = tempLine[3];
                    stop_time.Stop_sequence = Int32.Parse(tempLine[4]);

                    // add the stop_time object to the stop_timeRows ArrayList
                    stop_timeRows.Add(stop_time); 
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();

            return stop_timeRows;
        }

        public ArrayList Trips(string fileName)
        {
            ArrayList tripRows = new ArrayList();
            string[] tempLine;
            string tempRow = "";

            try
            {
                stream = new StreamReader(fileName);
                // skip the first row which are table headers
                tempRow = stream.ReadLine();

                while ((tempRow = stream.ReadLine()) != null)
                {
                    Trip tripObject = new Trip();
                    // Replace the quotes with white space, split the input by commas
                    // the file trips.txt does not contain quotes but left in incase the source changes
                    tempRow = tempRow.Replace("\"", "");
                    tempLine = tempRow.Split(',');

                    // This for loop converts all string representations of Boolean value "0" and "1" to the C#
                    // equivalents "false" and true". This is necessary because C# does not allow "0" and "1" to be 
                    // used as Boolean values and will throw an error if there is an attempt to cast
                    for (int i = 0; i < tempLine.Length; i++)
                    {
                        if (tempLine[i] == "0")
                        {
                            tempLine[i] = "false";
                        }
                        else if (tempLine[i] == "1")
                        {
                            tempLine[i] = "true";
                        }
                    }

                    // remove the T from the beginning of the trip_id ex: T134 is now 134
                    tripObject.Trip_id = Int32.Parse(tempLine[0].Substring(1));
                    tripObject.Route_id = tempLine[1];
                    tripObject.Direction_id = Boolean.Parse(tempLine[2]);
                    tripObject.Service_id = tempLine[3];

                    // add the Trip object to the tripRows ArrayList;
                    tripRows.Add(tripObject);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading from: " + fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            stream.Close();
            return tripRows;
        }

   
        /// <summary>
        /// private utility function to change a string in YYYYMMDD into a DateTime object
        /// </summary>
        /// <param name="date">string in the form YYYYMMDD</param>
        /// <returns>DateTime object with the date values filled in.</returns>
        private DateTime MakeDateTime(string date)
        {
            int year;
            int month;
            int day;

            year = Int32.Parse(date.Substring(0, 4));
            month = Int32.Parse(date.Substring(4, 2));
            day = Int32.Parse(date.Substring(6, 2));

            DateTime dateTime = new DateTime(year, month, day);

            return dateTime;
        }

        /// <summary>
        /// A private utility to convert string time values in the form "HH:MM:SS
        /// to seconds since midnight. Since the GTFS database does not run on a 24 hour clock (some trips
        /// may extend past midnight into the "24th" hour) storing arrival and departure times as DateTime objects
        /// is not possible. So they may either be stored as strings "HH:MM:SS" or as integers.
        /// </summary>
        /// <param name="a_time">string in the form "HH:MM:SS"</param>
        /// <returns>time as an integer, seconds since midnight</returns>
        private int ConvertTimeToSeconds(string a_time)
        {
            int secSinceMidnight = 0;
            int hours, minutes, seconds;
            // split the a_time string based on the semicolons
            string[] splitTime = a_time.Split(':');

            hours = Int32.Parse(splitTime[0]);
            minutes = Int32.Parse(splitTime[1]);
            seconds = Int32.Parse(splitTime[2]);

            secSinceMidnight = (3600 * hours) + (60 * minutes) + seconds;

            return secSinceMidnight;
        }

        private StreamReader stream;
    }
}