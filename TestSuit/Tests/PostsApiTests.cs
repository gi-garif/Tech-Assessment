using Serilog;
using System.Net;

[TestFixture]
public class PostsApiTests
{
    private ApiHelper _apiHelper;

    [SetUp]
    public void Setup()
    {
        _apiHelper = new ApiHelper();
        Log.Information("Test setup completed.");
    }

    [Test]
    public void PostNewPost_HappyPath()
    {
        var post = new
        {
            userId = 1,
            title = "Test Title",
            body = "Test Body"
        };

        var response = _apiHelper.Post("/posts", post);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Log.Information("PostNewPost_HappyPath test passed.");

        // Additional assertions 
        var createdPost = _apiHelper.DeserializeResponse<Post>(response);
        Assert.That(createdPost, Is.Not.Null, "Response should contain the created post.");
        Assert.That(createdPost.Id, Is.GreaterThan(0), "Post ID should be greater than 0.");
        Assert.That(createdPost.UserId, Is.EqualTo(post.userId), "User ID should match the request.");
        Assert.That(createdPost.Title, Is.EqualTo(post.title), "Post title should match the request.");
        Assert.That(createdPost.Body, Is.EqualTo(post.body), "Post body should match the request.");

    }

    [Test]
    public void GetPosts_HappyPath()
    {
        var response = _apiHelper.Get("/posts");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Log.Information("GetPosts_HappyPath test passed.");

        // Additional assertions for response content
        var posts = _apiHelper.DeserializeResponse<List<Post>>(response);
        Assert.That(posts, Is.Not.Null.And.Not.Empty, "Response should contain posts.");

        foreach (var post in posts)
        {
            Assert.That(post.Id, Is.GreaterThan(0), "Post ID should be greater than 0.");
            Assert.That(post.Title, Is.Not.Null.And.Not.Empty, "Post title should not be empty.");
            Assert.That(post.Body, Is.Not.Null.And.Not.Empty, "Post body should not be empty.");
            Assert.That(post.UserId, Is.GreaterThan(0), "User ID should be greater than 0.");
        };

    }

    [Test]
    public void PutPost_HappyPath()
    {
        var postId = 1;
        var updatedPost = new
        {
            title = "Updated Title",
            body = "Updated Body"
        };

        var response = _apiHelper.Put($"/posts/{postId}", updatedPost);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Log.Information("PutPost_HappyPath test passed.");

        // Additional assertions 
        var updatedPostResponse = _apiHelper.DeserializeResponse<Post>(response);
        Assert.That(updatedPostResponse, Is.Not.Null, "Response should contain the updated post.");
        Assert.That(updatedPostResponse.Id, Is.EqualTo(postId), "Post ID should match the request.");

        Assert.That(updatedPostResponse.Title, Is.EqualTo(updatedPost.title), "Post title should match the request.");
        Assert.That(updatedPostResponse.Body, Is.EqualTo(updatedPost.body), "Post body should match the request.");

    }

    [Test]
    public void PostComment_HappyPath()
    {
        var postId = 1;
        var comment = new
        {
            name = "Test Comment",
            email = "test@example.com",
            body = "This is a test comment."
        };

        var response = _apiHelper.Post($"/posts/{postId}/comments", comment);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Log.Information("PostComment_HappyPath test passed.");

        // Additional assertions
        var createdCommentResponse = _apiHelper.DeserializeResponse<Comment>(response);
        Assert.That(createdCommentResponse, Is.Not.Null, "Response should contain the created comment.");

        Assert.That(createdCommentResponse.Name, Is.EqualTo(comment.name), "Comment name should match.");
        Assert.That(createdCommentResponse.Email, Is.EqualTo(comment.email), "Comment email should match.");
        Assert.That(createdCommentResponse.Body, Is.EqualTo(comment.body), "Comment body should match.");

    }

    [Test]
    public void GetCommentsByPostId_HappyPath()
    {
        var postId = 1;
        var endpoint = $"/comments?postId={postId}";
        var response = _apiHelper.Get(endpoint);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Log.Information($"GetCommentsByPostId_HappyPath for PostId: {postId} test passed.");

        // Additional assertions for response content
        var comments = _apiHelper.DeserializeResponse<List<Comment>>(response);
        Assert.That(comments, Is.Not.Null.And.Not.Empty, "Response should contain comments.");

        foreach (var comment in comments)
        {
            Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greater than 0.");
            Assert.That(comment.Name, Is.Not.Null.And.Not.Empty, "Comment name should not be empty.");
            Assert.That(comment.Email, Is.Not.Null.And.Not.Empty, "Comment email should not be empty.");
            Assert.That(comment.Body, Is.Not.Null.And.Not.Empty, "Comment body should not be empty.");
            Assert.That(comment.PostId, Is.EqualTo(postId), "Comment should belong to the specified post.");
        }
    }


    [Test]
    public void DeletePost_HappyPath()
    {
        var postId = 1;
        var response = _apiHelper.Delete($"/posts/{postId}");

        // Check for a successful deletion (204 No Content)
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Log.Information("DeletePost_HappyPath test passed.");

        Assert.That(response.Content, Is.Empty, "Response body should be empty for a successful deletion.");

    }


    [Test]
    public void PutPost_InvalidPostId()
    {
        // Sending a PUT request with an invalid post ID
        var invalidPostId = "-1"; 
        var updatedPost = new
        {
            title = "Updated Title",
            body = "Updated Body"
        };

        var response = _apiHelper.Put($"/posts/{invalidPostId}", updatedPost);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Log.Information("PutPost_InvalidPostId test passed.");
    }

    [Test]
    public void DeletePost_InvalidPostId()
    {
        // Sending a DELETE request with an invalid post ID
        var invalidPostId = "-1"; 

        var response = _apiHelper.Delete($"/posts/{invalidPostId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Log.Information("DeletePost_InvalidPostId test passed.");
    }

    [Test]
    public void PostComment_InvalidPostId()
    {
        // Sending a POST request for comments with an invalid post ID
        var invalidPostId = "-1"; 
        var comment = new
        {
            name = "Test Comment",
            email = "test@example.com",
            body = "This is a test comment."
        };

        var response = _apiHelper.Post($"/posts/{invalidPostId}/comments", comment);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Log.Information("PostComment_InvalidPostId test passed.");

    }

    [Test]
    public void GetCommentsByPostId_InvalidPostId()
    {
        // Sending a GET request for comments with an invalid post ID
        var invalidPostId = "-1"; // 

        var response = _apiHelper.Get($"/comments?postId={invalidPostId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Log.Information("GetCommentsByPostId_InvalidPostId test passed.");

    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of resources used during tests
        _apiHelper.Dispose();
    }
}
