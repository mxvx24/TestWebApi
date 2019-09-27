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
            this.CreateMap<DTOs.Employee, TestWebApi.Domain.Entities.Employee>();
            this.CreateMap<TestWebApi.Domain.Entities.Employee, DTOs.Employee>();
        }
    }
}