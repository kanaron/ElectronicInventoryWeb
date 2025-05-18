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
import { observer } from "mobx-react-lite";
import { BomItem } from "../../models/BomItem";

export default observer(function BomList() {
  const { bomStore } = useStore();
  const [expandedRow, setExpandedRow] = useState<string | null>(null);
  const [groupBy, setGroupBy] = useState(false);

  const handleShowIrrelevant = () => {
    bomStore.setShowIrrelevant(!bomStore.showIrrevelant);
  };

  const handleShowPlaced = () => {
    bomStore.setShowPlaced(!bomStore.showPlaced);
  };

  const handleToggleGroupBy = () => {
    setGroupBy((prev) => !prev);
  };

  const sortByReference = (items: BomItem[]) => {
    return items.slice().sort((a, b) => {
      const refA = (a.references || "").split(",")[0].trim();
      const refB = (b.references || "").split(",")[0].trim();
      return refA.localeCompare(refB, undefined, { numeric: true });
    });
  };

  const groupedItems = React.useMemo(() => {
    if (!groupBy) return sortByReference(bomStore.filteredBomItems);

    const map = new Map<string, BomItem[]>();
    for (const item of bomStore.filteredBomItems) {
      const key = `${item.value}|${item.package}|${item.description}`;
      if (!map.has(key)) map.set(key, []);
      map.get(key)!.push(item);
    }

    const grouped = Array.from(map.values()).map((group) => {
      const base = group[0];
      return {
        ...base,
        id: base.id,
        references: group
          .flatMap((i) =>
            Array.isArray(i.references)
              ? i.references
              : i.references
                  .split(",")
                  .map((s) => s.replace(/[\[\]\s"]/g, "").trim())
          )
          .filter(Boolean)
          .sort((a, b) => a.localeCompare(b, undefined, { numeric: true }))
          .join(", "),
        quantity: group.reduce((sum, i) => sum + i.quantity, 0),
        _groupItems: group,
      };
    });

    return sortByReference(grouped);
  }, [groupBy, bomStore.filteredBomItems]);

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
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox toggle checked={groupBy} onChange={handleToggleGroupBy} />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Group same items
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

      <div style={{ overflowX: "auto", maxWidth: "100%" }}>
        <Table celled style={{ wordWrap: "break-word", tableLayout: "fixed" }}>
          <TableHeader>
            <TableRow>
              <Table.HeaderCell style={{ width: "15%" }}>
                References
              </Table.HeaderCell>
              <Table.HeaderCell>Category</Table.HeaderCell>
              <Table.HeaderCell>Value</Table.HeaderCell>
              <Table.HeaderCell>Package</Table.HeaderCell>
              <Table.HeaderCell>Quantity</Table.HeaderCell>
              <Table.HeaderCell>Description</Table.HeaderCell>
              <Table.HeaderCell>Is relevant</Table.HeaderCell>
              <Table.HeaderCell>Lost</Table.HeaderCell>
              <Table.HeaderCell>Is placed</Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
            </TableRow>
          </TableHeader>

          <Table.Body>
            {groupedItems.map((item) => (
              <React.Fragment key={item.id}>
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
                  <TableCell>{item.references}</TableCell>
                  <TableCell>{item.category}</TableCell>
                  <TableCell>{item.value}</TableCell>
                  <TableCell>{item.package}</TableCell>
                  <TableCell>{item.quantity}</TableCell>
                  <TableCell>{item.description}</TableCell>
                  <TableCell>
                    <Checkbox
                      checked={
                        (item as any)._groupItems
                          ? (item as any)._groupItems.every(
                              (i: BomItem) => i.isRelevant
                            )
                          : item.isRelevant
                      }
                      onChange={() => {
                        const items = (item as any)._groupItems ?? [item];
                        const shouldBeRelevant = !items.every(
                          (i: BomItem) => i.isRelevant
                        );
                        items.forEach((i: BomItem) => {
                          if (i.isRelevant !== shouldBeRelevant)
                            bomStore.toggleRelevance(i.id);
                        });
                        bomStore.setFilters();
                      }}
                    />
                  </TableCell>
                  <TableCell>
                    <input
                      type="number"
                      min="0"
                      value={item.lostQuantity ?? 0}
                      onChange={(e) => {
                        const updated = parseInt(e.target.value, 10);
                        item.lostQuantity = isNaN(updated) ? 0 : updated;
                      }}
                      style={{
                        width: "60px",
                        padding: "4px",
                        borderRadius: "4px",
                        border: "1px solid #ccc",
                        textAlign: "center",
                        backgroundColor: "#f9f9f9",
                        color: "#333",
                      }}
                    />
                  </TableCell>
                  <TableCell>
                    <Checkbox
                      checked={
                        (item as any)._groupItems
                          ? (item as any)._groupItems.every(
                              (i: BomItem) => i.isPlaced
                            )
                          : item.isPlaced
                      }
                      onChange={() => {
                        const items = (item as any)._groupItems ?? [item];
                        const shouldPlace = !items.every(
                          (i: BomItem) => i.isPlaced
                        );
                        items.forEach((i: BomItem) => {
                          if (i.isPlaced !== shouldPlace)
                            bomStore.togglePlaced(i.id);
                        });
                        bomStore.setFilters();
                      }}
                    />
                  </TableCell>
                  <TableCell>
                    {item.matchingItems.length > 0 && (
                      <Button
                        secondary
                        icon="info"
                        onClick={() =>
                          setExpandedRow((prev) =>
                            prev === item.id ? null : item.id
                          )
                        }
                        content={
                          expandedRow === item.id ? "Hide Details" : "Details"
                        }
                        size="small"
                      />
                    )}
                  </TableCell>
                </Table.Row>

                {expandedRow === item.id && item.matchingItems.length > 0 && (
                  <Table.Row>
                    <TableCell colSpan={10}>
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
                            const groupItems = (item as any)._groupItems ?? [
                              item,
                            ];
                            const current = [
                              ...groupItems[0].selectedInventoryItemIds,
                            ];
                            const priorityIndex = current.indexOf(inv.id!);
                            const isSelected = groupItems.every((i: BomItem) =>
                              i.selectedInventoryItemIds.includes(inv.id!)
                            );

                            const toggleSelect = () => {
                              const updated = [...current];
                              const idx = updated.indexOf(inv.id!);
                              if (idx >= 0) {
                                updated.splice(idx, 1);
                              } else {
                                updated.push(inv.id!);
                              }
                              groupItems.forEach((i: BomItem) => {
                                bomStore.setSelectedInventoryItemIds(
                                  i.id,
                                  updated
                                );
                              });
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
    </div>
  );
});
