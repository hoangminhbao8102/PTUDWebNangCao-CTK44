import axios from "axios";

export async function getCategories() {
    try {
        const response = await axios.get("https://localhost:7239/api/categories");
        const data = await response.data;
        if (data.isSuccess) {
            return data.result;
        } else {
            return null;
        }
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}