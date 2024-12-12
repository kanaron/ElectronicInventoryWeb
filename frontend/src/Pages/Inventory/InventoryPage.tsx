import React, { useEffect, useState } from "react";
import { Button, Grid, Segment } from "semantic-ui-react";
import { InventoryItem } from "../../models/InventoryItem";
import axios from "axios";
import InventoryItemList from "./InventoryItemList";
import ItemDetailsCard from "./ItemDetailsCard";

const InventoryPage: React.FC = () => {
  const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>([]);
  const [selectedItem, setSelectedItem] = useState<InventoryItem | null>();
  const [mode, setMode] = useState<"create" | "edit" | "details">("create");
  const [showCard, setShowCard] = useState<boolean>(false);

  useEffect(() => {
    axios
      .get<InventoryItem[]>(
        "https://localhost:7000/api/Inventory/GetInventoryItems"
      )
      .then((response) => {
        setInventoryItems(response.data);
      });
  }, []);

  const handleCreate = () => {
    setMode("create");
    setSelectedItem(null);
    setShowCard(true);
  };

  const handleEdit = (id: number) => {
    let item = inventoryItems.find((x) => x.id === id);
    setMode("edit");
    setSelectedItem(item);
    setShowCard(true);
  };

  const handleDetails = (id: number) => {
    let item = inventoryItems.find((x) => x.id === id);
    setMode("details");
    setSelectedItem(item);
    setShowCard(true);
  };

  const closeDetails = (ev?: React.MouseEvent) => {
    setSelectedItem(null);
    setShowCard(false);
  };

  return (
    <Segment clearing>
      <Grid>
        <Grid.Column width={10}>
          <InventoryItemList
            inventoryItems={inventoryItems}
            onEdit={handleEdit}
            onViewDetails={handleDetails}
          />
        </Grid.Column>

        <Grid.Column width={6}>
          <Button
            content="Add item"
            primary
            style={{ marginBottom: "1rem" }}
            onClick={handleCreate}
          />

          {showCard === true && (
            <ItemDetailsCard
              itemId={selectedItem?.id ?? undefined}
              existingItem={selectedItem}
              mode={mode}
              onClose={closeDetails}
            />
          )}
        </Grid.Column>
      </Grid>
    </Segment>
  );
};

export default InventoryPage;
