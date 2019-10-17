using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class DatabaseController
{
    private string dbpath = "URI=file:" + Application.dataPath + "/Data/fearful_data.sqlite";
    private IDbConnection dbcon;
    public DatabaseController()
    {
        // connection created in instance
        IDbConnection dbcon = new SqliteConnection(dbpath);
        dbcon.Open();

        IDbCommand dbcmd;
        IDataReader reader;

        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE test_table (id INTEGER PRIMARY KEY AUTOINCREMENT, val INTEGER);";
        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();

        IDbCommand cmd = dbcon.CreateCommand();
        cmd.CommandText = "INSERT INTO test_table (val) VALUES (5);";
        cmd.ExecuteNonQuery();
    }

    public IDataReader create(string sql)
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();

    }

    public IDataReader read(string sql)
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();
    }

    public void update(string sql)
    {
        IDbCommand cmd = this.dbcon.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
    public void delete(string sql) { }

    public void close()
    {
        this.dbcon.Close();
    }
    
}
