import React, { useEffect, useState } from "react";
import { get_api } from "../../Services/Methods";

const Index = () => {
    const [stats, setStats] = useState({});

    useEffect(() => {
        document.title = "Bảng điều khiển";
        get_api("https://localhost:7239/api/dashboard").then(data => {
            if (data) setStats(data);
        });
    }, []);

    return (
        <div>
            <h1>Bảng điều khiển</h1>
            <ul>
                <li>Tổng số bài viết: {stats.totalPosts}</li>
                <li>Bài viết chưa xuất bản: {stats.unpublishedPosts}</li>
                <li>Số lượng chủ đề: {stats.totalCategories}</li>
                <li>Số lượng tác giả: {stats.totalAuthors}</li>
                <li>Bình luận đang chờ phê duyệt: {stats.pendingComments}</li>
                <li>Tổng số người đăng ký: {stats.totalSubscribers}</li>
                <li>Người đăng ký mới hôm nay: {stats.newSubscribersToday}</li>
            </ul>
        </div>
    );
}

export default Index;