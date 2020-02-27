using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

/// <summary>
/// Class makes database interactions simpler.
/// </summary>
public class DatabaseController
{
    private string dbpath = "URI=file:" + Application.dataPath + "/Data/fearful_data.sqlite";
    private IDbConnection dbcon;
    
    /// <summary>
    /// Constructor for class. Opens a connection to our database.
    /// </summary>
    public DatabaseController()
    {
        // connection created in instance
        dbcon = new SqliteConnection(dbpath);
        dbcon.Open();
    }

    /// <summary>
    /// runs CREATE command. Used for "CREATE TABLE", etc.
    /// </summary>
    /// <param name="sql">Query to be executed.</param>
    /// <returns>IDataReader object with read() method to iterate through.</returns>
    public IDataReader Create(string sql)
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();

    }

    public IDataReader Read(string sql) //TODO: Delete?
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();
    }

    /// <summary>
    /// Runs an UPDATE query. Use for DELETE and UPDATE.
    /// </summary>
    /// <param name="sql">Query to be executed.</param>
    public void update(string sql)
    {
        IDbCommand cmd = this.dbcon.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Closes the database connection. 
    /// </summary>
    public void CloseDB()
    {
        this.dbcon.Close();
    }

    /// <summary>
    /// Reads a database file and returns the desired columns.
    /// </summary>
    /// <param name="columns">Columns to be retrieved.</param>
    /// <param name="table">Name of table(s).</param>
    /// <param name="additionalCmd">Optional. Allows for GROUP BY, WHERE clauses.</param>
    /// <returns>Dictionary object mapping stuff.</returns> 
    public Dictionary<string, int> ReadDB(string columns, string table, string additionalCmd = "")
    {
        Dictionary<string, int> items = new Dictionary<string, int>();
        string sql = $"SELECT {columns} FROM {table} {additionalCmd}";
        IDataReader reader = this.Read(sql);
        while (reader.Read())
        {
            // reader indices are based on query
            items.Add(reader.GetString(0), (int) (reader.GetFloat(1)) );
        }
        return items; //TODO: Generalize dict to return more objects as String,String dict
    }


}
