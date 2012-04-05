using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace SQL_Server_GTFS_builder_parser
{
    public class DBHelper
    {
        private GTFSDataContext context;
        private string ConnectionString;

        public DBHelper(string absDBPath)
        {
            ConnectionString = "Data Source=" + absDBPath + ";Persist Security Info=\"false";
            context = new GTFSDataContext(ConnectionString);         

        }

       public void CreateDataBase()
       {
 
            if (!context.DatabaseExists())
            {
                // create database if it does not exist
                context.CreateDatabase();
            }
       }

       public void DeleteDatabase()
       {

            if (context.DatabaseExists())
            {
                // delete database if it exists
                context.DeleteDatabase();
            }
       }

       public void AddAgency(Agency agency)
       {

            if (context.DatabaseExists())
            {
                context.Agencies.InsertOnSubmit(agency);
                context.SubmitChanges();
            }
       }

       public void AddCalendar(Calendar calendar)
       {

            if (context.DatabaseExists())
            {
                context.Calendars.InsertOnSubmit(calendar);
                context.SubmitChanges();
            }

       }

       public void AddRoutes(Route route)
       {

            if (context.DatabaseExists())
            {
                context.Routes.InsertOnSubmit(route);
                context.SubmitChanges();
            }
       }
/*

       public void AddShapes(Shape shape)
       {
            if (context.DatabaseExists())
            {
                context.Shapes.InsertOnSubmit(shape);
                context.SubmitChanges();
            }
       }
*/
       public void AddStops(Stop stop)
       {
            if (context.DatabaseExists())
            {
                context.Stops.InsertOnSubmit(stop);
                context.SubmitChanges();
            }
       }

       public void AddStop_times(Stop_time stop_time)
       {
            if (context.DatabaseExists())
            {
                context.Stop_times.InsertOnSubmit(stop_time);
                context.SubmitChanges();
            }
       }

       public void AddTrips(Trip trip)
       {
            if (context.DatabaseExists())
            {
                context.Trips.InsertOnSubmit(trip);
                context.SubmitChanges();
            }
       }
    }
}
