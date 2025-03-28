import React, { useState } from "react";
import { InventoryItem } from "../../models/InventoryItem";
import {
  Button,
  Checkbox,
  Dropdown,
  Header,
  Image,
  Input,
  Modal,
  ModalContent,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { NavLink } from "react-router-dom";

export default function InventoryItemList() {
  const { inventoryStore } = useStore();
  const [expandedRow, setExpandedRow] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterCategory, setFilterCategory] = useState("");
  const [showInactive, setShowInactive] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<InventoryItem | null>(null);

  const toggleRow = (id: string) => {
    setExpandedRow((prev) => (prev === id ? null : id));
  };

  const handleEdit = (selectedItem: InventoryItem) => {
    inventoryStore.selectedItem = selectedItem;
    inventoryStore.openForm(selectedItem.id);
  };

  const handleDelete = () => {
    if (selectedItem) {
      inventoryStore.removeItem(selectedItem);
      setIsModalOpen(false);
    }
  };

  const handleSetInactive = () => {
    if (selectedItem) {
      selectedItem.isActive = false;
      inventoryStore.editMode = true;
      inventoryStore.addOrUpdateItem(selectedItem);
      setIsModalOpen(false);
    }
  };

  const handleSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value.toLowerCase());
  };

  const openModal = (item: InventoryItem) => {
    setSelectedItem(item);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setSelectedItem(null);
    setIsModalOpen(false);
  };

  const handleFilterCategory = (event: any, data: any) => {
    setFilterCategory(data.value);
  };

  const handleShowInactive = () => {
    setShowInactive((prev) => !prev);
  };

  const handleAddItem = () => {
    inventoryStore.openForm();
  };

  const filteredItems = inventoryStore.items.filter((item) => {
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
    new Set(inventoryStore.items.map((item) => item.category))
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
        <Button
          positive
          icon="plus"
          content="Add item"
          onClick={() => handleAddItem()}
          as={NavLink}
          to="/addItem"
        />
      </div>

      <Table celled>
        <TableHeader>
          <TableRow>
            <Table.HeaderCell content="Photo" />
            <Table.HeaderCell content="Category" />
            <Table.HeaderCell content="Quantity" />
            <Table.HeaderCell content="Reserved" />
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
                <TableCell content={item.reservedForProjects} />
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
                      onClick={() => toggleRow(item.id!.toString())}
                      content={
                        expandedRow === item.id!.toString()
                          ? "Hide Details"
                          : "Details"
                      }
                      size="small"
                    />
                    <Button
                      icon="trash alternate"
                      color="red"
                      onClick={() => openModal(item)}
                      content="Remove"
                      size="small"
                    />
                  </div>
                </TableCell>
              </Table.Row>

              {/* Expanded Row */}
              {expandedRow === item.id!.toString() && (
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

      <Modal open={isModalOpen} onClose={closeModal} size="tiny">
        <Header>Confirm Action</Header>
        <ModalContent>
          <p style={{ color: "#333" }}>
            Do you want to permanently delete this item or set it as inactive?
          </p>
        </ModalContent>
        <Modal.Actions>
          <Button color="red" onClick={handleDelete}>
            Remove
          </Button>
          <Button color="yellow" onClick={handleSetInactive}>
            Set as Inactive
          </Button>
          <Button onClick={closeModal}>Cancel</Button>
        </Modal.Actions>
      </Modal>
    </div>
  );
}
