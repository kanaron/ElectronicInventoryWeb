import React, { useState } from "react";
import {
  Button,
  Checkbox,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
//import { BomItem } from "../../models/BomItem";
import { observer } from "mobx-react-lite";

export default observer(function BomList() {
  const { bomStore } = useStore();
  const [expandedRow, setExpandedRow] = useState<string | null>(null);
  //const [selectedItem, setSelectedItem] = useState<BomItem | null>(null);

  const toggleRow = (id: string) => {
    setExpandedRow((prev) => (prev === id ? null : id));
  };

  const handleShowIrrelevant = () => {
    bomStore.setShowIrrelevant(!bomStore.showIrrevelant);
  };

  const handleShowPlaced = () => {
    bomStore.setShowPlaced(!bomStore.showPlaced);
  };

  return (
    <div>
      <div style={{ marginBottom: "10px", display: "flex", gap: "10px" }}>
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox
            toggle
            checked={bomStore.showIrrevelant}
            onChange={handleShowIrrelevant}
          />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Show Irrelevant
          </label>
        </div>
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox
            toggle
            checked={bomStore.showPlaced}
            onChange={handleShowPlaced}
          />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Show Placed
          </label>
        </div>
        <div>
          <Button
            primary
            icon="save"
            onClick={bomStore.updateBomItems}
            content="Save changes"
            size="medium"
          />
        </div>
      </div>

      <Table celled>
        <TableHeader>
          <TableRow>
            <Table.HeaderCell content="References" />
            <Table.HeaderCell content="Category" />
            <Table.HeaderCell content="Value" />
            <Table.HeaderCell content="Package" />
            <Table.HeaderCell content="Quantity" />
            <Table.HeaderCell content="Description" />
            <Table.HeaderCell content="Is relevant" />
            <Table.HeaderCell content="Lost" />
            <Table.HeaderCell content="Is placed" />
          </TableRow>
        </TableHeader>

        <Table.Body>
          {bomStore.filteredBomItems.map((item) => (
            <React.Fragment key={item.id}>
              {/* Main Row */}
              <Table.Row
                style={{
                  backgroundColor:
                    item.isMatched === 0
                      ? "#ffcccc"
                      : item.isMatched === 1
                      ? "#fff3cd"
                      : item.isMatched === 2 ||
                        item.isMatched === 3 ||
                        item.isMatched === 4
                      ? "#d4edda"
                      : "inherit",
                }}
              >
                <TableCell className="references-column">
                  {item.references}
                </TableCell>
                <TableCell className="category-column">
                  {item.category}
                </TableCell>
                <TableCell className="value-column">{item.value}</TableCell>
                <TableCell className="package-column">{item.package}</TableCell>
                <TableCell>{item.quantity}</TableCell>
                <TableCell className="description-column">
                  {item.description}
                </TableCell>
                <TableCell>
                  <Checkbox
                    checked={item.isRelevant}
                    onChange={() => bomStore.toggleRelevance(item.id)}
                  />
                </TableCell>
                <TableCell>
                  <input
                    type="number"
                    min={0}
                    value={item.lostQuantity ?? 0}
                    style={{ width: "60px" }}
                    onChange={(e) => {
                      const updated = parseInt(e.target.value, 10);
                      item.lostQuantity = isNaN(updated) ? 0 : updated;
                    }}
                  />
                </TableCell>
                <TableCell>
                  <Checkbox
                    checked={item.isPlaced}
                    onChange={() => bomStore.togglePlaced(item.id)}
                  />
                </TableCell>
                <TableCell>
                  <div style={{ display: "flex", alignItems: "center" }}>
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
                  </div>
                </TableCell>
              </Table.Row>

              {/* Expanded Row */}
              {expandedRow === item.id && item.matchingItems.length > 0 && (
                <Table.Row>
                  <TableCell colSpan={9}>
                    <Table compact celled>
                      <TableHeader>
                        <TableRow>
                          <Table.HeaderCell>Priority</Table.HeaderCell>
                          <Table.HeaderCell>Select</Table.HeaderCell>
                          <Table.HeaderCell>Symbol</Table.HeaderCell>
                          <Table.HeaderCell>Category</Table.HeaderCell>
                          <Table.HeaderCell>Value</Table.HeaderCell>
                          <Table.HeaderCell>Package</Table.HeaderCell>
                          <Table.HeaderCell>Quantity</Table.HeaderCell>
                          <Table.HeaderCell>Location</Table.HeaderCell>
                        </TableRow>
                      </TableHeader>

                      <Table.Body>
                        {item.matchingItems.map((inv) => {
                          const priorityIndex =
                            item.selectedInventoryItemIds.indexOf(inv.id!);
                          const isSelected =
                            item.selectedInventoryItemIds.includes(inv.id!);

                          const toggleSelect = () => {
                            const current = [...item.selectedInventoryItemIds];
                            const index = current.indexOf(inv.id!);
                            if (index >= 0) {
                              current.splice(index, 1);
                            } else {
                              current.push(inv.id!);
                            }
                            bomStore.setSelectedInventoryItemIds(
                              item.id,
                              current
                            );
                          };

                          return (
                            <Table.Row key={inv.id}>
                              <TableCell textAlign="center">
                                {priorityIndex >= 0 ? priorityIndex + 1 : "-"}
                              </TableCell>

                              <TableCell collapsing>
                                <Checkbox
                                  checked={isSelected}
                                  onChange={toggleSelect}
                                />
                              </TableCell>
                              <TableCell>{inv.symbol}</TableCell>
                              <TableCell>{inv.category}</TableCell>
                              <TableCell>{inv.value}</TableCell>
                              <TableCell>{inv.package}</TableCell>
                              <TableCell>{inv.quantity}</TableCell>
                              <TableCell>{inv.location}</TableCell>
                            </Table.Row>
                          );
                        })}
                      </Table.Body>
                    </Table>
                  </TableCell>
                </Table.Row>
              )}
            </React.Fragment>
          ))}
        </Table.Body>
      </Table>
    </div>
  );
});
