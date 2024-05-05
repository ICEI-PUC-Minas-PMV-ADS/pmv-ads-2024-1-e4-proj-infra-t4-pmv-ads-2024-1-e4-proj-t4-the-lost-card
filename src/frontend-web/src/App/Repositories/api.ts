import axios from 'axios';

const api = axios.create({
  baseURL: `${import.meta.env.VITE_LOST_CARDS_API_BASEURL}/api`
});

api.interceptors.request.use((config) => {
  if (config.headers.Authorization === undefined)
    {
      const token = localStorage.getItem("token")!;
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

export default api;