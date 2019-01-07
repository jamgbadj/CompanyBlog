using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyBlog.Models;
using CompanyBlog.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace CompanyBlog.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            _context.Add(post);
        }

        public void DeletePost(int id)
        {
            _context.Post.Remove(GetPost(id));
        }

        public void UpdatePost(Post post)
        {
            _context.Post.Update(post);
        }

        public List<Post> GetAllPosts()
        {
            return _context.Post.ToList();
        }

        public Post GetPost(int id)
        {
            return _context.Post
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.PostID == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostID == id);
        }

        public void AddSubComment(SubComment subComment)
        {
            _context.SubComments.Add(subComment);
        }
    }
}
