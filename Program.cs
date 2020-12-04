using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        ShowAllRooms(roomRepo);
                        break;
                    case ("Search for room"):
                        SearchRoom(roomRepo);
                        break;
                    case ("Add a room"):
                        AddRoom(roomRepo);
                        break;
                    case ("Show all chores"):
                        ShowAllChores(choreRepo);
                        break;
                    case ("Search for chore"):
                        SearchChore(choreRepo);
                        break;
                    case ("Add a chore"):
                        AddChore(choreRepo);
                        break;
                    case ("Search for roommate"):
                        SearchRoommate(roommateRepo);
                        break;
                    case ("Show unassigned chores"):
                        ShowUnassignedChores(choreRepo);
                        break;
                    case ("Assign roommate to chore"):
                        AssignRoommateChore(choreRepo, roommateRepo);
                        break;
                    case ("Update a room"):
                        UpdateRoom(roomRepo);
                        break;
                    case ("Delete a room"):
                        DeleteRoom(roomRepo);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Search for roommate",
            "Show unassigned chores",
            "Assign roommate to chore",
            "Update a room",
            "Delete a room",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }

        static void ShowAllRooms(RoomRepository roomRepo)
        {
            List<Room> rooms = roomRepo.GetAll();
            foreach (Room r in rooms)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }
            ContinueMenu();
        }

        static void SearchRoom(RoomRepository roomRepo)
        {
            while(true)
            {
                try
                {
                    Console.Write("Room Id: ");
                    int id = int.Parse(Console.ReadLine());

                    Room room = roomRepo.GetById(id);

                    if(room == null)
                    {
                        throw new Exception();
                    } 
                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        ContinueMenu();
                    break;

                }
                catch (Exception)
                {

                    continue;
                }
            }
        }

        static void AddRoom(RoomRepository roomRepo)
        {
            while (true)
            {
                try
                {
                    Console.Write("Room name: ");
                    string name = Console.ReadLine();

                    Console.Write("Max occupancy: ");
                    int max = int.Parse(Console.ReadLine());

                    Room roomToAdd = new Room()
                    {
                        Name = name,
                        MaxOccupancy = max
                    };

                    roomRepo.Insert(roomToAdd);

                    Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                    ContinueMenu();
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        static void ShowAllChores(ChoreRepository choreRepo)
        {
            List<Chore> chores = choreRepo.GetAll();
            foreach(Chore c in chores)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }
            ContinueMenu();
        }

        static void SearchChore(ChoreRepository choreRepo)
        {
            while (true)
            {
                try
                {
                    Console.Write("Chore Id: ");
                    int id = int.Parse(Console.ReadLine());
                    Chore chore = choreRepo.GetChoreById(id);

                    if (chore == null)
                    {
                        throw new Exception();
                    }

                    Console.WriteLine($"{chore.Id} - {chore.Name}");
                    ContinueMenu();
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        static void AddChore(ChoreRepository choreRepo)
        {
            Console.Write("Chore name: ");
            string name = Console.ReadLine();

            Chore choreToAdd = new Chore()
            {
                Name = name
            };

            choreRepo.Insert(choreToAdd);
            Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
            ContinueMenu();
        }

        static void SearchRoommate(RoommateRepository roommateRepo)
        {
            Console.Write("Roommate Id: ");
            int id = int.Parse(Console.ReadLine());
            Roommate roommate = roommateRepo.GetById(id);


            Console.WriteLine($"{roommate.Firstname}'s rent portion is ${roommate.RentPortion}, occupies {roommate.Room.Name}.");
            ContinueMenu();
        }

        static void ShowUnassignedChores(ChoreRepository choreRepo)
        {
            List<Chore> chores = choreRepo.GetUnassignedChores();
            foreach(Chore c in chores)
            {
                Console.WriteLine($"{c.Name}");
            }
            ContinueMenu();
        }

        static void AssignRoommateChore(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Chores");
            Console.WriteLine("----------------------");
            List<Chore> chores = choreRepo.GetAll();
            chores.ForEach(c => Console.WriteLine($"{c.Id} - {c.Name}"));
            Console.WriteLine("");
            Console.Write("Enter chore Id: ");
            int choreId = int.Parse(Console.ReadLine());

            List<Roommate> roommates = roommateRepo.GetAll();
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Roommates");
            Console.WriteLine("----------------------");
            roommates.ForEach(r => Console.WriteLine($"{r.Id} - {r.Firstname}"));
            Console.WriteLine("");
            Console.Write("Enter roommate Id: ");
            int roommateId = int.Parse(Console.ReadLine());

            choreRepo.AssignChore(roommateId, choreId);

            Chore selectedChore = chores.Find(c => c.Id == choreId);
            Roommate selectedRoommate = roommates.Find(r => r.Id == roommateId);
            Console.WriteLine($"{selectedRoommate.Firstname} is assign to {selectedChore.Name}");
            ContinueMenu();
        }

        static void UpdateRoom (RoomRepository roomRepo)
        {
            List<Room> roomOptions = roomRepo.GetAll();
            foreach (Room r in roomOptions)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }

            Console.Write("Which room would you like to update? ");
            int selectedRoomId = int.Parse(Console.ReadLine());
            Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

            Console.Write("New Name: ");
            selectedRoom.Name = Console.ReadLine();

            Console.Write("New Max Occupancy: ");
            selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

            roomRepo.Update(selectedRoom);

            Console.WriteLine($"Room has been successfully updated");
            ContinueMenu();
        }

        static void DeleteRoom (RoomRepository roomRepo)
        {
            List<Room> roomOptions = roomRepo.GetAll();
            roomOptions.ForEach(r => Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})"));
            Console.Write("Which room would you like to delete? ");
            int selectedRoomId = int.Parse(Console.ReadLine());
            roomRepo.Delete(selectedRoomId);
            ContinueMenu();
        }

        static void ContinueMenu()
        {
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
    }
}