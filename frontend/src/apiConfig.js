const BASE_URL = process.env.REACT_APP_BACKEND_HOST;

export const customFetch = async (endpoint, options = {}) => {
  try {
    const response = await fetch(`${BASE_URL}${endpoint}`, options);

    if (!response.ok) {
      console.error(`Failed request to ${endpoint}`);
    }

    return response;
  } catch (error) {
    console.error(`Error in request to ${endpoint}:`, error);
    throw error;
  }
};