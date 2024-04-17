import axios from 'axios';

const url = 'http://localhost:7097';

const api = axios.create({
  baseURL: `${url}/api`
});

export default api;