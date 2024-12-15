import React, { useState } from "react";
import { InventoryItem } from "../../models/InventoryItem";
import {
  Button,
  Image,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";

interface Props {
  inventoryItems: InventoryItem[];
  onEdit: (id: number) => void;
  onViewDetails: (id: number) => void;
}

export default function InventoryItemList({
  inventoryItems,
  onEdit,
  onViewDetails,
}: Props) {
  const [expandedRow, setExpandedRow] = useState<string | null>(null);
  const toggleRow = (id: string) => {
    setExpandedRow((prev) => (prev === id ? null : id));
  };

  return (
    <Table celled>
      <TableHeader>
        <TableRow>
          <Table.HeaderCell content="Photo" />
          <Table.HeaderCell content="Quantity" />
          <Table.HeaderCell content="Type" />
          <Table.HeaderCell content="Value" />
          <Table.HeaderCell content="Package" />
          <Table.HeaderCell content="Location" />
          <Table.HeaderCell content="Actions" />
        </TableRow>
      </TableHeader>

      <Table.Body>
        {inventoryItems.map((item) => (
          <React.Fragment key={item.id}>
            {/* Main Row */}
            <Table.Row>
              <TableCell>
                <Image src={item.photoUrl} size="small" bordered />
              </TableCell>
              <TableCell content={item.quantity} />
              <TableCell content={item.type} />
              <TableCell content={item.value} />
              <TableCell content={item.package} />
              <TableCell content={item.location} />
              <TableCell>
                <div style={{ display: "flex", alignItems: "center" }}>
                  <Button
                    primary
                    onClick={() => onEdit(item.id)}
                    content="Edit"
                    size="small"
                  />
                  <Button
                    secondary
                    onClick={() => toggleRow(item.id.toString())}
                    content={
                      expandedRow === item.id.toString()
                        ? "Hide Details"
                        : "Details"
                    }
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
                    <strong>Category:</strong> {item.category} <br />
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
  );
}
