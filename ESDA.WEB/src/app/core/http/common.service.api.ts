import axios from 'axios';

const headers = {
  Accept: 'application/json',
  'Content-Type': 'application/json',
  //Authorization: `bearer ${localStorage.getItem('id_token')}`,
};

export const get = async (baseUrl: string, queryUrl: string) => {
  const response: any = axios.get(`${baseUrl}${queryUrl}`);

  if (response.status === 200) {
    const json = await response.data.result;
    return { data: json };
  }
  if (response.status === 401) {
    alert('UnAuthorise');
  }
  throw new Error('Error');
};

export const post = async (baseUrl: string, queryUrl: string, model: any) => {
  axios.defaults.headers.post['Content-Type'] = 'Application/json';

  const response = await axios.post(`${baseUrl}${queryUrl}`, model);
  if (response.status === 200) {
    const json = response;
    return { data: json };
  }
  if (response.status === 500) {
  }
  throw new Error('Error');
};

export const deleteItem = async (baseUrl: string, queryUrl: string) => {
  axios.defaults.headers.post['Content-Type'] = 'Application/json';

  const response = await axios.delete(`${baseUrl}${queryUrl}`);
  if (response.status === 200) {
    const json = response;
    return { data: json };
  }
  throw new Error('Error');
};

export const loginPost = async (
  baseUrl: string,
  queryUrl: string,
  model: any
) => {
  axios.defaults.headers.post['Content-Type'] =
    'application/x-www-form-urlencoded';
  debugger;
  const response = await axios.post(`${baseUrl}${queryUrl}`, model);
  if (response.status === 200) {
    const json = response;
    return { data: json };
  }
  if (response.status === 500) {
  }
  throw new Error('Error');
};
