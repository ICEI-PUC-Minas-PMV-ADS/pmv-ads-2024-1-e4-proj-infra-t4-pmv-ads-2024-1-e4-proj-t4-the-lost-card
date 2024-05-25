import axios from 'axios';

const api = axios.create({
  baseURL: `https://lost-cards-devfunc.azurewebsites.net/api`,
});

export default api;
