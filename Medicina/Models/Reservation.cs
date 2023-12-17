﻿namespace Medicina.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EquipmentCount { get; set; }
        public bool IsDone { get; set; }
        public Reservation() { }
        public Reservation(int userId, int count, bool isDone)
        {
            UserId = userId;
            EquipmentCount = count;
            IsDone = isDone;
        }

    }
}