import axios from 'axios';

axios.defaults.baseURL = 'https://localhost:7114/api';
axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;

export default axios;
