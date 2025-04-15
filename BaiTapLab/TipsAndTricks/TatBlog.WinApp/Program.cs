using Microsoft.EntityFrameworkCore;
using System.Threading;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
optionsBuilder.UseSqlServer(@"Server=LAPTOP-N4TOHTRH\SQLEXPRESS;Database=TatBlog;User ID=sa;Password=minhbao8102;TrustServerCertificate=True;");

// Tạo đối tượng DbContext để quản lý phiên làm việt với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext(optionsBuilder.Options);

// Tạo đối tượng khởi tạo dữ liệu mẫu
var seeder = new DataSeeder(context);

// Gọi hàm Inititalize để nhập dữ liệu mẫu
seeder.Initialize();

// Đọc danh sách tác giả từ cơ sở dữ liệu
var authors = context.Authors.ToList();

// Xuất danh sách tác giả ra màn hình
Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");

foreach (var author in authors)
{
    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:dd/MM/yyyy}", author.Id, author.FullName, author.Email, author.JoinedDate);
}

// Đọc danh sách bài viết từ cơ sở dữ liệu
// Lấy kèm tên tác giả và chuyên mục
var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name,
    })
    .ToList();

// Xuất danh sách bài viết ra màn hình
foreach (var post in posts)
{
    Console.WriteLine("ID      : {0}", post.Id);
    Console.WriteLine("Title   : {0}", post.Title);
    Console.WriteLine("View    : {0}", post.ViewCount);
    Console.WriteLine("Date    : {0:dd/MM/yyy}", post.PostedDate);
    Console.WriteLine("Author  : {0}", post.Author);
    Console.WriteLine("Category: {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}

// Tạo đối tượng BlogRepository
IBlogRepository blogRepo = new BlogRepository(context);

// Tìm 3 bài viết được xem/đọc nhiều nhất
var postBlogs = await blogRepo.GetPopularArticlesAsync(3);

// Xuất danh sách bài viết ra màn hình
foreach (var postBlog in postBlogs)
{
    Console.WriteLine("ID      : {0}", postBlog.Id);
    Console.WriteLine("Title   : {0}", postBlog.Title);
    Console.WriteLine("View    : {0}", postBlog.ViewCount);
    Console.WriteLine("Date    : {0:dd/MM/yyy}", postBlog.PostedDate);
    Console.WriteLine("Author  : {0}", postBlog.Author.FullName);
    Console.WriteLine("Category: {0}", postBlog.Category.Name);
    Console.WriteLine("".PadRight(80, '-'));
}

// Lấy danh sách chuyên mục
var categories = await blogRepo.GetCategoriesAsync();

// Xuất ra màn hình
Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

foreach (var item in categories)
{
    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
}

// Tạo đối tượng chứa tham số phân trang
var pagingParams = new PagingParams
{
    PageNumber = 1,      // Lấy kết quả ở trang số 1
    PageSize = 5,        // Lấy 5 mẫu tin
    SortColumn = "Name", // Sắp xếp theo tên
    SortOrder = "DESC"   // Theo chiều giảm dần
};

// Lấy danh sách từ khóa
var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

// Xuất ra màn hình
Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

foreach (var item in tagsList)
{
    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
}

// Bài tập 1:
Console.WriteLine("\n==== BÀI TẬP 1 ====");
// a. Tìm một thẻ (Tag) theo tên định danh (slug)
Console.Write("\na. Nhập slug của thẻ cần tìm: ");
string inputSlug = Console.ReadLine();

var tag = await blogRepo.GetTagBySlugAsync(inputSlug);
if (tag != null)
    Console.WriteLine($"Thẻ tìm được: {tag.Name} - {tag.Description}");
else
    Console.WriteLine("Không tìm thấy thẻ!");

// b. Tạo lớp DTO TagItem để chứa thông tin về thẻ và số lượng bài viết chứa thẻ đó (Đã tạo lớp TagItem ở TatBlog.Core.DTO)
// c. Lấy danh sách tất cả các thẻ kèm theo số bài viết chứa thẻ đó, trả về IList<TagItem>
var allTags = await blogRepo.GetAllTagsWithPostCountAsync();
Console.WriteLine("\nc. Danh sách tất cả các thẻ:");
foreach (var t in allTags)
{
    Console.WriteLine($"{t.Name} ({t.PostCount})");
}

// d. Xóa một thẻ theo mã cho trước
Console.Write("\nd. Nhập ID của thẻ cần xoá: ");
if (int.TryParse(Console.ReadLine(), out int tagIdToDelete))
{
    bool deleted = await blogRepo.DeleteTagByIdAsync(tagIdToDelete);
    Console.WriteLine(deleted ? "Xoá thành công." : "Không tìm thấy thẻ.");
}
else
{
    Console.WriteLine("ID không hợp lệ!");
}

// e. Tìm một chuyên mục (Category) theo tên định danh (slug)
Console.Write("\ne. Nhập slug chuyên mục: ");
string categorySlug = Console.ReadLine();

var catBySlug = await blogRepo.GetCategoryBySlugAsync(categorySlug);
Console.WriteLine(catBySlug != null
    ? $"Chuyên mục: {catBySlug.Name}"
    : "Không tìm thấy chuyên mục!");

// f. Tìm một chuyên mục theo mã số cho trước
Console.Write("\nf. Nhập ID chuyên mục: ");
if (int.TryParse(Console.ReadLine(), out int categoryId))
{
    var catById = await blogRepo.GetCategoryByIdAsync(categoryId);
    Console.WriteLine(catById != null
        ? $"Chuyên mục ID={categoryId}: {catById.Name}"
        : "Không tìm thấy chuyên mục!");
}

// g. Thêm hoặc cập nhật một chuyên mục/chủ đề
Console.WriteLine("\ng. Nhập thông tin chuyên mục mới:");

Console.Write("Tên chuyên mục: ");
string name = Console.ReadLine();

Console.Write("Slug: ");
string slug = Console.ReadLine();

Console.Write("Mô tả: ");
string desc = Console.ReadLine();

Console.Write("Hiển thị trên menu? (true/false): ");
bool showOnMenu = bool.TryParse(Console.ReadLine(), out var show) && show;

var newCategory = new Category
{
    Name = name,
    UrlSlug = slug,
    Description = desc,
    ShowOnMenu = showOnMenu
};

var savedCategory = await blogRepo.CreateOrUpdateCategoryAsync(newCategory);
Console.WriteLine($"Đã {(savedCategory.Id > 0 ? "lưu" : "thêm")} chuyên mục: {savedCategory.Name}");

// h. Xóa một chuyên mục theo mã số cho trước
Console.Write("\nh. Nhập ID chuyên mục cần xóa: ");
if (int.TryParse(Console.ReadLine(), out int catId))
{
    var deletedCat = await blogRepo.DeleteCategoryByIdAsync(catId);
    Console.WriteLine(deletedCat ? "Xoá thành công!" : "Không tìm thấy chuyên mục!");
}

// i. Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa
Console.Write("\ni. Nhập slug cần kiểm tra: ");
string checkSlug = Console.ReadLine();

bool isExisted = await blogRepo.IsCategorySlugExistedAsync(0, checkSlug);
Console.WriteLine(isExisted ? "Slug đã tồn tại!" : "Slug chưa tồn tại.");

// j. Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>
Console.WriteLine("\nj. Lấy chuyên mục có phân trang:");
Console.Write("Số trang: ");
int.TryParse(Console.ReadLine(), out int pageNumber);
Console.Write("Kích thước trang: ");
int.TryParse(Console.ReadLine(), out int pageSize);

var pagedCategories = await blogRepo.GetPagedCategoriesAsync(new PagingParams
{
    PageNumber = pageNumber,
    PageSize = pageSize
});

foreach (var c in pagedCategories)
{
    Console.WriteLine($"{c.Id,-3} {c.Name,-30} Bài viết: {c.PostCount}");
}

// k. Đếm số lượng bài viết trong N tháng gần nhất
Console.Write("\nk. Nhập số tháng gần nhất cần đếm bài viết: ");
if (int.TryParse(Console.ReadLine(), out int numMonths))
{
    var postCounts = await blogRepo.CountPostsInRecentMonthsAsync(numMonths);
    foreach (var item in postCounts)
    {
        Console.WriteLine($"Năm {item.Year}, Tháng {item.Month}: {item.PostCount} bài");
    }
}
else
{
    Console.WriteLine("Số tháng không hợp lệ!");
}

// l. Tìm một bài viết theo mã số
Console.Write("\nl. Nhập ID bài viết cần tìm: ");
if (int.TryParse(Console.ReadLine(), out int postId))
{
    var postDetail = await blogRepo.GetPostByIdAsync(postId);
    if (postDetail != null)
    {
        Console.WriteLine($"Tiêu đề: {postDetail.Title}");
        Console.WriteLine($"Tác giả: {postDetail.Author.FullName}, Chuyên mục: {postDetail.Category.Name}");
    }
    else
    {
        Console.WriteLine("Không tìm thấy bài viết!");
    }
}

// m. Thêm hay cập nhật một bài viết
Console.WriteLine("\nm. Nhập thông tin bài viết:");

Console.Write("Tiêu đề: ");
string title = Console.ReadLine();

Console.Write("Slug: ");
string postSlug = Console.ReadLine();

Console.Write("Mô tả ngắn: ");
string shortDesc = Console.ReadLine();

Console.Write("Nội dung: ");
string descPost = Console.ReadLine();

Console.Write("Meta: ");
string meta = Console.ReadLine();

Console.Write("Đăng bài? (true/false): ");
bool published = bool.TryParse(Console.ReadLine(), out var pub) && pub;

Console.Write("Mã tác giả: ");
int.TryParse(Console.ReadLine(), out int authorId);

Console.Write("Mã chuyên mục: ");
int.TryParse(Console.ReadLine(), out int catIdPost);

var newPost = new Post
{
    Title = title,
    UrlSlug = postSlug,
    ShortDescription = shortDesc,
    Description = descPost,
    Meta = meta,
    Published = published,
    PostedDate = DateTime.Now,
    ModifiedDate = DateTime.Now,
    AuthorId = authorId,
    CategoryId = catIdPost
};

var savedPost = await blogRepo.CreateOrUpdatePostAsync(newPost);
Console.WriteLine($"Đã lưu bài viết: {savedPost.Title}");

// n. Chuyển đổi trạng thái Published của bài viết
Console.Write("\nn. Nhập ID bài viết cần chuyển trạng thái: ");
if (int.TryParse(Console.ReadLine(), out int togglePostId))
{
    bool result = await blogRepo.TogglePublishedStatusAsync(togglePostId);
    Console.WriteLine(result ? "Đã đổi trạng thái Published!" : "Không tìm thấy bài viết.");
}

// o. Lấy ngẫu nhiên N bài viết
Console.Write("\no. Nhập số lượng bài viết ngẫu nhiên cần lấy: ");
int.TryParse(Console.ReadLine(), out int numRandom);

var randomPosts = await blogRepo.GetRandomPostsAsync(numRandom);
foreach (var post in randomPosts)
{
    Console.WriteLine($"- {post.Title} ({post.ViewCount} lượt xem)");
}

// p. Tạo lớp PostQuery để lưu trữ điều kiện tìm kiếm (Đã tạo lớp PostQuery ở TatBlog.Core.DTO)
// q. Tìm tất cả bài viết thỏa điều kiện trong PostQuery
Console.WriteLine("\nq. Nhập điều kiện tìm bài viết:");
Console.Write("Từ khóa: ");
string keyword = Console.ReadLine();

var query = new PostQuery
{
    Keyword = keyword
};

var foundPosts = await blogRepo.GetPostsByQueryAsync(query);
foreach (var post in foundPosts)
{
    Console.WriteLine($"- {post.Title}");
}

// r. Đếm số lượng bài viết thỏa điều kiện PostQuery
int totalPosts = await blogRepo.CountPostsByQueryAsync(query);
Console.WriteLine($"Tổng số bài viết chứa từ khóa: {totalPosts}");

// s. Phân trang các bài viết theo PostQuery
var pagedPosts = await blogRepo.GetPagedPostsAsync(query, new PagingParams
{
    PageNumber = 1,
    PageSize = 5
});

Console.WriteLine("\ns. Kết quả phân trang:");
foreach (var post in pagedPosts)
{
    Console.WriteLine($"- {post.Title}");
}

// t. Phân trang với ánh xạ sang kiểu T (generic)
var pagedPostItems = await blogRepo.GetPagedPostsAsync<PostItem>(
    query,
    new PagingParams { PageNumber = 1, PageSize = 5 },
    posts => posts.Select(x => new PostItem
    {
        Id = x.Id,
        Title = x.Title,
        UrlSlug = x.UrlSlug,
        ShortDescription = x.ShortDescription,
        ImageUrl = x.ImageUrl,
        ViewCount = x.ViewCount,
        Published = x.Published,
        PostedDate = x.PostedDate,
        CategoryName = x.Category.Name,
        AuthorName = x.Author.FullName,
        TagCount = x.Tags.Count
    }));

Console.WriteLine("\nt. Kết quả PostItem:");
foreach (var post in pagedPostItems)
{
    Console.WriteLine($"[{post.Id}] {post.Title} - {post.AuthorName} ({post.CategoryName}) - Tags: {post.TagCount}");
}

// Bài tập 2:
Console.WriteLine("\n==== BÀI TẬP 2 ====");
// a. Tạo interface IAuthorRepository và lớp AuthorRepository. (Đã tạo interface IAuthorRepository và lớp AuthorRepository ở TatBlog.Services.Blogs)
IAuthorRepository authorRepo = new AuthorRepository(context);

// b. Tìm một tác giả theo mã số.
Console.Write("b. Nhập ID tác giả cần tìm: ");
if (int.TryParse(Console.ReadLine(), out int authId))
{
    var author = await authorRepo.GetAuthorByIdAsync(authId);
    if (author != null)
        Console.WriteLine($"Tác giả: {author.FullName} - Email: {author.Email}");
    else
        Console.WriteLine("Không tìm thấy tác giả!");
}

// c. Tìm một tác giả theo tên định danh (slug).
Console.Write("\nc. Nhập slug tác giả cần tìm: ");
string authorSlug = Console.ReadLine();

var authorBySlug = await authorRepo.GetAuthorBySlugAsync(authorSlug);
if (authorBySlug != null)
    Console.WriteLine($"Tác giả: {authorBySlug.FullName} - Email: {authorBySlug.Email}");
else
    Console.WriteLine("Không tìm thấy tác giả!");

// d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả đó. Kết quả trả về kiểu IPagedList<AuthorItem>.
var pagedAuthors = await authorRepo.GetPagedAuthorsAsync(new PagingParams
{
    PageNumber = 1,
    PageSize = 5
});

Console.WriteLine("\nd. Danh sách tác giả (có phân trang):");
foreach (var a in pagedAuthors)
{
    Console.WriteLine($"{a.Id,-3} {a.FullName,-30} Bài viết: {a.PostCount}");
}

// e. Thêm hoặc cập nhật thông tin một tác giả.
Console.WriteLine("\ne. Nhập thông tin tác giả mới:");
Console.Write("Họ tên: ");
string fullName = Console.ReadLine();

Console.Write("Slug: ");
string authSlug = Console.ReadLine();

Console.Write("Email: ");
string email = Console.ReadLine();

Console.Write("Ảnh đại diện URL: ");
string imgUrl = Console.ReadLine();

Console.Write("Ghi chú: ");
string notes = Console.ReadLine();

var newAuthor = new Author
{
    FullName = fullName,
    UrlSlug = authSlug,
    Email = email,
    ImageUrl = imgUrl,
    Notes = notes,
    JoinedDate = DateTime.Now
};

bool saved = await authorRepo.AddOrUpdateAuthorAsync(newAuthor);
Console.WriteLine(saved ? "Lưu tác giả thành công!" : "Lưu tác giả thất bại!");

// f. Tìm danh sách N tác giả có nhiều bài viết nhất. N là tham số đầu vào.
Console.Write("\nf. Nhập số lượng tác giả muốn tìm theo số bài viết: ");
if (int.TryParse(Console.ReadLine(), out int topN))
{
    var topAuthors = await authorRepo.GetTopAuthorsAsync(topN);
    foreach (var a in topAuthors)
    {
        Console.WriteLine($"{a.FullName} - {a.PostCount} bài viết");
    }
}

// Bài tập 3:
Console.WriteLine("\n==== BÀI TẬP 3 ====");
// a. Tạo lớp Subscriber (Đã tạo lớp Subscriber ở TatBlog.Core.Entities)
// b. Tạo lớp SubscriberMap (Đã tạo lớp SubscriberMap ở TatBlog.Data.Mappings)
// c. Bổ sung vào BlogDbContext và Migration (Đã bổ sung vào BlogDbContext ở TatBlog.Data.Contexts và Migration thành công)
// d. Tạo ISubscriberRepository và SubscriberRepository (Đã tạo interface IAuthorRepository và lớp AuthorRepository ở TatBlog.Services.Blogs)
// e. Định nghĩa các phương thức để thực hiện các công việc sau:
ISubscriberRepository subRepo = new SubscriberRepository(context);
// - Đăng ký theo dõi: SubscribeAsync(email)
Console.WriteLine("\n1. Đăng ký theo dõi");
Console.Write("Nhập email người đăng ký: ");
var email1 = Console.ReadLine();
await subRepo.SubscribeAsync(email1);
Console.WriteLine("Đăng ký thành công.");

// - Hủy đăng ký: UnsubscribeAsync(email, reason, voluntary)
Console.WriteLine("\n2. Hủy đăng ký theo dõi");
Console.Write("Nhập email người hủy đăng ký: ");
var email2 = Console.ReadLine();
Console.Write("Lý do hủy: ");
var reason = Console.ReadLine();
Console.Write("Người dùng tự hủy? (true/false): ");
bool voluntary = bool.TryParse(Console.ReadLine(), out var vol) && vol;
await subRepo.UnsubscribeAsync(email2, reason, voluntary);
Console.WriteLine("Đã xử lý hủy đăng ký.");

// - Chặn một người theo dõi: BlockSubscriberAsync(id, reason, notes)
Console.WriteLine("\n3. Chặn người theo dõi");
Console.Write("Nhập ID người bị chặn: ");
int.TryParse(Console.ReadLine(), out int blockId);
Console.Write("Lý do: ");
var blockReason = Console.ReadLine();
Console.Write("Ghi chú quản trị viên: ");
var adminNotes = Console.ReadLine();
await subRepo.BlockSubscriberAsync(blockId, blockReason, adminNotes);
Console.WriteLine("Đã chặn người theo dõi.");

// - Xóa một người theo dõi: DeleteSubscriberAsync(id)
Console.WriteLine("\n4. Xoá người theo dõi");
int.TryParse(Console.ReadLine(), out int delId);
await subRepo.DeleteSubscriberAsync(delId);
Console.WriteLine("Đã xoá người theo dõi.");

// - Tìm người theo dõi bằng ID: GetSubscriberByIdAsync(id)
Console.WriteLine("\n5. Tìm người theo dõi theo ID");
Console.Write("Nhập ID: ");
int.TryParse(Console.ReadLine(), out int id6);
var subById = await subRepo.GetSubscriberByIdAsync(id6);
if (subById != null)
    Console.WriteLine($"Email: {subById.Email}, Subscribed: {subById.SubscribedDate:dd/MM/yyyy}");
else
    Console.WriteLine("Không tìm thấy.");

// - Tìm người theo dõi bằng email: GetSubscriberByEmailAsync(email)
Console.WriteLine("\n6. Tìm người theo dõi theo Email");
Console.Write("Nhập email: ");
var email5 = Console.ReadLine();
var subByEmail = await subRepo.GetSubscriberByEmailAsync(email5);
if (subByEmail != null)
    Console.WriteLine($"ID: {subByEmail.Id}, Subscribed: {subByEmail.SubscribedDate:dd/MM/yyyy}");
else
    Console.WriteLine("Không tìm thấy.");

// - Tìm danh sách người theo dõi theo nhiều tiêu chí khác nhau, kết quả được phân trang: Task<IPagedList<Subscriber>> SearchSubscribersAsync(pagingParams, keyword, unsubscribed, involuntary).
Console.WriteLine("\n7. Tìm kiếm người theo dõi (phân trang)");
Console.Write("Từ khoá tìm kiếm: ");
var searchKeyword = Console.ReadLine();
Console.Write("Hủy đăng ký? (true/false/blank): ");
var unsub = Console.ReadLine();
Console.Write("Bị chặn? (true/false/blank): ");
var invol = Console.ReadLine();

bool? unsubscribed = string.IsNullOrWhiteSpace(unsub) ? null : bool.Parse(unsub);
bool? involuntary = string.IsNullOrWhiteSpace(invol) ? null : bool.Parse(invol);

var results = await subRepo.SearchSubscribersAsync(
    new PagingParams { PageNumber = 1, PageSize = 10 },
    searchKeyword, unsubscribed, involuntary);

Console.WriteLine("{0,-5}{1,-30}{2,-20}{3,-10}", "ID", "Email", "Subscribed", "Unsubscribed");
foreach (var s in results)
{
    Console.WriteLine("{0,-5}{1,-30}{2,-20:dd/MM/yyyy}{3,-10:dd/MM/yyyy}",
        s.Id, s.Email, s.SubscribedDate, s.UnsubscribedDate);
}

// Bài tập 4:
Console.WriteLine("\n==== BÀI TẬP 4 ====");

ICommentRepository commentRepo = new CommentRepository(context);

Console.WriteLine("\n1. Thêm comment");
Console.Write("Nhập PostId: ");
int.TryParse(Console.ReadLine(), out int commentPostId);
Console.Write("Tên người dùng: ");
var comentName = Console.ReadLine();
Console.Write("Email: ");
var commentEmail = Console.ReadLine();
Console.Write("Nội dung: ");
var content = Console.ReadLine();

var newComment = new Comment
{
    AuthorName = comentName,
    Email = commentEmail,
    Content = content,
    PostId = commentPostId
};

await commentRepo.AddCommentAsync(newComment);
Console.WriteLine("Thêm comment thành công (chờ duyệt).");

Console.WriteLine("\n2. Duyệt comment");
Console.Write("Nhập ID comment cần duyệt: ");
int.TryParse(Console.ReadLine(), out int commentId);
var approved = await commentRepo.ApproveCommentAsync(commentId);
Console.WriteLine(approved ? "Duyệt thành công." : "Comment không tồn tại.");

Console.WriteLine("\n3. Xóa comment");
Console.Write("Nhập ID comment cần xóa: ");
int.TryParse(Console.ReadLine(), out int commentDelId);
var commentDeleted = await commentRepo.DeleteCommentAsync(commentDelId);
Console.WriteLine(commentDeleted ? "Xóa thành công." : "Comment không tồn tại.");

Console.WriteLine("\n4. Xem comment chờ duyệt");
var pendingComments = await commentRepo.GetPendingCommentsAsync();
foreach (var c in pendingComments)
{
    Console.WriteLine($"[{c.Id}] {c.AuthorName} - {c.Content} (PostId: {c.PostId})");
}

Console.WriteLine("\n5. Xem comment theo PostId");
Console.Write("Nhập PostId: ");
int.TryParse(Console.ReadLine(), out int pid);
var approvedComments = await commentRepo.GetCommentsAsync(pid, true);
foreach (var c in approvedComments)
{
    Console.WriteLine($"[{c.Id}] {c.AuthorName} - {c.Content}");
}

Console.ReadKey();
