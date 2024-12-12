import React, { useEffect, useState } from "react";
import { Button, Card, CheckboxProps, Form, Image } from "semantic-ui-react";
import { InventoryItem } from "../../models/InventoryItem";
import axios from "axios";

interface ItemDetailsFormProps {
  itemId?: number;
  existingItem?: InventoryItem | null | undefined;
  mode: "create" | "edit" | "details";
  onClose: () => void;
}

const ItemDetailsCard: React.FC<ItemDetailsFormProps> = ({
  itemId,
  existingItem,
  mode,
  onClose,
}) => {
  const [formData, setFormData] = useState<InventoryItem>(
    existingItem || {
      id: 0,
      type: "",
      symbol: "",
      category: "",
      value: "",
      package: "",
      quantity: 0,
      location: "",
      datasheetLink: "",
      storeLink: "",
      photoUrl: "",
      minStockLevel: 0,
      description: "",
      isActive: true,
      tags: [],
      dateAdded: new Date().toISOString(),
      lastUpdated: new Date().toISOString(),
    }
  );

  useEffect(() => {
    if (itemId && existingItem) {
      setFormData(existingItem);
    }
  }, [itemId, existingItem]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    if (mode !== "details") {
      const { name, value } = e.target;
      setFormData((prev) => ({
        ...prev,
        [name]:
          name === "quantity" || name === "minStockLevel"
            ? parseInt(value, 10)
            : value,
      }));
    }
  };

  const handleCheckboxChange = (
    e: React.FormEvent<HTMLInputElement>,
    data: CheckboxProps
  ) => {
    const { name, checked } = data;
    setFormData((prev) => ({
      ...prev,
      [name as keyof FormData]: checked,
    }));
  };

  const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = e.target;
    setFormData((prev) => ({
      ...prev,
      tags: value.split(",").map((tag) => tag.trim()),
    }));
  };

  const handleFetchFromTme = async () => {
    if (formData.symbol.trim() === "") {
      alert("Please enter a symbol to fetch data from TME.");
      return;
    }

    try {
      const response = await axios.get<InventoryItem>(
        `https://localhost:7000/api/Inventory/FetchFromTme?symbol=${formData.symbol}`
      );
      if (response.status === 200) {
        setFormData((prev) => ({
          ...prev,
          ...response.data,
        }));
      } else {
        alert(`Unexpected response: ${response.status}`);
      }
    } catch (error: any) {
      if (error.response) {
        if (error.response.status === 404) {
          alert(
            "Item not found in TME database. Please check the symbol or fill data manually."
          );
        } else {
          alert(
            `Error: ${error.response.status} - ${
              error.response.data.message || "Unknown error"
            }`
          );
        }
      } else {
        alert(`Error: ${error.message}`);
      }
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    if (mode === "create" || mode === "edit") {
      e.preventDefault();

      try {
        const url = itemId
          ? `https://localhost:7000/api/Inventory/UpdateInventoryItem/${itemId}`
          : `https://localhost:7000/api/Inventory/AddInventoryItem`;

        const method = itemId ? "put" : "post";

        await axios({
          method,
          url,
          data: formData,
          headers: {
            "Content-Type": "application/json",
          },
        });

        alert(
          itemId ? "Item updated successfully!" : "Item added successfully!"
        );
      } catch (error: any) {
        if (error.response) {
          alert(
            "Error: " + error.response.data || "An unknown error occurred."
          );
        } else {
          alert("Unable to reach the server. Please try again later.");
        }
      } finally {
        onClose?.();
      }
    } else {
      onClose?.();
    }
  };

  return (
    <Card style={{ width: "100%", padding: "20px" }}>
      {formData.photoUrl && <Image src={formData?.photoUrl} size="small" />}
      <Card.Content>
        <Card.Header>
          {mode === "create"
            ? "Add New Item"
            : mode === "edit"
            ? `Edit item with symbol ${existingItem?.symbol}`
            : `Details of item with symbol ${existingItem?.symbol}`}
        </Card.Header>
        <Form>
          <div
            style={{ display: "flex", flexDirection: "column", gap: "10px" }}
          >
            <div style={{ display: "flex", alignItems: "center" }}>
              <Form.Input
                label="Symbol"
                name="symbol"
                value={formData.symbol}
                onChange={handleChange}
                readOnly={mode === "details"}
                required={mode === "create" || mode === "edit"}
              />
              {(mode === "create" || mode === "edit") && (
                <Button
                  content="Fetch from TME"
                  onClick={handleFetchFromTme}
                  style={{ margin: 8 }}
                  disabled={!formData.symbol.trim()}
                />
              )}
            </div>

            <Form.Input
              label="Type"
              name="type"
              value={formData.type}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Category"
              name="category"
              value={formData.category}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Value"
              name="value"
              value={formData.value}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Package"
              name="package"
              value={formData.package}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Quantity"
              type="number"
              name="quantity"
              value={formData.quantity}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Location"
              name="location"
              value={formData.location}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            {mode === "details" && existingItem?.datasheetLink ? (
              <p>
                <a
                  href={existingItem.datasheetLink}
                  target="_blank"
                  rel="noreferrer"
                >
                  View datasheet
                </a>
              </p>
            ) : (
              <Form.Input
                label="Datasheet link"
                name="datasheetLink"
                value={formData.datasheetLink}
                onChange={handleChange}
              />
            )}

            {mode === "details" && existingItem?.storeLink ? (
              <p>
                <a
                  href={existingItem.storeLink}
                  target="_blank"
                  rel="noreferrer"
                >
                  View store link
                </a>
              </p>
            ) : (
              <Form.Input
                label="Store link"
                name="storeLink"
                value={formData.storeLink}
                onChange={handleChange}
              />
            )}

            {mode === "details" && existingItem?.photoUrl ? (
              <p>
                <a
                  href={existingItem.photoUrl}
                  target="_blank"
                  rel="noreferrer"
                >
                  Photo url
                </a>
              </p>
            ) : (
              <Form.Input
                label="Photo url"
                name="photoUrl"
                value={formData.photoUrl}
                onChange={handleChange}
              />
            )}

            <Form.Input
              label="Min stock level"
              type="number"
              name="minStockLevel"
              value={formData.minStockLevel}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.TextArea
              label="Description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              readOnly={mode === "details"}
            />

            <Form.Input
              label="Tags"
              name="tags"
              value={formData.tags}
              onChange={handleTagsChange}
              readOnly={mode === "details"}
            />

            <Form.Checkbox
              label="Is active"
              name="isActive"
              checked={formData.isActive}
              onChange={handleCheckboxChange}
              readOnly={mode === "details"}
            />

            <Button primary type="submit" onClick={handleSubmit}>
              {mode === "create" || mode === "edit" ? "Save" : "Close"}
            </Button>
          </div>
        </Form>
      </Card.Content>
    </Card>
  );
};

export default ItemDetailsCard;
