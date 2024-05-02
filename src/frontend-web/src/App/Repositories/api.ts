import axios from 'axios';

const url = 'https://lost-cards.azurewebsites.net';

const api = axios.create({
  baseURL: `${url}/api`
});

export default api;