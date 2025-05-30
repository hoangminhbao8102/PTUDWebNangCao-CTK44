USE [TatBlog]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 22/05/2025 09:02:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[UrlSlug] [nvarchar](100) NOT NULL,
	[ImageUrl] [nvarchar](150) NULL,
	[JoinedDate] [datetime] NOT NULL,
	[Email] [nvarchar](150) NULL,
	[Notes] [nvarchar](500) NULL,
 CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UrlSlug] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ShowOnMenu] [bit] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[PostedDate] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[PostId] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Subject] [nvarchar](200) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SentDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[ShortDescription] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Meta] [nvarchar](1000) NOT NULL,
	[UrlSlug] [nvarchar](200) NOT NULL,
	[ImageUrl] [nvarchar](1000) NULL,
	[ViewCount] [int] NOT NULL,
	[Published] [bit] NOT NULL,
	[PostedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CategoryId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostTags]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostTags](
	[PostId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_PostTags] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscribers]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscribers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[SubscribedDate] [datetime] NOT NULL,
	[UnsubscribedDate] [datetime] NULL,
	[UnsubscribeReason] [nvarchar](1000) NULL,
	[Involuntary] [bit] NULL,
	[AdminNotes] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Subscribers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 22/05/2025 09:02:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UrlSlug] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250403005530_InitialCreate', N'9.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250403081043_AddSubscribersTable', N'9.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250403090458_AddCommentsTable', N'9.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250421090150_AddFeedback', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250521021727_AddPostTagTable', N'9.0.5')
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([Id], [FullName], [UrlSlug], [ImageUrl], [JoinedDate], [Email], [Notes]) VALUES (1, N'Nguyen Van A', N'nguyen-van-a', N'/images/a.jpg', CAST(N'2024-04-03T11:25:08.800' AS DateTime), N'a@example.com', N'Backend expert')
INSERT [dbo].[Authors] ([Id], [FullName], [UrlSlug], [ImageUrl], [JoinedDate], [Email], [Notes]) VALUES (2, N'Tran Thi B', N'tran-thi-b', N'/images/b.jpg', CAST(N'2024-06-03T11:25:08.803' AS DateTime), N'b@example.com', N'Frontend wizard')
INSERT [dbo].[Authors] ([Id], [FullName], [UrlSlug], [ImageUrl], [JoinedDate], [Email], [Notes]) VALUES (3, N'Le Van C', N'le-van-c', N'/images/c.jpg', CAST(N'2024-08-03T11:25:08.803' AS DateTime), N'c@example.com', N'Fullstack developer')
INSERT [dbo].[Authors] ([Id], [FullName], [UrlSlug], [ImageUrl], [JoinedDate], [Email], [Notes]) VALUES (4, N'Pham Thi D', N'pham-thi-d', N'/images/d.jpg', CAST(N'2024-10-03T11:25:08.803' AS DateTime), N'd@example.com', N'DevOps engineer')
INSERT [dbo].[Authors] ([Id], [FullName], [UrlSlug], [ImageUrl], [JoinedDate], [Email], [Notes]) VALUES (5, N'Hoang Van E', N'hoang-van-e', N'/images/e.jpg', CAST(N'2024-12-03T11:25:08.803' AS DateTime), N'e@example.com', N'Cloud architect')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (1, N'Programming', N'programming', N'All about coding', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (2, N'Database', N'database', N'SQL, NoSQL, etc.', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (3, N'Web Development', N'web-dev', N'Frontend + Backend', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (4, N'Cloud', N'cloud', N'AWS, Azure, GCP', 0)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (5, N'Mobile', N'mobile', N'Android & iOS', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (6, N'AI & ML', N'ai-ml', N'Artificial Intelligence', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (7, N'Security', N'security', N'Cybersecurity', 0)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (8, N'DevOps', N'devops', N'CI/CD & Infrastructure', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (9, N'Tools', N'tools', N'Developer tools', 1)
INSERT [dbo].[Categories] ([Id], [Name], [UrlSlug], [Description], [ShowOnMenu]) VALUES (10, N'Career', N'career', N'Career tips', 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([Id], [AuthorName], [Email], [Content], [PostedDate], [IsApproved], [PostId]) VALUES (1, N'Nguyễn Văn A', N'nva@example.com', N'Bài viết rất hữu ích!', CAST(N'2025-05-20T17:23:37.847' AS DateTime), 0, 5)
INSERT [dbo].[Comments] ([Id], [AuthorName], [Email], [Content], [PostedDate], [IsApproved], [PostId]) VALUES (2, N'Trần Bảo Minh', N'minh@example.com', N'Cập nhật nội dung bình luận', CAST(N'2025-05-20T17:24:40.017' AS DateTime), 1, 3)
SET IDENTITY_INSERT [dbo].[Comments] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedbacks] ON 

INSERT [dbo].[Feedbacks] ([Id], [FullName], [Email], [Subject], [Message], [SentDate]) VALUES (1, N'Nguyễn Văn Anh', N'vananh@example.com', N'Góp ý giao diện', N'Trang chủ nên có màu sắc dễ nhìn hơn.', CAST(N'2025-05-20T10:27:57.347' AS DateTime))
SET IDENTITY_INSERT [dbo].[Feedbacks] OFF
GO
SET IDENTITY_INSERT [dbo].[Posts] ON 

INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (1, N'ASP.NET Core Diagnostic Scenarios', N'David and friends has a great repository', N'Here''s a few great DON''T and DO examples', N'ASP.NET Core, Diagnostics', N'aspnet-core-diagnostic-scenarios', NULL, 11, 1, CAST(N'2021-09-30T10:20:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (2, N'Exploring Razor Pages in ASP.NET Core', N'Razor Pages simplify page-focused scenarios', N'Learn how Razor Pages streamline UI coding in ASP.NET Core.', N'ASP.NET Core, Razor Pages', N'exploring-razor-pages-aspnet-core', NULL, 25, 1, CAST(N'2022-01-10T09:00:00.000' AS DateTime), NULL, 3, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (3, N'Blazor: Full-stack Web Development with C#', N'Blazor is a new framework for building web UIs with C#', N'Explore client-side Blazor and server-side Blazor and when to use each.', N'Blazor, C#, Web UI', N'blazor-fullstack-development', NULL, 40, 1, CAST(N'2022-02-05T14:30:00.000' AS DateTime), NULL, 1, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (4, N'Getting Started with Entity Framework Core', N'EF Core is a modern ORM for .NET developers', N'Learn how to use EF Core for database access in ASP.NET Core apps.', N'EF Core, ORM, Data Access', N'getting-started-ef-core', NULL, 60, 1, CAST(N'2023-05-20T08:15:00.000' AS DateTime), NULL, 2, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (5, N'Machine Learning Basics for Developers', N'A quick introduction to ML concepts and tools', N'Understand supervised, unsupervised learning, and how to build simple models.', N'ML, AI, Basics', N'machine-learning-basics', NULL, 80, 1, CAST(N'2024-03-10T11:45:00.000' AS DateTime), NULL, 6, 5)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (6, N'Understanding Middleware in ASP.NET Core', N'Explore how middleware works in the request pipeline', N'This article explains the concept of middleware and how to use it in ASP.NET Core applications.', N'ASP.NET Core, Middleware', N'understanding-middleware-aspnet-core', NULL, 55, 1, CAST(N'2023-06-05T10:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (7, N'Getting Started with Git and GitHub', N'Basic Git commands and working with GitHub repositories', N'Learn how to set up Git, create a repository, commit code, and push to GitHub.', N'Git, GitHub, Version Control', N'getting-started-git-github', NULL, 88, 1, CAST(N'2023-06-10T15:30:00.000' AS DateTime), NULL, 9, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (8, N'Designing RESTful APIs with ASP.NET Core', N'Build scalable APIs using ASP.NET Core Web API', N'This post walks through creating RESTful services using controllers, routing, and best practices.', N'REST, Web API, ASP.NET Core', N'designing-restful-apis-aspnet-core', NULL, 104, 1, CAST(N'2023-07-01T09:00:00.000' AS DateTime), NULL, 3, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (9, N'Introduction to .NET Core CLI', N'Use the .NET Core command-line interface like a pro', N'Learn useful .NET CLI commands for creating, building, and running projects.', N'dotnet CLI, .NET Core, Tools', N'introduction-dotnet-cli', NULL, 72, 1, CAST(N'2023-07-15T08:45:00.000' AS DateTime), NULL, 9, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (10, N'Deploying ASP.NET Core Apps to Azure', N'Deploy your web apps easily to Microsoft Azure', N'A complete guide to deploying web apps using App Services, Azure CLI, and GitHub Actions.', N'Azure, ASP.NET Core, Deployment', N'deploying-aspnetcore-to-azure', NULL, 93, 1, CAST(N'2023-08-05T13:20:00.000' AS DateTime), NULL, 4, 5)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (11, N'Understanding LINQ in C#', N'Powerful query syntax for objects and databases', N'Learn how to use LINQ for filtering, sorting, and projecting data in C# with real examples.', N'C#, LINQ, Data Queries', N'understanding-linq-csharp', NULL, 120, 1, CAST(N'2023-08-15T09:00:00.000' AS DateTime), NULL, 2, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (12, N'Top 10 Visual Studio Code Extensions for Web Devs', N'Make VS Code even more powerful with these tools', N'A curated list of extensions to boost your web development productivity in VS Code.', N'VS Code, Extensions, Tools', N'top-vscode-extensions-webdev', NULL, 85, 1, CAST(N'2023-08-25T14:30:00.000' AS DateTime), NULL, 9, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (13, N'Building Responsive UI with CSS Grid', N'Leverage CSS Grid to create flexible layouts', N'Step-by-step guide to using CSS Grid with practical examples for modern UI design.', N'CSS, UI Design, Frontend', N'responsive-ui-css-grid', NULL, 140, 1, CAST(N'2023-09-05T11:00:00.000' AS DateTime), NULL, 3, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (14, N'Securing Web Applications in ASP.NET Core', N'Security best practices every developer must know', N'Explore authentication, authorization, and securing APIs in ASP.NET Core apps.', N'Security, ASP.NET Core, Identity', N'securing-web-apps-aspnetcore', NULL, 130, 1, CAST(N'2023-09-18T10:15:00.000' AS DateTime), NULL, 7, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (15, N'Unit Testing in .NET with xUnit', N'Ensure code reliability with automated tests', N'Learn how to write and run unit tests in .NET projects using xUnit framework.', N'Testing, xUnit, .NET', N'unit-testing-dotnet-xunit', NULL, 95, 1, CAST(N'2023-10-02T09:45:00.000' AS DateTime), NULL, 10, 5)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (16, N'What’s New in .NET 8', N'Explore the latest features in the .NET 8 release', N'In this article, we cover performance improvements, new APIs, and platform support in .NET 8.', N'.NET 8, Release, New Features', N'whats-new-dotnet-8', NULL, 110, 1, CAST(N'2023-11-01T09:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (17, N'A Beginner''s Guide to HTML & CSS', N'Start your web journey with HTML and CSS', N'This post walks absolute beginners through the basics of web page structure and styling.', N'HTML, CSS, Web Basics', N'beginners-guide-html-css', NULL, 70, 1, CAST(N'2023-11-10T15:00:00.000' AS DateTime), NULL, 3, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (18, N'How to Use GitHub Actions for CI/CD', N'Automate builds and deployments using GitHub Actions', N'A complete walkthrough for setting up workflows to test, build, and deploy your projects.', N'CI/CD, GitHub Actions, DevOps', N'github-actions-cicd', NULL, 135, 1, CAST(N'2023-11-20T10:30:00.000' AS DateTime), NULL, 8, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (19, N'Mobile App Development with Flutter', N'Build beautiful cross-platform apps with Flutter', N'Learn how Flutter enables fast UI development and native performance for mobile apps.', N'Flutter, Mobile, Cross-Platform', N'mobile-development-flutter', NULL, 122, 1, CAST(N'2023-12-01T14:45:00.000' AS DateTime), NULL, 5, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (20, N'Career Tips for Junior Developers', N'Get ahead with these essential career strategies', N'Tips on building a portfolio, networking, interviews, and continuous learning.', N'Career, Developer, Tips', N'career-tips-junior-devs', NULL, 105, 1, CAST(N'2023-12-15T11:20:00.000' AS DateTime), NULL, 10, 5)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (21, N'Building Web APIs with Minimal API in .NET 6+', N'Use the new Minimal API approach to simplify your endpoints', N'Learn how to build lightweight APIs using .NET Minimal API with clean and concise syntax.', N'Minimal API, .NET 6, Web API', N'minimal-api-dotnet6', NULL, 98, 1, CAST(N'2024-01-05T10:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (22, N'Mastering Responsive Design with Flexbox', N'Flexbox makes layouts easier across devices', N'Dive into practical use-cases of CSS Flexbox to master responsive UI building techniques.', N'Flexbox, CSS, UI Design', N'responsive-design-flexbox', NULL, 110, 1, CAST(N'2024-01-12T13:30:00.000' AS DateTime), NULL, 3, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (23, N'Understanding Cloud Computing Models', N'IaaS, PaaS, and SaaS explained', N'This article breaks down the three primary cloud service models and when to use each.', N'Cloud, IaaS, PaaS, SaaS', N'cloud-computing-models', NULL, 89, 1, CAST(N'2024-01-20T09:00:00.000' AS DateTime), NULL, 4, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (24, N'How to Use Postman for API Testing', N'Postman makes API testing intuitive and powerful', N'Learn the basics of creating collections, sending requests, and writing tests with Postman.', N'Postman, API, Testing', N'using-postman-api-testing', NULL, 114, 1, CAST(N'2024-02-01T14:10:00.000' AS DateTime), NULL, 7, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (25, N'Debugging Tips in Visual Studio', N'Be a more efficient debugger with these tricks', N'Set breakpoints, use watches, run to cursor, and leverage diagnostic tools in Visual Studio.', N'Visual Studio, Debugging, Tools', N'debugging-tips-visual-studio', NULL, 130, 1, CAST(N'2024-02-10T11:30:00.000' AS DateTime), NULL, 9, 5)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (26, N'Optimizing SQL Queries for Better Performance', N'Make your SQL run faster with indexing and analysis', N'Learn about indexing strategies, query plans, and SQL optimization techniques.', N'SQL, Performance, Database', N'optimizing-sql-queries', NULL, 150, 1, CAST(N'2024-02-20T09:00:00.000' AS DateTime), NULL, 2, 1)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (27, N'An Introduction to Agile Development', N'Understand the basics of Agile and Scrum', N'This article covers key Agile principles, ceremonies, and team roles using real-life examples.', N'Agile, Scrum, Dev Process', N'intro-agile-development', NULL, 87, 1, CAST(N'2024-03-01T10:30:00.000' AS DateTime), NULL, 8, 2)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (28, N'Creating Interactive Charts with JavaScript', N'Add dynamic charts to your web apps using Chart.js', N'Learn how to display data using bar, pie, and line charts with Chart.js and custom options.', N'JavaScript, Chart.js, Data Viz', N'interactive-charts-javascript', NULL, 95, 1, CAST(N'2024-03-10T15:00:00.000' AS DateTime), NULL, 3, 3)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (29, N'Using SignalR for Real-time Web Apps', N'Add live chat and push updates with SignalR', N'Explore how to build real-time features using SignalR in ASP.NET Core applications.', N'SignalR, Real-time, ASP.NET', N'real-time-apps-signalr', NULL, 125, 1, CAST(N'2024-03-20T14:45:00.000' AS DateTime), NULL, 1, 4)
INSERT [dbo].[Posts] ([Id], [Title], [ShortDescription], [Description], [Meta], [UrlSlug], [ImageUrl], [ViewCount], [Published], [PostedDate], [ModifiedDate], [CategoryId], [AuthorId]) VALUES (30, N'Getting Started with Machine Learning in Python', N'ml-basics-python', N'Begin your ML journey with Python libraries', N'Deep Learning, Neural Network, Machine Learning, Data Science', N'getting-started-with-machine-learning-in-python', N'https://localhost:7239/uploads/pictures/b5c75adb3cce481488a62cd22e126e6f.jpg', 137, 1, CAST(N'2024-03-30T10:00:00.000' AS DateTime), CAST(N'2025-05-20T08:10:02.797' AS DateTime), 6, 4)
SET IDENTITY_INSERT [dbo].[Posts] OFF
GO
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (1, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (8, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (10, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (14, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (23, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (29, 1)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (1, 2)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (6, 2)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (2, 3)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (12, 3)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (3, 4)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (10, 4)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (18, 4)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (23, 4)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (29, 4)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (5, 5)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (5, 6)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (5, 7)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (5, 8)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (3, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (7, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (11, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (15, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (16, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (20, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (25, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (27, 9)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (3, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (7, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (9, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (10, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (12, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (16, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (19, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (21, 10)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (2, 11)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (6, 11)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (8, 11)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (21, 11)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (29, 11)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (2, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (4, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (8, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (14, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (21, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (24, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (26, 12)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (4, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (11, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (15, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (16, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (24, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (26, 13)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (4, 14)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (11, 14)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (26, 14)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (13, 15)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (19, 15)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (22, 15)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (28, 15)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (13, 16)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (17, 16)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (22, 16)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (28, 16)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (14, 17)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (17, 17)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (19, 17)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (9, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (12, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (13, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (17, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (20, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (22, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (25, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (28, 18)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (7, 19)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (9, 19)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (18, 19)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (23, 19)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (27, 19)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (15, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (18, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (20, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (24, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (25, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (27, 20)
INSERT [dbo].[PostTags] ([PostId], [TagId]) VALUES (30, 22)
GO
SET IDENTITY_INSERT [dbo].[Subscribers] ON 

INSERT [dbo].[Subscribers] ([Id], [Email], [SubscribedDate], [UnsubscribedDate], [UnsubscribeReason], [Involuntary], [AdminNotes]) VALUES (1, N'minhbao8102@gmail.com', CAST(N'2025-04-09T10:36:23.670' AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Subscribers] ([Id], [Email], [SubscribedDate], [UnsubscribedDate], [UnsubscribeReason], [Involuntary], [AdminNotes]) VALUES (2, N'example@gmail.com', CAST(N'2025-05-21T02:44:56.507' AS DateTime), CAST(N'2025-05-21T02:45:23.787' AS DateTime), N'I receive too many emails', 0, NULL)
SET IDENTITY_INSERT [dbo].[Subscribers] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (1, N'Google', N'google', N'Google applications and services')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (2, N'ASP.NET MVC', N'asp-net-mvc', N'ASP.NET MVC')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (3, N'Razor Page', N'razor-page', N'Razor Page')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (4, N'Blazor', N'blazor', N'Blazor')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (5, N'Deep Learning', N'deep-learning', N'Deep Learning')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (6, N'Neural Network', N'neural-network', N'Neural Network')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (7, N'Machine Learning', N'machine-learning', N'Machine Learning')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (8, N'Data Science', N'data-science', N'Data Science')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (9, N'C#', N'csharp', N'C# programming language')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (10, N'.NET Core', N'dotnet-core', N'.NET Core framework')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (11, N'Web API', N'web-api', N'Web API development')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (12, N'SQL Server', N'sql-server', N'SQL Server database')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (13, N'Entity Framework', N'entity-framework', N'Entity Framework ORM')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (14, N'LINQ', N'linq', N'Language Integrated Query')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (15, N'JavaScript', N'javascript', N'JavaScript programming')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (16, N'React', N'react', N'ReactJS framework')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (17, N'HTML', N'html', N'HTML markup language')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (18, N'CSS', N'css', N'Cascading Style Sheets')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (19, N'Git', N'git', N'Version control system')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (20, N'Agile', N'agile', N'Agile development methodology')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (22, N'string', N'string', N'string')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (23, N'aspnetcore', N'aspnetcore', N'aspnetcore')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (24, N'webapi', N'webapi', N'webapi')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (25, N'hook', N'hook', N'hook')
INSERT [dbo].[Tags] ([Id], [Name], [UrlSlug], [Description]) VALUES (26, N'useeffect', N'useeffect', N'useeffect')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [ShowOnMenu]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsApproved]
GO
ALTER TABLE [dbo].[Posts] ADD  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[Posts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Published]
GO
ALTER TABLE [dbo].[Subscribers] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Involuntary]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Comments] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Posts_Comments]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Authors] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Authors]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Categories]
GO
ALTER TABLE [dbo].[PostTags]  WITH CHECK ADD  CONSTRAINT [FK_PostTags_Posts_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostTags] CHECK CONSTRAINT [FK_PostTags_Posts_PostId]
GO
ALTER TABLE [dbo].[PostTags]  WITH CHECK ADD  CONSTRAINT [FK_PostTags_Tags_TagId] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostTags] CHECK CONSTRAINT [FK_PostTags_Tags_TagId]
GO
