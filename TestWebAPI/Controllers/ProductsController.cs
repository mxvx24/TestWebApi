namespace TestWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.Logging;

    using TestWebApi.Data;
    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;

    using TestWebAPI.Library;

    /// <summary>
    /// The Products controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;
        
        /// <summary>
        /// The Product repository.
        /// </summary>
        private readonly IRepository<Product> productRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public ProductsController(ILogger<ProductsController> logger, IRepository<Product> repository)
        {
            this.productRepository = repository;
            this.logger = logger;

            this.logger.LogTrace($"{nameof(ProductsController)} class has been instantiated.");
        }

        /// <summary>
        /// The get Product.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var product = await this.productRepository.GetAsync(id);

            if (product == null)
            {
                return this.NotFound($"Invalid Product id: {id}");
            }

            return this.Ok(product);
        }

        /// <summary>
        /// The get Products.
        /// </summary>
        /// <param name="nameLike">
        /// The name Like.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string nameLike = default)
        {
            var products = string.IsNullOrWhiteSpace(nameLike)
                               ? await this.productRepository.GetAllAsync()
                               : await this.productRepository.FindAsync(e => e.Name.Contains(nameLike));

            if (!products.Any())
            {
                return this.NotFound(products);
            }

            return this.Ok(products);
        }

        /// <summary>
        /// The post Product.
        /// </summary>
        /// <param name="product">
        /// The Product.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.productRepository.Add(product);
            await this.productRepository.SaveChangesAsync();

            return this.CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
    }
}