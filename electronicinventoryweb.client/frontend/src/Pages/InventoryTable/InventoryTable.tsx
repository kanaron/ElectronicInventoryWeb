import React from "react";

const InventoryTable: React.FC = () => {
  // Example data
  const inventoryItems = [
    {
      id: 1,
      type: "Resistor",
      name: "res",
      value: "10kΩ",
      quantity: 100,
      package: "0402",
      dateAdded: "2024-10-10",
    },
    {
      id: 2,
      type: "Capacitor",
      value: "100μF",
      quantity: 50,
      dateAdded: "2024-10-12",
    },
  ];

  const handleAddItem = async (item: any) => {
    try {
      const response = await fetch("/api/addinventory", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(item),
      });

      // if (response.ok) {
      //   const newItem = await response.json();
      //   setInventoryItems((prevItems) => [...prevItems, newItem]);
      //   setShowForm(false); // Close the form after adding the item
      // } else {
      //   console.error("Failed to add item:", response.statusText);
      // }
    } catch (error) {
      console.error("Error adding item:", error);
    }
  };

  const handleAddItemFromTME = () => {
    // Logic to add an item from TME
    console.log("Add item from TME clicked");
  };

  return (
    <div className="inventory-table-container">
      <div className="table-controls">
        <button onClick={handleAddItem} className="add-item-button">
          Add Item
        </button>
        <button
          onClick={handleAddItemFromTME}
          className="add-item-from-tme-button"
        >
          Add Item from TME
        </button>
      </div>
      <table className="inventory-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Quantity</th>
            <th>Package</th>
          </tr>
        </thead>
        <tbody>
          {inventoryItems.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.name}</td>
              <td>{item.quantity}</td>
              <td>{item.package}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default InventoryTable;
