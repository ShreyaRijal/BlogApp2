/*
 * This controller class has implemented the basic functionality of CRUD which 
 * involves the creation, reading, updating and, deletion of a blog object.
 * It also allows users to see all the blogs ever posted and a list of their 
 * own blogs in a "MyBlogs" page.
 * Users can also write comments for blogs and delete them.
 */

using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BlogController : Controller
    {
        //An instance of the database.
        private ApplicationDbContext db;

        //The username of the person currently logged in.
        public string username;

        private readonly UserManager<IdentityUser> _userManager;

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //Creating a constructor of this controller to have a database.
        public BlogController(ApplicationDbContext _db, UserManager<IdentityUser> userManager)
        {
            db = _db;

            this._userManager = userManager;
        }

        //Returns the username of the current logged in user.
        public async Task GetCurrentUserName()
        {
            var user = await GetCurrentUserAsync();

            username = user?.UserName;

        }

        //Returns a list of the user's own blogs.
        [HttpGet]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> MyBlogs()
        {
            await GetCurrentUserName();
            List<BlogModel> blogs = new List<BlogModel>();

            //A list of blogs beloning to the user that's logged in.
            blogs = await db.Blogs.Where(x => x.UserName == username).ToListAsync();

            return View(blogs);
        }

        //Get request for the CreateBlog view. Only those authorized to blog can access it.
        [HttpGet]
        [Authorize(Policy = "Blogger")]
        public IActionResult Create()
        {
            return View("CreateBlog");
        }

        //Post request to create a blog. User has to be authorized.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Blogger")]
        public async Task<IActionResult> Create(string title, string entry)
        {
            //Creating an instance of a blog object.
            BlogModel blog = new BlogModel();

            await GetCurrentUserName();

            //Check if user has entered something into the text fields.
            if (title != null && entry != null)
            {
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

                return View(blog);
            }
            return View("CreateBlog");

        }

        //Gets the blog and the comments for it. User doesn't have to be logged in to see this.
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string id)
        {
            await GetCurrentUserName();

            //Get the relevant blog.
            BlogModel blog = db.Blogs.Find(id);

            //Get the relevant comments.
            List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == id).ToListAsync();

            //Pass them to this specific view model.
            if (comments != null)
            {
                BlogWithComments b = new BlogWithComments(blog, comments);
                b.BlogID = id;
                b.UserName = username;
                return View(b);
            }
            BlogWithComments b2 = new BlogWithComments(blog);
            b2.BlogID = id;
            b2.UserName = username;
            return View(b2);
        }

        //User can post comments on a blog.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Read(string commentText, string blogID)
        {
            await GetCurrentUserName();

            CommentModel comment = new CommentModel();

            //Check if something was written in the text field.
            if (commentText != null)
            {
                comment.CommentText = commentText;
                comment.BlogCommentedOnID = blogID;
                comment.UserName = username;

                //Find the relevant blog
                BlogModel blog = db.Blogs.Find(blogID);

                //Find the relevant comments.
                List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == blogID).ToListAsync();

                //Create a view specific model using the two.
                BlogWithComments b = new BlogWithComments(blog, comments);
                b.BlogID = blogID;

                if (ModelState.IsValid)
                {
                    
                    db.Comments.Add(comment);

                    db.SaveChanges();

                    return await Read(blogID);
                }

            }
            return await Read(blogID);
        }

        //Returning all of the blogs.
        [HttpGet]
        [AllowAnonymous]
        public IActionResult JustBlogs()
        {
            return View(db.Blogs.ToList());
        }

        //User can toggle to like and remove the like from a blog.
        [Authorize]
        public async Task<IActionResult> AddLikes(string blogID)
        {
            await GetCurrentUserName();

            if (blogID != null)
            {
                BlogModel blog = await db.Blogs.FindAsync(blogID);
                UserLikesOrDislikesBlog liker = await db.LikesOrDislikesBlogs.FindAsync(username, blog.BlogEntryID);
                if (liker == null)
                {
                    //Add the user and their like to the database for a specific blog.
                    UserLikesOrDislikesBlog likesOrDislikes =
                        new UserLikesOrDislikesBlog();

                    likesOrDislikes.UserName = username;
                    likesOrDislikes.BlogID = blog.BlogEntryID;
                    likesOrDislikes.HasLiked = true;
                    likesOrDislikes.HasDisliked = false;
                    db.LikesOrDislikesBlogs.Add(likesOrDislikes);
                    blog.NumOfLikes++;
                    db.SaveChanges();
                    return PartialView("AddLikes", blog);
                    //return await Read(BlogID);
                }
                //Logic to toggle the like.
                if (liker.HasLiked == false && liker.HasDisliked == true)
                {
                    liker.HasDisliked = false;
                    liker.HasLiked = true;
                    blog.NumOfLikes++;
                    blog.NumOfDislikes--;
                    db.SaveChanges();
                    return PartialView("AddLikes", blog);
                }
                if (liker.HasLiked == true && liker.HasDisliked == false)
                {
                    liker.HasDisliked = false;
                    liker.HasLiked = false;
                    blog.NumOfLikes--;
                    db.SaveChanges();
                    return PartialView("AddLikes", blog);
                }
                if (liker.HasLiked == false && liker.HasDisliked == false)
                {
                    liker.HasDisliked = false;
                    liker.HasLiked = true;
                    blog.NumOfLikes++;
                    db.SaveChanges();
                    return PartialView("AddLikes", blog);
                    //return await Read(BlogID);
                }

            }
            return PartialView("AddLikes");
        }


        //Same as AddLikes method but this time user can toggle dislikes on a blog.
        [Authorize]
        public async Task<IActionResult> AddDislikes(string blogID)
        {
            await GetCurrentUserName();

            if (blogID != null)
            {
                BlogModel blog = await db.Blogs.FindAsync(blogID);
                UserLikesOrDislikesBlog liker = await db.LikesOrDislikesBlogs.FindAsync(username, blog.BlogEntryID);

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
                    return PartialView("AddDislikes", blog);
                }
                if (liker.HasDisliked == false && liker.HasLiked == true)
                {
                    liker.HasDisliked = true;
                    liker.HasLiked = false;
                    blog.NumOfLikes--;
                    blog.NumOfDislikes++;
                    db.SaveChanges();
                    return PartialView("AddDislikes", blog);

                }
                if (liker.HasDisliked == true && liker.HasLiked == false)
                {
                    liker.HasDisliked = false;
                    liker.HasLiked = false;
                    blog.NumOfDislikes--;
                    db.SaveChanges();
                    return PartialView("AddDislikes", blog);
                }
                if (liker.HasDisliked == false && liker.HasLiked == false)
                {
                    liker.HasDisliked = true;
                    liker.HasLiked = false;
                    blog.NumOfDislikes++;
                    db.SaveChanges();
                    return PartialView("AddDislikes", blog);
                }

            }

            return PartialView("AddDislikes");
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

            //Finds blog in database with corresponding id.
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

                //Updating and saving database.
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

            List<CommentModel> comments = await db.Comments.Where(x => x.BlogCommentedOnID == id).ToListAsync();

            foreach (CommentModel comment in comments)
            {
                db.Comments.Remove(comment);
            }

            //Updating changes.
            await db.SaveChangesAsync();

            //Going back to view with the list of blogs.
            return RedirectToAction(nameof(MyBlogs));
        }

        //Takes user into the DeleteComment view for the comment with the corresponding id.
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }

            var comment = await db.Comments
                .FirstOrDefaultAsync(m => m.CommentsId.Equals(id));
            if (comment != null)
            {
                return View(comment);
            }

            return NotFound();
        }

        //Delete the comment from the database. 
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmComment(int id, string blogID)
        {

            var comment = await db.Comments.FindAsync(id);

            db.Comments.Remove(comment);

            //Updating changes.
            await db.SaveChangesAsync();

            //return await Read(BlogID);
            return View(comment);

        }

    }
}