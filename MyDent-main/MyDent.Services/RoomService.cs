using Microsoft.EntityFrameworkCore;
using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services
{
    public class RoomService : IRoomService
    {
        private readonly MyDentDbContext _dbContext;

        public RoomService(MyDentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList();
        }

        public List<Room> GetRoomsByClinic(int clinicId)
        {
            return _dbContext.Rooms.Where(r => r.ClinicId == clinicId).ToList();
        }

        public Room GetRoomById(int roomId)
        {
            return _dbContext.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
        }

        public Room AddRoom(Room roomToAdd)
        {

            _dbContext.Rooms.Add(roomToAdd);

            _dbContext.SaveChanges();

            return roomToAdd;
        }

        public Room DeleteRoomById(int id)
        {
            var room = _dbContext.Rooms.FirstOrDefault(a => a.Id == id);

            if (room == null)
            {
                throw new NullReferenceException("Room with the given id was not found");
            }

            _dbContext.Rooms.Remove(room);
            _dbContext.SaveChanges();

            return room;
        }

        public Room UpdateRoom(Room room)
        {
            var roomToUpdate = _dbContext.Rooms.Include(r => r.Appointments).FirstOrDefault(r => r.Id == room.Id);
            roomToUpdate.Name = room.Name;
            roomToUpdate.MedicalEquipment = room.MedicalEquipment;

            _dbContext.SaveChanges();

            return _dbContext.Rooms.FirstOrDefault(r => r.Id == room.Id);
        }
    }
}
