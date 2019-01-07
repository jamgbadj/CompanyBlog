using CompanyBlog.Models;
using CompanyBlog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyBlog.Data.Repository
{
    public interface IRepository    
    {
        Post GetPost(int id);

        List<Post> GetAllPosts();

        void CreatePost(Post post);

        void UpdatePost(Post post);

        void DeletePost(int id);

        bool PostExists(int id);

        Task<bool> SaveChangesAsync();

        void AddSubComment(SubComment subComment);
    }
}
