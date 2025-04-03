﻿using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICommentRepository
    {
        Task<IList<Comment>> GetCommentsAsync(int postId, bool approvedOnly = true, CancellationToken cancellationToken = default);
        
        Task<IList<Comment>> GetPendingCommentsAsync(CancellationToken cancellationToken = default);
        
        Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default );
        
        Task AddCommentAsync(Comment comment, CancellationToken cancellationToken = default);
        
        Task<bool> ApproveCommentAsync(int id, CancellationToken cancellationToken = default);
        
        Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);
    }
}
