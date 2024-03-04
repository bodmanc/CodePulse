using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // Post
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            // Map Dto to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.urlHandle
            };

            await _categoryRepository.CreateAsync(category);

            // Domain model to Dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                urlHandle = category.UrlHandle
            };
          
            return Ok(response);

        }

        // GET https://localhost:7061/api/categories
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            // Map Domain model to Dto
            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto 
                { 
                    Id = category.Id, 
                    Name = category.Name,
                    urlHandle = category.UrlHandle,
                });
            }

            return Ok(response);
        }

        // Get https://localhost:7061/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
           var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                urlHandle = existingCategory.UrlHandle
            };
            
            return Ok(response);
        }


        //Put https://localhost:7061/api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            // Convert Dto to Domain Model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.urlHandle
            };

            category = await _categoryRepository.UpdateAsync(category);

            if (category is null)
            {
                return NotFound();
            }
            // Convert domain model to dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                urlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        // Delete  https://localhost:7061/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id) 
        {
          var category =  await _categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            // convert domain to dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                urlHandle = category.UrlHandle
            };

            return Ok(response);

        }
    }
}
