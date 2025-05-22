using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any())
            {
                return;
            }

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new() { FullName = "Nguyen Van A", UrlSlug = "nguyen-van-a", Email = "a@example.com", ImageUrl = "/images/person_1.jpg", JoinedDate = new DateTime(2020, 8, 25, 0, 0, 0), Notes = "Backend expert" },
                new() { FullName = "Tran Thi B", UrlSlug = "tran-thi-b", Email = "b@example.com", ImageUrl = "/images/person_2.jpg", JoinedDate = new DateTime(2020, 12, 30, 0, 0, 0), Notes = "Frontend wizard" },
                new() { FullName = "Le Van C", UrlSlug = "le-van-c", Email = "c@example.com", ImageUrl = "/images/person_3.jpg", JoinedDate = new DateTime(2021, 2, 5, 0, 0, 0), Notes = "Fullstack developer" },
                new() { FullName = "Pham Thi D", UrlSlug = "pham-thi-d", Email = "d@example.com", ImageUrl = "/images/person_4.jpg", JoinedDate = new DateTime(2021, 3, 2, 0, 0, 0), Notes = "DevOps engineer" },
                new() { FullName = "Hoang Van E", UrlSlug = "hoang-van-e", Email = "e@example.com", ImageUrl = "/images/person_5.jpg", JoinedDate = new DateTime(2021, 4, 15, 0, 0, 0), Notes = "Cloud architect" }
            };

            _dbContext.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }

        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() { Name = "Programming", UrlSlug = "programming", Description = "All about coding", ShowOnMenu = true },
                new() { Name = "Database", UrlSlug = "database", Description = "SQL, NoSQL, etc.", ShowOnMenu = true },
                new() { Name = "Web Development", UrlSlug = "web-dev", Description = "Frontend + Backend", ShowOnMenu = true },
                new() { Name = "Cloud", UrlSlug = "cloud", Description = "AWS, Azure, GCP", ShowOnMenu = false },
                new() { Name = "Mobile", UrlSlug = "mobile", Description = "Android & iOS", ShowOnMenu = true },
                new() { Name = "AI & ML", UrlSlug = "ai-ml", Description = "Artificial Intelligence", ShowOnMenu = true },
                new() { Name = "Security", UrlSlug = "security", Description = "Cybersecurity", ShowOnMenu = false },
                new() { Name = "DevOps", UrlSlug = "devops", Description = "CI/CD & Infrastructure", ShowOnMenu = true },
                new() { Name = "Tools", UrlSlug = "tools", Description = "Developer tools", ShowOnMenu = true },
                new() { Name = "Career", UrlSlug = "career", Description = "Career tips", ShowOnMenu = true }
            };

            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }


        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() { Name = "Google", Description = "Google applications and services", UrlSlug = "google" },
                new() { Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp-net-mvc" },
                new() { Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page" },
                new() { Name = "Blazor", Description = "Blazor", UrlSlug = "blazor" },
                new() { Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning" },
                new() { Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network" },
                new() { Name = "Machine Learning", Description = "Machine Learning", UrlSlug = "machine-learning" },
                new() { Name = "Data Science", Description = "Data Science", UrlSlug = "data-science" },
                new() { Name = "C#", Description = "C# programming language", UrlSlug = "csharp" },
                new() { Name = ".NET Core", Description = ".NET Core framework", UrlSlug = "dotnet-core" },
                new() { Name = "Web API", Description = "Web API development", UrlSlug = "web-api" },
                new() { Name = "SQL Server", Description = "SQL Server database", UrlSlug = "sql-server" },
                new() { Name = "Entity Framework", Description = "Entity Framework ORM", UrlSlug = "entity-framework" },
                new() { Name = "LINQ", Description = "Language Integrated Query", UrlSlug = "linq" },
                new() { Name = "JavaScript", Description = "JavaScript programming", UrlSlug = "javascript" },
                new() { Name = "React", Description = "ReactJS framework", UrlSlug = "react" },
                new() { Name = "HTML", Description = "HTML markup language", UrlSlug = "html" },
                new() { Name = "CSS", Description = "Cascading Style Sheets", UrlSlug = "css" },
                new() { Name = "Git", Description = "Version control system", UrlSlug = "git" },
                new() { Name = "Agile", Description = "Agile development methodology", UrlSlug = "agile" }
            };

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }

        private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository",
                    Description = "Here's a few great DON'T and DO examples",
                    Meta = "ASP.NET Core, Diagnostics",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    ImageUrl = "/images/image_1.jpg",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[0] },
                        new PostTag { Tag = tags[1] }
                    }
                },
                new()
                {
                    Title = "Exploring Razor Pages in ASP.NET Core",
                    ShortDescription = "Razor Pages simplify page-focused scenarios",
                    Description = "Learn how Razor Pages streamline UI coding in ASP.NET Core.",
                    Meta = "ASP.NET Core, Razor Pages",
                    UrlSlug = "exploring-razor-pages-aspnet-core",
                    ImageUrl = "/images/image_2.jpg",
                    Published = true,
                    PostedDate = new DateTime(2022, 1, 10, 9, 0, 0),
                    ViewCount = 25,
                    Author = authors[1],
                    Category = categories[2],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[2] },
                        new PostTag { Tag = tags[10] },
                        new PostTag { Tag = tags[11] }
                    }
                },
                new()
                {
                    Title = "Blazor: Full-stack Web Development with C#",
                    ShortDescription = "Blazor is a new framework for building web UIs with C#",
                    Description = "Explore client-side Blazor and server-side Blazor and when to use each.",
                    Meta = "Blazor, C#, Web UI",
                    UrlSlug = "blazor-fullstack-development",
                    ImageUrl = "/images/image_3.jpg",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 5, 14, 30, 0),
                    ViewCount = 40,
                    Author = authors[2],
                    Category = categories[0],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[3] },
                        new PostTag { Tag = tags[8] },
                        new PostTag { Tag = tags[9] }
                    }
                },
                new()
                {
                    Title = "Getting Started with Entity Framework Core",
                    ShortDescription = "EF Core is a modern ORM for .NET developers",
                    Description = "Learn how to use EF Core for database access in ASP.NET Core apps.",
                    Meta = "EF Core, ORM, Data Access",
                    UrlSlug = "getting-started-ef-core",
                    ImageUrl = "/images/image_4.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 5, 20, 8, 15, 0),
                    ViewCount = 60,
                    Author = authors[3],
                    Category = categories[1],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[12] },
                        new PostTag { Tag = tags[13] },
                        new PostTag { Tag = tags[11] }
                    }
                },
                new()
                {
                    Title = "Machine Learning Basics for Developers",
                    ShortDescription = "A quick introduction to ML concepts and tools",
                    Description = "Understand supervised, unsupervised learning, and how to build simple models.",
                    Meta = "ML, AI, Basics",
                    UrlSlug = "machine-learning-basics",
                    ImageUrl = "/images/image_5.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 3, 10, 11, 45, 0),
                    ViewCount = 80,
                    Author = authors[4],
                    Category = categories[5],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[4] },
                        new PostTag { Tag = tags[5] },
                        new PostTag { Tag = tags[6] },
                        new PostTag { Tag = tags[7] }
                    }
                },
                new()
                {
                    Title = "Understanding Middleware in ASP.NET Core",
                    ShortDescription = "Explore how middleware works in the request pipeline",
                    Description = "This article explains the concept of middleware and how to use it in ASP.NET Core applications.",
                    Meta = "ASP.NET Core, Middleware",
                    UrlSlug = "understanding-middleware-aspnet-core",
                    ImageUrl = "/images/image_6.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 6, 5, 10, 0, 0),
                    ViewCount = 55,
                    Author = authors[0],
                    Category = categories[0],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[1] },
                        new PostTag { Tag = tags[10] }
                    }
                },
                new()
                {
                    Title = "Getting Started with Git and GitHub",
                    ShortDescription = "Basic Git commands and working with GitHub repositories",
                    Description = "Learn how to set up Git, create a repository, commit code, and push to GitHub.",
                    Meta = "Git, GitHub, Version Control",
                    UrlSlug = "getting-started-git-github",
                    ImageUrl = "/images/image_7.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 6, 10, 15, 30, 0),
                    ViewCount = 88,
                    Author = authors[1],
                    Category = categories[8],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[18] },
                        new PostTag { Tag = tags[18] },
                        new PostTag { Tag = tags[9] }
                    }
                },
                new()
                {
                    Title = "Designing RESTful APIs with ASP.NET Core",
                    ShortDescription = "Build scalable APIs using ASP.NET Core Web API",
                    Description = "This post walks through creating RESTful services using controllers, routing, and best practices.",
                    Meta = "REST, Web API, ASP.NET Core",
                    UrlSlug = "designing-restful-apis-aspnet-core",
                    ImageUrl = "/images/image_8.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 7, 1, 9, 0, 0),
                    ViewCount = 104,
                    Author = authors[2],
                    Category = categories[2],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[10] },
                        new PostTag { Tag = tags[11] },
                        new PostTag { Tag = tags[0] }
                    }
                },
                new()
                {
                    Title = "Introduction to .NET Core CLI",
                    ShortDescription = "Use the .NET Core command-line interface like a pro",
                    Description = "Learn useful .NET CLI commands for creating, building, and running projects.",
                    Meta = "dotnet CLI, .NET Core, Tools",
                    UrlSlug = "introduction-dotnet-cli",
                    ImageUrl = "/images/image_9.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 7, 15, 8, 45, 0),
                    ViewCount = 72,
                    Author = authors[3],
                    Category = categories[8],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[9] },
                        new PostTag { Tag = tags[17] },
                        new PostTag { Tag = tags[18] } 
                    }
                },
                new()
                {
                    Title = "Deploying ASP.NET Core Apps to Azure",
                    ShortDescription = "Deploy your web apps easily to Microsoft Azure",
                    Description = "A complete guide to deploying web apps using App Services, Azure CLI, and GitHub Actions.",
                    Meta = "Azure, ASP.NET Core, Deployment",
                    UrlSlug = "deploying-aspnetcore-to-azure",
                    ImageUrl = "/images/image_10.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 8, 5, 13, 20, 0),
                    ViewCount = 93,
                    Author = authors[4],
                    Category = categories[3],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[0] }, 
                        new PostTag { Tag = tags[9] }, 
                        new PostTag { Tag = tags[3] } 
                    }
                },
                new()
                {
                    Title = "Understanding LINQ in C#",
                    ShortDescription = "Powerful query syntax for objects and databases",
                    Description = "Learn how to use LINQ for filtering, sorting, and projecting data in C# with real examples.",
                    Meta = "C#, LINQ, Data Queries",
                    UrlSlug = "understanding-linq-csharp",
                    ImageUrl = "/images/image_11.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 8, 15, 9, 0, 0),
                    ViewCount = 120,
                    Author = authors[0],
                    Category = categories[1],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[13] }, 
                        new PostTag { Tag = tags[12] }, 
                        new PostTag { Tag = tags[8] } 
                    }
                },
                new()
                {
                    Title = "Top 10 Visual Studio Code Extensions for Web Devs",
                    ShortDescription = "Make VS Code even more powerful with these tools",
                    Description = "A curated list of extensions to boost your web development productivity in VS Code.",
                    Meta = "VS Code, Extensions, Tools",
                    UrlSlug = "top-vscode-extensions-webdev",
                    ImageUrl = "/images/image_12.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 8, 25, 14, 30, 0),
                    ViewCount = 85,
                    Author = authors[1],
                    Category = categories[8],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[17] }, 
                        new PostTag { Tag = tags[9] }, 
                        new PostTag { Tag = tags[2] } 
                    }
                },
                new()
                {
                    Title = "Building Responsive UI with CSS Grid",
                    ShortDescription = "Leverage CSS Grid to create flexible layouts",
                    Description = "Step-by-step guide to using CSS Grid with practical examples for modern UI design.",
                    Meta = "CSS, UI Design, Frontend",
                    UrlSlug = "responsive-ui-css-grid",
                    ImageUrl = "/images/image_13.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 9, 5, 11, 0, 0),
                    ViewCount = 140,
                    Author = authors[2],
                    Category = categories[2],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[17] }, 
                        new PostTag { Tag = tags[14] }, 
                        new PostTag { Tag = tags[15] } 
                    }
                },
                new()
                {
                    Title = "Securing Web Applications in ASP.NET Core",
                    ShortDescription = "Security best practices every developer must know",
                    Description = "Explore authentication, authorization, and securing APIs in ASP.NET Core apps.",
                    Meta = "Security, ASP.NET Core, Identity",
                    UrlSlug = "securing-web-apps-aspnetcore",
                    ImageUrl = "/images/image_14.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 9, 18, 10, 15, 0),
                    ViewCount = 130,
                    Author = authors[3],
                    Category = categories[6],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[0] }, 
                        new PostTag { Tag = tags[11] }, 
                        new PostTag { Tag = tags[16] } 
                    }
                },
                new()
                {
                    Title = "Unit Testing in .NET with xUnit",
                    ShortDescription = "Ensure code reliability with automated tests",
                    Description = "Learn how to write and run unit tests in .NET projects using xUnit framework.",
                    Meta = "Testing, xUnit, .NET",
                    UrlSlug = "unit-testing-dotnet-xunit",
                    ImageUrl = "/images/image_15.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 10, 2, 9, 45, 0),
                    ViewCount = 95,
                    Author = authors[4],
                    Category = categories[9],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[12] }, 
                        new PostTag { Tag = tags[19] }, 
                        new PostTag { Tag = tags[8] } 
                    }
                },
                new()
                {
                    Title = "What’s New in .NET 8",
                    ShortDescription = "Explore the latest features in the .NET 8 release",
                    Description = "In this article, we cover performance improvements, new APIs, and platform support in .NET 8.",
                    Meta = ".NET 8, Release, New Features",
                    UrlSlug = "whats-new-dotnet-8",
                    ImageUrl = "/images/image_16.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 11, 1, 9, 0, 0),
                    ViewCount = 110,
                    Author = authors[0],
                    Category = categories[0],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[8] }, 
                        new PostTag { Tag = tags[9] }, 
                        new PostTag { Tag = tags[12] } 
                    }
                },
                new()
                {
                    Title = "A Beginner's Guide to HTML & CSS",
                    ShortDescription = "Start your web journey with HTML and CSS",
                    Description = "This post walks absolute beginners through the basics of web page structure and styling.",
                    Meta = "HTML, CSS, Web Basics",
                    UrlSlug = "beginners-guide-html-css",
                    ImageUrl = "/images/image_17.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 11, 10, 15, 0, 0),
                    ViewCount = 70,
                    Author = authors[1],
                    Category = categories[2],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[15] }, 
                        new PostTag { Tag = tags[16] }, 
                        new PostTag { Tag = tags[17] } 
                    }
                },
                new()
                {
                    Title = "How to Use GitHub Actions for CI/CD",
                    ShortDescription = "Automate builds and deployments using GitHub Actions",
                    Description = "A complete walkthrough for setting up workflows to test, build, and deploy your projects.",
                    Meta = "CI/CD, GitHub Actions, DevOps",
                    UrlSlug = "github-actions-cicd",
                    ImageUrl = "/images/image_18.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 11, 20, 10, 30, 0),
                    ViewCount = 135,
                    Author = authors[2],
                    Category = categories[7],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[18] }, 
                        new PostTag { Tag = tags[19] }, 
                        new PostTag { Tag = tags[3] } 
                    }
                },
                new()
                {
                    Title = "Mobile App Development with Flutter",
                    ShortDescription = "Build beautiful cross-platform apps with Flutter",
                    Description = "Learn how Flutter enables fast UI development and native performance for mobile apps.",
                    Meta = "Flutter, Mobile, Cross-Platform",
                    UrlSlug = "mobile-development-flutter",
                    ImageUrl = "/images/image_19.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 12, 1, 14, 45, 0),
                    ViewCount = 122,
                    Author = authors[3],
                    Category = categories[4],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[16] }, 
                        new PostTag { Tag = tags[9] }, 
                        new PostTag { Tag = tags[14] } 
                    }
                },
                new()
                {
                    Title = "Career Tips for Junior Developers",
                    ShortDescription = "Get ahead with these essential career strategies",
                    Description = "Tips on building a portfolio, networking, interviews, and continuous learning.",
                    Meta = "Career, Developer, Tips",
                    UrlSlug = "career-tips-junior-devs",
                    ImageUrl = "/images/image_20.jpg",
                    Published = true,
                    PostedDate = new DateTime(2023, 12, 15, 11, 20, 0),
                    ViewCount = 105,
                    Author = authors[4],
                    Category = categories[9],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[19] }, 
                        new PostTag { Tag = tags[8] }, 
                        new PostTag { Tag = tags[17] } 
                    }
                },
                new()
                {
                    Title = "Building Web APIs with Minimal API in .NET 6+",
                    ShortDescription = "Use the new Minimal API approach to simplify your endpoints",
                    Description = "Learn how to build lightweight APIs using .NET Minimal API with clean and concise syntax.",
                    Meta = "Minimal API, .NET 6, Web API",
                    UrlSlug = "minimal-api-dotnet6",
                    ImageUrl = "/images/image_21.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 1, 5, 10, 0, 0),
                    ViewCount = 98,
                    Author = authors[0],
                    Category = categories[0],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[9] }, 
                        new PostTag { Tag = tags[10] }, 
                        new PostTag { Tag = tags[11] } 
                    }
                },
                new()
                {
                    Title = "Mastering Responsive Design with Flexbox",
                    ShortDescription = "Flexbox makes layouts easier across devices",
                    Description = "Dive into practical use-cases of CSS Flexbox to master responsive UI building techniques.",
                    Meta = "Flexbox, CSS, UI Design",
                    UrlSlug = "responsive-design-flexbox",
                    ImageUrl = "/images/image_22.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 1, 12, 13, 30, 0),
                    ViewCount = 110,
                    Author = authors[1],
                    Category = categories[2],
                    PostTags = new List<PostTag> 
                    {
                        new PostTag { Tag = tags[15] }, 
                        new PostTag { Tag = tags[17] }, 
                        new PostTag { Tag = tags[14] } 
                    }
                },
                new()
                {
                    Title = "Understanding Cloud Computing Models",
                    ShortDescription = "IaaS, PaaS, and SaaS explained",
                    Description = "This article breaks down the three primary cloud service models and when to use each.",
                    Meta = "Cloud, IaaS, PaaS, SaaS",
                    UrlSlug = "cloud-computing-models",
                    ImageUrl = "/images/image_23.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 1, 20, 9, 0, 0),
                    ViewCount = 89,
                    Author = authors[2],
                    Category = categories[3],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[0] }, 
                        new PostTag { Tag = tags[3] }, 
                        new PostTag { Tag = tags[18] } 
                    }
                },
                new()
                {
                    Title = "How to Use Postman for API Testing",
                    ShortDescription = "Postman makes API testing intuitive and powerful",
                    Description = "Learn the basics of creating collections, sending requests, and writing tests with Postman.",
                    Meta = "Postman, API, Testing",
                    UrlSlug = "using-postman-api-testing",
                    ImageUrl = "/images/image_24.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 2, 1, 14, 10, 0),
                    ViewCount = 114,
                    Author = authors[3],
                    Category = categories[6],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[11] }, 
                        new PostTag { Tag = tags[12] }, 
                        new PostTag { Tag = tags[19] } 
                    }
                },
                new()
                {
                    Title = "Debugging Tips in Visual Studio",
                    ShortDescription = "Be a more efficient debugger with these tricks",
                    Description = "Set breakpoints, use watches, run to cursor, and leverage diagnostic tools in Visual Studio.",
                    Meta = "Visual Studio, Debugging, Tools",
                    UrlSlug = "debugging-tips-visual-studio",
                    ImageUrl = "/images/image_25.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 2, 10, 11, 30, 0),
                    ViewCount = 130,
                    Author = authors[4],
                    Category = categories[8],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[8] }, 
                        new PostTag { Tag = tags[17] }, 
                        new PostTag { Tag = tags[19] } 
                    }
                },
                new()
                {
                    Title = "Optimizing SQL Queries for Better Performance",
                    ShortDescription = "Make your SQL run faster with indexing and analysis",
                    Description = "Learn about indexing strategies, query plans, and SQL optimization techniques.",
                    Meta = "SQL, Performance, Database",
                    UrlSlug = "optimizing-sql-queries",
                    ImageUrl = "/images/image_26.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 2, 20, 9, 0, 0),
                    ViewCount = 150,
                    Author = authors[0],
                    Category = categories[1],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[11] }, 
                        new PostTag { Tag = tags[12] }, 
                        new PostTag { Tag = tags[13] } 
                    }
                },
                new()
                {
                    Title = "An Introduction to Agile Development",
                    ShortDescription = "Understand the basics of Agile and Scrum",
                    Description = "This article covers key Agile principles, ceremonies, and team roles using real-life examples.",
                    Meta = "Agile, Scrum, Dev Process",
                    UrlSlug = "intro-agile-development",
                    ImageUrl = "/images/image_27.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 3, 1, 10, 30, 0),
                    ViewCount = 87,
                    Author = authors[1],
                    Category = categories[7],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[19] }, 
                        new PostTag { Tag = tags[8] }, 
                        new PostTag { Tag = tags[18] } 
                    }
                },
                new()
                {
                    Title = "Creating Interactive Charts with JavaScript",
                    ShortDescription = "Add dynamic charts to your web apps using Chart.js",
                    Description = "Learn how to display data using bar, pie, and line charts with Chart.js and custom options.",
                    Meta = "JavaScript, Chart.js, Data Viz",
                    UrlSlug = "interactive-charts-javascript",
                    ImageUrl = "/images/image_28.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 3, 10, 15, 0, 0),
                    ViewCount = 95,
                    Author = authors[2],
                    Category = categories[2],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[14] }, 
                        new PostTag { Tag = tags[15] }, 
                        new PostTag { Tag = tags[17] } 
                    }
                },
                new()
                {
                    Title = "Using SignalR for Real-time Web Apps",
                    ShortDescription = "Add live chat and push updates with SignalR",
                    Description = "Explore how to build real-time features using SignalR in ASP.NET Core applications.",
                    Meta = "SignalR, Real-time, ASP.NET",
                    UrlSlug = "real-time-apps-signalr",
                    ImageUrl = "/images/image_29.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 3, 20, 14, 45, 0),
                    ViewCount = 125,
                    Author = authors[3],
                    Category = categories[0],
                    PostTags = new List<PostTag> 
                    { 
                        new PostTag { Tag = tags[0] }, 
                        new PostTag { Tag = tags[3] }, 
                        new PostTag { Tag = tags[10] } 
                    }
                },
                new()
                {
                    Title = "Getting Started with Machine Learning in Python",
                    ShortDescription = "Begin your ML journey with Python libraries",
                    Description = "Set up your environment, explore datasets, and train simple models with scikit-learn.",
                    Meta = "Python, ML, scikit-learn",
                    UrlSlug = "ml-basics-python",
                    ImageUrl = "/images/image_30.jpg",
                    Published = true,
                    PostedDate = new DateTime(2024, 3, 30, 10, 0, 0),
                    ViewCount = 135,
                    Author = authors[4],
                    Category = categories[5],
                    PostTags = new List<PostTag>
                    {
                        new PostTag { Tag = tags[4] },
                        new PostTag { Tag = tags[5] },
                        new PostTag { Tag = tags[6] },
                        new PostTag { Tag = tags[7] }
                    }
                }
            };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }
    }
}
