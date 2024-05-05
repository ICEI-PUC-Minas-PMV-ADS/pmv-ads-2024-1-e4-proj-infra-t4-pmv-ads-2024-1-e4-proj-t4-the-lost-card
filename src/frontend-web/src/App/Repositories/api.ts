import axios from 'axios';

const api = axios.create({
  baseURL: `${import.meta.env.VITE_LOST_CARDS_API_BASEURL}/api`
});

export default api;