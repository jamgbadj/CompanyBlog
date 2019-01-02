using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyBlog.Data;
using CompanyBlog.Models;
using CompanyBlog.Data.Repository;
using CompanyBlog.Data.FileManager;

namespace CompanyBlog.Controllers
{
    public class PostsController : Controller
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public PostsController(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        // GET: Posts
        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        // GET: Posts/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _repository.GetPost((int)id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View(new PostViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel viewModel)
        {
            var post = new Post {
                PostID = viewModel.PostID,
                Title = viewModel.Title,
                Author = viewModel.Author,
                Body = viewModel.Body,
                Image = await _fileManager.SaveImage(viewModel.Image)
            };
            if (ModelState.IsValid)
            {
                _repository.CreatePost(post);
                if (await _repository.SaveChangesAsync())
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(post);
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _repository.GetPost((int)id);
            if (post == null)
            {
                return NotFound();
            }
            return View(new PostViewModel {
                PostID = post.PostID,
                Title = post.Title,
                Author = post.Author,
                Body = post.Body
            });
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostViewModel viewModel)
        {
            var post = new Post
            {
                PostID = viewModel.PostID,
                Title = viewModel.Title,
                Author = viewModel.Author,
                Body = viewModel.Body,
                Image = await _fileManager.SaveImage(viewModel.Image)
            };

            if (id != post.PostID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdatePost(post);
                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _repository.GetPost((int)id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _repository.DeletePost(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
           return _repository.PostExists(id);
        }

        //Stream Image through path specified
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mimeType = image.Substring(image.LastIndexOf('.') +1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mimeType}");
        }
    }
}
