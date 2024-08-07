using System;
using System.Diagnostics;
using SQLite;
using AppNotas.Database;

namespace AppNotas.Dependencies
{
	public class SQLiteDefaultConnection : SQLiteConnection
	{
		public SQLiteDefaultConnection() : base(Database.Database.DatabasePath, Database.Database.Flags)
		{
			this.Tracer = new Action<string>(q => Debug.WriteLine(q));
			this.Trace = true;
        }

        public SQLiteConnection parent() { return parent(); }
    }

    public class SQLiteDefaultConnectionAsync : SQLiteAsyncConnection
    {
        public SQLiteDefaultConnectionAsync() : base(Database.Database.DatabasePath, Database.Database.Flags)
        {
        }
    }
}

