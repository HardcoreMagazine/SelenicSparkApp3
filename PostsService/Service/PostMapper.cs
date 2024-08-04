using PostsService.Models.DTO;
using PostsService.Models;

namespace PostsService.Service
{
    public static class PostMapper
    {
        public static Post MapToPostFromRequest(PostRequest pr)
        {
            return new Post
            {
                Title = pr.title,
                Author = pr.author,
                Text = pr.text
            };
        }

        public static Post MapToPostFromResponse(PostResponse pr)
        {
            return new Post
            {
                ID = pr.id,
                Title = pr.title,
                Author = pr.author,
                Text = pr.text,
                DateCreated = pr.dateCreated
            };
        }

        public static PostResponse MapToResponseFromPost(Post post)
        {
            return new PostResponse(post.ID, post.Title, post.Author, post.Text, post.DateCreated);
        }
    }
}
