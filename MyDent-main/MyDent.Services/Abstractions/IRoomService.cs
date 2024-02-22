using MyDent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();

        List<Room> GetRoomsByClinic(int clinicId);

        Room AddRoom(Room roomToAdd);

        Room DeleteRoomById(int id);

        Room UpdateRoom(Room room);

        Room GetRoomById(int roomId);
    }
}
