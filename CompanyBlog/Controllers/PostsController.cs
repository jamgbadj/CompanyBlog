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
using CompanyBlog.Models.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CompanyBlog.Controllers
{
    public class PostsController : Controller
    {
        private IRepository _repository;
        private IFileManager _fileManager;
        private UserManager<CompanyUser> _userManager;

        public PostsController(IRepository repository, IFileManager fileManager, UserManager<CompanyUser> userManager)
        {
            _repository = repository;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        // GET: Posts
        [AllowAnonymous]
        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        // GET: Posts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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
            post.ViewCount++;
            _repository.UpdatePost(post);
            await _repository.SaveChangesAsync();
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View(new PostViewModel());
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel viewModel)
        {
            string userName = _userManager.GetUserName(User);

            var post = new Post {
                PostID = viewModel.PostID,
                Title = viewModel.Title,
                Author = userName,
                Body = viewModel.Body,
                Image = await _fileManager.SaveImage(viewModel.Image)
            };

            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid)
            {
                _repository.CreatePost(post);
                if (await _repository.SaveChangesAsync())
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(viewModel);
            }
            return View(viewModel);
        }

        // GET: Posts/Edit/5
        [Authorize(Policy = "AdminOnly")]
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
                Body = post.Body,
                CurrentImage = post.Image,
                ViewCount = post.ViewCount
            });
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostViewModel viewModel)
        {
            var viewcount = viewModel.ViewCount;
            var post = new Post
            {
                PostID = viewModel.PostID,
                Title = viewModel.Title,
                Author = User.Identity.Name,
                Body = viewModel.Body,
                ViewCount = viewModel.ViewCount
            };

            if (viewModel.Image == null)
            {
                post.Image = viewModel.CurrentImage;
            }

            else
            {
                post.Image = await _fileManager.SaveImage(viewModel.Image);

            }

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
            return View(viewModel);
        }

        // GET: Posts/Delete/5
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
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

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = viewModel.PostId });
            }

            var post = _repository.GetPost(viewModel.PostId);

            if (viewModel.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = viewModel.Message,
                    PostedOn = DateTime.Now,
                });
                _repository.UpdatePost(post);
            }

            else
            {
                var comment = new SubComment
                {
                    MainCommentId = viewModel.MainCommentId,
                    Message = viewModel.Message,
                    PostedOn = DateTime.Now
                };
                _repository.AddSubComment(comment);
            }

            await _repository.SaveChangesAsync();

            return RedirectToAction("Details", new { id = viewModel.PostId });
        }
    }
}
