using System;
using _65471.Data.Models;

namespace _65471.Data.Mappers
{
    public static class Mapper
    {
        public static DataDto MapDataToDto(Models.Data data)
        {
            var dataDto = new DataDto
            {
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                Time = data.Time,
                Line = data.Line,
                Brigade = data.Brigade
            };

            return dataDto;
        }
    }
}