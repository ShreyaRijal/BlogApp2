/*
 * This controller class has implemented the basic functionality of CRUD which 
 * involves the creation, reading, updating and, deletion of a blog object.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using BlogApp2.Data;
using BlogApp2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp2.Controllers
{

    //As this class is a controller, it inherits from the controller class.
    public class BlogController : Controller
    {
        //An instance of the database.
        private ApplicationDbContext db;
        public string username;
        private readonly UserManager<IdentityUser> _userManager;

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //Creating a constructor of this controller to have a database.
        public BlogController(ApplicationDbContext _db, UserManager<IdentityUser> userManager)
        {
            db = _db;

            this._userManager = userManager;
        }


        public async Task GetCurrentUserName()
        {
            var user = await GetCurrentUserAsync();

            username = user?.UserName;

        }

        //Get request for the CreateBlog view.
        [HttpGet]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> MyBlogs()
        {
            await GetCurrentUserName();
            List<BlogModel> blogs = new List<BlogModel>();
            blogs = await db.Blogs.Where (x => x.UserName == username).ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        [Authorize(Policy = "Blogger")]
        public IActionResult Create()
        {
            return View("CreateBlog");
        }

        //Post request from the view CreateBlog.
        [HttpPost]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Create(string title, string entry)
        {

            //Creating an instance of a blog object.
            BlogModel blog = new BlogModel();

            await GetCurrentUserName();

            blog.UserName = username;
            blog.BlogTitle = title;
            blog.BlogEntry = entry;
            if (ModelState.IsValid)
            {
                //Updating the database by adding the blog object.
                db.Blogs.Add(blog);

                //Saving the changes made to the database.
                db.SaveChanges();
                return RedirectToAction(nameof(MyBlogs));
            }

            //Returning the appropriate view.
            return View(blog);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Read(string CommentText, string BlogID)
        {
            await GetCurrentUserName();

            CommentModel comment = new CommentModel();

            comment.CommentText = CommentText;
            comment.BlogCommentedOnID = BlogID;
            comment.UserName = username;

            BlogModel blog = db.Blogs.Find(BlogID);

            List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == BlogID).ToListAsync();

            BlogWithComments b = new BlogWithComments(blog, comments);
            b.BlogID = BlogID;

            if (ModelState.IsValid)
            {

                db.Comments.Add(comment);

                db.SaveChanges();

                if (BlogID != null)
                {
                    return await Read(BlogID);
                }
            }

            //Returning the appropriate view.
            return View(b);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string id)
        {
            await GetCurrentUserName();
            BlogModel blog = db.Blogs.Find(id);

            List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == id).ToListAsync();

            BlogWithComments b = new BlogWithComments(blog, comments);
            b.BlogID = id;
            b.UserName = username;
            return View(b);
        }

        //A get request for the ShowPosts view.

        [HttpGet]
        [AllowAnonymous]
        public IActionResult JustBlogs()
        {
            //Returning the list of blog objects saved. 

            return View(db.Blogs.ToList());
        }

        [Authorize]
        public async Task<IActionResult> AddLikes(string BlogID)
        {
            await GetCurrentUserName();

            if (BlogID != null)
            {
                BlogModel blog = await db.Blogs.FindAsync(BlogID);
                UserLikesOrDislikesBlog liker = await db.LikesOrDislikesBlogs.FindAsync(username, blog.BlogEntryID);
                if (liker == null)
                {
                    UserLikesOrDislikesBlog likesOrDislikes =
                        new UserLikesOrDislikesBlog();

                    likesOrDislikes.UserName = username;
                    likesOrDislikes.BlogID = blog.BlogEntryID;
                    likesOrDislikes.HasLiked = true;
                    likesOrDislikes.HasDisliked = false;
                    db.LikesOrDislikesBlogs.Add(likesOrDislikes);
                    blog.NumOfLikes++;
                    db.SaveChanges();
                    return RedirectToAction("JustBlogs");
                }
                if (liker.HasDisliked == true)
                {
                    liker.HasDisliked = false;
                    liker.HasLiked = true;
                    blog.NumOfLikes++;
                    blog.NumOfDislikes--;
                    db.SaveChanges();
                    return RedirectToAction("JustBlogs");
                }
                if (liker.HasLiked == true)
                {
                    return RedirectToAction("JustBlogs");
                }

            }
            //Returning the list of blog objects saved. 
            return View("JustBlogs");
        }

        [Authorize]
        public async Task<IActionResult> AddDislikes(string BlogID)
        {
            await GetCurrentUserName();

            if (BlogID != null)
            {
                BlogModel blog = await db.Blogs.FindAsync(BlogID);
                UserLikesOrDislikesBlog liker = await db.LikesOrDislikesBlogs.FindAsync(username,blog.BlogEntryID);

                if (liker == null)
                {
                    UserLikesOrDislikesBlog likesOrDislikes =
                        new UserLikesOrDislikesBlog();

                    likesOrDislikes.UserName = username;
                    likesOrDislikes.BlogID = blog.BlogEntryID;
                    likesOrDislikes.HasLiked = false;
                    likesOrDislikes.HasDisliked = true;
                    db.LikesOrDislikesBlogs.Add(likesOrDislikes);
                    blog.NumOfDislikes++;
                    db.SaveChanges();
                    return RedirectToAction("JustBlogs");
                }
                if (liker.HasLiked == true)
                {
                    liker.HasDisliked = true;
                    liker.HasLiked = false;
                    blog.NumOfLikes--;
                    blog.NumOfDislikes++;
                    db.SaveChanges();
                    return RedirectToAction("JustBlogs");
                }
                if (liker.HasDisliked == true)
                {
                    return RedirectToAction("JustBlogs");
                }

            }

            //Returning the list of blog objects saved. 
            return View("JustBlogs");
        }

        /*Checking if a blog exists by checking for its ID which is a private key (no duplication).*/
        [Authorize]
        private bool BlogExists(string id)
        {
            return db.Blogs.Any(e => e.BlogEntryID == id);
        }

        /*Finds the blog associated with the id passed into this method and passes the parameters 
        of the blog into the Update view.*/
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Update(string id)
        {
            await GetCurrentUserName();

            if (id == null)
            {
                return NotFound();
            }

            //Finds id in the database.
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            if (blog.UserName == username)
            {
                return View(blog);
            }
            return NotFound();
        }


        /*Post request for the update view. Using a model binder to link the HTTP request with a model.
        This method also mitigates cross site request forgeries.*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Update(string id, [Bind("BlogEntryID,BlogTitle,BlogEntry")] BlogModel blog)
        {
            await GetCurrentUserName();
            //Searching for id.
            if (id != blog.BlogEntryID)
            {
                return NotFound();
            }
            try
            {

                blog.UserName = username;
                //Updating and saving database
                db.Update(blog);
                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(blog.BlogEntryID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return RedirectToAction(nameof(MyBlogs));
        }

        /*Finds the blog associated with the id passed into this method and passes the parameters 
        of the blog into the Delete view.*/
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Delete(string id)
        {
            await GetCurrentUserName();
            if (id == null)
            {
                return NotFound();
            }

            //Finding the blog in the database
            var blog = await db.Blogs
                .FirstOrDefaultAsync(m => m.BlogEntryID.Equals(id));
            if (blog == null)
            {
                return NotFound();
            }

            if (blog.UserName == username)
            {
                return View(blog);
            }
            return NotFound();
        }

        /*This method confirms deletion once selected and mitigates cross site request forgeries.
        It is also a post request.*/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Confirm(string id)
        {
            //Finding blog in the database.
            var blog = await db.Blogs.FindAsync(id);

            //Removing blog from database.
            db.Blogs.Remove(blog);

            //Updating changes.
            await db.SaveChangesAsync();

            //Going back to view with the list of blogs.
            return RedirectToAction(nameof(MyBlogs));
        }

        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }

            var comment = await db.Comments
                .FirstOrDefaultAsync(m => m.CommentsId.Equals(id));
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmComment(int id)
        {

            var comment = await db.Comments.FindAsync(id);

            string BlogID = comment.BlogCommentedOnID;

            db.Comments.Remove(comment);

            //Updating changes.
            await db.SaveChangesAsync();


            return View(comment);
        }

    }

}