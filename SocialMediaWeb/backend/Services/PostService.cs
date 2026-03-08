using Backend.Data;
using Backend.DTO;
using Backend.DTO.CategoryDTO;
using Backend.DTO.PostDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class PostService : IPostService
    {
        public readonly BlogDbContext context;

        public PostService(BlogDbContext context)
        {
            this.context = context;
        }

        public PostResponse createPost(PostRequest postRequest)
        {
            var UserId = 1;
            var User=context.Users.FirstOrDefault(u => u.Id == UserId);
            var category=context.Categories.FirstOrDefault(c=>c.Id==postRequest.CategoryId);
            if (category == null)
            {
                throw new Exception("không tìm thể loại");
            }
            Post post = new Post
            {
                Title = postRequest.Title,
                Content = postRequest.Content,
                CreatedAt = DateTime.Now,
                ImageUrl = postRequest.ImageUrl,
                UserId = UserId,
                CategoryId = category.Id,
            };
            context.Posts.Add(post);
            context.SaveChanges();
            return mapToResponse(post);
        }

        public void Delete(int id)
        {
            var post=context.Posts.FirstOrDefault(p => p.Id == id && p.IsDeleted ==false);
            if (post == null)
            {
                throw new Exception("không tìm thấy bài viết");
            }
            post.IsDeleted = true;
            post.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public List<PostResponse> GetAll()
        {
            return context.Posts
                .Where(p => p.IsDeleted == false)
                .Include(p=>p.User)
                .Include(p=>p.Category)
                .Select(p=>mapToResponse(p))
                .ToList();
        }

        public PageResult<PostResponse> GetPage(int page, int pageSize)
        {
            var total = context.Posts
                .Where(p=>p.IsDeleted == false)
                .Count();
            var item=context.Posts
                 .Where(p => p.IsDeleted == false)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();
            var result=item.Select(p=>mapToResponse(p)).ToList();
            return new PageResult<PostResponse>
            {
                Items = result,
                Total = total
            };
        }

        public PostResponse GetPost(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if (post == null)
            {
                throw new Exception("không tìm thấy bài viết");
            }
            return mapToResponse(post);
        }

        public PostResponse Update(int id, PostRequest request)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if (post == null)
            {
                throw new Exception("không tìm thấy bài viết");
            }
            var category = context.Categories.FirstOrDefault(c => c.Id == request.CategoryId);
            if (category == null)
            {
                throw new Exception("không tìm thể loại");
            }
            post.CategoryId = category.Id;
            post.Title=request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.Now;
            post.ImageUrl=request.ImageUrl;
            context.SaveChanges();
            return mapToResponse(post);
        }
        public  PostResponse mapToResponse(Post post)
        {
            return new PostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                IsDeleted = post.IsDeleted,
                UserId = post.UserId,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                CategoryDescription = post.Category?.Description,
                AuthorName = post.User?.FullName
            };
        }

        public PageResult<PostResponse> search(PostSearch postSearch)
        {
            var total = context.Posts
                .Where(p => p.IsDeleted == false)
                .Where(p => p.Title.Contains(postSearch.tittle))
                .Where(p=>p.Category.Name.Contains(postSearch.categoryName))
                .Count();
            var item = context.Posts
                 .Where(p => p.IsDeleted == false)
                  .Where(p => p.Title.Contains(postSearch.tittle))
                .Where(p => p.Category.Name.Contains(postSearch.categoryName))
                .Skip((postSearch.page - 1) * postSearch.pageSize)
                .Take(postSearch.pageSize)
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();
            var result = item.Select(p => mapToResponse(p)).ToList();
            return new PageResult<PostResponse>
            {
                Items = result,
                Total = total
            };
        }
    }
}
