using Backend.Data;
using Backend.DTO;
using Backend.DTO.CategoryDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using System;

namespace Backend.Services
{
    public class CategoryServicecs : ICategoryService
    {
        public  readonly BlogDbContext context;

        public CategoryServicecs(BlogDbContext context)
        {
            this.context = context;
        }

        public CategoryResponse createCategory(CategoryRequest categoryRequest)
        {
            Category category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description,

            };
            context.Categories.Add(category);
            context.SaveChanges();
            return mapToResponse(category);
        }

        public void Delete(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id && c.IsDeleted==false);
            if (category == null)
            {
                throw new Exception("the loai khong ton tai voi id:" + id);
            }
            category.IsDeleted = true;
            category.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public List<CategoryResponse> GetAll()
        {
            return context.Categories
                .Where(c => c.IsDeleted==false)
                .Select(c => new CategoryResponse {
                    Id=c.Id,
                    Name=c.Name,
                    Description=c.Description,
                    UpdatedAt=c.UpdatedAt,
                    CreatedAt=c.CreatedAt,
                    IsDeleted=c.IsDeleted,
                }).ToList();
        }

        public CategoryResponse GetCategory(int id)
        {
            var category= context.Categories.FirstOrDefault(c => c.Id == id && c.IsDeleted==false);
            if (category == null) {
                throw new Exception("the loai khong ton tai voi id:"+id);
            }
            return mapToResponse(category);
        }

        public PageResult<CategoryResponse> GetPage(int page, int pageSize)
        {
            var item=context.Categories
             .Where(c=>c.IsDeleted==false)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            var result=item.Select(c=>mapToResponse(c)).ToList();
            return new PageResult<CategoryResponse>
            {
                Items = result,
                Total=item.Count,
            };
        }

        public CategoryResponse Update(int id, CategoryRequest request)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (category == null)
            {
                throw new Exception("the loai khong ton tai voi id:" + id);
            }
            category.UpdatedAt = DateTime.Now;
            category.Description = request.Description;
            category.Name = request.Name;
            context.SaveChanges();
            return mapToResponse(category);
        }
        public  CategoryResponse mapToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                IsDeleted = category.IsDeleted,
            };
        }
    }
}
