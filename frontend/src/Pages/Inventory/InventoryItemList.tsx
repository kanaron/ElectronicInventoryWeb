import React, { useState } from "react";
import { InventoryItem } from "../../models/InventoryItem";
import {
  Button,
  Checkbox,
  Dropdown,
  Image,
  Input,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { NavLink } from "react-router-dom";

interface Props {
  inventoryItems: InventoryItem[];
}

export default function InventoryItemList({ inventoryItems }: Props) {
  const { inventoryStore } = useStore();
  const [expandedRow, setExpandedRow] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterCategory, setFilterCategory] = useState("");
  const [showInactive, setShowInactive] = useState(false);

  const toggleRow = (id: string) => {
    setExpandedRow((prev) => (prev === id ? null : id));
  };

  const handleEdit = (selectedItem: InventoryItem) => {
    inventoryStore.selectedItem = selectedItem;
    inventoryStore.openForm(selectedItem.id);
  };

  const handleDelete = (selectedItem: InventoryItem) => {
    inventoryStore.removeItem(selectedItem);
  };

  const handleSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value.toLowerCase());
  };

  const handleFilterCategory = (event: any, data: any) => {
    setFilterCategory(data.value);
  };

  const handleShowInactive = () => {
    setShowInactive((prev) => !prev);
  };

  const filteredItems = inventoryItems.filter((item) => {
    const matchesSearch =
      searchTerm === "" ||
      item.type.toLowerCase().includes(searchTerm) ||
      item.symbol.toLowerCase().includes(searchTerm) ||
      item.value.toLowerCase().includes(searchTerm) ||
      item.package.toLowerCase().includes(searchTerm) ||
      item.description.toLowerCase().includes(searchTerm) ||
      item.tags.some((tag) => tag.toLowerCase().includes(searchTerm));

    const matchesCategory =
      filterCategory === "" || item.category === filterCategory;

    const matchesActiveStatus = showInactive || item.isActive;

    return matchesSearch && matchesCategory && matchesActiveStatus;
  });

  const categoryOptions = Array.from(
    new Set(inventoryItems.map((item) => item.category))
  ).map((category) => ({
    key: category,
    text: category,
    value: category,
  }));

  return (
    <div>
      <div style={{ marginBottom: "10px", display: "flex", gap: "10px" }}>
        <Input
          icon="search"
          placeholder="Search..."
          onChange={handleSearch}
          value={searchTerm}
        />
        <Dropdown
          placeholder="Filter by Category"
          clearable
          selection
          options={categoryOptions}
          onChange={handleFilterCategory}
        />
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox
            toggle
            checked={showInactive}
            onChange={handleShowInactive}
          />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Show Inactive
          </label>
        </div>
      </div>

      <Table celled>
        <TableHeader>
          <TableRow>
            <Table.HeaderCell content="Photo" />
            <Table.HeaderCell content="Category" />
            <Table.HeaderCell content="Quantity" />
            <Table.HeaderCell content="Type" />
            <Table.HeaderCell content="Value" />
            <Table.HeaderCell content="Package" />
            <Table.HeaderCell content="Location" />
            <Table.HeaderCell content="Actions" />
          </TableRow>
        </TableHeader>

        <Table.Body>
          {filteredItems.map((item) => (
            <React.Fragment key={item.id}>
              {/* Main Row */}
              <Table.Row>
                <TableCell>
                  <Image src={item.photoUrl} size="tiny" bordered />
                </TableCell>
                <TableCell content={item.category} />
                <TableCell content={item.quantity} />
                <TableCell content={item.type} />
                <TableCell content={item.value} />
                <TableCell content={item.package} />
                <TableCell content={item.location} />
                <TableCell>
                  <div style={{ display: "flex", alignItems: "center" }}>
                    <Button
                      primary
                      icon="edit"
                      onClick={() => handleEdit(item)}
                      content="Edit"
                      size="small"
                      as={NavLink}
                      to="/addItem"
                    />
                    <Button
                      secondary
                      icon="info"
                      onClick={() => toggleRow(item.id.toString())}
                      content={
                        expandedRow === item.id.toString()
                          ? "Hide Details"
                          : "Details"
                      }
                      size="small"
                    />
                    <Button
                      icon="remove"
                      onClick={() => handleDelete(item)}
                      content="Remove"
                      size="small"
                    />
                  </div>
                </TableCell>
              </Table.Row>

              {/* Expanded Row */}
              {expandedRow === item.id.toString() && (
                <Table.Row>
                  <TableCell colSpan="7">
                    <div
                      style={{
                        padding: "10px",
                        backgroundColor: "#f9f9f9",
                        borderRadius: "5px",
                      }}
                    >
                      <strong>Symbol:</strong> {item.symbol} <br />
                      <strong>Datasheet:</strong>{" "}
                      <a
                        href={item.datasheetLink}
                        target="_blank"
                        rel="noopener noreferrer"
                      >
                        {item.datasheetLink}
                      </a>{" "}
                      <br />
                      <strong>Store Link:</strong>{" "}
                      <a
                        href={item.storeLink}
                        target="_blank"
                        rel="noopener noreferrer"
                      >
                        {item.storeLink}
                      </a>{" "}
                      <br />
                      <strong>Min Stock Level:</strong> {item.minStockLevel}{" "}
                      <br />
                      <strong>Description:</strong> {item.description} <br />
                      <strong>Tags:</strong> {item.tags.join(", ")} <br />
                      <strong>Date Added:</strong> {item.dateAdded} <br />
                      <strong>Last Updated:</strong> {item.lastUpdated} <br />
                      <strong>Active:</strong> {item.isActive ? "Yes" : "No"}{" "}
                      <br />
                    </div>
                  </TableCell>
                </Table.Row>
              )}
            </React.Fragment>
          ))}
        </Table.Body>
      </Table>
    </div>
  );
}
