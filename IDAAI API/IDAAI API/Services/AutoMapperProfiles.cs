using AutoMapper;
using IDAAI_API.DTOs;
using IDAAI_API.Entidades.Models;

namespace IDAAI_API.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Estudiante, EstudianteDTO>();
            CreateMap<Asistencia, AsistenciaDTO>();
        }
    }
}
