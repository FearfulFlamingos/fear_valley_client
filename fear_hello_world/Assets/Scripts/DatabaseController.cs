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
        dbcon = new SqliteConnection(dbpath);
        dbcon.Open();
    }

    public IDataReader Create(string sql)
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();

    }

    public IDataReader Read(string sql)
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

    public void CloseDB()
    {
        this.dbcon.Close();
    }

    public Dictionary<string, int> ReadDB(string columns, string table)
    {
        Dictionary<string, int> items = new Dictionary<string, int>();
        string sql = $"SELECT {columns} FROM {table}";
        IDataReader reader = this.Read(sql);
        while (reader.Read())
        {
            // reader indices are based on query
            items.Add(reader.GetString(0), (int) (reader.GetFloat(1)) );
        }
        return items;
    }


}
