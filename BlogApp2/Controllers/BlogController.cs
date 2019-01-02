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

        private readonly UserManager<IdentityUser> _userManager;
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //Creating a constructor of this controller to have a database.
        public BlogController(ApplicationDbContext _db, UserManager<IdentityUser> userManager)
        {
            db = _db;

            this._userManager = userManager;
        }

        //Get request for the CreateBlog view.

        [HttpGet]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> MyBlogs()
        {
            var user = await GetCurrentUserAsync();

            string username = user?.UserName;

            List<BlogModel> blogs = new List<BlogModel>();
            blogs = await db.Blogs.Where(x => x.UserName == username).ToListAsync();

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

            var user = await GetCurrentUserAsync();

            string username = user?.UserName;

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
            CommentModel comment = new CommentModel();

            comment.CommentText = CommentText;
            comment.BlogCommentedOnID = BlogID;

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

        /*Finds the blog associated with the id passed into this method and passes the parameters 
            of the blog into the Read view.*/
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string id)
        {

            BlogModel blog = db.Blogs.Find(id);

            List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == id).ToListAsync();

            BlogWithComments b = new BlogWithComments(blog, comments);
            b.BlogID = id;

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

        /*Checking if a blog exists by checking for its ID which is a private key (no duplication).*/
        [Authorize]
        private bool BlogExists(string id)
        {
            return db.Blogs.Any(e => e.BlogEntryID == id);
        }

        /*Finds the blog associated with the id passed into this method and passes the parameters 
        of the blog into the Update view.*/
        [Authorize]
        public async Task<IActionResult> Update(string id)
        {

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

            return View(blog);
        }


        /*Post request for the update view. Using a model binder to link the HTTP request with a model.
        This method also mitigates cross site request forgeries.*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(string id, [Bind("BlogEntryID,BlogTitle,BlogEntry")] BlogModel blog)
        {

            var user = await GetCurrentUserAsync();
            string username = user?.UserName;

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
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {

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

            return View(blog);
        }

        /*This method confirms deletion once selected and mitigates cross site request forgeries.
        It is also a post request.*/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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


    }

}