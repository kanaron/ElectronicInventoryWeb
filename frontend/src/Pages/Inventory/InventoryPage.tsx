import React, { useEffect, useState } from "react";
import { Button, Grid, Segment } from "semantic-ui-react";
import { InventoryItem } from "../../models/InventoryItem";
import InventoryItemList from "./InventoryItemList";
import ItemDetailsCard from "./ItemDetailsCard";
import agent from "../../app/agent";
import LoadingComponent from "../../mainComponents/LoadingComponent";

const InventoryPage: React.FC = () => {
  const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>([]);
  const [selectedItem, setSelectedItem] = useState<InventoryItem | null>();
  const [mode, setMode] = useState<"create" | "edit" | "details">("create");
  const [showCard, setShowCard] = useState<boolean>(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    agent.InventoryItems.list().then((response) => {
      let inventoryItems: InventoryItem[] = [];
      response.forEach((item) => {
        item.dateAdded = item.dateAdded.split("T")[0];
        item.lastUpdated = item.lastUpdated.split("T")[0];
        inventoryItems.push(item);
      });
      setInventoryItems(inventoryItems);
      setLoading(false);
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

  if (loading) return <LoadingComponent content="Loading data" />;

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
            fluid
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
