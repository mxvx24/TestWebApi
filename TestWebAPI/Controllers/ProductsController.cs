namespace TestWebAPI.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using TestWebApi.Data.Contexts;
    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;

    using TestWebAPI.Services;

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
        /// The product db context.
        /// </summary>
        private readonly ProductDbContext productDbContext;

        /// <summary>
        /// The product update hosted service.
        /// </summary>
        private readonly ProductUpdateHostedService productUpdateHostedService;

        private readonly IServiceProvider services;

        /// <summary>
        /// The task queue.
        /// </summary>
        private readonly ITaskQueue taskQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="hostedService">
        /// The hosted Service.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="queue">
        /// The queue.
        /// </param>
        /// <param name="services"></param>
        public ProductsController(
                ILogger<ProductsController> logger,
                IHostedService hostedService,
                IRepository<Product> repository,
                ProductDbContext context,
                ITaskQueue queue,
                IServiceProvider services)
        {
            this.productRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.productDbContext = context ?? throw new ArgumentNullException(nameof(context));
            this.taskQueue = queue ?? throw new ArgumentNullException(nameof(queue));
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.productUpdateHostedService = hostedService as ProductUpdateHostedService;

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
            Product product = await this.productRepository.GetAsync(id).ConfigureAwait(false);

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
            List<Product> products = string.IsNullOrWhiteSpace(nameLike)
                               ? await this.productRepository.GetAllAsync().ConfigureAwait(false)
                               : await this.productRepository.FindAsync(e => e.Name.Contains(nameLike)).ConfigureAwait(false);

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

            await this.productRepository.Add(product).ConfigureAwait(false);
            await this.productRepository.SaveChangesAsync().ConfigureAwait(false);

            return this.CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// The update all products.
        /// </summary>
        /// <param name="updatedBy">
        /// The updated by.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPut("bulk")]
        public IActionResult UpdateAllProducts(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
            {
                updatedBy = "TestWebApi";
            }

            // this.productUpdateHostedService.StartAsync(new System.Threading.CancellationToken());
            this.taskQueue.AddWorkItem(async (token, context) =>
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Prevent throwing if the Delay is cancelled
                    }

                    this.logger.LogInformation("About to execute the stored proc...");
                    
                    _ = (context as ProductDbContext)?.Products.FromSql("SpUpdateAllProducts @p0, @p1", new object[] { "1,2,3", $"{updatedBy}" }).ToList();
                });

            return this.Accepted();
        }
    }
}