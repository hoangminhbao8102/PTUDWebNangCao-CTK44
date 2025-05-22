import axios from "axios";

export async function get_api(your_api) {
  try {
    const response = await axios.get(your_api);
    const data = response.data;
    if (data.isSuccess) {
        return data.result;
    } else {
        return null;
    }
  } catch (error) {
    console.error("Error", error);
    throw error;
  }
}

export async function post_api(your_api) {
  try {
    const response = await axios.post(your_api);
    const data = response.data;
    if (data.isSuccess) {
        return data.result;
    } else {
        return null;
    }
  } catch (error) {
    console.error("Error", error);
    throw error;
  }
}

export async function put_api(your_api, payload) {
  try {
    const response = await axios.put(your_api, payload);
    const data = response.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    console.error("Error", error);
    throw error;
  }
}

export async function delete_api(your_api) {
  try {
    const response = await axios.delete(your_api);
    const data = response.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    console.error("Error", error);
    throw error;
  }
}