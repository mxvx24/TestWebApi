namespace TestWebApi.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using TestWebApi.Data;
    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;

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
        /// Initializes a new instance of the <see cref="RepositoryTests"/> class.
        /// </summary>
        public RepositoryTests()
        {
            var databaseName = new Guid().ToString();
            var options = new DbContextOptionsBuilder<EmployeeDataContext>().UseInMemoryDatabase(databaseName).Options;
            this.context = new EmployeeDataContext(options);

            this.context.Employees.AddRangeAsync(DataGenerator.GetEmployee(500));
            this.context.SaveChanges();
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
            // var repo = new GenericRepository<Employee, EmployeeDataContext>(this.context, );
            // var activeEmployeeSpecification = new ActiveEmployeeSpecification();

            // ACT
            // var activeEmployees = repo.FindAsync()

            // ASSERT
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
