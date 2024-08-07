using System;
using System.Collections.Generic;
using System.IO;
using AppNotas.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using AppNotas.Dependencies;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace AppNotas.Database
{
    public static class Database
    {

        /*************************************************************************
         * 
         *                      DATABASE CONSTANTS SECTION 
         * 
         *************************************************************************/

        public enum MigrationStatus : int
        {
            NONE = 0,
            RESTART = 1,
            RESTARTANDSEED = 2,
            RESTORE = 3,
            RESTORED = 4,
            MIGRATED = 5,
            MIGRATEDANDSEEDED = 6,
        }

        /*
         * Migration database status used to run 
         * migrations depending on status, can be 
         * set by the user view interface
         */
        public static MigrationStatus CurrentMigrationStatus
        {
            get
            {
                Setting setting;
                try
                {
                    setting = new SQLiteDefaultConnection()
                        .Get<Setting>(i => i.key == "migration");
                } catch (Exception)
                {
                    return MigrationStatus.NONE;
                }
                return (MigrationStatus)setting.value;
            }
            set
            {
                SQLiteDefaultConnection obj = new SQLiteDefaultConnection();
                Setting setting = new Setting();

                setting.key = "migration";
                setting.value = (int)value;

                List<Setting> settings = obj.GetAllWithChildren<Setting>();

                if (CurrentMigrationStatus == MigrationStatus.NONE)
                    obj.Insert(setting);
                else
                {
                    setting = obj.Get<Setting>(i => i.key == "migration");
                    setting.value = (int)value;

                    obj.Update(setting);
                }
            }
        }

        /*
         * Connection variable constants
         */
        public const string DatabaseFilename = "taskDatabase.db3";

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        /*************************************************************************
         * 
         *                  DATABASE MIGRATION SECTION 
         * 
         *************************************************************************/

        /*
         * Start database public functions
         */
        public static void Create()
        {
            new SQLiteDefaultConnection()
                .RunInTransaction(databaseUp);
        }
        public static void Remove()
        {
            new SQLiteDefaultConnection()
                .RunInTransaction(databaseDown);
        }

        /*
         * Drop and create database tables
         */
        private static void databaseUp()
        {
            var obj = new SQLiteDefaultConnection();             

            obj.CreateTable<Section>();
            obj.CreateTable<Note>();
            obj.CreateTable<Setting>();
            obj.CreateTable<Music>();
        }
        private static void databaseDown()
        {
            var obj = new SQLiteDefaultConnection();

            obj.DropTable<Section>();
            obj.DropTable<Note>();
            obj.DropTable<Setting>();
            obj.DropTable<Music>();
        }

        /*
         * Forcefully restarts database 
         */
        public static void ForceRestart()
        {
            Remove();
            Create();
            CurrentMigrationStatus = MigrationStatus.MIGRATED;
        } 
        public static void ForceRestartAndSeed()
        {
            ForceRestart();
            Seed();
            CurrentMigrationStatus = MigrationStatus.MIGRATEDANDSEEDED;
        }

        /*
         * Keeps the data save in memory and 
         * restarts and inserts data back again
         * Will restore the following data:
         *      -Notes
         *      -Sections
         */
        public static void Restore()
        {
            SQLiteDefaultConnection db= new SQLiteDefaultConnection();
            List<Section> sections = db
                .GetAllWithChildren<Section>(i => i.FatherId == null, true);

            ForceRestart();
            foreach(Section section in sections)
                db.InsertWithChildren(section, true);

            CurrentMigrationStatus = MigrationStatus.RESTORED;
        }

        /*
         * Runs migration depending on database status
         */
        public static void runMigration()
        {
            switch (CurrentMigrationStatus)
            {
                case MigrationStatus.NONE:
                case MigrationStatus.RESTARTANDSEED:
                    ForceRestartAndSeed();
                    break;
                case MigrationStatus.RESTART:
                    ForceRestart();
                    break;
                case MigrationStatus.RESTORE:
                    Restore();
                    break;
                case MigrationStatus.RESTORED:
                case MigrationStatus.MIGRATED:
                case MigrationStatus.MIGRATEDANDSEEDED:
                    break;
            }
        }

        /*************************************************************************
         * 
         *                          SEEDER SECTION 
         * 
         *************************************************************************/

        /*
         * Creates dummy data for the aplication
         */
        public static void Seed()
        {
            var connection = new SQLiteDefaultConnection();

            SectionSeeder(connection);

            List<Section> sections = connection
                .GetAllWithChildren<Section>(i => i.isFinal == true, true);
            foreach (Section section in sections) 
                NoteSeeder(connection, section);
        }

        private static void SectionSeeder(SQLiteDefaultConnection conn)
        {
            for (int i = 0; i < 3; i++)
            {
                var father = new Section("Father " + i);
                for (int j = 0; j < 3; j++)
                {
                    Section medium = new Section("SonSection " + i + " " + j);
                    for (int k = 0; k < 5; k++)
                    {
                        var child = new Section("SonSection " + i + " " + j + " " + k);
                        medium.Sections.Add(child);
                        medium.isFinal = false;
                    }
                    father.Sections.Add(medium);
                    father.isFinal = false;
                }
                conn.InsertWithChildren(father, true);
            }
        }

        private static void NoteSeeder(SQLiteDefaultConnection conn, Section father)
        {
            if (father == null)
                return;

            for (int i = 0; i < 3; i++)
            {
                var note = new Note("Note " + i);
                note.text = "note number " + i;
                conn.Insert(note);
                father.Notes.Add(note);
            }
            conn.UpdateWithChildren(father);
        }
    }
}

