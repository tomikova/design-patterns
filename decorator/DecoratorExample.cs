using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoratorExample
{
    public struct Param
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string Key { get; set; }

        public string Ispis()
        {
            return "Table: " + Table + "; Column: " + Column + "; Key: " + Key; 
        }
    }

    abstract class Database
    {
        public abstract int Query(Param p);
    }

    class MyDatabase : Database
    {
        override public int Query(Param p)
        {
            int random = new Random().Next(-1, 1);
            return random;
        }
    }

    abstract class Decorator : Database
    {
        public Database _database;
        public Decorator(Database database)
        {
            _database = database;
        }
    }

    class LogFileDecorator : Decorator
    {
        private string LogFilePath;

        public LogFileDecorator(Database database, string filePath) : base(database)
        {
            this.LogFilePath = filePath;
        }

        override public int Query(Param p)
        {
            int query = _database.Query(p);

            if (!File.Exists(LogFilePath))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(LogFilePath)) { }
            }

            using (StreamWriter sw = File.AppendText(LogFilePath))
            {
                sw.WriteLine("Query: " + p.Ispis());
                sw.WriteLine("Result: " + query.ToString());
            }
            return query;
        }
    }

    class ExceptionDecorator : Decorator
    {
        public ExceptionDecorator(Database database)
            : base(database)
        {
        }

        public override int Query(Param p)
        {
            int query = _database.Query(p);
            if (query != 0)
            {
                throw new Exception("Database error");
            }

            return query;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "var\\tmp\\mybase.log";

            Param param = new Param()
            {
                Table = "Animal",
                Column = "Species",
                Key = "Id"
            };

            Database db = new MyDatabase();
            LogFileDecorator lfd = new LogFileDecorator(db, filePath);
            ExceptionDecorator exd = new ExceptionDecorator(lfd);

            lfd.Query(param);

            try
            {
                exd.Query(param);
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("Error: " + ex.Message);
                 
                }         
            }
        }
    }
}
