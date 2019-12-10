using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

/// <summary>
/// Class makes database interactions simpler.
/// </summary>
public class DatabaseController
{
    //public static DatabaseController DBInstance { private set; get; }

    private readonly string dbpath = "URI=file:" + Application.dataPath + "/Data/fearful_data.sqlite";
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

    public void OpenDB()
    {
        this.dbcon.Open();
    }
    #region Basic Commands
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
    /// <summary>
    /// Internal function to build more complicated, repeated queries.
    /// </summary>
    /// <param name="sql">Query to be executed.</param>
    /// <returns>IDataReader object with read() method.</returns>
    private IDataReader Read(string sql)
    {
        IDbCommand dbcmd = this.dbcon.CreateCommand();
        dbcmd.CommandText = sql;
        return dbcmd.ExecuteReader();
    }

    /// <summary>
    /// Runs an UPDATE query. Use for DELETE and UPDATE.
    /// </summary>
    /// <param name="sql">Query to be executed.</param>
    public void Update(string sql)
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
    #endregion

    #region Complex Queries
    /// <summary>
    /// Adds a troop to the database. Assumes the client has enough money.
    /// </summary>
    /// <param name="teamNum">Player connection id</param>
    /// <param name="troop">Class.</param>
    /// <param name="weapon">Name of weapon.</param>
    /// <param name="armor">Name of armor.</param>
    /// <param name="posX">Relative X position on board.</param>
    /// <param name="posZ">Relative Z position on board.</param>
    /// <param name="isLeader">Leader (default false).</param>
    public void AddTroopToDB(int teamNum, string troop, string weapon, string armor, int posX, int posZ, bool isLeader = false)
    {
        Update("INSERT INTO Army " +
            "(teamNumber,class,armor,weapon,isLeader,pos_x,pox_z) " +
            "VALUES (" +
            $"{teamNum}," +
            $"'{troop}'," +
            $"'{armor}'," +
            $"'{weapon}'," +
            $"'{isLeader}'," +
            $"{posX}," +
            $"{posZ});");
    }

    public List<Troop> GetAllTroops()
    {
        List<Troop> allTroops = new List<Troop>();
        IDataReader troops = Read("SELECT " +
            "Ay.teamNumber, Ay.class, Ay.weapon, Ay.isLeader, Ay.pos_x, Ay.pox_z, " +//5 
            "T.health, T.attack, T.damage, T.movement, T.perception, T.magicattack, " +//11 
            "W.damage, W.range, W.AOE, " +//14
            "Ar.bonus, Ar.stealth " +//16
            "" +
            "FROM Army Ay " +
            "INNER JOIN Troop T ON Ay.class = T.class " +
            "INNER JOIN Weapon W ON Ay.weapon = W.name " +
            "INNER JOIN Armor Ar ON Ay.armor = Ar.armor;");
        while (troops.Read())
        {
            Troop t = new Troop(troops.GetInt32(0), //teamNum
                troops.GetString(1), //TroopType
                (int) troops.GetDouble(15)+10, //Armor
                (int) troops.GetDouble(7), //WeapMod
                (int) troops.GetDouble(12), //WeapDmg
                (int) troops.GetDouble(6), //Health
                troops.GetBoolean(3),//leader
                troops.GetDouble(4),//xpos
                troops.GetDouble(5));//zPos
            allTroops.Add(t);
        }

        return allTroops;
    }
    #endregion
}
