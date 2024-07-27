using AutoMapper;
using Moq;
using UnitOfWorkExample.Core.DTOs;
using UnitOfWorkExample.Core;
using UnitOfWorkExample.Core.Interfaces;
using UnitOfWorkExample.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using MyProject.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using UnitOfWorkExample.core.Entities;

namespace MyProject.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ProductsController(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1" },
                new ProductDto { Id = 2, Name = "Product 2" }
            };

            _mockUnitOfWork.Setup(u => u.Products.GetAllAsync()).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithProductDto()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };

            _mockUnitOfWork.Setup(u => u.Products.GetByIdAsync(1)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithProductDto()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockMapper.Setup(m => m.Map<Product>(productDto)).Returns(product);
            _mockUnitOfWork.Setup(u => u.Products.AddAsync(product)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(productDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
            Assert.Equal(productDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };

            // Act
            var result = await _controller.Update(2, productDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockMapper.Setup(m => m.Map<Product>(productDto)).Returns(product);
            _mockUnitOfWork.Setup(u => u.Products.Update(product)).Verifiable();
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Update(1, productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockUnitOfWork.Setup(u => u.Products.GetByIdAsync(1)).ReturnsAsync(product);
            _mockUnitOfWork.Setup(u => u.Products.Delete(product)).Verifiable();
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetProductsByCategory_ReturnsOkResult_WithListOfProductDtos()
        {
            // Arrange
            var categoryId = 1;
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", CategoryId = categoryId },
                new Product { Id = 2, Name = "Product 2", CategoryId = categoryId }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1" },
                new ProductDto { Id = 2, Name = "Product 2" }
            };

            _mockUnitOfWork.Setup(u => u.Products.GetProductsByCategoryAsync(categoryId)).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _controller.GetProductsByCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }
    }
}

