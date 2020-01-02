namespace TestWebApi.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using TestWebApi.Data;
    using TestWebApi.Data.Contexts;
    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;
    using TestWebApi.Domain.Specifications;

    using Xunit;

    /// <summary>
    /// The repository tests.
    /// </summary>
    public class RepositoryTests : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The context.
        /// </summary>
        private readonly EmployeeDataContext context;

        /// <summary>
        /// The provider.
        /// </summary>
        private readonly ServiceProvider provider;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryTests"/> class.
        /// </summary>
        public RepositoryTests()
        {
            var databaseName = new Guid().ToString();
            DbContextOptions<EmployeeDataContext> options = new DbContextOptionsBuilder<EmployeeDataContext>()
                .UseInMemoryDatabase(databaseName).Options;
            
            this.context = new EmployeeDataContext(options);
            
            this.context.Employees.AddRangeAsync(DataGenerator.GetEmployee(500));

            this.context.SaveChanges();
            
            var mapperConfig = new MapperConfiguration(
                c =>
                    {
                    });

            this.mapper = mapperConfig.CreateMapper();
        }

        /// <summary>
        /// The test active employee specification.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task TestActiveEmployeeSpecification()
        {
            // ARRANGE
            var repo = new GenericRepository<Employee, EmployeeDataContext>(this.context, this.mapper);
            var activeEmployeeSpecification = new ActiveEmployeeSpecification();
            List<Employee> activeEmployeesExpected = await this.context.Employees.Where(e => e.IsActive()).ToListAsync();

            // ACT
            List<Employee> activeEmployeesActual = await repo.FindAsync(activeEmployeeSpecification);

            // ASSERT
            Assert.NotNull(activeEmployeesActual);
            Assert.Equal(activeEmployeesExpected.Count, activeEmployeesActual.Count);
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
            }

            this.disposed = true;
        }
    }
}
