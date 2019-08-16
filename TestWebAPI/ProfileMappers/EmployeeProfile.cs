namespace TestWebAPI.ProfileMappers
{
    using AutoMapper;

    /// <summary>
    /// The employee profile.
    /// </summary>
    public class EmployeeProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeProfile"/> class.
        /// </summary>
        public EmployeeProfile()
        {
            this.CreateMap<DTOs.Employee, Entities.Employee>();
            this.CreateMap<Entities.Employee, DTOs.Employee>();
        }
    }
}