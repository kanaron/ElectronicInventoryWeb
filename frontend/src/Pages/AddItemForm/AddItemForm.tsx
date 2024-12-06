import axios from "axios";
import React, { useEffect, useState } from "react";

const AddItemForm: React.FC = () => {
  const [formData, setFormData] = useState({
    type: "",
    symbol: "",
    category: "",
    value: "",
    package: "",
    quantity: 0,
    location: "Default",
    datasheetLink: "",
    storeLink: "",
    photoUrl: "",
    minStockLevel: 0,
    description: "",
    isActive: true,
    tags: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: checked,
    }));
  };

  const handleFetchFromTme = () => {
    if (formData.symbol.trim() === "") {
      alert("Please enter a symbol to fetch data from TME.");
      return;
    }

    axios
      .get(
        `https://localhost:7000/api/Inventory/FetchFromTme?symbol=${formData.symbol}`
      )
      .then((response) => {
        if (response.status === 200) {
          setFormData((prev) => ({
            ...prev,
            ...response.data,
          }));
        } else {
          alert(`Unexpected response: ${response.status}`);
        }
      })
      .catch((error) => {
        if (error.response) {
          if (error.response.status === 404) {
            alert(
              "Item not found in TME database. Please check symbol or fill data manually"
            );
          } else {
            alert(
              `Error: ${error.response.status} - ${
                error.response.data.message || "Unknown error"
              }`
            );
          }
        } else if (error.requirest) {
          alert("Network error. Please try again later.");
        } else {
          alert(`Error: ${error.message}`);
        }
      });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = axios.post(
        "https://localhost:7000/api/Inventory/AddInventoryItem",
        formData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
    } catch (error: any) {
      if (error.response) {
        if (error.response.status === 400) {
          alert("Bad request: " + error.response.data);
        } else if (error.response.status === 401) {
          alert("Unauthorized: Please log in again.");
        } else {
          alert(
            "Error: " + error.response.data || "An unknown error occurred."
          );
        }
      } else {
        alert("Unable to reach the server. Please try again later.");
      }
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Add New Inventory Item</h2>
      <div>
        <label>
          Symbol:
          <input
            type="text"
            name="symbol"
            value={formData.symbol}
            onChange={handleChange}
            required
          />
          <button type="button" onClick={handleFetchFromTme}>
            Fetch from TME
          </button>
        </label>
      </div>
      <div>
        <label>
          Type:
          <input
            type="text"
            name="type"
            value={formData.type}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Category:
          <input
            type="text"
            name="category"
            value={formData.category}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Value:
          <input
            type="text"
            name="value"
            value={formData.value}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Package:
          <input
            type="text"
            name="package"
            value={formData.package}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Quantity:
          <input
            type="number"
            name="quantity"
            value={formData.quantity}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Location:
          <input
            type="text"
            name="location"
            value={formData.location}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Datasheet Link:
          <input
            type="text"
            name="datasheetLink"
            value={formData.datasheetLink}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Store Link:
          <input
            type="text"
            name="storeLink"
            value={formData.storeLink}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Photo URL:
          <input
            type="text"
            name="photoUrl"
            value={formData.photoUrl}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Min Stock Level:
          <input
            type="number"
            name="minStockLevel"
            value={formData.minStockLevel}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Description:
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Tags (comma-separated):
          <input
            type="text"
            name="tags"
            value={formData.tags}
            onChange={handleChange}
          />
        </label>
      </div>
      <div>
        <label>
          Active:
          <input
            type="checkbox"
            name="isActive"
            checked={formData.isActive}
            onChange={handleCheckboxChange}
          />
        </label>
      </div>
      <button type="submit">Save Item</button>
    </form>
  );
};

export default AddItemForm;
