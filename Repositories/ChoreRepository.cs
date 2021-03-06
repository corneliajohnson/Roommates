﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;


namespace Roommates.Repositories
{
    class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) {}

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();

                    while(reader.Read())
                    {
                        int idColumnPostion = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPostion);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                        };

                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;
                }
            }
        }

        public Chore GetChoreById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;
                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();
                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();
                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Chore.Id as Id, Chore.Name as Name
                                        FROM Chore
                                        Full Outer Join RoommateChore On RoommateChore.ChoreId = Chore.Id
                                        WHERE Chore.Id IS NULL OR ChoreId IS NULL";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();
                    
                    while(reader.Read())
                    {
                        int idColumnPostion = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPostion);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };
                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    RoommateChore roommateChore = new RoommateChore()
                    {
                        RoommateId = roommateId,
                        ChoreId = choreId
                    };
                    cmd.CommandText = @" INSERT INTO RoommateChore(ChoreId, RoommateId)
                                         OUTPUT INSERTED.Id 
                                         VALUES (@choreId, @roommateId)";
                    cmd.Parameters.AddWithValue("@choreId",roommateChore.RoommateId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateChore.ChoreId);
                    int id = (int)cmd.ExecuteScalar();
                    roommateChore.Id = id;
                }
            }
        }

        public void Update(Chore chore)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                        SET Name = @name
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM RoommateChore WHERE choreId = @id
                                        DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<RoommateChoreCount> ChoreCount()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, COUNT(ChoreId) as NumOfChores
                                        FROM RoommateChore
                                        JOIN Roommate ON Roommate.Id = RoommateId
                                        GROUP BY(FirstName)";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<RoommateChoreCount> roommateChoreCounts = new List<RoommateChoreCount>();

                    while(reader.Read())
                    {
                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int countColumnPostion = reader.GetOrdinal("NumOfChores");
                        int countValue = reader.GetInt32(countColumnPostion);

                        RoommateChoreCount roommateChoreCount = new RoommateChoreCount
                        {
                            FirstName = firstNameValue,
                            NumOfChores = countValue
                        };
                        roommateChoreCounts.Add(roommateChoreCount);
                    }
                    reader.Close();
                    return roommateChoreCounts;
                }
            }
        }
    }
}
