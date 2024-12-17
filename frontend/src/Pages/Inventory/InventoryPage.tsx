import React, { useEffect } from "react";
import { Segment } from "semantic-ui-react";
import InventoryItemList from "./InventoryItemList";
import LoadingComponent from "../../mainComponents/LoadingComponent";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

const InventoryPage: React.FC = () => {
  const { inventoryStore } = useStore();

  useEffect(() => {
    inventoryStore.loadItems();
  }, [inventoryStore]);

  if (inventoryStore.loadingInitial)
    return <LoadingComponent content="Loading data" />;

  return (
    <Segment clearing>
      <InventoryItemList inventoryItems={inventoryStore.items} />

      {/* <Button
        content="Add item"
        primary
        fluid
        style={{ marginBottom: "1rem" }}
        onClick={handleCreate}
      />

      <ItemDetailsCard
        itemId={selectedItem?.id ?? undefined}
        existingItem={selectedItem}
        mode={mode}
        onClose={closeDetails}
      /> */}
    </Segment>
  );
};

export default observer(InventoryPage);
