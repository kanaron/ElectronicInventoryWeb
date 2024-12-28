import React, { useState } from "react";
import { Button, Card, CheckboxProps, Form, Image } from "semantic-ui-react";
import { InventoryItem } from "../../models/InventoryItem";
import LoadingComponent from "../../mainComponents/LoadingComponent";
import agent from "../../app/agent";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router-dom";

const ItemDetailsCard: React.FC = () => {
  const { inventoryStore } = useStore();
  const navigate = useNavigate();
  const [createNext, setCreateNext] = useState(false);

  const [formData, setFormData] = useState<InventoryItem>(
    inventoryStore.selectedItem || {
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

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]:
        name === "quantity" || name === "minStockLevel"
          ? parseInt(value, 10)
          : value,
    }));
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

  const handleCreateNextChange = (
    e: React.FormEvent<HTMLInputElement>,
    data: any
  ) => {
    setCreateNext(data.checked);
  };

  const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = e.target;
    setFormData((prev) => ({
      ...prev,
      tags: value.split(",").map((tag) => tag.trim()),
    }));
  };

  const handleFetchFromTme = async () => {
    inventoryStore.setLoading(true);

    if (formData.symbol.trim() === "") {
      alert("Please enter a symbol to fetch data from TME.");
      return;
    }

    try {
      const response = await agent.InventoryItems.fetchFromTme(formData.symbol);
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
    } finally {
      inventoryStore.setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    inventoryStore.addOrUpdateItem(formData);
    inventoryStore.closeForm();

    if (createNext) {
      setFormData({
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
      });
    } else {
      navigate(`/inventory`);
    }
  };

  return (
    <Card style={{ width: "100%", padding: "8px" }}>
      {inventoryStore.loading && <LoadingComponent content="Fetching data" />}
      {formData.photoUrl && (
        <Image src={formData?.photoUrl} size="small" centered />
      )}
      <Card.Content>
        <Card.Header>
          {inventoryStore.editMode
            ? `Edit item with symbol ${inventoryStore.selectedItem?.symbol}`
            : "Add New Item"}
        </Card.Header>
        <Form>
          <div style={{ display: "flex", flexDirection: "column", gap: "0px" }}>
            <div style={{ display: "flex", alignItems: "center" }}>
              <Form.Input
                label="Symbol"
                name="symbol"
                value={formData.symbol}
                onChange={handleChange}
                required={true}
              />
              <Button
                content="Fetch from TME"
                onClick={handleFetchFromTme}
                style={{ margin: 8 }}
                disabled={!formData.symbol.trim()}
              />
            </div>

            <Form.Input
              label="Type"
              name="type"
              value={formData.type}
              onChange={handleChange}
            />

            <Form.Input
              label="Category"
              name="category"
              value={formData.category}
              onChange={handleChange}
            />

            <Form.Input
              label="Value"
              name="value"
              value={formData.value}
              onChange={handleChange}
            />

            <Form.Input
              label="Package"
              name="package"
              value={formData.package}
              onChange={handleChange}
            />

            <Form.Input
              label="Quantity"
              type="number"
              name="quantity"
              value={formData.quantity}
              onChange={handleChange}
            />

            <Form.Input
              label="Location"
              name="location"
              value={formData.location}
              onChange={handleChange}
            />

            <Form.Input
              label="Datasheet link"
              name="datasheetLink"
              value={formData.datasheetLink}
              onChange={handleChange}
            />

            <Form.Input
              label="Store link"
              name="storeLink"
              value={formData.storeLink}
              onChange={handleChange}
            />

            <Form.Input
              label="Photo url"
              name="photoUrl"
              value={formData.photoUrl}
              onChange={handleChange}
            />

            <Form.Input
              label="Min stock level"
              type="number"
              name="minStockLevel"
              value={formData.minStockLevel}
              onChange={handleChange}
            />

            <Form.TextArea
              label="Description"
              name="description"
              value={formData.description}
              onChange={handleChange}
            />

            <Form.Input
              label="Tags"
              name="tags"
              value={formData.tags}
              onChange={handleTagsChange}
            />

            <Form.Checkbox
              label="Is active"
              name="isActive"
              checked={formData.isActive}
              onChange={handleCheckboxChange}
            />

            {inventoryStore.editMode === false && (
              <Form.Checkbox
                label="Create next"
                name="createNext"
                checked={createNext}
                onChange={handleCreateNextChange}
              />
            )}

            <Button primary type="submit" onClick={handleSubmit}>
              Save
            </Button>
          </div>
        </Form>
      </Card.Content>
    </Card>
  );
};

export default observer(ItemDetailsCard);
