import React, { useEffect, useState } from 'react';
import { decode, isEmptyOrSpaces, isInteger } from '../../../Utils/Utils';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { Link, useParams, Navigate } from 'react-router-dom';
import { addOrUpdatePost, getFilter, getPostById } from '../../../Services/BlogRepository';

const initialState = {
    id: 0,
    title: "",
    shortDescription: "",
    description: "",
    urlSlug: "",
    meta: "",
    imageUrl: "",
    category: {},
    author: {},
    tags: [],
    selectedTags: "",
    published: false,
};

const Edit = () => {
    const [validated, setValidated] = useState(false);
    const [post, setPost] = useState(initialState);
    const [filter, setFilter] = useState({
        authorList: [],
        categoryList: [],
    });

    let { id } = useParams();
    id = id ?? 0;

    useEffect(() => {
        document.title = "Thêm/cập nhật bài viết";

        getPostById(id).then(data => {
            if (data) {
                setPost({
                    ...data,
                    selectedTags: data.tags.map(tag => tag?.name).join("\r\n")
                });
            } else {
                setPost(initialState);
            }
        });

        getFilter().then(data => {
            if (data) {
                setFilter({
                    authorList: data.authorList,
                    categoryList: data.categoryList,
                });
            }
        });
    }, [id]);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (e.currentTarget.checkValidity() === false) {
            e.stopPropagation();
            setValidated(true);
        } else {
            let form = new FormData(e.target);

            // Log để kiểm tra dữ liệu gửi đi nếu cần
            // for (let [key, val] of form.entries()) console.log(key, val);

            addOrUpdatePost(form).then(data => {
                if (data) {
                    alert("Đã lưu thành công!");
                } else {
                    alert("Đã xảy ra lỗi!");
                }
            });
        }
    };

    if (id && !isInteger(id)) {
        return <Navigate to={`/400?redirectTo=/admin/posts`} />
    }

    return (
        <>
            <Form method='post' encType='multipart/form-data' onSubmit={handleSubmit} noValidate validated={validated}>
                <Form.Control type="hidden" name="id" value={post.id} />
                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Tiêu đề</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control type="text" name="title" required value={post.title || ''} onChange={e => setPost({ ...post, title: e.target.value })} />
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Slug</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control type="text" name="urlSlug" value={post.urlSlug || ''} onChange={e => setPost({ ...post, urlSlug: e.target.value })} />
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Giới thiệu</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control as="textarea" required name="shortDescription" value={decode(post.shortDescription || '')} onChange={e => setPost({ ...post, shortDescription: e.target.value })} />
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Nội dung</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control as="textarea" rows={10} required name="description" value={decode(post.description || '')} onChange={e => setPost({ ...post, description: e.target.value })} />
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Metadata</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control type="text" name="meta" required value={decode(post.meta || '')} onChange={e => setPost({ ...post, meta: e.target.value })} />
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Tác giả</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Select name="authorId" value={post.author?.id || ""} required onChange={e => setPost({ ...post, author: { id: parseInt(e.target.value) } })}>
                            <option value="">-- Chọn tác giả --</option>
                            {filter.authorList.map((item, index) =>
                                <option key={index} value={item.value}>{item.text}</option>
                            )}
                        </Form.Select>
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Chủ đề</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Select name="categoryId" value={post.category?.id || ""} required onChange={e => setPost({ ...post, category: { id: parseInt(e.target.value) } })}>
                            <option value="">-- Chọn chủ đề --</option>
                            {filter.categoryList.map((item, index) =>
                                <option key={index} value={item.value}>{item.text}</option>
                            )}
                        </Form.Select>
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Từ khóa (mỗi từ 1 dòng)</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control as="textarea" name="selectedTags" required value={post.selectedTags} onChange={e => setPost({ ...post, selectedTags: e.target.value })} />
                        <Form.Control.Feedback type="invalid">Không được bỏ trống</Form.Control.Feedback>
                    </div>
                </div>

                {!isEmptyOrSpaces(post.imageUrl) && <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Hình hiện tại</Form.Label>
                    <div className='col-sm-10'>
                        <img src={post.imageUrl} alt={post.title} style={{ maxHeight: '200px' }} />
                    </div>
                </div>}

                <div className='row mb-3'>
                    <Form.Label className='col-sm-2 col-form-label'>Chọn hình ảnh</Form.Label>
                    <div className='col-sm-10'>
                        <Form.Control type="file" name="imageFile" accept='image/*' />
                    </div>
                </div>

                <div className='row mb-3'>
                    <div className='col-sm-10 offset-sm-2'>
                        <div className='form-check'>
                            <input className='form-check-input' type="checkbox" name="published" checked={post.published} onChange={e => setPost({ ...post, published: e.target.checked })} />
                            <Form.Label className='form-check-label'>Đã xuất bản</Form.Label>
                        </div>
                    </div>
                </div>

                <div className='text-center'>
                    <Button variant="primary" type="submit">Lưu các thay đổi</Button>
                    <Link to="/admin/posts" className='btn btn-danger ms-2'>Hủy và quay lại</Link>
                </div>
            </Form>
        </>
    );
};

export default Edit;
