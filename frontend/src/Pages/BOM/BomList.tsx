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
            <Table.HeaderCell content="Is placed" />
          </TableRow>
        </TableHeader>

        <Table.Body>
          {bomStore.filteredBomItems.map((item) => (
            <React.Fragment key={item.id}>
              {/* Main Row */}
              <Table.Row>
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
                          <Table.HeaderCell content="Symbol" />
                          <Table.HeaderCell content="Category" />
                          <Table.HeaderCell content="Value" />
                          <Table.HeaderCell content="Package" />
                          <Table.HeaderCell content="Quantity" />
                          <Table.HeaderCell content="Location" />
                        </TableRow>
                      </TableHeader>
                      <Table.Body>
                        {item.matchingItems.map((inventoryItem) => (
                          <Table.Row key={inventoryItem.id}>
                            <TableCell content={inventoryItem.symbol} />
                            <TableCell content={inventoryItem.category} />
                            <TableCell content={inventoryItem.value} />
                            <TableCell content={inventoryItem.package} />
                            <TableCell content={inventoryItem.quantity} />
                            <TableCell content={inventoryItem.location} />
                          </Table.Row>
                        ))}
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
