using Backend.Controllers;
using Backend.Data;
using Backend.DTO;
using Backend.DTO.PostDTO;
using Backend.DTO.UserDTO;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Backend.Services
{
    public class TestPostService
    {
        public readonly RedisService redisService;
        public readonly BlogDbContext context;

        private readonly ILogger<TestPostService> _logger;

        public TestPostService(RedisService redisService, BlogDbContext context, ILogger<TestPostService> logger)
        {
            this.redisService = redisService;
            this.context = context;
            _logger = logger;
        }

        public List<PostResponse> GetPostsFromFollowing(int userId)
        {
            _logger.LogInformation("Getting posts from following for user {UserId}", userId);

            var posts = context.Posts
                .FromSqlRaw(@"
            SELECT p.*
            FROM Follows f
            JOIN Posts p ON p.UserId = f.FollowingId
            WHERE f.FollowerId = @userId
            ORDER BY p.CreatedAt DESC
        ", new MySqlParameter("@userId", userId))
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();
            var result = posts.Select(p => mapToResponse(p)).ToList();
            return result;
        }
        public PageResult<PostResponse> searchRedis(PostSearch postSearch)
        {
            // 1. Normalize input
            var title = postSearch.tittle.ToLower();
            var category = postSearch.categoryName.ToLower();

            string cacheKey = $"posts:search:{title}:{category}:page:{postSearch.page}:size:{postSearch.pageSize}";

            // 2. Lấy từ cache
            var cached = redisService.GetData<PageResult<PostResponse>>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            // 3. Query DB
            var query = context.Posts
                .Where(p => !p.IsDeleted)
                .Where(p => p.Title.ToLower().Contains(title))
                .Where(p => p.Category.Name.ToLower().Contains(category));

            var total = query.Count();

            var item = query
                .Skip((postSearch.page - 1) * postSearch.pageSize)
                .Take(postSearch.pageSize)
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();

            // 4. Map DTO
            var result = item.Select(p => mapToResponse(p)).ToList();

            var response = new PageResult<PostResponse>
            {
                Items = result,
                Total = total
            };

            // 5. Cache lại
            redisService.SetData(cacheKey, response, 5);

            return response;
        }
        public List<PostResponse> GetAllWithRedis()
        {
            string cacheKey = "posts";

            // 1. Lấy từ cache (DTO luôn)
            var cachedPosts = redisService.GetData<List<PostResponse>>(cacheKey);
            if (cachedPosts != null)
            {
                return cachedPosts;
            }

            // 2. Lấy từ DB
            var item = context.Posts
                .Where(p => p.IsDeleted == false)
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();

            // 3. Map sang DTO
            var result = item.Select(p => mapToResponse(p)).ToList();

            // 4. Cache DTO 
            redisService.SetData(cacheKey, result, 5);

            return result;
        }
        public PostResponse mapToResponse(Post post)
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
                AuthorName = post.User?.FullName,
                ImageUrl = post.ImageUrl,
            };
        }
       

        // Lấy danh sách bài viết của những người mà user đang follow
        public List<PostResponse> GetPostsFromFollowing1(int userId)
        {
            // Lấy danh sách ID của những user mà current user đang follow
            var followingIds = context.Follows
                .Where(f => f.FollowerId == userId) 
                .Select(f => f.FollowingId);        

            // Lấy tất cả bài viết của những user trong danh sách trên
            var posts = context.Posts
                .Where(p => followingIds.Contains(p.UserId))
                .ToList();

            
            return posts.Select(p => mapToResponse(p)).ToList();
        }


        // Lấy bài viết thuộc category "Tech" từ những người đang follow
        public List<PostResponse> GetTechPostsFromFollowing(int userId)
        {
            // Lấy danh sách ID người đang follow
            var followingIds = context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId);

            // Lọc bài viết:
            // - thuộc người mình follow
            // - category là Tech
            var posts = context.Posts
                .Where(p => followingIds.Contains(p.UserId)
                         && p.Category.Name == "Tech")
                .ToList();

            return posts.Select(p => mapToResponse(p)).ToList();
        }


        // Lấy bài viết có hơn 5 reaction
        public List<Post> GetPostsWithMoreThan5Reactions()
        {
            return context.Posts
                .Where(p =>
                    // Đếm số lượng reaction > 5
                    p.Reactions.Count() > 5
                )
                .ToList();
        }


        // Lấy bài viết có cả comment và reaction
        public List<Post> GetActivePosts()
        {
            return context.Posts
                .Where(p =>
                    // Có ít nhất 1 comment
                    p.Comments.Any()
                    // Và có ít nhất 1 reaction
                    && p.Reactions.Any()
                )
                .ToList();
        }



        // Lấy user có ít nhất 1 bài post có reaction
        public List<User> GetUsersWithPostsHavingReactions()
        {
            return context.Users
                .Where(u =>
                    // Kiểm tra user có post nào không
                    // và trong post đó có reaction
                    u.Posts.Any(p => p.Reactions.Any())
                )
                .ToList();
        }


        // Lấy user đã comment vào bài viết của mình
        public List<User> GetUsersCommentedOnMyPosts(int userId)
        {
            // Lấy danh sách ID bài viết của mình
            var myPostIds = context.Posts
                .Where(p => p.UserId == userId)
                .Select(p => p.Id);

            // Lấy user có comment vào các bài viết đó
            return context.Users
                .Where(u =>
                    u.Comments.Any(c => myPostIds.Contains(c.PostId))
                )
                .ToList();
        }


        // Lấy user đã report bài viết thuộc category "Tech"
        public List<User> GetUsersReportedTechPosts()
        {
            return context.Users
                .Where(u =>
                    // Kiểm tra user có report nào không
                    // và post của report thuộc category Tech
                    u.Reports.Any(r => r.Post.Category.Name == "Tech")
                )
                .ToList();
        }


        // Lấy user đã react vào bài viết của mình
        public List<User> GetUsersReactedToMyPosts(int userId)
        {
            // Lấy ID bài viết của mình
            var myPostIds = context.Posts
                .Where(p => p.UserId == userId)
                .Select(p => p.Id);

            // Lấy user có reaction vào các bài viết đó
            return context.Users
                .Where(u =>
                    u.Reactions.Any(r => myPostIds.Contains(r.PostId))
                )
                .ToList();
        }
        public Post getPostTopReact()
        {
            var post = context.Posts
                .OrderByDescending(p => p.Reactions.Count())
                .Take(1)
                .FirstOrDefault();
            if (post != null)
            {
                return post;
            }
            return new Post();
        }
        public List<Post> getPostLeast10Like(int postId)
        {
            return context.Posts.Where(p => p.Reactions
            .Count(r => r.ReactType == Reaction.ReactionType.Like) > 10)
            .ToList();
        }
        public List<Post> getPostHasCommentA(int postId)
        {
            return context.Posts
                .Where(p => p.Comments.Any(c => c.Content.Contains("A")))
                .ToList();
        }
        public List<User> getUserCommnetMyPost(int userId)
        {
            var posts = context.Posts
                .Where(p => p.UserId == userId)
                .Select(p => p.Id);
            return context.Users
                .Where(u => u.Comments.Any(c => posts.Contains(c.Post.Id)))
                .ToList();
        }
        public PageResult<PostResponse> searchPostAdvance(PostSearch postSearch)
        {
            _logger.LogInformation("Getting parma pagesize: "+postSearch.pageSize);
            var cachedkey = $"posttile:{postSearch.tittle},category:{postSearch.categoryName}" +
                $",page:{postSearch.page},size:{postSearch.pageSize}";
            var post = redisService.GetData<PageResult<PostResponse>>(cachedkey);
            if (post != null)
            {
                return post;

            }
            var query = context.Posts
                .Where(p => p.IsDeleted == false)
                .Where(p => p.Title.Contains(postSearch.tittle))
                .Where(p => p.Category.Name.Contains(postSearch.categoryName));
            var total = query.Count();
            var item = query.Skip((postSearch.page - 1) * postSearch.pageSize)
                .Take(postSearch.pageSize)
                .ToList();
            var reult=item.Select(i=>mapToResponse(i)).ToList();
            var pageResult = new PageResult<PostResponse>
            {
                Total = total,
                Items = reult
            };
            redisService.SetData(cachedkey, pageResult, 10);
            return pageResult;
        }
        public PageResult<UserResponse> searchUserAdvanceWithRedis(string fullName, DateTime date1, DateTime date2, int page, int pageSize)
        {
            var cacheKey = $"fullname:{fullName},Date1:{date1},Date2:{date2},page:{page},pagesize:{pageSize}";
            var result = redisService.GetData<PageResult<UserResponse>>(cacheKey);
            if (result != null)
            {
                return result;
            }
            var user = context.Users
                      .Where(u => u.FullName.Contains(fullName))
                      .Where(u => u.CreatedAt >= date1 && u.CreatedAt <= date2)
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToList();
            var data = user.Select(u => MapToResponse(u)).ToList();
            var item = new PageResult<UserResponse>
            {
                Total = data.Count,
                Items = data
            };
            redisService.SetData(cacheKey, item);
            return item;
        }
        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = user.Role.ToString()
            };
        }
    }
}
